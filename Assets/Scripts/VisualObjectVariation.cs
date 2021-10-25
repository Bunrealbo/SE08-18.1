using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class VisualObjectVariation : MonoBehaviour
{
	public Sprite thumbnailSprite
	{
		get
		{
			if (this.savedThumbnailSprite != null)
			{
				return this.savedThumbnailSprite;
			}
			string thumbnailNamePrefix = this.visualObjectBehaviour.visualObject.sceneObjectInfo.thumbnailNamePrefix;
			if (!string.IsNullOrEmpty(thumbnailNamePrefix))
			{
				for (int i = 0; i < this.sprites.Count; i++)
				{
					VisualSprite visualSprite = this.sprites[i];
					if (!visualSprite.visualSprite.isShadow && !(visualSprite.spriteRenderer == null))
					{
						Sprite sprite = visualSprite.spriteRenderer.sprite;
						if (visualSprite.visualSprite.spriteName.ToLower().StartsWith(thumbnailNamePrefix.ToLower()))
						{
							return sprite;
						}
					}
				}
			}
			for (int j = 0; j < this.sprites.Count; j++)
			{
				VisualSprite visualSprite2 = this.sprites[j];
				if (!visualSprite2.visualSprite.isShadow && visualSprite2.spriteRenderer != null)
				{
					return visualSprite2.spriteRenderer.sprite;
				}
			}
			return null;
		}
	}

	public void SetActive(bool isActive)
	{
		this.animationEnum = null;
		if (isActive)
		{
			this.ResetSprites();
		}
		GGUtil.SetActive(base.gameObject, isActive);
	}

	public void SetVariationActive(string variationName)
	{
		this.SetActive(base.name == variationName);
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public VisualSprite CreateSprite(GraphicsSceneConfig.VisualSprite vSprite)
	{
		VisualSprite visualSprite = new GameObject(vSprite.spriteName)
		{
			transform = 
			{
				parent = base.transform
			}
		}.AddComponent<VisualSprite>();
		visualSprite.Init(vSprite);
		return visualSprite;
	}

	public void Init(VisualObjectBehaviour visualObjectBehaviour, GraphicsSceneConfig.Variation variation)
	{
		this.visualObjectBehaviour = visualObjectBehaviour;
		this.variation = variation;
		this.savedThumbnailSprite = variation.thumbnailSprite;
		for (int i = 0; i < variation.sprites.Count; i++)
		{
			GraphicsSceneConfig.VisualSprite vSprite = variation.sprites[i];
			VisualSprite item = this.CreateSprite(vSprite);
			this.sprites.Add(item);
		}
	}

	public void DestroySelf()
	{
		VisualObjectVariation.Destroy(base.gameObject);
	}

	public static void Destroy(GameObject obj)
	{
		if (!Application.isPlaying)
		{
			UnityEngine.Object.DestroyImmediate(obj);
			return;
		}
		UnityEngine.Object.Destroy(obj);
	}

	private void ResetSprites()
	{
		for (int i = 0; i < this.sprites.Count; i++)
		{
			this.sprites[i].ResetVisually();
		}
	}

	public void ScaleAnimation(float delay, bool hide = false)
	{
		this.ResetSprites();
		this.animationEnum = this.DoScaleAnimation(delay, hide);
	}

	private IEnumerator DoScaleAnimation(float delay, bool hide)
	{
		return new VisualObjectVariation._003CDoScaleAnimation_003Ed__16(0)
		{
			_003C_003E4__this = this,
			delay = delay,
			hide = hide
		};
	}

	private void Update()
	{
		if (this.animationEnum == null)
		{
			return;
		}
		if (!this.animationEnum.MoveNext())
		{
			this.animationEnum = null;
		}
	}

	public Sprite savedThumbnailSprite;

	public List<VisualSprite> sprites = new List<VisualSprite>();

	[NonSerialized]
	public GraphicsSceneConfig.Variation variation;

	public VisualObjectBehaviour visualObjectBehaviour;

	private IEnumerator animationEnum;

	private sealed class _003CDoScaleAnimation_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoScaleAnimation_003Ed__16(int _003C_003E1__state)
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
			VisualObjectVariation visualObjectVariation = this._003C_003E4__this;
			switch (num)
			{
			case 0:
				this._003C_003E1__state = -1;
				this._003CanimatedSprites_003E5__2 = new List<VisualSprite>();
				for (int i = 0; i < visualObjectVariation.sprites.Count; i++)
				{
					VisualSprite visualSprite = visualObjectVariation.sprites[i];
					if (!visualSprite.visualSprite.isShadow)
					{
						this._003CanimatedSprites_003E5__2.Add(visualSprite);
					}
				}
				this._003Cconfig_003E5__3 = ScriptableObjectSingleton<DecoratingSceneConfig>.instance.GetScaleAnimationSettingsOrDefault(visualObjectVariation.visualObjectBehaviour.visualObject.animationSettingsName);
				this._003Ctime_003E5__4 = 0f;
				this._003CstartScale_003E5__5 = this._003Cconfig_003E5__3.scaleFrom;
				if (this.delay <= 0f)
				{
					goto IL_15C;
				}
				if (this.hide)
				{
					for (int j = 0; j < this._003CanimatedSprites_003E5__2.Count; j++)
					{
						VisualSprite visualSprite2 = this._003CanimatedSprites_003E5__2[j];
						Color color = visualSprite2.spriteRenderer.color;
						color.a = 0.1f;
						visualSprite2.spriteRenderer.color = color;
					}
				}
				break;
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_2C3;
			default:
				return false;
			}
			if (this._003Ctime_003E5__4 < this.delay)
			{
				this._003Ctime_003E5__4 += Time.deltaTime;
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003Ctime_003E5__4 -= this.delay;
			IL_15C:
			this._003Cduration_003E5__6 = this._003Cconfig_003E5__3.duration;
			IL_2C3:
			if (this._003Ctime_003E5__4 >= this._003Cduration_003E5__6)
			{
				return false;
			}
			this._003Ctime_003E5__4 += Time.deltaTime;
			float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__6, this._003Ctime_003E5__4);
			float num2 = this._003Cconfig_003E5__3.scaleCurve.Evaluate(time);
			Vector3 localScale = Vector3.one;
			if (num2 > 1f)
			{
				localScale = Vector3.LerpUnclamped(this._003Cconfig_003E5__3.scaleFrom, Vector3.one, num2);
			}
			else
			{
				localScale = Vector3.LerpUnclamped(this._003CstartScale_003E5__5, Vector3.one, num2);
			}
			for (int k = 0; k < this._003CanimatedSprites_003E5__2.Count; k++)
			{
				this._003CanimatedSprites_003E5__2[k].pivotTransform.localScale = localScale;
			}
			float t = this._003Cconfig_003E5__3.localPositionCurve.Evaluate(time);
			Vector3 b = Vector3.LerpUnclamped(this._003Cconfig_003E5__3.localPositionFrom, Vector3.zero, t);
			for (int l = 0; l < this._003CanimatedSprites_003E5__2.Count; l++)
			{
				VisualSprite visualSprite3 = this._003CanimatedSprites_003E5__2[l];
				visualSprite3.pivotTransform.localPosition = visualSprite3.visualSprite.pivotPosition + b;
				visualSprite3.spriteRenderer.color = Color.white;
			}
			this._003C_003E2__current = null;
			this._003C_003E1__state = 2;
			return true;
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

		public VisualObjectVariation _003C_003E4__this;

		public float delay;

		public bool hide;

		private List<VisualSprite> _003CanimatedSprites_003E5__2;

		private DecoratingSceneConfig.ScaleAnimationSettings _003Cconfig_003E5__3;

		private float _003Ctime_003E5__4;

		private Vector3 _003CstartScale_003E5__5;

		private float _003Cduration_003E5__6;
	}
}
