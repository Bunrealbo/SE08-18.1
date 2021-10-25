using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[ExecuteInEditMode]
[Serializable]
public class _2dxFX_AL_JellyAutoMove : MonoBehaviour
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
		this.Heat = 0f;
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
		this.SaveMove1 = base.transform.position;
		if (this.SaveMove1.x != this.SaveMove2.x)
		{
			this.Heat += 0.5f;
		}
		if (this.SaveMove1.y != this.SaveMove2.y)
		{
			this.Heat += 0.5f;
		}
		if (this.SaveMove1.z != this.SaveMove2.z)
		{
			this.Heat += 0.5f;
		}
		this.Heat -= Time.deltaTime * this.Stabilisation;
		if (this.Heat > 4f)
		{
			this.Heat = 4f;
		}
		if (this.Heat < 0f)
		{
			this.RandomPos = (float)UnityEngine.Random.Range(0, 256);
			this.Heat = 0f;
		}
		this.SaveMove2 = this.SaveMove1;
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
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_RandomPos", this.RandomPos);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Inside", this.Inside);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_Speed", this.Speed);
				return;
			}
			if (this.CanvasImage != null)
			{
				this.CanvasImage.material.SetFloat("_Alpha", 1f - this._Alpha);
				this.CanvasImage.material.SetFloat("_Distortion", this.Heat);
				this.CanvasImage.material.SetFloat("_RandomPos", this.RandomPos);
				this.CanvasImage.material.SetFloat("_Inside", this.Inside);
				this.CanvasImage.material.SetFloat("_Speed", this.Speed);
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

	private string shader = "2DxFX/AL/Jelly";

	public float _Alpha = 1f;

	public float Heat = 1f;

	public float RandomPos = 1f;

	public float Inside = 1f;

	public float Stabilisation = 4f;

	public float Speed = 1f;

	public int ShaderChange;

	private Material tempMaterial;

	private Material defaultMaterial;

	private Image CanvasImage;

	private Vector3 SaveMove1;

	private Vector3 SaveMove2;

	private SpriteRenderer CanvasSpriteRenderer;

	public bool ActiveUpdate = true;
}
