using System;

namespace GGMatch3
{
	public class SlotColorSlate : SlotComponent
	{
		private MultiLayerItemBehaviour itemBehaviour
		{
			get
			{
				return base.GetComponentBehaviour<MultiLayerItemBehaviour>();
			}
		}

		public override bool isBlockingChip
		{
			get
			{
				return false;
			}
		}

		public override bool isDestroyedByMatchingNextTo
		{
			get
			{
				return false;
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

		public override int blockerLevel
		{
			get
			{
				return this.level;
			}
		}

		public override int sortingOrder
		{
			get
			{
				return 5;
			}
		}

		public void Init(int level)
		{
			this.level = level;
		}

		private void DestroyLayer(SlotDestroyParams destroyParams)
		{
			Match3Game game = this.lastConnectedSlot.game;
			this.level--;
			if (!game.board.burriedElements.ContainsPosition(this.slot.position))
			{
				this.level = 0;
			}
			MultiLayerItemBehaviour itemBehaviour = this.itemBehaviour;
			if (itemBehaviour != null)
			{
				itemBehaviour.SetLayerIndex(this.level - 1);
			}
			if (this.slot != null && this.level > 0)
			{
				game.particles.CreateParticles(this.slot.localPositionOfCenter, Match3Particles.PositionType.BoxDestroy, ChipType.PickupGrass, ItemColor.Unknown);
			}
			game.Play(new GGSoundSystem.PlayParameters
			{
				soundType = GGSoundSystem.SFXType.BreakColorSlate,
				variationIndex = this.level
			});
			if (this.level > 0)
			{
				return;
			}
			Match3Goals.ChipTypeDef chipTypeDef = default(Match3Goals.ChipTypeDef);
			chipTypeDef.chipType = ChipType.PickupGrass;
			chipTypeDef.itemColor = ItemColor.Unknown;
			Match3Goals.GoalBase activeGoal = game.goals.GetActiveGoal(chipTypeDef);
			if (activeGoal != null)
			{
				destroyParams.goalsCollected++;
				game.OnPickupGoal(new GoalCollectParams(activeGoal, destroyParams));
			}
			game.board.burriedElements.OnSlateDestroyed(this.slot, destroyParams);
			base.RemoveFromGame();
		}

		public override bool isMovingWithConveyor
		{
			get
			{
				return false;
			}
		}

		public override void AddToGoalsAtStart(Match3Goals goals)
		{
			goals.GetChipTypeCounter(new Match3Goals.ChipTypeDef
			{
				chipType = ChipType.PickupGrass,
				itemColor = ItemColor.Unknown
			}).countAtStart++;
		}

		public override bool IsCompatibleWithPickupGoal(Match3Goals.ChipTypeDef chipTypeDef)
		{
			return this.level > 0 && chipTypeDef.chipType == ChipType.PickupGrass;
		}

		public override SlotDestroyResolution OnDestroyNeighbourSlotComponent(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			if (this.level <= 0)
			{
				return result;
			}
			if (!this.isDestroyedByMatchingNextTo)
			{
				return result;
			}
			result.isDestroyed = true;
			result.stopPropagation = true;
			this.DestroyLayer(destroyParams);
			return result;
		}

		public override SlotDestroyResolution OnDestroySlotComponent(SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			if (this.level <= 0)
			{
				return result;
			}
			result.isDestroyed = true;
			result.stopPropagation = true;
			this.DestroyLayer(destroyParams);
			return result;
		}

		private int level = 1;
	}
}
