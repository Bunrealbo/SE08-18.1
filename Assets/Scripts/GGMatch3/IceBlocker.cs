using System;

namespace GGMatch3
{
	public class IceBlocker : SlotComponent
	{
		private MultiLayerItemBehaviour itemBehaviour
		{
			get
			{
				return base.GetComponentBehaviour<MultiLayerItemBehaviour>();
			}
		}

		private TransformBehaviour transformBehaviour
		{
			get
			{
				return base.GetComponentBehaviour<TransformBehaviour>();
			}
		}

		private IceBehaviour iceBehaviour
		{
			get
			{
				TransformBehaviour transformBehaviour = this.transformBehaviour;
				if (transformBehaviour == null)
				{
					return null;
				}
				return transformBehaviour.GetComponent<IceBehaviour>();
			}
		}

		public override int sortingOrder
		{
			get
			{
				return this.initProperties.sortingOrder;
			}
		}

		public void Init(IceBlocker.InitProperties initProperties)
		{
			this.initProperties = initProperties;
			this.level = initProperties.level;
			IceBehaviour iceBehaviour = this.iceBehaviour;
			if (iceBehaviour != null)
			{
				iceBehaviour.Init(initProperties.chip, this.level);
			}
		}

		public override bool isBlockingChip
		{
			get
			{
				return true;
			}
		}

		public override int blockerLevel
		{
			get
			{
				return this.level;
			}
		}

		public override bool IsAvailableForDiscoBombSuspended(bool replaceWithBombs)
		{
			return replaceWithBombs;
		}

		public override bool isSlotMatchingSuspended
		{
			get
			{
				return false;
			}
		}

		public override bool isMoveIntoSlotSuspended
		{
			get
			{
				return false;
			}
		}

		public override bool isSlotGravitySuspended
		{
			get
			{
				return false;
			}
		}

		public override bool isMovedByGravity
		{
			get
			{
				return true;
			}
		}

		public override bool isSlotSwapSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isPreventingGravity
		{
			get
			{
				return false;
			}
		}

		public override bool isAttachGrowingElementSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isPowerupReplacementSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isCreatePowerupWithThisSlotSuspended
		{
			get
			{
				return false;
			}
		}

		public override bool isMovingWithConveyor
		{
			get
			{
				return true;
			}
		}

		public override void OnSlotComponentMadeATransformChange(SlotComponent component)
		{
			if (this.slot == null)
			{
				return;
			}
			TransformBehaviour componentBehaviour = component.GetComponentBehaviour<TransformBehaviour>();
			if (componentBehaviour == null)
			{
				return;
			}
			TransformBehaviour transformBehaviour = this.transformBehaviour;
			if (transformBehaviour == null)
			{
				return;
			}
			transformBehaviour.localPosition = componentBehaviour.localPosition;
			transformBehaviour.slotOffsetPosition = componentBehaviour.slotOffsetPosition;
		}

		public override void OnUpdate(float deltaTime)
		{
			if (this.slot == null)
			{
				return;
			}
			this.wobbleAnimation.Update(deltaTime);
			Chip slotComponent = this.slot.GetSlotComponent<Chip>();
			IceBehaviour iceBehaviour = this.iceBehaviour;
			if (iceBehaviour != null)
			{
				iceBehaviour.TryInitIfDifferent(slotComponent, this.level);
			}
			if (slotComponent == null)
			{
				return;
			}
			TransformBehaviour componentBehaviour = slotComponent.GetComponentBehaviour<TransformBehaviour>();
			if (componentBehaviour == null)
			{
				return;
			}
			TransformBehaviour transformBehaviour = this.transformBehaviour;
			if (transformBehaviour == null)
			{
				return;
			}
			transformBehaviour.localPosition = componentBehaviour.localPosition;
			transformBehaviour.slotOffsetPosition = componentBehaviour.slotOffsetPosition;
		}

		public override SlotDestroyResolution OnDestroyNeighbourSlotComponent(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			return new SlotDestroyResolution
			{
				stopPropagation = true
			};
		}

		public override SlotDestroyResolution OnDestroySlotComponent(SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			result.isDestroyed = true;
			if (destroyParams.isFromTap)
			{
				result.stopPropagation = true;
				return result;
			}
			result.stopPropagation = true;
			result.isNeigbourDestroySuspendedForThisChipOnly = true;
			return this.DestroyLayer(destroyParams);
		}

		private SlotDestroyResolution DestroyLayer(SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			result.isDestroyed = true;
			result.isNeigbourDestroySuspendedForThisChipOnly = true;
			result.stopPropagation = true;
			Match3Game game = this.lastConnectedSlot.game;
			this.level--;
			MultiLayerItemBehaviour itemBehaviour = this.itemBehaviour;
			if (itemBehaviour != null)
			{
				itemBehaviour.SetLayerIndex(this.level - 1);
			}
			game.Play(GGSoundSystem.SFXType.BreakIce);
			if (this.slot != null)
			{
				game.particles.CreateParticles(this.slot.localPositionOfCenter, Match3Particles.PositionType.BoxDestroy, ChipType.IceOnChip, ItemColor.Unknown);
				this.slot.Wobble(Match3Settings.instance.chipWobbleSettings);
			}
			IceBehaviour iceBehaviour = this.iceBehaviour;
			if (this.level > 0)
			{
				if (iceBehaviour != null)
				{
					iceBehaviour.Init(this.slot.GetSlotComponent<Chip>(), this.level);
				}
				return result;
			}
			if (iceBehaviour != null)
			{
				iceBehaviour.DoOnDestroy(this.lastConnectedSlot.GetSlotComponent<Chip>());
			}
			base.RemoveFromGame();
			return result;
		}

		public override void Wobble(WobbleAnimation.Settings settings)
		{
			if (settings == null)
			{
				return;
			}
			this.wobbleAnimation.Init(settings, this.transformBehaviour);
		}

		private int level;

		private IceBlocker.InitProperties initProperties;

		private WobbleAnimation wobbleAnimation = new WobbleAnimation();

		public struct InitProperties
		{
			public int level;

			public int sortingOrder;

			public Chip chip;
		}
	}
}
