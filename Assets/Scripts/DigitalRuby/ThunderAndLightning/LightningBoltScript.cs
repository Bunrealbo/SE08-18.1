using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBoltScript : MonoBehaviour
	{
		public Action<LightningBoltParameters, Vector3, Vector3> LightningStartedCallback
		{
			get
			{
				return this._003CLightningStartedCallback_003Ek__BackingField;
			}
			set
			{
				this._003CLightningStartedCallback_003Ek__BackingField = value;
			}
		}

		public Action<LightningBoltParameters, Vector3, Vector3> LightningEndedCallback
		{
			get
			{
				return this._003CLightningEndedCallback_003Ek__BackingField;
			}
			set
			{
				this._003CLightningEndedCallback_003Ek__BackingField = value;
			}
		}

		public Action<Light> LightAddedCallback
		{
			get
			{
				return this._003CLightAddedCallback_003Ek__BackingField;
			}
			set
			{
				this._003CLightAddedCallback_003Ek__BackingField = value;
			}
		}

		public Action<Light> LightRemovedCallback
		{
			get
			{
				return this._003CLightRemovedCallback_003Ek__BackingField;
			}
			set
			{
				this._003CLightRemovedCallback_003Ek__BackingField = value;
			}
		}

		public bool HasActiveBolts
		{
			get
			{
				return this.activeBolts.Count != 0;
			}
		}

		public static Vector4 TimeVectorSinceStart
		{
			get
			{
				return LightningBoltScript._003CTimeVectorSinceStart_003Ek__BackingField;
			}
			private set
			{
				LightningBoltScript._003CTimeVectorSinceStart_003Ek__BackingField = value;
			}
		}

		public static float TimeSinceStart
		{
			get
			{
				return LightningBoltScript._003CTimeSinceStart_003Ek__BackingField;
			}
			private set
			{
				LightningBoltScript._003CTimeSinceStart_003Ek__BackingField = value;
			}
		}

		public static float DeltaTime
		{
			get
			{
				return LightningBoltScript._003CDeltaTime_003Ek__BackingField;
			}
			private set
			{
				LightningBoltScript._003CDeltaTime_003Ek__BackingField = value;
			}
		}

		public virtual void CreateLightningBolt(LightningBoltParameters p)
		{
			if (p != null && this.Camera != null)
			{
				this.UpdateTexture();
				this.oneParameterArray[0] = p;
				LightningBolt orCreateLightningBolt = this.GetOrCreateLightningBolt();
				LightningBoltDependencies dependencies = this.CreateLightningBoltDependencies(this.oneParameterArray);
				orCreateLightningBolt.SetupLightningBolt(dependencies);
			}
		}

		public void CreateLightningBolts(ICollection<LightningBoltParameters> parameters)
		{
			if (parameters != null && parameters.Count != 0 && this.Camera != null)
			{
				this.UpdateTexture();
				LightningBolt orCreateLightningBolt = this.GetOrCreateLightningBolt();
				LightningBoltDependencies dependencies = this.CreateLightningBoltDependencies(parameters);
				orCreateLightningBolt.SetupLightningBolt(dependencies);
			}
		}

		protected virtual void Awake()
		{
			this.UpdateShaderIds();
		}

		protected virtual void Start()
		{
			this.UpdateCamera();
			this.UpdateMaterialsForLastTexture();
			this.UpdateShaderParameters();
			this.CheckCompensateForParentTransform();
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			if (this.MultiThreaded)
			{
				this.threadState = new LightningThreadState();
			}
		}

		protected virtual void Update()
		{
			if (LightningBoltScript.needsTimeUpdate)
			{
				LightningBoltScript.needsTimeUpdate = false;
				LightningBoltScript.DeltaTime = (this.UseGameTime ? Time.deltaTime : Time.unscaledDeltaTime) * LightningBoltScript.TimeScale;
				LightningBoltScript.TimeSinceStart += LightningBoltScript.DeltaTime;
			}
			if (this.HasActiveBolts)
			{
				this.UpdateCamera();
				this.UpdateShaderParameters();
				this.CheckCompensateForParentTransform();
				this.UpdateActiveBolts();
				Shader.SetGlobalVector(LightningBoltScript.shaderId_LightningTime, LightningBoltScript.TimeVectorSinceStart = new Vector4(LightningBoltScript.TimeSinceStart * 0.05f, LightningBoltScript.TimeSinceStart, LightningBoltScript.TimeSinceStart * 2f, LightningBoltScript.TimeSinceStart * 3f));
			}
			if (this.threadState != null)
			{
				this.threadState.UpdateMainThreadActions();
			}
		}

		protected virtual void LateUpdate()
		{
			LightningBoltScript.needsTimeUpdate = true;
		}

		protected virtual LightningBoltParameters OnCreateParameters()
		{
			return LightningBoltParameters.GetOrCreateParameters();
		}

		protected LightningBoltParameters CreateParameters()
		{
			LightningBoltParameters lightningBoltParameters = this.OnCreateParameters();
			lightningBoltParameters.quality = this.QualitySetting;
			this.PopulateParameters(lightningBoltParameters);
			return lightningBoltParameters;
		}

		protected virtual void PopulateParameters(LightningBoltParameters p)
		{
		}

		internal Material lightningMaterialMeshInternal
		{
			get
			{
				return this._003ClightningMaterialMeshInternal_003Ek__BackingField;
			}
			private set
			{
				this._003ClightningMaterialMeshInternal_003Ek__BackingField = value;
			}
		}

		internal Material lightningMaterialMeshNoGlowInternal
		{
			get
			{
				return this._003ClightningMaterialMeshNoGlowInternal_003Ek__BackingField;
			}
			private set
			{
				this._003ClightningMaterialMeshNoGlowInternal_003Ek__BackingField = value;
			}
		}

		private Coroutine StartCoroutineWrapper(IEnumerator routine)
		{
			if (base.isActiveAndEnabled)
			{
				return base.StartCoroutine(routine);
			}
			return null;
		}

		private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			LightningBolt.ClearCache();
		}

		private LightningBoltDependencies CreateLightningBoltDependencies(ICollection<LightningBoltParameters> parameters)
		{
			LightningBoltDependencies lightningBoltDependencies;
			if (this.dependenciesCache.Count == 0)
			{
				lightningBoltDependencies = new LightningBoltDependencies();
				lightningBoltDependencies.AddActiveBolt = new Action<LightningBolt>(this.AddActiveBolt);
				lightningBoltDependencies.LightAdded = new Action<Light>(this.OnLightAdded);
				lightningBoltDependencies.LightRemoved = new Action<Light>(this.OnLightRemoved);
				lightningBoltDependencies.ReturnToCache = new Action<LightningBoltDependencies>(this.ReturnLightningDependenciesToCache);
				lightningBoltDependencies.StartCoroutine = new Func<IEnumerator, Coroutine>(this.StartCoroutineWrapper);
				lightningBoltDependencies.Parent = base.gameObject;
			}
			else
			{
				int index = this.dependenciesCache.Count - 1;
				lightningBoltDependencies = this.dependenciesCache[index];
				this.dependenciesCache.RemoveAt(index);
			}
			lightningBoltDependencies.CameraPos = this.Camera.transform.position;
			lightningBoltDependencies.CameraIsOrthographic = this.Camera.orthographic;
			lightningBoltDependencies.CameraMode = this.calculatedCameraMode;
			lightningBoltDependencies.LevelOfDetailDistance = this.LevelOfDetailDistance;
			lightningBoltDependencies.DestParticleSystem = this.LightningDestinationParticleSystem;
			lightningBoltDependencies.LightningMaterialMesh = this.lightningMaterialMeshInternal;
			lightningBoltDependencies.LightningMaterialMeshNoGlow = this.lightningMaterialMeshNoGlowInternal;
			lightningBoltDependencies.OriginParticleSystem = this.LightningOriginParticleSystem;
			lightningBoltDependencies.SortLayerName = this.SortLayerName;
			lightningBoltDependencies.SortOrderInLayer = this.SortOrderInLayer;
			lightningBoltDependencies.UseWorldSpace = this.UseWorldSpace;
			lightningBoltDependencies.ThreadState = this.threadState;
			if (this.threadState != null)
			{
				lightningBoltDependencies.Parameters = new List<LightningBoltParameters>(parameters);
			}
			else
			{
				lightningBoltDependencies.Parameters = parameters;
			}
			lightningBoltDependencies.LightningBoltStarted = this.LightningStartedCallback;
			lightningBoltDependencies.LightningBoltEnded = this.LightningEndedCallback;
			return lightningBoltDependencies;
		}

		private void ReturnLightningDependenciesToCache(LightningBoltDependencies d)
		{
			d.Parameters = null;
			d.OriginParticleSystem = null;
			d.DestParticleSystem = null;
			d.LightningMaterialMesh = null;
			d.LightningMaterialMeshNoGlow = null;
			this.dependenciesCache.Add(d);
		}

		internal void OnLightAdded(Light l)
		{
			if (this.LightAddedCallback != null)
			{
				this.LightAddedCallback(l);
			}
		}

		internal void OnLightRemoved(Light l)
		{
			if (this.LightRemovedCallback != null)
			{
				this.LightRemovedCallback(l);
			}
		}

		internal void AddActiveBolt(LightningBolt bolt)
		{
			this.activeBolts.Add(bolt);
		}

		private void UpdateShaderIds()
		{
			if (LightningBoltScript.shaderId_MainTex != -2147483648)
			{
				return;
			}
			LightningBoltScript.shaderId_MainTex = Shader.PropertyToID("_MainTex");
			LightningBoltScript.shaderId_TintColor = Shader.PropertyToID("_TintColor");
			LightningBoltScript.shaderId_JitterMultiplier = Shader.PropertyToID("_JitterMultiplier");
			LightningBoltScript.shaderId_Turbulence = Shader.PropertyToID("_Turbulence");
			LightningBoltScript.shaderId_TurbulenceVelocity = Shader.PropertyToID("_TurbulenceVelocity");
			LightningBoltScript.shaderId_SrcBlendMode = Shader.PropertyToID("_SrcBlendMode");
			LightningBoltScript.shaderId_DstBlendMode = Shader.PropertyToID("_DstBlendMode");
			LightningBoltScript.shaderId_InvFade = Shader.PropertyToID("_InvFade");
			LightningBoltScript.shaderId_LightningTime = Shader.PropertyToID("_LightningTime");
			LightningBoltScript.shaderId_IntensityFlicker = Shader.PropertyToID("_IntensityFlicker");
			LightningBoltScript.shaderId_IntensityFlickerTexture = Shader.PropertyToID("_IntensityFlickerTexture");
		}

		private void UpdateMaterialsForLastTexture()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.calculatedCameraMode = CameraMode.Unknown;
			this.lightningMaterialMeshInternal = new Material(this.LightningMaterialMesh);
			this.lightningMaterialMeshNoGlowInternal = new Material(this.LightningMaterialMeshNoGlow);
			if (this.LightningTexture != null)
			{
				this.lightningMaterialMeshNoGlowInternal.SetTexture(LightningBoltScript.shaderId_MainTex, this.LightningTexture);
			}
			if (this.LightningGlowTexture != null)
			{
				this.lightningMaterialMeshInternal.SetTexture(LightningBoltScript.shaderId_MainTex, this.LightningGlowTexture);
			}
			this.SetupMaterialCamera();
		}

		private void UpdateTexture()
		{
			if (this.LightningTexture != null && this.LightningTexture != this.lastLightningTexture)
			{
				this.lastLightningTexture = this.LightningTexture;
				this.UpdateMaterialsForLastTexture();
			}
			if (this.LightningGlowTexture != null && this.LightningGlowTexture != this.lastLightningGlowTexture)
			{
				this.lastLightningGlowTexture = this.LightningGlowTexture;
				this.UpdateMaterialsForLastTexture();
			}
		}

		private void SetMaterialPerspective()
		{
			if (this.calculatedCameraMode != CameraMode.Perspective)
			{
				this.calculatedCameraMode = CameraMode.Perspective;
				this.lightningMaterialMeshInternal.EnableKeyword("PERSPECTIVE");
				this.lightningMaterialMeshNoGlowInternal.EnableKeyword("PERSPECTIVE");
				this.lightningMaterialMeshInternal.DisableKeyword("ORTHOGRAPHIC_XY");
				this.lightningMaterialMeshNoGlowInternal.DisableKeyword("ORTHOGRAPHIC_XY");
				this.lightningMaterialMeshInternal.DisableKeyword("ORTHOGRAPHIC_XZ");
				this.lightningMaterialMeshNoGlowInternal.DisableKeyword("ORTHOGRAPHIC_XZ");
			}
		}

		private void SetMaterialOrthographicXY()
		{
			if (this.calculatedCameraMode != CameraMode.OrthographicXY)
			{
				this.calculatedCameraMode = CameraMode.OrthographicXY;
				this.lightningMaterialMeshInternal.EnableKeyword("ORTHOGRAPHIC_XY");
				this.lightningMaterialMeshNoGlowInternal.EnableKeyword("ORTHOGRAPHIC_XY");
				this.lightningMaterialMeshInternal.DisableKeyword("ORTHOGRAPHIC_XZ");
				this.lightningMaterialMeshNoGlowInternal.DisableKeyword("ORTHOGRAPHIC_XZ");
				this.lightningMaterialMeshInternal.DisableKeyword("PERSPECTIVE");
				this.lightningMaterialMeshNoGlowInternal.DisableKeyword("PERSPECTIVE");
			}
		}

		private void SetMaterialOrthographicXZ()
		{
			if (this.calculatedCameraMode != CameraMode.OrthographicXZ)
			{
				this.calculatedCameraMode = CameraMode.OrthographicXZ;
				this.lightningMaterialMeshInternal.EnableKeyword("ORTHOGRAPHIC_XZ");
				this.lightningMaterialMeshNoGlowInternal.EnableKeyword("ORTHOGRAPHIC_XZ");
				this.lightningMaterialMeshInternal.DisableKeyword("ORTHOGRAPHIC_XY");
				this.lightningMaterialMeshNoGlowInternal.DisableKeyword("ORTHOGRAPHIC_XY");
				this.lightningMaterialMeshInternal.DisableKeyword("PERSPECTIVE");
				this.lightningMaterialMeshNoGlowInternal.DisableKeyword("PERSPECTIVE");
			}
		}

		private void SetupMaterialCamera()
		{
			if (this.Camera == null && this.CameraMode == CameraMode.Auto)
			{
				this.SetMaterialPerspective();
				return;
			}
			if (this.CameraMode == CameraMode.Auto)
			{
				if (this.Camera.orthographic)
				{
					this.SetMaterialOrthographicXY();
					return;
				}
				this.SetMaterialPerspective();
				return;
			}
			else
			{
				if (this.CameraMode == CameraMode.Perspective)
				{
					this.SetMaterialPerspective();
					return;
				}
				if (this.CameraMode == CameraMode.OrthographicXY)
				{
					this.SetMaterialOrthographicXY();
					return;
				}
				this.SetMaterialOrthographicXZ();
				return;
			}
		}

		private void EnableKeyword(string keyword, bool enable, Material m)
		{
			if (enable)
			{
				m.EnableKeyword(keyword);
				return;
			}
			m.DisableKeyword(keyword);
		}

		private void UpdateShaderParameters()
		{
			this.lightningMaterialMeshInternal.SetColor(LightningBoltScript.shaderId_TintColor, this.GlowTintColor);
			this.lightningMaterialMeshInternal.SetFloat(LightningBoltScript.shaderId_JitterMultiplier, this.JitterMultiplier);
			this.lightningMaterialMeshInternal.SetFloat(LightningBoltScript.shaderId_Turbulence, this.Turbulence * LightningBoltParameters.Scale);
			this.lightningMaterialMeshInternal.SetVector(LightningBoltScript.shaderId_TurbulenceVelocity, this.TurbulenceVelocity * LightningBoltParameters.Scale);
			this.lightningMaterialMeshInternal.SetInt(LightningBoltScript.shaderId_SrcBlendMode, (int)this.SourceBlendMode);
			this.lightningMaterialMeshInternal.SetInt(LightningBoltScript.shaderId_DstBlendMode, (int)this.DestinationBlendMode);
			this.lightningMaterialMeshInternal.renderQueue = this.RenderQueue;
			this.lightningMaterialMeshInternal.SetFloat(LightningBoltScript.shaderId_InvFade, this.SoftParticlesFactor);
			this.lightningMaterialMeshNoGlowInternal.SetColor(LightningBoltScript.shaderId_TintColor, this.LightningTintColor);
			this.lightningMaterialMeshNoGlowInternal.SetFloat(LightningBoltScript.shaderId_JitterMultiplier, this.JitterMultiplier);
			this.lightningMaterialMeshNoGlowInternal.SetFloat(LightningBoltScript.shaderId_Turbulence, this.Turbulence * LightningBoltParameters.Scale);
			this.lightningMaterialMeshNoGlowInternal.SetVector(LightningBoltScript.shaderId_TurbulenceVelocity, this.TurbulenceVelocity * LightningBoltParameters.Scale);
			this.lightningMaterialMeshNoGlowInternal.SetInt(LightningBoltScript.shaderId_SrcBlendMode, (int)this.SourceBlendMode);
			this.lightningMaterialMeshNoGlowInternal.SetInt(LightningBoltScript.shaderId_DstBlendMode, (int)this.DestinationBlendMode);
			this.lightningMaterialMeshNoGlowInternal.renderQueue = this.RenderQueue;
			this.lightningMaterialMeshNoGlowInternal.SetFloat(LightningBoltScript.shaderId_InvFade, this.SoftParticlesFactor);
			if (this.IntensityFlicker != LightningBoltScript.intensityFlickerDefault && this.IntensityFlickerTexture != null)
			{
				this.lightningMaterialMeshInternal.SetVector(LightningBoltScript.shaderId_IntensityFlicker, this.IntensityFlicker);
				this.lightningMaterialMeshInternal.SetTexture(LightningBoltScript.shaderId_IntensityFlickerTexture, this.IntensityFlickerTexture);
				this.lightningMaterialMeshNoGlowInternal.SetVector(LightningBoltScript.shaderId_IntensityFlicker, this.IntensityFlicker);
				this.lightningMaterialMeshNoGlowInternal.SetTexture(LightningBoltScript.shaderId_IntensityFlickerTexture, this.IntensityFlickerTexture);
				this.lightningMaterialMeshInternal.EnableKeyword("INTENSITY_FLICKER");
				this.lightningMaterialMeshNoGlowInternal.EnableKeyword("INTENSITY_FLICKER");
			}
			else
			{
				this.lightningMaterialMeshInternal.DisableKeyword("INTENSITY_FLICKER");
				this.lightningMaterialMeshNoGlowInternal.DisableKeyword("INTENSITY_FLICKER");
			}
			this.SetupMaterialCamera();
		}

		private void CheckCompensateForParentTransform()
		{
			if (this.CompensateForParentTransform)
			{
				Transform parent = base.transform.parent;
				if (parent != null)
				{
					base.transform.position = parent.position;
					base.transform.localScale = new Vector3(1f / parent.localScale.x, 1f / parent.localScale.y, 1f / parent.localScale.z);
					base.transform.rotation = parent.rotation;
				}
			}
		}

		private void UpdateCamera()
		{
			this.Camera = ((this.Camera == null) ? ((Camera.current == null) ? Camera.main : Camera.current) : this.Camera);
		}

		private LightningBolt GetOrCreateLightningBolt()
		{
			if (this.lightningBoltCache.Count == 0)
			{
				return new LightningBolt();
			}
			LightningBolt result = this.lightningBoltCache[this.lightningBoltCache.Count - 1];
			this.lightningBoltCache.RemoveAt(this.lightningBoltCache.Count - 1);
			return result;
		}

		private void UpdateActiveBolts()
		{
			for (int i = this.activeBolts.Count - 1; i >= 0; i--)
			{
				LightningBolt lightningBolt = this.activeBolts[i];
				if (!lightningBolt.Update())
				{
					this.activeBolts.RemoveAt(i);
					lightningBolt.Cleanup();
					this.lightningBoltCache.Add(lightningBolt);
				}
			}
		}

		private void OnApplicationQuit()
		{
			if (this.threadState != null)
			{
				this.threadState.Running = false;
			}
		}

		private void Cleanup()
		{
			foreach (LightningBolt lightningBolt in this.activeBolts)
			{
				lightningBolt.Cleanup();
			}
			this.activeBolts.Clear();
		}

		private void OnDestroy()
		{
			if (this.threadState != null)
			{
				this.threadState.TerminateAndWaitForEnd();
			}
			if (this.lightningMaterialMeshInternal != null)
			{
				UnityEngine.Object.Destroy(this.lightningMaterialMeshInternal);
			}
			if (this.lightningMaterialMeshNoGlowInternal != null)
			{
				UnityEngine.Object.Destroy(this.lightningMaterialMeshNoGlowInternal);
			}
			this.Cleanup();
		}

		private void OnDisable()
		{
			this.Cleanup();
		}

		public Camera Camera;

		public CameraMode CameraMode;

		internal CameraMode calculatedCameraMode = CameraMode.Unknown;

		public bool UseWorldSpace = true;

		public bool CompensateForParentTransform;

		public LightningBoltQualitySetting QualitySetting;

		public bool MultiThreaded;

		public float LevelOfDetailDistance;

		public bool UseGameTime;

		public string SortLayerName;

		public int SortOrderInLayer;

		public float SoftParticlesFactor = 3f;

		public int RenderQueue = -1;

		public Material LightningMaterialMesh;

		public Material LightningMaterialMeshNoGlow;

		public Texture2D LightningTexture;

		public Texture2D LightningGlowTexture;

		public ParticleSystem LightningOriginParticleSystem;

		public ParticleSystem LightningDestinationParticleSystem;

		public Color LightningTintColor = Color.white;

		public Color GlowTintColor = new Color(0.1f, 0.2f, 1f, 1f);

		public BlendMode SourceBlendMode = BlendMode.SrcAlpha;

		public BlendMode DestinationBlendMode = BlendMode.One;

		public float JitterMultiplier;

		public float Turbulence;

		public Vector3 TurbulenceVelocity = Vector3.zero;

		public Vector4 IntensityFlicker = LightningBoltScript.intensityFlickerDefault;

		private static readonly Vector4 intensityFlickerDefault = new Vector4(1f, 1f, 1f, 0f);

		public Texture2D IntensityFlickerTexture;

		private Action<LightningBoltParameters, Vector3, Vector3> _003CLightningStartedCallback_003Ek__BackingField;

		private Action<LightningBoltParameters, Vector3, Vector3> _003CLightningEndedCallback_003Ek__BackingField;

		private Action<Light> _003CLightAddedCallback_003Ek__BackingField;

		private Action<Light> _003CLightRemovedCallback_003Ek__BackingField;

		private static Vector4 _003CTimeVectorSinceStart_003Ek__BackingField;

		private static float _003CTimeSinceStart_003Ek__BackingField;

		private static float _003CDeltaTime_003Ek__BackingField;

		public static float TimeScale = 1f;

		private static bool needsTimeUpdate = true;

		private Material _003ClightningMaterialMeshInternal_003Ek__BackingField;

		private Material _003ClightningMaterialMeshNoGlowInternal_003Ek__BackingField;

		private Texture2D lastLightningTexture;

		private Texture2D lastLightningGlowTexture;

		private readonly List<LightningBolt> activeBolts = new List<LightningBolt>();

		private readonly LightningBoltParameters[] oneParameterArray = new LightningBoltParameters[1];

		private readonly List<LightningBolt> lightningBoltCache = new List<LightningBolt>();

		private readonly List<LightningBoltDependencies> dependenciesCache = new List<LightningBoltDependencies>();

		private LightningThreadState threadState;

		private static int shaderId_MainTex = int.MinValue;

		private static int shaderId_TintColor;

		private static int shaderId_JitterMultiplier;

		private static int shaderId_Turbulence;

		private static int shaderId_TurbulenceVelocity;

		private static int shaderId_SrcBlendMode;

		private static int shaderId_DstBlendMode;

		private static int shaderId_InvFade;

		private static int shaderId_LightningTime;

		private static int shaderId_IntensityFlicker;

		private static int shaderId_IntensityFlickerTexture;
	}
}
