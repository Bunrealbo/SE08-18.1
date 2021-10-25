using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[Serializable]
public class _2dxFX_Waterfall : MonoBehaviour
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
		this.__MainTex2 = (Resources.Load("_2dxFX_WaterfallTXT") as Texture2D);
		this.ShaderChange = 0;
		if (this.CanvasSpriteRenderer != null)
		{
			this.CanvasSpriteRenderer.sharedMaterial.SetTexture("_MainTex2", this.__MainTex2);
		}
		else if (this.CanvasImage != null)
		{
			this.CanvasImage.material.SetTexture("_MainTex2", this.__MainTex2);
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
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Alpha", this._Alpha);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Distortion", this.Liquid);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Speed", this.Speed);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("EValue", this.EValue);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("TValue", this.TValue);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("Light", this.Light);
				this.CanvasSpriteRenderer.sharedMaterial.SetColor("Lightcolor", this.LightColor);
				return;
			}
			if (this.CanvasImage != null)
			{
				this.CanvasImage.material.SetFloat("_Alpha", this._Alpha);
				this.CanvasImage.material.SetFloat("_Distortion", this.Liquid);
				this.CanvasImage.material.SetFloat("_Speed", this.Speed);
				this.CanvasImage.material.SetFloat("EValue", this.EValue);
				this.CanvasImage.material.SetFloat("TValue", this.TValue);
				this.CanvasImage.material.SetFloat("Light", this.Light);
				this.CanvasImage.material.SetColor("Lightcolor", this.LightColor);
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
			this.__MainTex2 = (Resources.Load("_2dxFX_WaterfallTXT") as Texture2D);
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
			this.__MainTex2 = (Resources.Load("_2dxFX_WaterfallTXT") as Texture2D);
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

	private string shader = "2DxFX/Standard/Waterfall";

	public float _Alpha = 1f;

	public Texture2D __MainTex2;

	public float Liquid = 1f;

	public float Speed = 1f;

	public float EValue = -0.65f;

	public float TValue = 0.6f;

	public Color LightColor = new Color(0.2f, 0.5f, 1f, 1f);

	public float Light = 0.5f;

	public int ShaderChange;

	private Material tempMaterial;

	private Material defaultMaterial;

	private Image CanvasImage;

	private SpriteRenderer CanvasSpriteRenderer;

	public bool ActiveUpdate = true;
}
