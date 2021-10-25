using System;

namespace GGMatch3
{
	public class BasketBlocker : SlotComponent
	{
		private BasketBlocker.Settings settings
		{
			get
			{
				return Match3Settings.instance.basketBlockerSettings;
			}
		}

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

		public override bool isMovingElementRequired
		{
			get
			{
				return true;
			}
		}

		public override int sortingOrder
		{
			get
			{
				return this.initProperties.sortingOrder;
			}
		}

		public void Init(BasketBlocker.InitProperties initProperties)
		{
			this.initProperties = initProperties;
			this.level = initProperties.level;
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
			return true;
		}

		public override bool isDestroyedByMatchingNextTo
		{
			get
			{
				return true;
			}
		}

		public override bool isSlotMatchingSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isMoveIntoSlotSuspended
		{
			get
			{
				return true;
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

		private TransformBehaviour GetTransformBehaviour()
		{
			Chip slotComponent = this.slot.GetSlotComponent<Chip>();
			if (slotComponent != null)
			{
				return slotComponent.GetComponentBehaviour<TransformBehaviour>();
			}
			MovingElement slotComponent2 = this.slot.GetSlotComponent<MovingElement>();
			if (slotComponent2 != null)
			{
				return slotComponent2.GetComponentBehaviour<TransformBehaviour>();
			}
			return null;
		}

		public override void OnSlotComponentMadeAStartMove(SlotComponent.SlotStartMoveParams moveParams)
		{
			LightSlotComponent backLight = moveParams.fromSlot.backLight;
			backLight.AddLightWithDuration(backLight.maxIntensity, this.settings.lightDuration);
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
			TransformBehaviour transformBehaviour = this.GetTransformBehaviour();
			if (transformBehaviour == null)
			{
				return;
			}
			TransformBehaviour transformBehaviour2 = this.transformBehaviour;
			if (transformBehaviour2 == null)
			{
				return;
			}
			transformBehaviour2.localPosition = transformBehaviour.localPosition;
			transformBehaviour2.slotOffsetPosition = transformBehaviour.slotOffsetPosition;
		}

		public override SlotDestroyResolution OnDestroyNeighbourSlotComponent(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			if (destroyParams.isHitByBomb && !destroyParams.isBombAllowingNeighbourDestroy)
			{
				return result;
			}
			result.isDestroyed = true;
			result.stopPropagation = true;
			return this.DestroyLayer(destroyParams);
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
			game.Play(new GGSoundSystem.PlayParameters
			{
				soundType = GGSoundSystem.SFXType.ChocolateBreak,
				variationIndex = this.level
			});
			this.level--;
			MultiLayerItemBehaviour itemBehaviour = this.itemBehaviour;
			if (itemBehaviour != null)
			{
				itemBehaviour.SetLayerIndex(this.level - 1);
			}
			if (this.slot != null)
			{
				game.particles.CreateParticles(this.slot.localPositionOfCenter, Match3Particles.PositionType.BoxDestroy, ChipType.BasketBlocker, ItemColor.Unknown);
			}
			if (this.level > 0)
			{
				return result;
			}
			Chip chip = null;
			if (this.slot != null)
			{
				chip = this.slot.GetSlotComponent<Chip>();
			}
			if (this.initProperties.canFallthroughPickup && chip != null && chip.isPickupElement)
			{
				result.stopPropagation = false;
			}
			base.RemoveFromGame();
			return result;
		}

		private int level;

		private BasketBlocker.InitProperties initProperties;

		public struct InitProperties
		{
			public int level;

			public int sortingOrder;

			public bool canFallthroughPickup;
		}

		[Serializable]
		public class Settings
		{
			public float lightDuration = 0.5f;
		}
	}
}
