using System;
using UnityEngine;

namespace GGMatch3
{
	public class NetMatchOrDestroyNextToLock : SlotComponent
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
				return this.initProperties.isBlockingBurriedElement;
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
				return this.initProperties.isAttachGrowingElementSuspended;
			}
		}

		public override bool isPlaceBubbleSuspended
		{
			get
			{
				return true;
			}
		}

		public override void AddToGoalsAtStart(Match3Goals goals)
		{
			goals.GetChipTypeCounter(new Match3Goals.ChipTypeDef
			{
				chipType = this.initProperties.chipType,
				itemColor = ItemColor.Unknown
			}).countAtStart += this.level;
		}

		public override bool IsCompatibleWithPickupGoal(Match3Goals.ChipTypeDef chipTypeDef)
		{
			return chipTypeDef.chipType == this.initProperties.chipType;
		}

		public override int sortingOrder
		{
			get
			{
				return this.initProperties.sortingOrder;
			}
		}

		public void Init(NetMatchOrDestroyNextToLock.InitProperties initProperties)
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
			if (replaceWithBombs)
			{
				return !this.initProperties.isAvailableForDiscoBombReplaceBombs;
			}
			return this.initProperties.isAvailableForDiscoBombSuspended;
		}

		public override bool isSlotMatchingSuspended
		{
			get
			{
				return this.initProperties.isSlotMatchingSuspended;
			}
		}

		public override bool isMoveIntoSlotSuspended
		{
			get
			{
				return this.initProperties.isMoveIntoSlotSuspended;
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

		public override bool isDestroyedByMatchingNextTo
		{
			get
			{
				return !this.initProperties.isDestroyByMatchingNeighborSuspended;
			}
		}

		public override SlotDestroyResolution OnDestroyNeighbourSlotComponent(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			if (this.initProperties.isDestroyByMatchingNeighborSuspended)
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
			Slot lastConnectedSlot = this.lastConnectedSlot;
			CollectPointsAction.OnBlockerDestroy(this.lastConnectedSlot, destroyParams);
			Match3Goals.ChipTypeDef chipTypeDef = default(Match3Goals.ChipTypeDef);
			chipTypeDef.chipType = this.initProperties.chipType;
			chipTypeDef.itemColor = ItemColor.Unknown;
			Match3Goals.GoalBase activeGoal = game.goals.GetActiveGoal(chipTypeDef);
			if (activeGoal != null)
			{
				destroyParams.goalsCollected++;
				game.OnPickupGoal(new GoalCollectParams(activeGoal, destroyParams));
			}
			if (this.initProperties.useSound)
			{
				game.Play(new GGSoundSystem.PlayParameters
				{
					soundType = this.initProperties.soundType,
					variationIndex = this.level
				});
			}
			this.level--;
			MultiLayerItemBehaviour itemBehaviour = this.itemBehaviour;
			if (itemBehaviour != null)
			{
				itemBehaviour.SetLayerIndex(Mathf.Max(0, this.level - 1));
			}
			Chip chip = null;
			if (lastConnectedSlot != null)
			{
				chip = lastConnectedSlot.GetSlotComponent<Chip>();
			}
			if (lastConnectedSlot != null && this.initProperties.wobbleSettings != null)
			{
				lastConnectedSlot.Wobble(this.initProperties.wobbleSettings);
			}
			if (this.level > 0)
			{
				if (this.slot != null)
				{
					game.particles.CreateParticles(lastConnectedSlot.localPositionOfCenter, Match3Particles.PositionType.BoxDestroy, this.initProperties.chipType, this.level);
				}
				return result;
			}
			if (this.initProperties.canFallthroughPickup && chip != null && chip.isPickupElement)
			{
				result.stopPropagation = false;
			}
			base.RemoveFromSlot();
			if (this.initProperties.chipType == ChipType.Box)
			{
				DestroyBoxAction.InitArguments initArguments = default(DestroyBoxAction.InitArguments);
				initArguments.slot = this.lastConnectedSlot;
				initArguments.chip = this;
				DestroyBoxAction destroyBoxAction = new DestroyBoxAction();
				destroyBoxAction.Init(initArguments);
				game.board.actionManager.AddAction(destroyBoxAction);
				return result;
			}
			if (lastConnectedSlot != null)
			{
				game.particles.CreateParticles(lastConnectedSlot.localPositionOfCenter, Match3Particles.PositionType.BoxDestroy, this.initProperties.chipType, this.level);
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
			this.wobbleAnimation.Init(this.initProperties.wobbleSettings, base.GetComponentBehaviour<TransformBehaviour>());
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.wobbleAnimation.Update(deltaTime);
		}

		private WobbleAnimation wobbleAnimation = new WobbleAnimation();

		private int level;

		private NetMatchOrDestroyNextToLock.InitProperties initProperties;

		public struct InitProperties
		{
			public ChipType displayChipType
			{
				get
				{
					if (this.overrideDisplayChipType)
					{
						return this.displayChipType_;
					}
					return this.chipType;
				}
			}

			public void SetDisplayChipType(ChipType type)
			{
				this.overrideDisplayChipType = true;
				this.displayChipType_ = type;
			}

			public bool isMoveIntoSlotSuspended;

			public bool isSlotMatchingSuspended;

			public bool isAvailableForDiscoBombSuspended;

			public bool isBlockingBurriedElement;

			public bool isAvailableForDiscoBombReplaceBombs;

			public bool isAttachGrowingElementSuspended;

			public WobbleAnimation.Settings wobbleSettings;

			public bool isDestroyByMatchingNeighborSuspended;

			public bool canFallthroughPickup;

			public int level;

			public ChipType chipType;

			public int sortingOrder;

			private bool overrideDisplayChipType;

			private ChipType displayChipType_;

			public bool useSound;

			public GGSoundSystem.SFXType soundType;
		}
	}
}
