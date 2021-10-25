using System;
using UnityEngine;

namespace GGMatch3
{
	public class ChipBehaviour : SlotComponentBehavoiour
	{
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

		public bool hasBounce
		{
			get
			{
				return !this.cloth.directlyFollow;
			}
			set
			{
				this.cloth.directlyFollow = !value;
			}
		}

		public void Init(Chip chip)
		{
			this.chip = chip;
			this.clothRenderer.SetClothTexture(chip.itemColor);
			chip.SetTransformToMove(base.transform);
			this.clothRenderer.UpdateMaterialSettings();
			GGUtil.SetActive(this.feather, false);
		}

		public void SetFeatherActive(bool active)
		{
			GGUtil.SetActive(this.feather, active);
		}

		public void ChangeClothTexture(ItemColor itemColor)
		{
			this.clothRenderer.SetClothTexture(this.chip.itemColor);
			this.ResetVisually();
		}

		public void ResetCloth()
		{
			this.cloth.Init();
		}

		public void ResetVisually()
		{
			if (this.chip != null && this.chip.slot != null)
			{
				base.transform.localPosition = this.chip.slot.localPositionOfCenter;
			}
			this.cloth.Init();
		}

		public void StartChipDestroyAnimation(GameObject particles)
		{
			if (this.chipAnimator == null)
			{
				return;
			}
			this.chipAnimator.gameObject.SetActive(true);
			if (particles != null && this.particleParent != null)
			{
				this.prevParticleParent = particles.transform.parent;
				this.particlesGameObject = particles;
				particles.transform.parent = this.particleParent;
			}
		}

		public void SetBrightness(float brightness)
		{
			this.clothRenderer.SetBrightness(brightness);
		}

		public override void RemoveFromGame()
		{
			if (this.particlesGameObject != null)
			{
				this.particlesGameObject.transform.parent = this.prevParticleParent;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void SetActive(bool active)
		{
			GGUtil.SetActive(this, active);
		}

		private void LateUpdate()
		{
			if (this.chipAnimator == null)
			{
				return;
			}
			if (!this.chipAnimator.gameObject.activeSelf)
			{
				return;
			}
			TransformBehaviour component = base.GetComponent<TransformBehaviour>();
			if (component == null)
			{
				return;
			}
			component.localScale = this.chipTransform.localScale;
		}

		[SerializeField]
		private SquareClothRenderer clothRenderer;

		[SerializeField]
		private ClothDemo cloth;

		[NonSerialized]
		private Chip chip;

		[SerializeField]
		private Transform feather;

		[SerializeField]
		private Transform partOfGoal;

		[SerializeField]
		private Animator chipAnimator;

		[SerializeField]
		private Transform chipTransform;

		[SerializeField]
		private Transform particleParent;

		private GameObject particlesGameObject;

		private Transform prevParticleParent;
	}
}
