using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

namespace DigitalRuby.ThunderAndLightning
{
    public class LightningBolt
    {
        public float MinimumDelay
        {
            get
            {
                return this._003CMinimumDelay_003Ek__BackingField;
            }
            private set
            {
                this._003CMinimumDelay_003Ek__BackingField = value;
            }
        }

        public bool HasGlow
        {
            get
            {
                return this._003CHasGlow_003Ek__BackingField;
            }
            private set
            {
                this._003CHasGlow_003Ek__BackingField = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return this.elapsedTime < this.lifeTime;
            }
        }

        public CameraMode CameraMode
        {
            get
            {
                return this._003CCameraMode_003Ek__BackingField;
            }
            private set
            {
                this._003CCameraMode_003Ek__BackingField = value;
            }
        }

        public void SetupLightningBolt(LightningBoltDependencies dependencies)
        {
            if (dependencies == null || dependencies.Parameters.Count == 0)
            {
                UnityEngine.Debug.LogError("Lightning bolt dependencies must not be null");
                return;
            }
            if (this.dependencies != null)
            {
                UnityEngine.Debug.LogError("This lightning bolt is already in use!");
                return;
            }
            this.dependencies = dependencies;
            this.CameraMode = dependencies.CameraMode;
            this.timeSinceLevelLoad = LightningBoltScript.TimeSinceStart;
            this.CheckForGlow(dependencies.Parameters);
            this.MinimumDelay = float.MaxValue;
            if (dependencies.ThreadState != null)
            {
                this.startTimeOffset = DateTime.UtcNow;
                dependencies.ThreadState.AddActionForBackgroundThread(new Action(this.ProcessAllLightningParameters));
                return;
            }
            this.ProcessAllLightningParameters();
        }

        public bool Update()
        {
            this.elapsedTime += LightningBoltScript.DeltaTime;
            if (this.elapsedTime > this.maxLifeTime)
            {
                return false;
            }
            if (this.hasLight)
            {
                this.UpdateLights();
            }
            return true;
        }

        public void Cleanup()
        {
            foreach (LightningBoltSegmentGroup lightningBoltSegmentGroup in this.segmentGroupsWithLight)
            {
                foreach (Light l in lightningBoltSegmentGroup.Lights)
                {
                    this.CleanupLight(l);
                }
                lightningBoltSegmentGroup.Lights.Clear();
            }
            List<LightningBoltSegmentGroup> obj = LightningBolt.groupCache;
            lock (obj)
            {
                foreach (LightningBoltSegmentGroup item in this.segmentGroups)
                {
                    LightningBolt.groupCache.Add(item);
                }
            }
            this.hasLight = false;
            this.elapsedTime = 0f;
            this.lifeTime = 0f;
            this.maxLifeTime = 0f;
            if (this.dependencies != null)
            {
                this.dependencies.ReturnToCache(this.dependencies);
                this.dependencies = null;
            }
            foreach (LightningBolt.LineRendererMesh lineRendererMesh in this.activeLineRenderers)
            {
                if (lineRendererMesh != null)
                {
                    lineRendererMesh.Reset();
                    LightningBolt.lineRendererCache.Add(lineRendererMesh);
                }
            }
            this.segmentGroups.Clear();
            this.segmentGroupsWithLight.Clear();
            this.activeLineRenderers.Clear();
        }

        public LightningBoltSegmentGroup AddGroup()
        {
            List<LightningBoltSegmentGroup> obj = LightningBolt.groupCache;
            LightningBoltSegmentGroup lightningBoltSegmentGroup;
            lock (obj)
            {
                if (LightningBolt.groupCache.Count == 0)
                {
                    lightningBoltSegmentGroup = new LightningBoltSegmentGroup();
                }
                else
                {
                    int index = LightningBolt.groupCache.Count - 1;
                    lightningBoltSegmentGroup = LightningBolt.groupCache[index];
                    lightningBoltSegmentGroup.Reset();
                    LightningBolt.groupCache.RemoveAt(index);
                }
            }
            this.segmentGroups.Add(lightningBoltSegmentGroup);
            return lightningBoltSegmentGroup;
        }

        public static void ClearCache()
        {
            foreach (LightningBolt.LineRendererMesh lineRendererMesh in LightningBolt.lineRendererCache)
            {
                if (lineRendererMesh != null)
                {
                    UnityEngine.Object.Destroy(lineRendererMesh.GameObject);
                }
            }
            foreach (Light light in LightningBolt.lightCache)
            {
                if (light != null)
                {
                    UnityEngine.Object.Destroy(light.gameObject);
                }
            }
            LightningBolt.lineRendererCache.Clear();
            LightningBolt.lightCache.Clear();
            List<LightningBoltSegmentGroup> obj = LightningBolt.groupCache;
            lock (obj)
            {
                LightningBolt.groupCache.Clear();
            }
        }

        private void CleanupLight(Light l)
        {
            if (l != null)
            {
                this.dependencies.LightRemoved(l);
                LightningBolt.lightCache.Add(l);
                l.gameObject.SetActive(false);
                LightningBolt.lightCount--;
            }
        }

        private void EnableLineRenderer(LightningBolt.LineRendererMesh lineRenderer, int tag)
        {
            if (lineRenderer != null && lineRenderer.GameObject != null && lineRenderer.Tag == tag && this.IsActive)
            {
                lineRenderer.PopulateMesh();
            }
        }

        private IEnumerator EnableLastRendererCoRoutine()
        {
            return new LightningBolt._003CEnableLastRendererCoRoutine_003Ed__39(0)
            {
                _003C_003E4__this = this
            };
        }

        private LightningBolt.LineRendererMesh GetOrCreateLineRenderer()
        {
            LightningBolt.LineRendererMesh lineRendererMesh;
            while (LightningBolt.lineRendererCache.Count != 0)
            {
                int index = LightningBolt.lineRendererCache.Count - 1;
                lineRendererMesh = LightningBolt.lineRendererCache[index];
                LightningBolt.lineRendererCache.RemoveAt(index);
                if (lineRendererMesh != null && !(lineRendererMesh.Transform == null))
                {
                    lineRendererMesh.Transform.parent = null;
                    lineRendererMesh.Transform.rotation = Quaternion.identity;
                    lineRendererMesh.Transform.localScale = Vector3.one;
                    lineRendererMesh.Transform.parent = this.dependencies.Parent.transform;
                    lineRendererMesh.GameObject.layer = this.dependencies.Parent.layer;
                    if (this.dependencies.UseWorldSpace)
                    {
                        lineRendererMesh.GameObject.transform.position = Vector3.zero;
                    }
                    else
                    {
                        lineRendererMesh.GameObject.transform.localPosition = Vector3.zero;
                    }
                    lineRendererMesh.MaterialGlow = this.dependencies.LightningMaterialMesh;
                    lineRendererMesh.MaterialBolt = this.dependencies.LightningMaterialMeshNoGlow;
                    if (!string.IsNullOrEmpty(this.dependencies.SortLayerName))
                    {
                        lineRendererMesh.MeshRendererGlow.sortingLayerName = (lineRendererMesh.MeshRendererBolt.sortingLayerName = this.dependencies.SortLayerName);
                        lineRendererMesh.MeshRendererGlow.sortingOrder = (lineRendererMesh.MeshRendererBolt.sortingOrder = this.dependencies.SortOrderInLayer);
                    }
                    else
                    {
                        lineRendererMesh.MeshRendererGlow.sortingLayerName = (lineRendererMesh.MeshRendererBolt.sortingLayerName = null);
                        lineRendererMesh.MeshRendererGlow.sortingOrder = (lineRendererMesh.MeshRendererBolt.sortingOrder = 0);
                    }
                    this.activeLineRenderers.Add(lineRendererMesh);
                    return lineRendererMesh;
                }
            }
            lineRendererMesh = new LightningBolt.LineRendererMesh();
            lineRendererMesh.Transform.parent = null;
            lineRendererMesh.Transform.rotation = Quaternion.identity;
            lineRendererMesh.Transform.localScale = Vector3.one;
            lineRendererMesh.Transform.parent = this.dependencies.Parent.transform;
            lineRendererMesh.GameObject.layer = this.dependencies.Parent.layer;
            if (this.dependencies.UseWorldSpace)
            {
                lineRendererMesh.GameObject.transform.position = Vector3.zero;
            }
            else
            {
                lineRendererMesh.GameObject.transform.localPosition = Vector3.zero;
            }
            lineRendererMesh.MaterialGlow = this.dependencies.LightningMaterialMesh;
            lineRendererMesh.MaterialBolt = this.dependencies.LightningMaterialMeshNoGlow;
            if (!string.IsNullOrEmpty(this.dependencies.SortLayerName))
            {
                lineRendererMesh.MeshRendererGlow.sortingLayerName = (lineRendererMesh.MeshRendererBolt.sortingLayerName = this.dependencies.SortLayerName);
                lineRendererMesh.MeshRendererGlow.sortingOrder = (lineRendererMesh.MeshRendererBolt.sortingOrder = this.dependencies.SortOrderInLayer);
            }
            else
            {
                lineRendererMesh.MeshRendererGlow.sortingLayerName = (lineRendererMesh.MeshRendererBolt.sortingLayerName = null);
                lineRendererMesh.MeshRendererGlow.sortingOrder = (lineRendererMesh.MeshRendererBolt.sortingOrder = 0);
            }
            this.activeLineRenderers.Add(lineRendererMesh);
            return lineRendererMesh;
        }

        private void RenderGroup(LightningBoltSegmentGroup group, LightningBoltParameters p)
        {
            LightningBolt._003C_003Ec__DisplayClass41_0 _003C_003Ec__DisplayClass41_ = new LightningBolt._003C_003Ec__DisplayClass41_0();
            _003C_003Ec__DisplayClass41_._003C_003E4__this = this;
            if (group.SegmentCount == 0)
            {
                return;
            }
            float num = (this.dependencies.ThreadState == null) ? 0f : ((float)(DateTime.UtcNow - this.startTimeOffset).TotalSeconds);
            float num2 = this.timeSinceLevelLoad + group.Delay + num;
            Vector4 fadeLifeTime = new Vector4(num2, num2 + group.PeakStart, num2 + group.PeakEnd, num2 + group.LifeTime);
            float num3 = group.LineWidth * 0.5f * LightningBoltParameters.Scale;
            int num4 = group.Segments.Count - group.StartIndex;
            float num5 = (num3 - num3 * group.EndWidthMultiplier) / (float)num4;
            float num6;
            if (p.GrowthMultiplier > 0f)
            {
                num6 = group.LifeTime / (float)num4 * p.GrowthMultiplier;
                num = 0f;
            }
            else
            {
                num6 = 0f;
                num = 0f;
            }
            _003C_003Ec__DisplayClass41_.currentLineRenderer = ((this.activeLineRenderers.Count == 0) ? this.GetOrCreateLineRenderer() : this.activeLineRenderers[this.activeLineRenderers.Count - 1]);
            if (!_003C_003Ec__DisplayClass41_.currentLineRenderer.PrepareForLines(num4))
            {
                if (_003C_003Ec__DisplayClass41_.currentLineRenderer.CustomTransform != null)
                {
                    return;
                }
                if (this.dependencies.ThreadState != null)
                {
                    this.dependencies.ThreadState.AddActionForMainThread(new Action(_003C_003Ec__DisplayClass41_._003CRenderGroup_003Eb__0), true);
                }
                else
                {
                    this.EnableCurrentLineRenderer();
                    _003C_003Ec__DisplayClass41_.currentLineRenderer = this.GetOrCreateLineRenderer();
                }
            }
            _003C_003Ec__DisplayClass41_.currentLineRenderer.BeginLine(group.Segments[group.StartIndex].Start, group.Segments[group.StartIndex].End, num3, group.Color, p.Intensity, fadeLifeTime, p.GlowWidthMultiplier, p.GlowIntensity);
            for (int i = group.StartIndex + 1; i < group.Segments.Count; i++)
            {
                num3 -= num5;
                if (p.GrowthMultiplier < 1f)
                {
                    num += num6;
                    fadeLifeTime = new Vector4(num2 + num, num2 + group.PeakStart + num, num2 + group.PeakEnd, num2 + group.LifeTime);
                }
                _003C_003Ec__DisplayClass41_.currentLineRenderer.AppendLine(group.Segments[i].Start, group.Segments[i].End, num3, group.Color, p.Intensity, fadeLifeTime, p.GlowWidthMultiplier, p.GlowIntensity);
            }
        }

        private static IEnumerator NotifyBolt(LightningBoltDependencies dependencies, LightningBoltParameters p, Transform transform, Vector3 start, Vector3 end)
        {
            return new LightningBolt._003CNotifyBolt_003Ed__42(0)
            {
                dependencies = dependencies,
                p = p,
                transform = transform,
                start = start,
                end = end
            };
        }

        private void ProcessParameters(LightningBoltParameters p, RangeOfFloats delay, LightningBoltDependencies depends)
        {
            this.MinimumDelay = Mathf.Min(delay.Minimum, this.MinimumDelay);
            p.delaySeconds = delay.Random(p.Random);
            if (depends.LevelOfDetailDistance > Mathf.Epsilon)
            {
                float num;
                if (p.Points.Count > 1)
                {
                    num = Vector3.Distance(depends.CameraPos, p.Points[0]);
                    num = Mathf.Min(new float[]
                    {
                        Vector3.Distance(depends.CameraPos, p.Points[p.Points.Count - 1])
                    });
                }
                else
                {
                    num = Vector3.Distance(depends.CameraPos, p.Start);
                    num = Mathf.Min(new float[]
                    {
                        Vector3.Distance(depends.CameraPos, p.End)
                    });
                }
                int num2 = Mathf.Min(8, (int)(num / depends.LevelOfDetailDistance));
                p.Generations = Mathf.Max(1, p.Generations - num2);
                p.GenerationWhereForksStopSubtractor = Mathf.Clamp(p.GenerationWhereForksStopSubtractor - num2, 0, 8);
            }
            p.generationWhereForksStop = p.Generations - p.GenerationWhereForksStopSubtractor;
            this.lifeTime = Mathf.Max(p.LifeTime + p.delaySeconds, this.lifeTime);
            this.maxLifeTime = Mathf.Max(this.lifeTime, this.maxLifeTime);
            p.forkednessCalculated = (int)Mathf.Ceil(p.Forkedness * (float)p.Generations);
            if (p.Generations > 0)
            {
                p.Generator = (p.Generator ?? LightningGenerator.GeneratorInstance);
                Vector3 start;
                Vector3 end;
                p.Generator.GenerateLightningBolt(this, p, out start, out end);
                p.Start = start;
                p.End = end;
            }
        }

        private void ProcessAllLightningParameters()
        {
            LightningBolt._003C_003Ec__DisplayClass44_0 _003C_003Ec__DisplayClass44_ = new LightningBolt._003C_003Ec__DisplayClass44_0();
            int maxLights = LightningBolt.MaximumLightsPerBatch / this.dependencies.Parameters.Count;
            RangeOfFloats delay = default(RangeOfFloats);
            List<int> list = new List<int>(this.dependencies.Parameters.Count + 1);
            int num = 0;
            foreach (LightningBoltParameters lightningBoltParameters in this.dependencies.Parameters)
            {
                delay.Minimum = lightningBoltParameters.DelayRange.Minimum + lightningBoltParameters.Delay;
                delay.Maximum = lightningBoltParameters.DelayRange.Maximum + lightningBoltParameters.Delay;
                lightningBoltParameters.maxLights = maxLights;
                list.Add(this.segmentGroups.Count);
                this.ProcessParameters(lightningBoltParameters, delay, this.dependencies);
            }
            list.Add(this.segmentGroups.Count);
            _003C_003Ec__DisplayClass44_.dependenciesRef = this.dependencies;
            using (IEnumerator<LightningBoltParameters> enumerator = _003C_003Ec__DisplayClass44_.dependenciesRef.Parameters.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    LightningBolt._003C_003Ec__DisplayClass44_1 _003C_003Ec__DisplayClass44_2 = new LightningBolt._003C_003Ec__DisplayClass44_1();
                    _003C_003Ec__DisplayClass44_2.CS_0024_003C_003E8__locals1 = _003C_003Ec__DisplayClass44_;
                    _003C_003Ec__DisplayClass44_2.parameters = enumerator.Current;
                    LightningBolt._003C_003Ec__DisplayClass44_2 _003C_003Ec__DisplayClass44_3 = new LightningBolt._003C_003Ec__DisplayClass44_2();
                    _003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2 = _003C_003Ec__DisplayClass44_2;
                    _003C_003Ec__DisplayClass44_3.transform = this.RenderLightningBolt(_003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2.parameters.quality, _003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2.parameters.Generations, list[num], list[++num], _003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2.parameters);
                    if (_003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.dependenciesRef.ThreadState != null)
                    {
                        _003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.dependenciesRef.ThreadState.AddActionForMainThread(new Action(_003C_003Ec__DisplayClass44_3._003CProcessAllLightningParameters_003Eb__0), false);
                    }
                    else
                    {
                        _003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.dependenciesRef.StartCoroutine(LightningBolt.NotifyBolt(_003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.dependenciesRef, _003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2.parameters, _003C_003Ec__DisplayClass44_3.transform, _003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2.parameters.Start, _003C_003Ec__DisplayClass44_3.CS_0024_003C_003E8__locals2.parameters.End));
                    }
                }
            }
            if (this.dependencies.ThreadState != null)
            {
                this.dependencies.ThreadState.AddActionForMainThread(new Action(this.EnableCurrentLineRendererFromThread), false);
                return;
            }
            this.EnableCurrentLineRenderer();
            this.dependencies.AddActiveBolt(this);
        }

        private void EnableCurrentLineRendererFromThread()
        {
            this.EnableCurrentLineRenderer();
            this.dependencies.ThreadState = null;
            this.dependencies.AddActiveBolt(this);
        }

        private void EnableCurrentLineRenderer()
        {
            if (this.activeLineRenderers.Count == 0)
            {
                return;
            }
            if (this.MinimumDelay <= 0f)
            {
                this.EnableLineRenderer(this.activeLineRenderers[this.activeLineRenderers.Count - 1], this.activeLineRenderers[this.activeLineRenderers.Count - 1].Tag);
                return;
            }
            this.dependencies.StartCoroutine(this.EnableLastRendererCoRoutine());
        }

        private void RenderParticleSystems(Vector3 start, Vector3 end, float trunkWidth, float lifeTime, float delaySeconds)
        {
            if (trunkWidth > 0f)
            {
                if (this.dependencies.OriginParticleSystem != null)
                {
                    this.dependencies.StartCoroutine(this.GenerateParticleCoRoutine(this.dependencies.OriginParticleSystem, start, delaySeconds));
                }
                if (this.dependencies.DestParticleSystem != null)
                {
                    this.dependencies.StartCoroutine(this.GenerateParticleCoRoutine(this.dependencies.DestParticleSystem, end, delaySeconds + lifeTime * 0.8f));
                }
            }
        }

        private Transform RenderLightningBolt(LightningBoltQualitySetting quality, int generations, int startGroupIndex, int endGroupIndex, LightningBoltParameters parameters)
        {
            LightningBolt._003C_003Ec__DisplayClass48_0 _003C_003Ec__DisplayClass48_ = new LightningBolt._003C_003Ec__DisplayClass48_0();
            _003C_003Ec__DisplayClass48_._003C_003E4__this = this;
            _003C_003Ec__DisplayClass48_.parameters = parameters;
            _003C_003Ec__DisplayClass48_.startGroupIndex = startGroupIndex;
            _003C_003Ec__DisplayClass48_.quality = quality;
            if (this.segmentGroups.Count == 0 || _003C_003Ec__DisplayClass48_.startGroupIndex >= this.segmentGroups.Count || endGroupIndex > this.segmentGroups.Count)
            {
                return null;
            }
            Transform result = null;
            _003C_003Ec__DisplayClass48_.lp = _003C_003Ec__DisplayClass48_.parameters.LightParameters;
            if (_003C_003Ec__DisplayClass48_.lp != null)
            {
                if (this.hasLight |= _003C_003Ec__DisplayClass48_.lp.HasLight)
                {
                    _003C_003Ec__DisplayClass48_.lp.LightPercent = Mathf.Clamp(_003C_003Ec__DisplayClass48_.lp.LightPercent, Mathf.Epsilon, 1f);
                    _003C_003Ec__DisplayClass48_.lp.LightShadowPercent = Mathf.Clamp(_003C_003Ec__DisplayClass48_.lp.LightShadowPercent, 0f, 1f);
                }
                else
                {
                    _003C_003Ec__DisplayClass48_.lp = null;
                }
            }
            LightningBoltSegmentGroup lightningBoltSegmentGroup = this.segmentGroups[_003C_003Ec__DisplayClass48_.startGroupIndex];
            _003C_003Ec__DisplayClass48_.start = lightningBoltSegmentGroup.Segments[lightningBoltSegmentGroup.StartIndex].Start;
            _003C_003Ec__DisplayClass48_.end = lightningBoltSegmentGroup.Segments[lightningBoltSegmentGroup.StartIndex + lightningBoltSegmentGroup.SegmentCount - 1].End;
            _003C_003Ec__DisplayClass48_.parameters.FadePercent = Mathf.Clamp(_003C_003Ec__DisplayClass48_.parameters.FadePercent, 0f, 0.5f);
            if (_003C_003Ec__DisplayClass48_.parameters.CustomTransform != null)
            {
                LightningBolt._003C_003Ec__DisplayClass48_1 _003C_003Ec__DisplayClass48_2 = new LightningBolt._003C_003Ec__DisplayClass48_1();
                _003C_003Ec__DisplayClass48_2.CS_0024_003C_003E8__locals1 = _003C_003Ec__DisplayClass48_;
                _003C_003Ec__DisplayClass48_2.currentLineRenderer = ((this.activeLineRenderers.Count == 0 || !this.activeLineRenderers[this.activeLineRenderers.Count - 1].Empty) ? null : this.activeLineRenderers[this.activeLineRenderers.Count - 1]);
                if (_003C_003Ec__DisplayClass48_2.currentLineRenderer == null)
                {
                    if (this.dependencies.ThreadState != null)
                    {
                        this.dependencies.ThreadState.AddActionForMainThread(new Action(_003C_003Ec__DisplayClass48_2._003CRenderLightningBolt_003Eb__1), true);
                    }
                    else
                    {
                        this.EnableCurrentLineRenderer();
                        _003C_003Ec__DisplayClass48_2.currentLineRenderer = this.GetOrCreateLineRenderer();
                    }
                }
                if (_003C_003Ec__DisplayClass48_2.currentLineRenderer == null)
                {
                    return null;
                }
                _003C_003Ec__DisplayClass48_2.currentLineRenderer.CustomTransform = _003C_003Ec__DisplayClass48_2.CS_0024_003C_003E8__locals1.parameters.CustomTransform;
                result = _003C_003Ec__DisplayClass48_2.currentLineRenderer.Transform;
            }
            for (int i = _003C_003Ec__DisplayClass48_.startGroupIndex; i < endGroupIndex; i++)
            {
                LightningBoltSegmentGroup lightningBoltSegmentGroup2 = this.segmentGroups[i];
                lightningBoltSegmentGroup2.Delay = _003C_003Ec__DisplayClass48_.parameters.delaySeconds;
                lightningBoltSegmentGroup2.LifeTime = _003C_003Ec__DisplayClass48_.parameters.LifeTime;
                lightningBoltSegmentGroup2.PeakStart = lightningBoltSegmentGroup2.LifeTime * _003C_003Ec__DisplayClass48_.parameters.FadePercent;
                lightningBoltSegmentGroup2.PeakEnd = lightningBoltSegmentGroup2.LifeTime - lightningBoltSegmentGroup2.PeakStart;
                float num = lightningBoltSegmentGroup2.PeakEnd - lightningBoltSegmentGroup2.PeakStart;
                float num2 = lightningBoltSegmentGroup2.LifeTime - lightningBoltSegmentGroup2.PeakEnd;
                lightningBoltSegmentGroup2.PeakStart *= _003C_003Ec__DisplayClass48_.parameters.FadeInMultiplier;
                lightningBoltSegmentGroup2.PeakEnd = lightningBoltSegmentGroup2.PeakStart + num * _003C_003Ec__DisplayClass48_.parameters.FadeFullyLitMultiplier;
                lightningBoltSegmentGroup2.LifeTime = lightningBoltSegmentGroup2.PeakEnd + num2 * _003C_003Ec__DisplayClass48_.parameters.FadeOutMultiplier;
                lightningBoltSegmentGroup2.LightParameters = _003C_003Ec__DisplayClass48_.lp;
                this.RenderGroup(lightningBoltSegmentGroup2, _003C_003Ec__DisplayClass48_.parameters);
            }
            if (this.dependencies.ThreadState != null)
            {
                this.dependencies.ThreadState.AddActionForMainThread(new Action(_003C_003Ec__DisplayClass48_._003CRenderLightningBolt_003Eb__0), false);
            }
            else
            {
                this.RenderParticleSystems(_003C_003Ec__DisplayClass48_.start, _003C_003Ec__DisplayClass48_.end, _003C_003Ec__DisplayClass48_.parameters.TrunkWidth, _003C_003Ec__DisplayClass48_.parameters.LifeTime, _003C_003Ec__DisplayClass48_.parameters.delaySeconds);
                if (_003C_003Ec__DisplayClass48_.lp != null)
                {
                    this.CreateLightsForGroup(this.segmentGroups[_003C_003Ec__DisplayClass48_.startGroupIndex], _003C_003Ec__DisplayClass48_.lp, _003C_003Ec__DisplayClass48_.quality, _003C_003Ec__DisplayClass48_.parameters.maxLights);
                }
            }
            return result;
        }

        private void CreateLightsForGroup(LightningBoltSegmentGroup group, LightningLightParameters lp, LightningBoltQualitySetting quality, int maxLights)
        {
            if (LightningBolt.lightCount == LightningBolt.MaximumLightCount || maxLights <= 0)
            {
                return;
            }
            float num = (this.lifeTime - group.PeakEnd) * lp.FadeOutMultiplier;
            float num2 = (group.PeakEnd - group.PeakStart) * lp.FadeFullyLitMultiplier;
            float num3 = group.PeakStart * lp.FadeInMultiplier + num2 + num;
            this.maxLifeTime = Mathf.Max(this.maxLifeTime, group.Delay + num3);
            this.segmentGroupsWithLight.Add(group);
            int segmentCount = group.SegmentCount;
            float num4;
            float num5;
            if (quality == LightningBoltQualitySetting.LimitToQualitySetting)
            {
                int qualityLevel = QualitySettings.GetQualityLevel();
                LightningQualityMaximum lightningQualityMaximum;
                if (LightningBoltParameters.QualityMaximums.TryGetValue(qualityLevel, out lightningQualityMaximum))
                {
                    num4 = Mathf.Min(lp.LightPercent, lightningQualityMaximum.MaximumLightPercent);
                    num5 = Mathf.Min(lp.LightShadowPercent, lightningQualityMaximum.MaximumShadowPercent);
                }
                else
                {
                    UnityEngine.Debug.LogError("Unable to read lightning quality for level " + qualityLevel.ToString());
                    num4 = lp.LightPercent;
                    num5 = lp.LightShadowPercent;
                }
            }
            else
            {
                num4 = lp.LightPercent;
                num5 = lp.LightShadowPercent;
            }
            maxLights = Mathf.Max(1, Mathf.Min(maxLights, (int)((float)segmentCount * num4)));
            int num6 = Mathf.Max(1, segmentCount / maxLights);
            int num7 = maxLights - (int)((float)maxLights * num5);
            int num8 = num7;
            for (int i = group.StartIndex + (int)((float)num6 * 0.5f); i < group.Segments.Count; i += num6)
            {
                if (this.AddLightToGroup(group, lp, i, num6, num7, ref maxLights, ref num8))
                {
                    return;
                }
            }
        }

        private bool AddLightToGroup(LightningBoltSegmentGroup group, LightningLightParameters lp, int segmentIndex, int nthLight, int nthShadows, ref int maxLights, ref int nthShadowCounter)
        {
            Light orCreateLight = this.GetOrCreateLight(lp);
            group.Lights.Add(orCreateLight);
            Vector3 vector = (group.Segments[segmentIndex].Start + group.Segments[segmentIndex].End) * 0.5f;
            if (this.dependencies.CameraIsOrthographic)
            {
                if (this.dependencies.CameraMode == CameraMode.OrthographicXZ)
                {
                    vector.y = this.dependencies.CameraPos.y + lp.OrthographicOffset;
                }
                else
                {
                    vector.z = this.dependencies.CameraPos.z + lp.OrthographicOffset;
                }
            }
            if (this.dependencies.UseWorldSpace)
            {
                orCreateLight.gameObject.transform.position = vector;
            }
            else
            {
                orCreateLight.gameObject.transform.localPosition = vector;
            }
            if (lp.LightShadowPercent != 0f)
            {
                int num = nthShadowCounter + 1;
                nthShadowCounter = num;
                if (num >= nthShadows)
                {
                    orCreateLight.shadows = LightShadows.Soft;
                    nthShadowCounter = 0;
                    goto IL_100;
                }
            }
            orCreateLight.shadows = LightShadows.None;
            IL_100:
            if (++LightningBolt.lightCount != LightningBolt.MaximumLightCount)
            {
                int num = maxLights - 1;
                maxLights = num;
                return num == 0;
            }
            return true;
        }

        private Light GetOrCreateLight(LightningLightParameters lp)
        {
            Light light;
            while (LightningBolt.lightCache.Count != 0)
            {
                light = LightningBolt.lightCache[LightningBolt.lightCache.Count - 1];
                LightningBolt.lightCache.RemoveAt(LightningBolt.lightCache.Count - 1);
                if (!(light == null))
                {
                    light.bounceIntensity = lp.BounceIntensity;
                    light.shadowNormalBias = lp.ShadowNormalBias;
                    light.color = lp.LightColor;
                    light.renderMode = lp.RenderMode;
                    light.range = lp.LightRange;
                    light.shadowStrength = lp.ShadowStrength;
                    light.shadowBias = lp.ShadowBias;
                    light.intensity = 0f;
                    light.gameObject.transform.parent = this.dependencies.Parent.transform;
                    light.gameObject.SetActive(true);
                    this.dependencies.LightAdded(light);
                    return light;
                }
            }
            light = new GameObject("LightningBoltLight").AddComponent<Light>();
            light.type = LightType.Point;
            light.bounceIntensity = lp.BounceIntensity;
            light.shadowNormalBias = lp.ShadowNormalBias;
            light.color = lp.LightColor;
            light.renderMode = lp.RenderMode;
            light.range = lp.LightRange;
            light.shadowStrength = lp.ShadowStrength;
            light.shadowBias = lp.ShadowBias;
            light.intensity = 0f;
            light.gameObject.transform.parent = this.dependencies.Parent.transform;
            light.gameObject.SetActive(true);
            this.dependencies.LightAdded(light);
            return light;
        }

        private void UpdateLight(LightningLightParameters lp, IEnumerable<Light> lights, float delay, float peakStart, float peakEnd, float lifeTime)
        {
            if (this.elapsedTime < delay)
            {
                return;
            }
            float num = (lifeTime - peakEnd) * lp.FadeOutMultiplier;
            float num2 = (peakEnd - peakStart) * lp.FadeFullyLitMultiplier;
            peakStart *= lp.FadeInMultiplier;
            peakEnd = peakStart + num2;
            lifeTime = peakEnd + num;
            float num3 = this.elapsedTime - delay;
            if (num3 >= peakStart)
            {
                if (num3 <= peakEnd)
                {
                    using (IEnumerator<Light> enumerator = lights.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Light light = enumerator.Current;
                            light.intensity = lp.LightIntensity;
                        }
                        return;
                    }
                }
                float t = (num3 - peakEnd) / (lifeTime - peakEnd);
                using (IEnumerator<Light> enumerator = lights.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Light light2 = enumerator.Current;
                        light2.intensity = Mathf.Lerp(lp.LightIntensity, 0f, t);
                    }
                    return;
                }
            }
            float t2 = num3 / peakStart;
            foreach (Light light3 in lights)
            {
                light3.intensity = Mathf.Lerp(0f, lp.LightIntensity, t2);
            }
        }

        private void UpdateLights()
        {
            foreach (LightningBoltSegmentGroup lightningBoltSegmentGroup in this.segmentGroupsWithLight)
            {
                this.UpdateLight(lightningBoltSegmentGroup.LightParameters, lightningBoltSegmentGroup.Lights, lightningBoltSegmentGroup.Delay, lightningBoltSegmentGroup.PeakStart, lightningBoltSegmentGroup.PeakEnd, lightningBoltSegmentGroup.LifeTime);
            }
        }

        private IEnumerator GenerateParticleCoRoutine(ParticleSystem p, Vector3 pos, float delay)
        {
            return new LightningBolt._003CGenerateParticleCoRoutine_003Ed__54(0)
            {
                p = p,
                pos = pos,
                delay = delay
            };
        }

        private void CheckForGlow(IEnumerable<LightningBoltParameters> parameters)
        {
            foreach (LightningBoltParameters lightningBoltParameters in parameters)
            {
                this.HasGlow = (lightningBoltParameters.GlowIntensity >= Mathf.Epsilon && lightningBoltParameters.GlowWidthMultiplier >= Mathf.Epsilon);
                if (this.HasGlow)
                {
                    break;
                }
            }
        }

        public static int MaximumLightCount = 128;

        public static int MaximumLightsPerBatch = 8;

        private float _003CMinimumDelay_003Ek__BackingField;

        private bool _003CHasGlow_003Ek__BackingField;

        private CameraMode _003CCameraMode_003Ek__BackingField;

        private DateTime startTimeOffset;

        private LightningBoltDependencies dependencies;

        private float elapsedTime;

        private float lifeTime;

        private float maxLifeTime;

        private bool hasLight;

        private float timeSinceLevelLoad;

        private readonly List<LightningBoltSegmentGroup> segmentGroups = new List<LightningBoltSegmentGroup>();

        private readonly List<LightningBoltSegmentGroup> segmentGroupsWithLight = new List<LightningBoltSegmentGroup>();

        private readonly List<LightningBolt.LineRendererMesh> activeLineRenderers = new List<LightningBolt.LineRendererMesh>();

        private static int lightCount;

        private static readonly List<LightningBolt.LineRendererMesh> lineRendererCache = new List<LightningBolt.LineRendererMesh>();

        private static readonly List<LightningBoltSegmentGroup> groupCache = new List<LightningBoltSegmentGroup>();

        private static readonly List<Light> lightCache = new List<Light>();

        public class LineRendererMesh
        {
            public GameObject GameObject
            {
                get
                {
                    return this._003CGameObject_003Ek__BackingField;
                }
                private set
                {
                    this._003CGameObject_003Ek__BackingField = value;
                }
            }

            public Material MaterialGlow
            {
                set
                {
                    this.meshRendererGlow.sharedMaterial = value;
                }
            }

            public Material MaterialBolt
            {
                set
                {
                    this.meshRendererBolt.sharedMaterial = value;
                }
            }

            public MeshRenderer MeshRendererGlow
            {
                get
                {
                    return this.meshRendererGlow;
                }
            }

            public MeshRenderer MeshRendererBolt
            {
                get
                {
                    return this.meshRendererBolt;
                }
            }

            public int Tag
            {
                get
                {
                    return this._003CTag_003Ek__BackingField;
                }
                set
                {
                    this._003CTag_003Ek__BackingField = value;
                }
            }

            public Action<LightningCustomTransformStateInfo> CustomTransform
            {
                get
                {
                    return this._003CCustomTransform_003Ek__BackingField;
                }
                set
                {
                    this._003CCustomTransform_003Ek__BackingField = value;
                }
            }

            public Transform Transform
            {
                get
                {
                    return this._003CTransform_003Ek__BackingField;
                }
                private set
                {
                    this._003CTransform_003Ek__BackingField = value;
                }
            }

            public bool Empty
            {
                get
                {
                    return this.vertices.Count == 0;
                }
            }

            public LineRendererMesh()
            {
                this.GameObject = new GameObject("LightningBoltMeshRenderer");
                this.GameObject.SetActive(false);
                this.mesh = new Mesh
                {
                    name = "ProceduralLightningMesh"
                };
                this.mesh.MarkDynamic();
                GameObject gameObject = new GameObject("LightningBoltMeshRendererGlow");
                gameObject.transform.parent = this.GameObject.transform;
                GameObject gameObject2 = new GameObject("LightningBoltMeshRendererBolt");
                gameObject2.transform.parent = this.GameObject.transform;
                this.meshFilterGlow = gameObject.AddComponent<MeshFilter>();
                this.meshFilterBolt = gameObject2.AddComponent<MeshFilter>();
                this.meshFilterGlow.sharedMesh = (this.meshFilterBolt.sharedMesh = this.mesh);
                this.meshRendererGlow = gameObject.AddComponent<MeshRenderer>();
                this.meshRendererBolt = gameObject2.AddComponent<MeshRenderer>();
                this.meshRendererGlow.shadowCastingMode = (this.meshRendererBolt.shadowCastingMode = ShadowCastingMode.Off);
                this.meshRendererGlow.reflectionProbeUsage = (this.meshRendererBolt.reflectionProbeUsage = ReflectionProbeUsage.Off);
                this.meshRendererGlow.lightProbeUsage = (this.meshRendererBolt.lightProbeUsage = LightProbeUsage.Off);
                this.meshRendererGlow.receiveShadows = (this.meshRendererBolt.receiveShadows = false);
                this.Transform = this.GameObject.GetComponent<Transform>();
            }

            public void PopulateMesh()
            {
                if (this.vertices.Count == 0)
                {
                    this.mesh.Clear();
                    return;
                }
                this.PopulateMeshInternal();
            }

            public bool PrepareForLines(int lineCount)
            {
                int num = lineCount * 4;
                return this.vertices.Count + num <= 64999;
            }

            public void BeginLine(Vector3 start, Vector3 end, float radius, Color32 color, float colorIntensity, Vector4 fadeLifeTime, float glowWidthModifier, float glowIntensity)
            {
                Vector4 vector = end - start;
                vector.w = radius;
                this.AppendLineInternal(ref start, ref end, ref vector, ref vector, ref vector, color, colorIntensity, ref fadeLifeTime, glowWidthModifier, glowIntensity);
            }

            public void AppendLine(Vector3 start, Vector3 end, float radius, Color32 color, float colorIntensity, Vector4 fadeLifeTime, float glowWidthModifier, float glowIntensity)
            {
                Vector4 vector = end - start;
                vector.w = radius;
                Vector4 vector2 = this.lineDirs[this.lineDirs.Count - 3];
                Vector4 vector3 = this.lineDirs[this.lineDirs.Count - 1];
                this.AppendLineInternal(ref start, ref end, ref vector, ref vector2, ref vector3, color, colorIntensity, ref fadeLifeTime, glowWidthModifier, glowIntensity);
            }

            public void Reset()
            {
                this.CustomTransform = null;
                int tag = this.Tag;
                this.Tag = tag + 1;
                this.GameObject.SetActive(false);
                this.mesh.Clear();
                this.indices.Clear();
                this.vertices.Clear();
                this.colors.Clear();
                this.lineDirs.Clear();
                this.ends.Clear();
                this.texCoordsAndGlowModifiers.Clear();
                this.fadeLifetimes.Clear();
                this.currentBoundsMaxX = (this.currentBoundsMaxY = (this.currentBoundsMaxZ = -1147483648));
                this.currentBoundsMinX = (this.currentBoundsMinY = (this.currentBoundsMinZ = 1147483647));
            }

            private void PopulateMeshInternal()
            {
                this.GameObject.SetActive(true);
                this.mesh.SetVertices(this.vertices);
                this.mesh.SetTangents(this.lineDirs);
                this.mesh.SetColors(this.colors);
                this.mesh.SetUVs(0, this.texCoordsAndGlowModifiers);
                this.mesh.SetUVs(1, this.fadeLifetimes);
                this.mesh.SetNormals(this.ends);
                this.mesh.SetTriangles(this.indices, 0);
                Bounds bounds = default(Bounds);
                Vector3 b = new Vector3((float)(this.currentBoundsMinX - 2), (float)(this.currentBoundsMinY - 2), (float)(this.currentBoundsMinZ - 2));
                Vector3 a = new Vector3((float)(this.currentBoundsMaxX + 2), (float)(this.currentBoundsMaxY + 2), (float)(this.currentBoundsMaxZ + 2));
                bounds.center = (a + b) * 0.5f;
                bounds.size = (a - b) * 1.2f;
                this.mesh.bounds = bounds;
            }

            private void UpdateBounds(ref Vector3 point1, ref Vector3 point2)
            {
                int num = (int)point1.x - (int)point2.x;
                num &= num >> 31;
                int num2 = (int)point2.x + num;
                int num3 = (int)point1.x - num;
                num = this.currentBoundsMinX - num2;
                num &= num >> 31;
                this.currentBoundsMinX = num2 + num;
                num = this.currentBoundsMaxX - num3;
                num &= num >> 31;
                this.currentBoundsMaxX -= num;
                int num4 = (int)point1.y - (int)point2.y;
                num4 &= num4 >> 31;
                int num5 = (int)point2.y + num4;
                int num6 = (int)point1.y - num4;
                num4 = this.currentBoundsMinY - num5;
                num4 &= num4 >> 31;
                this.currentBoundsMinY = num5 + num4;
                num4 = this.currentBoundsMaxY - num6;
                num4 &= num4 >> 31;
                this.currentBoundsMaxY -= num4;
                int num7 = (int)point1.z - (int)point2.z;
                num7 &= num7 >> 31;
                int num8 = (int)point2.z + num7;
                int num9 = (int)point1.z - num7;
                num7 = this.currentBoundsMinZ - num8;
                num7 &= num7 >> 31;
                this.currentBoundsMinZ = num8 + num7;
                num7 = this.currentBoundsMaxZ - num9;
                num7 &= num7 >> 31;
                this.currentBoundsMaxZ -= num7;
            }

            private void AddIndices()
            {
                int count = this.vertices.Count;
                this.indices.Add(count++);
                this.indices.Add(count++);
                this.indices.Add(count);
                this.indices.Add(count--);
                this.indices.Add(count);
                this.indices.Add(count + 2);
            }

            private void AppendLineInternal(ref Vector3 start, ref Vector3 end, ref Vector4 dir, ref Vector4 dirPrev1, ref Vector4 dirPrev2, Color32 color, float colorIntensity, ref Vector4 fadeLifeTime, float glowWidthModifier, float glowIntensity)
            {
                this.AddIndices();
                color.a = (byte)Mathf.Lerp(0f, 255f, colorIntensity * 0.1f);
                Vector4 item = new Vector4(LightningBolt.LineRendererMesh.uv1.x, LightningBolt.LineRendererMesh.uv1.y, glowWidthModifier, glowIntensity);
                this.vertices.Add(start);
                this.lineDirs.Add(dirPrev1);
                this.colors.Add(color);
                this.ends.Add(dir);
                this.vertices.Add(end);
                this.lineDirs.Add(dir);
                this.colors.Add(color);
                this.ends.Add(dir);
                dir.w = -dir.w;
                this.vertices.Add(start);
                this.lineDirs.Add(dirPrev2);
                this.colors.Add(color);
                this.ends.Add(dir);
                this.vertices.Add(end);
                this.lineDirs.Add(dir);
                this.colors.Add(color);
                this.ends.Add(dir);
                this.texCoordsAndGlowModifiers.Add(item);
                item.x = LightningBolt.LineRendererMesh.uv2.x;
                item.y = LightningBolt.LineRendererMesh.uv2.y;
                this.texCoordsAndGlowModifiers.Add(item);
                item.x = LightningBolt.LineRendererMesh.uv3.x;
                item.y = LightningBolt.LineRendererMesh.uv3.y;
                this.texCoordsAndGlowModifiers.Add(item);
                item.x = LightningBolt.LineRendererMesh.uv4.x;
                item.y = LightningBolt.LineRendererMesh.uv4.y;
                this.texCoordsAndGlowModifiers.Add(item);
                this.fadeLifetimes.Add(fadeLifeTime);
                this.fadeLifetimes.Add(fadeLifeTime);
                this.fadeLifetimes.Add(fadeLifeTime);
                this.fadeLifetimes.Add(fadeLifeTime);
                this.UpdateBounds(ref start, ref end);
            }

            private GameObject _003CGameObject_003Ek__BackingField;

            private int _003CTag_003Ek__BackingField;

            private Action<LightningCustomTransformStateInfo> _003CCustomTransform_003Ek__BackingField;

            private Transform _003CTransform_003Ek__BackingField;

            private const int defaultListCapacity = 2048;

            private static readonly Vector2 uv1 = new Vector2(0f, 0f);

            private static readonly Vector2 uv2 = new Vector2(1f, 0f);

            private static readonly Vector2 uv3 = new Vector2(0f, 1f);

            private static readonly Vector2 uv4 = new Vector2(1f, 1f);

            private readonly List<int> indices = new List<int>(2048);

            private readonly List<Vector3> vertices = new List<Vector3>(2048);

            private readonly List<Vector4> lineDirs = new List<Vector4>(2048);

            private readonly List<Color32> colors = new List<Color32>(2048);

            private readonly List<Vector3> ends = new List<Vector3>(2048);

            private readonly List<Vector4> texCoordsAndGlowModifiers = new List<Vector4>(2048);

            private readonly List<Vector4> fadeLifetimes = new List<Vector4>(2048);

            private const int boundsPadder = 1000000000;

            private int currentBoundsMinX = 1147483647;

            private int currentBoundsMinY = 1147483647;

            private int currentBoundsMinZ = 1147483647;

            private int currentBoundsMaxX = -1147483648;

            private int currentBoundsMaxY = -1147483648;

            private int currentBoundsMaxZ = -1147483648;

            private Mesh mesh;

            private MeshFilter meshFilterGlow;

            private MeshFilter meshFilterBolt;

            private MeshRenderer meshRendererGlow;

            private MeshRenderer meshRendererBolt;
        }

        private sealed class _003CEnableLastRendererCoRoutine_003Ed__39 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CEnableLastRendererCoRoutine_003Ed__39(int _003C_003E1__state)
            {
                this._003C_003E1__state = _003C_003E1__state;
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
            }

            bool IEnumerator.MoveNext()
            {
                int num = this._003C_003E1__state;
                LightningBolt lightningBolt = this._003C_003E4__this;
                if (num == 0)
                {
                    this._003C_003E1__state = -1;
                    this._003ClineRenderer_003E5__2 = lightningBolt.activeLineRenderers[lightningBolt.activeLineRenderers.Count - 1];
                    LightningBolt.LineRendererMesh lineRendererMesh = this._003ClineRenderer_003E5__2;
                    int tag = lineRendererMesh.Tag + 1;
                    lineRendererMesh.Tag = tag;
                    this._003Ctag_003E5__3 = tag;
                    this._003C_003E2__current = new WaitForSecondsLightning(lightningBolt.MinimumDelay);
                    this._003C_003E1__state = 1;
                    return true;
                }
                if (num != 1)
                {
                    return false;
                }
                this._003C_003E1__state = -1;
                lightningBolt.EnableLineRenderer(this._003ClineRenderer_003E5__2, this._003Ctag_003E5__3);
                return false;
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            private int _003C_003E1__state;

            private object _003C_003E2__current;

            public LightningBolt _003C_003E4__this;

            private LightningBolt.LineRendererMesh _003ClineRenderer_003E5__2;

            private int _003Ctag_003E5__3;
        }

        private sealed class _003C_003Ec__DisplayClass41_0
        {
            internal void _003CRenderGroup_003Eb__0()
            {
                this._003C_003E4__this.EnableCurrentLineRenderer();
                this.currentLineRenderer = this._003C_003E4__this.GetOrCreateLineRenderer();
            }

            public LightningBolt _003C_003E4__this;

            public LightningBolt.LineRendererMesh currentLineRenderer;
        }

        private sealed class _003CNotifyBolt_003Ed__42 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CNotifyBolt_003Ed__42(int _003C_003E1__state)
            {
                this._003C_003E1__state = _003C_003E1__state;
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
            }

            bool IEnumerator.MoveNext()
            {
                switch (this._003C_003E1__state)
                {
                    case 0:
                        {
                            this._003C_003E1__state = -1;
                            float delaySeconds = this.p.delaySeconds;
                            this._003ClifeTime_003E5__2 = this.p.LifeTime;
                            this._003C_003E2__current = new WaitForSecondsLightning(delaySeconds);
                            this._003C_003E1__state = 1;
                            return true;
                        }
                    case 1:
                        this._003C_003E1__state = -1;
                        if (this.dependencies.LightningBoltStarted != null)
                        {
                            this.dependencies.LightningBoltStarted(this.p, this.start, this.end);
                        }
                        this._003Cstate_003E5__3 = ((this.p.CustomTransform == null) ? null : LightningCustomTransformStateInfo.GetOrCreateStateInfo());
                        if (this._003Cstate_003E5__3 != null)
                        {
                            this._003Cstate_003E5__3.Parameters = this.p;
                            this._003Cstate_003E5__3.BoltStartPosition = this.start;
                            this._003Cstate_003E5__3.BoltEndPosition = this.end;
                            this._003Cstate_003E5__3.State = LightningCustomTransformState.Started;
                            this._003Cstate_003E5__3.Transform = this.transform;
                            this.p.CustomTransform(this._003Cstate_003E5__3);
                            this._003Cstate_003E5__3.State = LightningCustomTransformState.Executing;
                        }
                        if (this.p.CustomTransform == null)
                        {
                            this._003C_003E2__current = new WaitForSecondsLightning(this._003ClifeTime_003E5__2);
                            this._003C_003E1__state = 2;
                            return true;
                        }
                        break;
                    case 2:
                        this._003C_003E1__state = -1;
                        goto IL_19F;
                    case 3:
                        this._003C_003E1__state = -1;
                        break;
                    default:
                        return false;
                }
                if (this._003ClifeTime_003E5__2 > 0f)
                {
                    this.p.CustomTransform(this._003Cstate_003E5__3);
                    this._003ClifeTime_003E5__2 -= LightningBoltScript.DeltaTime;
                    this._003C_003E2__current = null;
                    this._003C_003E1__state = 3;
                    return true;
                }
                IL_19F:
                if (this.p.CustomTransform != null)
                {
                    this._003Cstate_003E5__3.State = LightningCustomTransformState.Ended;
                    this.p.CustomTransform(this._003Cstate_003E5__3);
                    LightningCustomTransformStateInfo.ReturnStateInfoToCache(this._003Cstate_003E5__3);
                }
                if (this.dependencies.LightningBoltEnded != null)
                {
                    this.dependencies.LightningBoltEnded(this.p, this.start, this.end);
                }
                LightningBoltParameters.ReturnParametersToCache(this.p);
                return false;
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            private int _003C_003E1__state;

            private object _003C_003E2__current;

            public LightningBoltParameters p;

            public LightningBoltDependencies dependencies;

            public Vector3 start;

            public Vector3 end;

            public Transform transform;

            private float _003ClifeTime_003E5__2;

            private LightningCustomTransformStateInfo _003Cstate_003E5__3;
        }

        private sealed class _003C_003Ec__DisplayClass44_0
        {
            public LightningBoltDependencies dependenciesRef;
        }

        private sealed class _003C_003Ec__DisplayClass44_1
        {
            public LightningBoltParameters parameters;

            public LightningBolt._003C_003Ec__DisplayClass44_0 CS_0024_003C_003E8__locals1;
        }

        private sealed class _003C_003Ec__DisplayClass44_2
        {
            internal void _003CProcessAllLightningParameters_003Eb__0()
            {
                this.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.dependenciesRef.StartCoroutine(LightningBolt.NotifyBolt(this.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.dependenciesRef, this.CS_0024_003C_003E8__locals2.parameters, this.transform, this.CS_0024_003C_003E8__locals2.parameters.Start, this.CS_0024_003C_003E8__locals2.parameters.End));
            }

            public Transform transform;

            public LightningBolt._003C_003Ec__DisplayClass44_1 CS_0024_003C_003E8__locals2;
        }

        private sealed class _003C_003Ec__DisplayClass48_0
        {
            internal void _003CRenderLightningBolt_003Eb__0()
            {
                this._003C_003E4__this.RenderParticleSystems(this.start, this.end, this.parameters.TrunkWidth, this.parameters.LifeTime, this.parameters.delaySeconds);
                if (this.lp != null)
                {
                    this._003C_003E4__this.CreateLightsForGroup(this._003C_003E4__this.segmentGroups[this.startGroupIndex], this.lp, this.quality, this.parameters.maxLights);
                }
            }

            public LightningBolt _003C_003E4__this;

            public Vector3 start;

            public Vector3 end;

            public LightningBoltParameters parameters;

            public LightningLightParameters lp;

            public int startGroupIndex;

            public LightningBoltQualitySetting quality;
        }

        private sealed class _003C_003Ec__DisplayClass48_1
        {
            internal void _003CRenderLightningBolt_003Eb__1()
            {
                this.CS_0024_003C_003E8__locals1._003C_003E4__this.EnableCurrentLineRenderer();
                this.currentLineRenderer = this.CS_0024_003C_003E8__locals1._003C_003E4__this.GetOrCreateLineRenderer();
            }

            public LightningBolt.LineRendererMesh currentLineRenderer;

            public LightningBolt._003C_003Ec__DisplayClass48_0 CS_0024_003C_003E8__locals1;
        }

        private sealed class _003CGenerateParticleCoRoutine_003Ed__54 : IEnumerator<object>, IEnumerator, IDisposable
        {
            [DebuggerHidden]
            public _003CGenerateParticleCoRoutine_003Ed__54(int _003C_003E1__state)
            {
                this._003C_003E1__state = _003C_003E1__state;
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
            }

            bool IEnumerator.MoveNext()
            {
                int num = this._003C_003E1__state;
                if (num == 0)
                {
                    this._003C_003E1__state = -1;
                    this._003C_003E2__current = new WaitForSecondsLightning(this.delay);
                    this._003C_003E1__state = 1;
                    return true;
                }
                if (num != 1)
                {
                    return false;
                }
                this._003C_003E1__state = -1;
                this.p.transform.position = this.pos;
                if (this.p.emission.burstCount > 0)
                {
                    ParticleSystem.Burst[] array = new ParticleSystem.Burst[this.p.emission.burstCount];
                    this.p.emission.GetBursts(array);
                    int num2 = UnityEngine.Random.Range((int)array[0].minCount, (int)(array[0].maxCount + 1));
                    this.p.Emit(num2);
                }
                else
                {
                    ParticleSystem.MinMaxCurve rateOverTime = this.p.emission.rateOverTime;
                    int num2 = (int)((rateOverTime.constantMax - rateOverTime.constantMin) * 0.5f);
                    num2 = UnityEngine.Random.Range(num2, num2 * 2);
                    this.p.Emit(num2);
                }
                return false;
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this._003C_003E2__current;
                }
            }

            private int _003C_003E1__state;

            private object _003C_003E2__current;

            public float delay;

            public ParticleSystem p;

            public Vector3 pos;
        }
    }
}
