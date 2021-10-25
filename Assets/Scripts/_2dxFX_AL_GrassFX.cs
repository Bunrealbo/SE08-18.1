using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[ExecuteInEditMode]
[Serializable]
public class _2dxFX_AL_GrassFX : MonoBehaviour
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
		this.ShaderChange = 0;
		this.Wind = new AnimationCurve();
		this.Wind.AddKey(0f, 0f);
		this.Wind.keys[0].inTangent = 0f;
		this.Wind.keys[0].outTangent = 0f;
		this.Wind.AddKey(0.1004994f, 0.06637689f);
		this.Wind.keys[1].inTangent = 0f;
		this.Wind.keys[1].outTangent = 0f;
		this.Wind.AddKey(0.2430963f, -0.06465532f);
		this.Wind.keys[2].inTangent = -0.07599592f;
		this.Wind.keys[2].outTangent = -0.07599592f;
		this.Wind.AddKey(0.3425266f, 0.02290122f);
		this.Wind.keys[3].inTangent = 0.03580004f;
		this.Wind.keys[3].outTangent = 0.03580004f;
		this.Wind.AddKey(0.4246872f, -0.02232522f);
		this.Wind.keys[4].inTangent = -0.006025657f;
		this.Wind.keys[4].outTangent = -0.006025657f;
		this.Wind.AddKey(0.5104106f, 0.1647801f);
		this.Wind.keys[5].inTangent = 0.02981164f;
		this.Wind.keys[5].outTangent = 0.02981164f;
		this.Wind.AddKey(0.6082056f, -0.04679203f);
		this.Wind.keys[6].inTangent = -0.3176928f;
		this.Wind.keys[6].outTangent = -0.3176928f;
		this.Wind.AddKey(0.7794942f, 0.2234365f);
		this.Wind.keys[7].inTangent = 0.2063811f;
		this.Wind.keys[7].outTangent = 0.2063811f;
		this.Wind.AddKey(0.8546611f, -0.003165513f);
		this.Wind.keys[8].inTangent = 0.02264977f;
		this.Wind.keys[8].outTangent = 0.02264977f;
		this.Wind.AddKey(1.022495f, -0.07358052f);
		this.Wind.keys[9].inTangent = 2.450916f;
		this.Wind.keys[9].outTangent = 2.450916f;
		this.Wind.AddKey(1.250894f, -0.1813075f);
		this.Wind.keys[10].inTangent = 0.02214685f;
		this.Wind.keys[10].outTangent = 0.02214685f;
		this.Wind.AddKey(1.369877f, -0.06861454f);
		this.Wind.keys[11].inTangent = -1.860534f;
		this.Wind.keys[11].outTangent = -1.860534f;
		this.Wind.AddKey(1.484951f, -0.1543293f);
		this.Wind.keys[12].inTangent = 0.0602752f;
		this.Wind.keys[12].outTangent = 0.0602752f;
		this.Wind.AddKey(1.583562f, 0.100938f);
		this.Wind.keys[13].inTangent = 0.08665025f;
		this.Wind.keys[13].outTangent = 0.08665025f;
		this.Wind.AddKey(1.687307f, -0.100769f);
		this.Wind.keys[14].inTangent = 0.01110137f;
		this.Wind.keys[14].outTangent = 0.01110137f;
		this.Wind.AddKey(1.797593f, 0.04921142f);
		this.Wind.keys[15].inTangent = 3.407104f;
		this.Wind.keys[15].outTangent = 3.407104f;
		this.Wind.AddKey(1.927248f, -0.1877219f);
		this.Wind.keys[16].inTangent = -0.001117587f;
		this.Wind.keys[16].outTangent = -0.001117587f;
		this.Wind.AddKey(2.067694f, 0.2742145f);
		this.Wind.keys[17].inTangent = 4.736587f;
		this.Wind.keys[17].outTangent = 4.736587f;
		this.Wind.AddKey(2.184602f, -0.06127208f);
		this.Wind.keys[18].inTangent = -0.1308322f;
		this.Wind.keys[18].outTangent = -0.1308322f;
		this.Wind.AddKey(2.305948f, 0.1891117f);
		this.Wind.keys[19].inTangent = 0.04030764f;
		this.Wind.keys[19].outTangent = 0.04030764f;
		this.Wind.AddKey(2.428946f, -0.1695723f);
		this.Wind.keys[20].inTangent = -0.2463162f;
		this.Wind.keys[20].outTangent = -0.2463162f;
		this.Wind.AddKey(2.55922f, 0.0359862f);
		this.Wind.keys[21].inTangent = 0.3967434f;
		this.Wind.keys[21].outTangent = 0.3967434f;
		this.Wind.AddKey(2.785119f, -0.08398628f);
		this.Wind.keys[22].inTangent = -0.2388284f;
		this.Wind.keys[22].outTangent = -0.2388284f;
		this.Wind.AddKey(3f, 0f);
		this.Wind.keys[23].inTangent = 0f;
		this.Wind.keys[23].outTangent = 0f;
		this.Wind.postWrapMode = WrapMode.Loop;
		this.Wind.preWrapMode = WrapMode.Loop;
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
				if (_2DxFX.ActiveShadow && this.AddShadow)
				{
					this.CanvasSpriteRenderer.shadowCastingMode = ShadowCastingMode.On;
					if (this.ReceivedShadow)
					{
						this.CanvasSpriteRenderer.receiveShadows = true;
						this.CanvasSpriteRenderer.sharedMaterial.renderQueue = 2450;
						this.CanvasSpriteRenderer.sharedMaterial.SetInt("_Z", 1);
					}
					else
					{
						this.CanvasSpriteRenderer.receiveShadows = false;
						this.CanvasSpriteRenderer.sharedMaterial.renderQueue = 3000;
						this.CanvasSpriteRenderer.sharedMaterial.SetInt("_Z", 0);
					}
				}
				else
				{
					this.CanvasSpriteRenderer.shadowCastingMode = ShadowCastingMode.Off;
					this.CanvasSpriteRenderer.receiveShadows = false;
					this.CanvasSpriteRenderer.sharedMaterial.renderQueue = 3000;
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_Z", 0);
				}
				if (this.BlendMode == 0)
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_BlendOp", 0);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_SrcBlend", 1);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_DstBlend", 10);
				}
				if (this.BlendMode == 1)
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_BlendOp", 0);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_SrcBlend", 1);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_DstBlend", 1);
				}
				if (this.BlendMode == 2)
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_BlendOp", 2);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_SrcBlend", 1);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_DstBlend", 2);
				}
				if (this.BlendMode == 3)
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_BlendOp", 4);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_SrcBlend", 1);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_DstBlend", 1);
				}
				if (this.BlendMode == 4)
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_BlendOp", 2);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_SrcBlend", 1);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_DstBlend", 1);
				}
				if (this.BlendMode == 5)
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_BlendOp", 4);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_SrcBlend", 10);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_DstBlend", 10);
				}
				if (this.BlendMode == 6)
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_BlendOp", 0);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_SrcBlend", 2);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_DstBlend", 10);
				}
				if (this.BlendMode == 7)
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_BlendOp", 0);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_SrcBlend", 4);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_DstBlend", 1);
				}
				if (this.BlendMode == 8)
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_BlendOp", 2);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_SrcBlend", 7);
					this.CanvasSpriteRenderer.sharedMaterial.SetInt("_DstBlend", 2);
				}
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Distortion", this.Heat);
				if (this.Wind != null)
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Wind", this.Wind.Evaluate(this.WindTime1));
				}
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Speed", this.Speed);
				this.WindTime1 += Time.deltaTime / 8f * this.Speed;
				return;
			}
			if (this.CanvasImage != null)
			{
				this.CanvasImage.material.SetFloat("_Alpha", 1f - this._Alpha);
				this.CanvasImage.material.SetFloat("_Distortion", this.Heat);
				if (this.Wind != null)
				{
					this.CanvasImage.material.SetFloat("_Wind", this.Wind.Evaluate(this.WindTime1));
				}
				this.CanvasImage.material.SetFloat("_Speed", this.Speed);
				this.WindTime1 += Time.deltaTime / 8f * this.Speed;
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
		this.WindTime1 = 0f;
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
				return;
			}
			if (this.CanvasImage != null)
			{
				this.CanvasImage.material = this.tempMaterial;
				return;
			}
		}
		else
		{
			this.ForceMaterial.shader = Shader.Find(this.shader);
			this.ForceMaterial.hideFlags = HideFlags.None;
			if (this.CanvasSpriteRenderer != null)
			{
				this.CanvasSpriteRenderer.sharedMaterial = this.ForceMaterial;
				return;
			}
			if (this.CanvasImage != null)
			{
				this.CanvasImage.material = this.ForceMaterial;
			}
		}
	}

	public Material ForceMaterial;

	public bool ActiveChange = true;

	public bool AddShadow = true;

	public bool ReceivedShadow;

	public int BlendMode;

	private string shader = "2DxFX/AL/GrassFX";

	public float _Alpha = 1f;

	public float Heat = 1f;

	public float Speed = 1f;

	private AnimationCurve Wind;

	private float WindTime1;

	public int ShaderChange;

	private Material tempMaterial;

	private Material defaultMaterial;

	private Image CanvasImage;

	private SpriteRenderer CanvasSpriteRenderer;

	public bool ActiveUpdate = true;
}
