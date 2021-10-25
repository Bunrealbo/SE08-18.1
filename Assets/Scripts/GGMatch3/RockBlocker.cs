using System;

namespace GGMatch3
{
	public class RockBlocker : SlotComponent
	{
		private MultiLayerItemBehaviour itemBehaviour
		{
			get
			{
				return base.GetComponentBehaviour<MultiLayerItemBehaviour>();
			}
		}

		public override bool isBlockingBurriedElement
		{
			get
			{
				return true;
			}
		}

		public override bool isBlockingCarpetSpread
		{
			get
			{
				return true;
			}
		}

		public override bool isAttachGrowingElementSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isPlaceBubbleSuspended
		{
			get
			{
				return true;
			}
		}

		private ChipType chipType
		{
			get
			{
				return ChipType.RockBlocker;
			}
		}

		public override void AddToGoalsAtStart(Match3Goals goals)
		{
			goals.GetChipTypeCounter(new Match3Goals.ChipTypeDef
			{
				chipType = this.chipType,
				itemColor = ItemColor.Unknown
			}).countAtStart += this.level;
		}

		public override bool IsCompatibleWithPickupGoal(Match3Goals.ChipTypeDef chipTypeDef)
		{
			return chipTypeDef.chipType == this.chipType;
		}

		public override int sortingOrder
		{
			get
			{
				return this.sortingOrder_;
			}
		}

		public void Init(RockBlocker.InitArguments initArguments)
		{
			this.initArguments = initArguments;
			this.level = initArguments.level;
			this.sortingOrder_ = initArguments.sortingOrder;
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
				return this.level + 1;
			}
		}

		public override bool IsAvailableForDiscoBombSuspended(bool replaceWithBombs)
		{
			return true;
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

		public override bool isPreventingOtherChipsToFallIntoSlot
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

		public override SlotDestroyResolution OnDestroyNeighbourSlotComponent(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			if (this.level > 0)
			{
				result.stopPropagation = true;
				return result;
			}
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
			Match3Game game = this.lastConnectedSlot.game;
			if (this.level > 0 && !destroyParams.isHitByBomb)
			{
				if (this.initArguments.cancelsSnow && game != null)
				{
					game.board.bubblesBoardComponent.CancelSpread();
				}
				return result;
			}
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
			Match3Goals.ChipTypeDef chipTypeDef = default(Match3Goals.ChipTypeDef);
			chipTypeDef.chipType = this.chipType;
			chipTypeDef.itemColor = ItemColor.Unknown;
			Match3Goals.GoalBase activeGoal = game.goals.GetActiveGoal(chipTypeDef);
			if (activeGoal != null)
			{
				destroyParams.goalsCollected++;
				game.OnPickupGoal(new GoalCollectParams(activeGoal, destroyParams));
			}
			game.Play(new GGSoundSystem.PlayParameters
			{
				soundType = GGSoundSystem.SFXType.RockBreak,
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
				game.particles.CreateParticles(this.slot.localPositionOfCenter, Match3Particles.PositionType.BoxDestroy, this.chipType, ItemColor.Unknown);
				this.slot.Wobble(Match3Settings.instance.chipWobbleSettings);
			}
			CollectPointsAction.OnBlockerDestroy(this.lastConnectedSlot, destroyParams);
			if (this.level >= 0)
			{
				return result;
			}
			if (this.slot != null)
			{
				this.slot.GetSlotComponent<Chip>();
			}
			if (this.initArguments.cancelsSnow)
			{
				game.board.bubblesBoardComponent.CancelSpread();
			}
			base.RemoveFromGame();
			return result;
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.wobbleAnimation.Update(deltaTime);
		}

		public override void Wobble(WobbleAnimation.Settings settings)
		{
			if (settings == null)
			{
				return;
			}
			this.wobbleAnimation.Init(settings, base.GetComponentBehaviour<TransformBehaviour>());
		}

		private RockBlocker.InitArguments initArguments;

		private int level;

		private WobbleAnimation wobbleAnimation = new WobbleAnimation();

		private int sortingOrder_;

		public struct InitArguments
		{
			public int level;

			public bool cancelsSnow;

			public int sortingOrder;
		}
	}
}
