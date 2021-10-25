using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[Serializable]
public class _2dxFX_Offset : MonoBehaviour
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
				if (this._AutoScrollX)
				{
					this._AutoScrollCountX += this._AutoScrollSpeedX * 0.01f * Time.deltaTime;
					if (this._AutoScrollCountX < 0f)
					{
						this._AutoScrollCountX = 1f;
					}
					this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_OffsetX", 1f + this._AutoScrollCountX);
				}
				else
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_OffsetX", 1f + this._OffsetX);
				}
				if (this._AutoScrollY)
				{
					this._AutoScrollCountY += this._AutoScrollSpeedY * 0.01f * Time.deltaTime;
					if (this._AutoScrollCountY < 0f)
					{
						this._AutoScrollCountY = 1f;
					}
					this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_OffsetY", 1f + this._AutoScrollCountY);
				}
				else
				{
					this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_OffsetY", 1f + this._OffsetY);
				}
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_ZoomX", this._ZoomX * this._ZoomXY);
				this.CanvasSpriteRenderer.sharedMaterial.SetFloat("_ZoomY", this._ZoomY * this._ZoomXY);
				return;
			}
			if (this.CanvasImage != null)
			{
				this.CanvasImage.material.SetFloat("_Alpha", 1f - this._Alpha);
				if (this._AutoScrollX)
				{
					this._AutoScrollCountX += this._AutoScrollSpeedX * 0.01f * Time.deltaTime;
					if (this._AutoScrollCountX < 0f)
					{
						this._AutoScrollCountX = 1f;
					}
					this.CanvasImage.material.SetFloat("_OffsetX", 1f + this._AutoScrollCountX);
				}
				else
				{
					this.CanvasImage.material.SetFloat("_OffsetX", 1f + this._OffsetX);
				}
				if (this._AutoScrollY)
				{
					this._AutoScrollCountY += this._AutoScrollSpeedY * 0.01f * Time.deltaTime;
					if (this._AutoScrollCountY < 0f)
					{
						this._AutoScrollCountY = 1f;
					}
					this.CanvasImage.material.SetFloat("_OffsetY", 1f + this._AutoScrollCountY);
				}
				else
				{
					this.CanvasImage.material.SetFloat("_OffsetY", 1f + this._OffsetY);
				}
				this.CanvasImage.material.SetFloat("_ZoomX", this._ZoomX * this._ZoomXY);
				this.CanvasImage.material.SetFloat("_ZoomY", this._ZoomY * this._ZoomXY);
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

	private string shader = "2DxFX/Standard/Offset";

	public float _Alpha = 1f;

	public float _OffsetX;

	public float _OffsetY;

	public float _ZoomX = 1f;

	public float _ZoomY = 1f;

	public float _ZoomXY = 1f;

	public bool _AutoScrollX;

	public float _AutoScrollSpeedX;

	public bool _AutoScrollY;

	public float _AutoScrollSpeedY;

	private float _AutoScrollCountX;

	private float _AutoScrollCountY;

	public int ShaderChange;

	private Material tempMaterial;

	private Material defaultMaterial;

	private Image CanvasImage;

	private SpriteRenderer CanvasSpriteRenderer;

	public bool ActiveUpdate = true;
}
