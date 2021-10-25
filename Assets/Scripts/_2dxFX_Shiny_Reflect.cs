using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[Serializable]
public class _2dxFX_Shiny_Reflect : MonoBehaviour
{
	private void Awake()
	{
		if (base.gameObject.GetComponent<Image>() != null)
		{
			this.CanvasImage = base.gameObject.GetComponent<Image>();
		}
		if (base.gameObject.GetComponent<SpriteRenderer>() != null)
		{
			this.CanvasSpriteRenderer = base.gameObject.GetComponent<SpriteRenderer>();
		}
	}

	private void Start()
	{
		this.__MainTex2 = (Resources.Load("_2dxFX_Gradient") as Texture2D);
		this.ShaderChange = 0;
		if (this.CanvasSpriteRenderer != null)
		{
			this.CanvasSpriteRenderer.sharedMaterial.SetTexture("_MainTex2", this.__MainTex2);
		}
		else if (this.CanvasImage != null)
		{
			this.CanvasImage.material.SetTexture("_MainTex2", this.__MainTex2);
		}
		if (this.ShinyLightCurve == null)
		{
			this.ShinyLightCurve = new AnimationCurve();
		}
		if (this.ShinyLightCurve.length == 0)
		{
			this.ShinyLightCurve.AddKey(7.780734E-06f, -0.4416301f);
			this.ShinyLightCurve.keys[0].inTangent = 0f;
			this.ShinyLightCurve.keys[0].outTangent = 0f;
			this.ShinyLightCurve.AddKey(0.4310643f, 1.113406f);
			this.ShinyLightCurve.keys[1].inTangent = 0.2280953f;
			this.ShinyLightCurve.keys[1].outTangent = 0.2280953f;
			this.ShinyLightCurve.AddKey(0.5258899f, 1.229086f);
			this.ShinyLightCurve.keys[2].inTangent = -0.1474274f;
			this.ShinyLightCurve.keys[2].outTangent = -0.1474274f;
			this.ShinyLightCurve.AddKey(0.6136486f, 1.113075f);
			this.ShinyLightCurve.keys[3].inTangent = 0.005268873f;
			this.ShinyLightCurve.keys[3].outTangent = 0.005268873f;
			this.ShinyLightCurve.AddKey(0.9367767f, -0.4775873f);
			this.ShinyLightCurve.keys[4].inTangent = -3.890693f;
			this.ShinyLightCurve.keys[4].outTangent = -3.890693f;
			this.ShinyLightCurve.AddKey(1.144408f, -0.4976555f);
			this.ShinyLightCurve.keys[5].inTangent = 0f;
			this.ShinyLightCurve.keys[5].outTangent = 0f;
			this.ShinyLightCurve.postWrapMode = WrapMode.Loop;
			this.ShinyLightCurve.preWrapMode = WrapMode.Loop;
		}
		this.XUpdate();
	}

	public void CallUpdate()
	{
		this.XUpdate();
	}

	private void Update()
	{
		if (this.ActiveUpdate)
		{
			this.XUpdate();
		}
	}

	private void XUpdate()
	{
		if (this.CanvasImage == null && base.gameObject.GetComponent<Image>() != null)
		{
			this.CanvasImage = base.gameObject.GetComponent<Image>();
		}
		if (this.CanvasSpriteRenderer == null && base.gameObject.GetComponent<SpriteRenderer>() != null)
		{
			this.CanvasSpriteRenderer = base.gameObject.GetComponent<SpriteRenderer>();
		}
		if (this.ShaderChange == 0 && this.ForceMaterial != null)
		{
			this.ShaderChange = 1;
			if (this.tempMaterial != null)
			{
				UnityEngine.Object.DestroyImmediate(this.tempMaterial);
			}
			if (this.CanvasSpriteRenderer != null)
			{
				this.CanvasSpriteRenderer.sharedMaterial = this.ForceMaterial;
			}
			else if (this.CanvasImage != null)
			{
				this.CanvasImage.material = this.ForceMaterial;
			}
			this.ForceMaterial.hideFlags = HideFlags.None;
			this.ForceMaterial.shader = Shader.Find(this.shader);
		}
		if (this.ForceMaterial == null && this.ShaderChange == 1)
		{
			if (this.tempMaterial != null)
			{
				UnityEngine.Object.DestroyImmediate(this.tempMaterial);
			}
			this.tempMaterial = new Material(Shader.Find(this.shader));
			this.tempMaterial.hideFlags = HideFlags.None;
			if (this.CanvasSpriteRenderer != null)
			{
				this.CanvasSpriteRenderer.sharedMaterial = this.tempMaterial;
			}
			else if (this.CanvasImage != null)
			{
				this.CanvasImage.material = this.tempMaterial;
			}
			this.ShaderChange = 0;
		}
		if (this.ActiveChange)
		{
			if (this.CanvasSpriteRenderer != null)
			{
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Alpha", 1f - this._Alpha);
				if (this.UseShinyCurve)
				{
					if (this.ShinyLightCurve != null)
					{
						this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Distortion", this.ShinyLightCurve.Evaluate(this.ShinyLightCurveTime));
					}
					this.ShinyLightCurveTime += Time.deltaTime / 8f * this.AnimationSpeedReduction;
				}
				else
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Distortion", this.Light);
				}
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Value2", this.LightSize);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Value3", this.Intensity);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Value4", this.OnlyLight);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Value5", this.LightBump);
				return;
			}
			if (this.CanvasImage != null)
			{
				this.CanvasImage.material.SetFloat("_Alpha", 1f - this._Alpha);
				if (this.UseShinyCurve)
				{
					this.CanvasImage.material.SetFloat("_Distortion", this.ShinyLightCurve.Evaluate(this.ShinyLightCurveTime));
					this.ShinyLightCurveTime += Time.deltaTime / 8f * this.AnimationSpeedReduction;
				}
				else
				{
					this.CanvasImage.material.SetFloat("_Distortion", this.Light);
				}
				this.CanvasImage.material.SetFloat("_Value2", this.LightSize);
				this.CanvasImage.material.SetFloat("_Value3", this.Intensity);
				this.CanvasImage.material.SetFloat("_Value4", this.OnlyLight);
				this.CanvasImage.material.SetFloat("_Value5", this.LightBump);
			}
		}
	}

	private void OnDestroy()
	{
		if (!Application.isPlaying && Application.isEditor)
		{
			if (this.tempMaterial != null)
			{
				UnityEngine.Object.DestroyImmediate(this.tempMaterial);
			}
			if (base.gameObject.activeSelf && this.defaultMaterial != null)
			{
				if (this.CanvasSpriteRenderer != null)
				{
					this.CanvasSpriteRenderer.sharedMaterial = this.defaultMaterial;
					this.CanvasSpriteRenderer.sharedMaterial.hideFlags = HideFlags.None;
					return;
				}
				if (this.CanvasImage != null)
				{
					this.CanvasImage.material = this.defaultMaterial;
					this.CanvasImage.material.hideFlags = HideFlags.None;
				}
			}
		}
	}

	private void OnDisable()
	{
		if (base.gameObject.activeSelf && this.defaultMaterial != null)
		{
			if (this.CanvasSpriteRenderer != null)
			{
				this.CanvasSpriteRenderer.sharedMaterial = this.defaultMaterial;
				this.CanvasSpriteRenderer.sharedMaterial.hideFlags = HideFlags.None;
				return;
			}
			if (this.CanvasImage != null)
			{
				this.CanvasImage.material = this.defaultMaterial;
				this.CanvasImage.material.hideFlags = HideFlags.None;
			}
		}
	}

	private void OnEnable()
	{
		if (this.defaultMaterial == null)
		{
			this.defaultMaterial = new Material(Shader.Find("Sprites/Default"));
		}
		if (this.ForceMaterial == null)
		{
			this.ActiveChange = true;
			this.tempMaterial = new Material(Shader.Find(this.shader));
			this.tempMaterial.hideFlags = HideFlags.None;
			if (this.CanvasSpriteRenderer != null)
			{
				this.CanvasSpriteRenderer.sharedMaterial = this.tempMaterial;
			}
			else if (this.CanvasImage != null)
			{
				this.CanvasImage.material = this.tempMaterial;
			}
			this.__MainTex2 = (Resources.Load("_2dxFX_Gradient") as Texture2D);
		}
		else
		{
			this.ForceMaterial.shader = Shader.Find(this.shader);
			this.ForceMaterial.hideFlags = HideFlags.None;
			if (this.CanvasSpriteRenderer != null)
			{
				this.CanvasSpriteRenderer.sharedMaterial = this.ForceMaterial;
			}
			else if (this.CanvasImage != null)
			{
				this.CanvasImage.material = this.ForceMaterial;
			}
			this.__MainTex2 = (Resources.Load("_2dxFX_Gradient") as Texture2D);
		}
		if (this.__MainTex2)
		{
			this.__MainTex2.wrapMode = TextureWrapMode.Repeat;
			if (this.CanvasSpriteRenderer != null)
			{
				this.CanvasSpriteRenderer.sharedMaterial.SetTexture("_MainTex2", this.__MainTex2);
				return;
			}
			if (this.CanvasImage != null)
			{
				this.CanvasImage.material.SetTexture("_MainTex2", this.__MainTex2);
			}
		}
	}

	public Material ForceMaterial;

	public bool ActiveChange = true;

	public Texture2D __MainTex2;

	private string shader = "2DxFX/Standard/Shiny_Reflect";

	public float _Alpha = 1f;

	public float Light = 1f;

	public float LightSize = 0.5f;

	public bool UseShinyCurve = true;

	public AnimationCurve ShinyLightCurve;

	public float AnimationSpeedReduction = 3f;

	public float Intensity = 1f;

	public float OnlyLight;

	public float LightBump = 0.05f;

	private float ShinyLightCurveTime;

	public int ShaderChange;

	private Material tempMaterial;

	private Material defaultMaterial;

	private Image CanvasImage;

	private SpriteRenderer CanvasSpriteRenderer;

	public bool ActiveUpdate = true;
}
