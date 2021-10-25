using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GGMatch3
{
	public class TransformBehaviour : SlotComponentBehavoiour
	{
		public void SetPartOfGoalActive(bool active)
		{
			GGUtil.SetActive(this.partOfGoalContainer, active);
		}

		public void SetShadowActive(bool active)
		{
			GGUtil.SetActive(this.shadowTransform, active);
		}

		public void AddChild(TransformBehaviour t)
		{
			if (t == null)
			{
				return;
			}
			t.transform.parent = this.childrenContainer;
			this.children.Add(t);
		}

		public Vector3 showMatchActionLocalScale
		{
			get
			{
				return this.showMatchActionLocalScale_;
			}
			set
			{
				this.showMatchActionLocalScale_ = value;
				this.ApplyScale();
			}
		}

		public Vector3 slotLocalScale
		{
			get
			{
				return this.slotLocalScale_;
			}
			set
			{
				this.slotLocalScale_ = value;
				this.ApplyScale();
			}
		}

		public Vector3 wobbleLocalScale
		{
			get
			{
				return this.wobbleLocalScale_;
			}
			set
			{
				this.wobbleLocalScale_ = value;
				this.ApplyScale();
			}
		}

		public Vector3 localScale
		{
			get
			{
				return this.localScale_;
			}
			set
			{
				this.localScale_ = value;
				this.ApplyScale();
			}
		}

		public Vector3 totalLocalScale
		{
			get
			{
				return Vector3.Scale(Vector3.Scale(Vector3.Scale(Vector3.Scale(Vector3.one, this.localScale_), this.showMatchActionLocalScale_), this.slotLocalScale_), this.wobbleLocalScale_);
			}
		}

		private void ApplyScale()
		{
			if (this.scalerTransform == null)
			{
				return;
			}
			this.scalerTransform.localScale = this.totalLocalScale;
		}

		public Vector3 WorldToLocalPosition(Vector3 worldPosition)
		{
			return base.transform.parent.InverseTransformPoint(worldPosition);
		}

		public Vector3 localPosition
		{
			get
			{
				return base.transform.localPosition;
			}
			set
			{
				base.transform.localPosition = value;
			}
		}

		public Vector3 slotOffsetPosition
		{
			get
			{
				return this.slotOffsetPosition_;
			}
			set
			{
				this.slotOffsetPosition_ = value;
				this.ApplyPosition();
			}
		}

		public List<TransformBehaviour.SortingLayerSettings> SaveSortingLayerSettings()
		{
			this.sortingSettings.Clear();
			for (int i = 0; i < this.spriteRenderers.Count; i++)
			{
				SpriteRenderer spriteRenderer = this.spriteRenderers[i];
				if (!(spriteRenderer == null))
				{
					TransformBehaviour.SortingLayerSettings sortingLayerSettings = new TransformBehaviour.SortingLayerSettings();
					sortingLayerSettings.sortingSettings.sortingLayerId = spriteRenderer.sortingLayerID;
					sortingLayerSettings.sortingSettings.sortingOrder = spriteRenderer.sortingOrder;
					sortingLayerSettings.spriteRenderer = spriteRenderer;
					this.sortingSettings.Add(sortingLayerSettings);
				}
			}
			for (int j = 0; j < this.clothRenderers.Count; j++)
			{
				SquareClothRenderer squareClothRenderer = this.clothRenderers[j];
				if (!(squareClothRenderer == null))
				{
					TransformBehaviour.SortingLayerSettings sortingLayerSettings2 = new TransformBehaviour.SortingLayerSettings();
					sortingLayerSettings2.sortingSettings.sortingLayerId = squareClothRenderer.sortingLayerID;
					sortingLayerSettings2.sortingSettings.sortingOrder = squareClothRenderer.sortingLayerOrder;
					sortingLayerSettings2.squareClothRenderer = squareClothRenderer;
					this.sortingSettings.Add(sortingLayerSettings2);
				}
			}
			return this.sortingSettings;
		}

		public void ResetSortingLayerSettings()
		{
			for (int i = 0; i < this.sortingSettings.Count; i++)
			{
				TransformBehaviour.SortingLayerSettings sortingLayerSettings = this.sortingSettings[i];
				sortingLayerSettings.sortingSettings.Set(sortingLayerSettings.spriteRenderer);
				if (sortingLayerSettings.squareClothRenderer != null)
				{
					sortingLayerSettings.squareClothRenderer.SetSortingLayers(sortingLayerSettings.sortingSettings.sortingLayerId, sortingLayerSettings.sortingSettings.sortingOrder);
				}
			}
		}

		private void ApplyPosition()
		{
			if (this.offsetTransform == null)
			{
				return;
			}
			this.offsetTransform.localPosition = this.slotOffsetPosition_ + this.localOffsetPosition_ + this.localPotentialMatchOffsetPosition_;
		}

		public Vector3 localOffsetPosition
		{
			get
			{
				return this.localOffsetPosition_;
			}
			set
			{
				this.localOffsetPosition_ = value;
				this.ApplyPosition();
			}
		}

		public Vector3 localPotentialMatchOffsetPosition
		{
			get
			{
				return this.localPotentialMatchOffsetPosition_;
			}
			set
			{
				this.localPotentialMatchOffsetPosition_ = value;
				this.ApplyPosition();
			}
		}

		public Quaternion localRotationOffset
		{
			set
			{
				base.transform.localRotation = value;
			}
		}

		public void SetAlpha(float alpha)
		{
			for (int i = 0; i < this.spriteRenderers.Count; i++)
			{
				SpriteRenderer spriteRenderer = this.spriteRenderers[i];
				if (!(spriteRenderer == null))
				{
					Color color = spriteRenderer.color;
					color.a = alpha;
					spriteRenderer.color = color;
				}
			}
			for (int j = 0; j < this.clothRenderers.Count; j++)
			{
				SquareClothRenderer squareClothRenderer = this.clothRenderers[j];
				if (!(squareClothRenderer == null))
				{
					squareClothRenderer.SetAlpha(alpha);
				}
			}
			for (int k = 0; k < this.textRenderers.Count; k++)
			{
				TextMeshPro textMeshPro = this.textRenderers[k];
				if (!(textMeshPro == null))
				{
					textMeshPro.alpha = alpha;
				}
			}
		}

		public void SetText(string text)
		{
			for (int i = 0; i < this.textRenderers.Count; i++)
			{
				TextMeshPro textMeshPro = this.textRenderers[i];
				if (!(textMeshPro == null))
				{
					textMeshPro.text = text;
				}
			}
		}

		public void SetColor(Color color)
		{
			for (int i = 0; i < this.spriteRenderers.Count; i++)
			{
				SpriteRenderer spriteRenderer = this.spriteRenderers[i];
				if (!(spriteRenderer == null))
				{
					spriteRenderer.color = color;
				}
			}
			for (int j = 0; j < this.clothRenderers.Count; j++)
			{
				SquareClothRenderer squareClothRenderer = this.clothRenderers[j];
				if (!(squareClothRenderer == null))
				{
					squareClothRenderer.SetColor(color);
				}
			}
			for (int k = 0; k < this.textRenderers.Count; k++)
			{
				TextMeshPro textMeshPro = this.textRenderers[k];
				if (!(textMeshPro == null))
				{
					textMeshPro.color = color;
				}
			}
		}

		public void SetBrightness(float brightness)
		{
			for (int i = 0; i < this.spriteRenderers.Count; i++)
			{
				SpriteRenderer spriteRenderer = this.spriteRenderers[i];
				if (!(spriteRenderer == null))
				{
					spriteRenderer.material.SetFloat("_ColorHSV_Brightness_1", brightness);
				}
			}
			for (int j = 0; j < this.clothRenderers.Count; j++)
			{
				SquareClothRenderer squareClothRenderer = this.clothRenderers[j];
				if (!(squareClothRenderer == null))
				{
					squareClothRenderer.SetBrightness(brightness);
				}
			}
		}

		public void SetSortingLayer(SpriteSortingSettings s)
		{
			this.SetSortingLayer(s.sortingLayerId, s.sortingOrder);
		}

		public void SetSortingLayer(int sortingLayerId, int orderInLayer)
		{
			for (int i = 0; i < this.spriteRenderers.Count; i++)
			{
				SpriteRenderer spriteRenderer = this.spriteRenderers[i];
				if (!(spriteRenderer == null))
				{
					spriteRenderer.sortingLayerID = sortingLayerId;
					spriteRenderer.sortingOrder = orderInLayer;
				}
			}
			for (int j = 0; j < this.clothRenderers.Count; j++)
			{
				SquareClothRenderer squareClothRenderer = this.clothRenderers[j];
				if (!(squareClothRenderer == null))
				{
					squareClothRenderer.SetSortingLayers(sortingLayerId, orderInLayer);
				}
			}
			for (int k = 0; k < this.children.Count; k++)
			{
				TransformBehaviour transformBehaviour = this.children[k];
				if (!(transformBehaviour == null))
				{
					transformBehaviour.SetSortingLayer(sortingLayerId, orderInLayer + k + 1);
				}
			}
		}

		public void ForceRemoveFromGame()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public override void RemoveFromGame()
		{
			if (this.destroyWhenRemovedFromGame)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private List<TransformBehaviour.SortingLayerSettings> sortingSettings = new List<TransformBehaviour.SortingLayerSettings>();

		[SerializeField]
		private Transform shadowTransform;

		[SerializeField]
		private Transform partOfGoalContainer;

		[SerializeField]
		private bool destroyWhenRemovedFromGame;

		[SerializeField]
		private Transform offsetTransform;

		[SerializeField]
		public Transform scalerTransform;

		[SerializeField]
		private Transform childrenContainer;

		private List<TransformBehaviour> children = new List<TransformBehaviour>();

		[SerializeField]
		private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

		[SerializeField]
		private List<SquareClothRenderer> clothRenderers = new List<SquareClothRenderer>();

		[SerializeField]
		private List<TextMeshPro> textRenderers = new List<TextMeshPro>();

		private Vector3 showMatchActionLocalScale_ = Vector3.one;

		private Vector3 slotLocalScale_ = Vector3.one;

		private Vector3 wobbleLocalScale_ = Vector3.one;

		private Vector3 localScale_ = Vector3.one;

		private Vector3 slotOffsetPosition_ = Vector3.zero;

		private Vector3 localOffsetPosition_;

		private Vector3 localPotentialMatchOffsetPosition_;

		public class SortingLayerSettings
		{
			public SpriteRenderer spriteRenderer;

			public SquareClothRenderer squareClothRenderer;

			public SpriteSortingSettings sortingSettings = new SpriteSortingSettings();
		}
	}
}
