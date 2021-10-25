using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
    public class LightningGeneratorPath : LightningGenerator
    {
        public void GenerateLightningBoltPath(LightningBolt bolt, Vector3 start, Vector3 end, LightningBoltParameters p)
        {
            if (p.Points.Count < 2)
            {
                UnityEngine.Debug.LogError("Lightning path should have at least two points");
                return;
            }
            int generations = p.Generations;
            int totalGenerations = generations;
            float num = (generations == p.Generations) ? p.ChaosFactor : p.ChaosFactorForks;
            int num2 = p.SmoothingFactor - 1;
            LightningBoltSegmentGroup lightningBoltSegmentGroup = bolt.AddGroup();
            lightningBoltSegmentGroup.LineWidth = p.TrunkWidth;
            lightningBoltSegmentGroup.Generation = generations--;
            lightningBoltSegmentGroup.EndWidthMultiplier = p.EndWidthMultiplier;
            lightningBoltSegmentGroup.Color = p.Color;
            p.Start = p.Points[0] + start;
            p.End = p.Points[p.Points.Count - 1] + end;
            end = p.Start;
            for (int i = 1; i < p.Points.Count; i++)
            {
                start = end;
                end = p.Points[i];
                Vector3 a = end - start;
                float num3 = PathGenerator.SquareRoot(a.sqrMagnitude);
                if (num > 0f)
                {
                    if (bolt.CameraMode == CameraMode.Perspective)
                    {
                        end += num3 * num * base.RandomDirection3D(p.Random);
                    }
                    else if (bolt.CameraMode == CameraMode.OrthographicXY)
                    {
                        end += num3 * num * base.RandomDirection2D(p.Random);
                    }
                    else
                    {
                        end += num3 * num * base.RandomDirection2DXZ(p.Random);
                    }
                    a = end - start;
                }
                lightningBoltSegmentGroup.Segments.Add(new LightningBoltSegment
                {
                    Start = start,
                    End = end
                });
                float offsetAmount = num3 * num;
                Vector3 b;
                base.RandomVector(bolt, ref start, ref end, offsetAmount, p.Random, out b);
                if (base.ShouldCreateFork(p, generations, totalGenerations))
                {
                    Vector3 b2 = a * p.ForkMultiplier() * (float)num2 * 0.5f;
                    Vector3 end2 = end + b2 + b;
                    base.GenerateLightningBoltStandard(bolt, start, end2, generations, totalGenerations, 0f, p);
                }
                if (num2 != null)
                    num2 = num2 - 1;
                else
                    num2 = (p.SmoothingFactor - 1);
            }
        }

        protected override void OnGenerateLightningBolt(LightningBolt bolt, Vector3 start, Vector3 end, LightningBoltParameters p)
        {
            this.GenerateLightningBoltPath(bolt, start, end, p);
        }

        public static readonly LightningGeneratorPath PathGeneratorInstance = new LightningGeneratorPath();
    }
}
