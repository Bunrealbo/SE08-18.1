using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class ThunderAndLightningScript : MonoBehaviour
	{
		private void Start()
		{
			this.EnableLightning = true;
			if (this.Camera == null)
			{
				this.Camera = Camera.main;
			}
			if (RenderSettings.skybox != null)
			{
				this.skyboxMaterial = (RenderSettings.skybox = new Material(RenderSettings.skybox));
			}
			this.skyboxExposureOriginal = (this.skyboxExposureStorm = ((this.skyboxMaterial == null || !this.skyboxMaterial.HasProperty("_Exposure")) ? 1f : this.skyboxMaterial.GetFloat("_Exposure")));
			this.audioSourceThunder = base.gameObject.AddComponent<AudioSource>();
			this.lightningBoltHandler = new ThunderAndLightningScript.LightningBoltHandler(this);
			this.lightningBoltHandler.VolumeMultiplier = this.VolumeMultiplier;
		}

		private void Update()
		{
			if (this.lightningBoltHandler != null && this.EnableLightning)
			{
				this.lightningBoltHandler.VolumeMultiplier = this.VolumeMultiplier;
				this.lightningBoltHandler.Update();
			}
		}

		public void CallNormalLightning()
		{
			this.CallNormalLightning(null, null);
		}

		public void CallNormalLightning(Vector3? start, Vector3? end)
		{
			base.StartCoroutine(this.lightningBoltHandler.ProcessLightning(start, end, false, true));
		}

		public void CallIntenseLightning()
		{
			this.CallIntenseLightning(null, null);
		}

		public void CallIntenseLightning(Vector3? start, Vector3? end)
		{
			base.StartCoroutine(this.lightningBoltHandler.ProcessLightning(start, end, true, true));
		}

		public float SkyboxExposureOriginal
		{
			get
			{
				return this.skyboxExposureOriginal;
			}
		}

		public bool EnableLightning
		{
			get
			{
				return this._003CEnableLightning_003Ek__BackingField;
			}
			set
			{
				this._003CEnableLightning_003Ek__BackingField = value;
			}
		}

		public LightningBoltPrefabScript LightningBoltScript;

		public Camera Camera;

		public RangeOfFloats LightningIntervalTimeRange = new RangeOfFloats
		{
			Minimum = 10f,
			Maximum = 25f
		};

		public float LightningIntenseProbability = 0.2f;

		public AudioClip[] ThunderSoundsNormal;

		public AudioClip[] ThunderSoundsIntense;

		public bool LightningAlwaysVisible = true;

		public float CloudLightningChance = 0.5f;

		public bool ModifySkyboxExposure;

		public float BaseLightRange = 2000f;

		public float LightningYStart = 500f;

		public float VolumeMultiplier = 1f;

		private float skyboxExposureOriginal;

		private float skyboxExposureStorm;

		private float nextLightningTime;

		private bool lightningInProgress;

		private AudioSource audioSourceThunder;

		private ThunderAndLightningScript.LightningBoltHandler lightningBoltHandler;

		private Material skyboxMaterial;

		private AudioClip lastThunderSound;

		private bool _003CEnableLightning_003Ek__BackingField;

		private class LightningBoltHandler
		{
			public float VolumeMultiplier
			{
				get
				{
					return this._003CVolumeMultiplier_003Ek__BackingField;
				}
				set
				{
					this._003CVolumeMultiplier_003Ek__BackingField = value;
				}
			}

			public LightningBoltHandler(ThunderAndLightningScript script)
			{
				this.script = script;
				this.CalculateNextLightningTime();
			}

			private void UpdateLighting()
			{
				if (this.script.lightningInProgress)
				{
					return;
				}
				if (this.script.ModifySkyboxExposure)
				{
					this.script.skyboxExposureStorm = 0.35f;
					if (this.script.skyboxMaterial != null && this.script.skyboxMaterial.HasProperty("_Exposure"))
					{
						this.script.skyboxMaterial.SetFloat("_Exposure", this.script.skyboxExposureStorm);
					}
				}
				this.CheckForLightning();
			}

			private void CalculateNextLightningTime()
			{
				this.script.nextLightningTime = DigitalRuby.ThunderAndLightning.LightningBoltScript.TimeSinceStart + this.script.LightningIntervalTimeRange.Random(this.random);
				this.script.lightningInProgress = false;
				if (this.script.ModifySkyboxExposure && this.script.skyboxMaterial.HasProperty("_Exposure"))
				{
					this.script.skyboxMaterial.SetFloat("_Exposure", this.script.skyboxExposureStorm);
				}
			}

			public IEnumerator ProcessLightning(Vector3? _start, Vector3? _end, bool intense, bool visible)
			{
				return new ThunderAndLightningScript.LightningBoltHandler._003CProcessLightning_003Ed__9(0)
				{
					_003C_003E4__this = this,
					_start = _start,
					_end = _end,
					intense = intense,
					visible = visible
				};
			}

			private void Strike(Vector3? _start, Vector3? _end, bool intense, float intensity, Camera camera, Camera visibleInCamera)
			{
				float min = intense ? -1000f : -5000f;
				float max = intense ? 1000f : 5000f;
				float num = intense ? 500f : 2500f;
				float num2 = (UnityEngine.Random.Range(0, 2) == 0) ? UnityEngine.Random.Range(min, -num) : UnityEngine.Random.Range(num, max);
				float y = this.script.LightningYStart;
				float num3 = (UnityEngine.Random.Range(0, 2) == 0) ? UnityEngine.Random.Range(min, -num) : UnityEngine.Random.Range(num, max);
				Vector3 vector = this.script.Camera.transform.position;
				vector.x += num2;
				vector.y = y;
				vector.z += num3;
				if (visibleInCamera != null)
				{
					Quaternion rotation = visibleInCamera.transform.rotation;
					visibleInCamera.transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
					float x = UnityEngine.Random.Range((float)visibleInCamera.pixelWidth * 0.1f, (float)visibleInCamera.pixelWidth * 0.9f);
					float z = UnityEngine.Random.Range(visibleInCamera.nearClipPlane + num + num, max);
					vector = visibleInCamera.ScreenToWorldPoint(new Vector3(x, 0f, z));
					vector.y = y;
					visibleInCamera.transform.rotation = rotation;
				}
				Vector3 vector2 = vector;
				num2 = UnityEngine.Random.Range(-100f, 100f);
				y = ((UnityEngine.Random.Range(0, 4) == 0) ? UnityEngine.Random.Range(-1f, 600f) : -1f);
				num3 += UnityEngine.Random.Range(-100f, 100f);
				vector2.x += num2;
				vector2.y = y;
				vector2.z += num3;
				vector2.x += num * camera.transform.forward.x;
				vector2.z += num * camera.transform.forward.z;
				while ((vector - vector2).magnitude < 500f)
				{
					vector2.x += num * camera.transform.forward.x;
					vector2.z += num * camera.transform.forward.z;
				}
				vector = (_start ?? vector);
				vector2 = (_end ?? vector2);
				RaycastHit raycastHit;
				if (Physics.Raycast(vector, (vector - vector2).normalized, out raycastHit, 3.40282347E+38f))
				{
					vector2 = raycastHit.point;
				}
				int generations = this.script.LightningBoltScript.Generations;
				RangeOfFloats trunkWidthRange = this.script.LightningBoltScript.TrunkWidthRange;
				if (UnityEngine.Random.value < this.script.CloudLightningChance)
				{
					this.script.LightningBoltScript.TrunkWidthRange = default(RangeOfFloats);
					this.script.LightningBoltScript.Generations = 1;
				}
				this.script.LightningBoltScript.LightParameters.LightIntensity = intensity * 0.5f;
				this.script.LightningBoltScript.Trigger(new Vector3?(vector), new Vector3?(vector2));
				this.script.LightningBoltScript.TrunkWidthRange = trunkWidthRange;
				this.script.LightningBoltScript.Generations = generations;
			}

			private void CheckForLightning()
			{
				if (Time.time >= this.script.nextLightningTime)
				{
					bool intense = UnityEngine.Random.value < this.script.LightningIntenseProbability;
					this.script.StartCoroutine(this.ProcessLightning(null, null, intense, this.script.LightningAlwaysVisible));
				}
			}

			public void Update()
			{
				this.UpdateLighting();
			}

			private float _003CVolumeMultiplier_003Ek__BackingField;

			private ThunderAndLightningScript script;

			private readonly System.Random random = new System.Random();

			private sealed class _003CProcessLightning_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
			{
				[DebuggerHidden]
				public _003CProcessLightning_003Ed__9(int _003C_003E1__state)
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
					ThunderAndLightningScript.LightningBoltHandler lightningBoltHandler = this._003C_003E4__this;
					if (num != 0)
					{
						if (num != 1)
						{
							return false;
						}
						this._003C_003E1__state = -1;
						AudioClip audioClip;
						do
						{
							audioClip = this._003Csounds_003E5__2[UnityEngine.Random.Range(0, this._003Csounds_003E5__2.Length - 1)];
						}
						while (this._003Csounds_003E5__2.Length > 1 && audioClip == lightningBoltHandler.script.lastThunderSound);
						lightningBoltHandler.script.lastThunderSound = audioClip;
						lightningBoltHandler.script.audioSourceThunder.PlayOneShot(audioClip, this._003Cintensity_003E5__3 * 0.5f * lightningBoltHandler.VolumeMultiplier);
					}
					else
					{
						this._003C_003E1__state = -1;
						lightningBoltHandler.script.lightningInProgress = true;
						float time;
						if (this.intense)
						{
							float t = UnityEngine.Random.Range(0f, 1f);
							this._003Cintensity_003E5__3 = Mathf.Lerp(2f, 8f, t);
							time = 5f / this._003Cintensity_003E5__3;
							this._003Csounds_003E5__2 = lightningBoltHandler.script.ThunderSoundsIntense;
						}
						else
						{
							float t2 = UnityEngine.Random.Range(0f, 1f);
							this._003Cintensity_003E5__3 = Mathf.Lerp(0f, 2f, t2);
							time = 30f / this._003Cintensity_003E5__3;
							this._003Csounds_003E5__2 = lightningBoltHandler.script.ThunderSoundsNormal;
						}
						if (lightningBoltHandler.script.skyboxMaterial != null && lightningBoltHandler.script.ModifySkyboxExposure)
						{
							lightningBoltHandler.script.skyboxMaterial.SetFloat("_Exposure", Mathf.Max(this._003Cintensity_003E5__3 * 0.5f, lightningBoltHandler.script.skyboxExposureStorm));
						}
						lightningBoltHandler.Strike(this._start, this._end, this.intense, this._003Cintensity_003E5__3, lightningBoltHandler.script.Camera, this.visible ? lightningBoltHandler.script.Camera : null);
						lightningBoltHandler.CalculateNextLightningTime();
						if (this._003Cintensity_003E5__3 >= 1f && this._003Csounds_003E5__2 != null && this._003Csounds_003E5__2.Length != 0)
						{
							this._003C_003E2__current = new WaitForSecondsLightning(time);
							this._003C_003E1__state = 1;
							return true;
						}
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

				public ThunderAndLightningScript.LightningBoltHandler _003C_003E4__this;

				public bool intense;

				public Vector3? _start;

				public Vector3? _end;

				public bool visible;

				private AudioClip[] _003Csounds_003E5__2;

				private float _003Cintensity_003E5__3;
			}
		}
	}
}
