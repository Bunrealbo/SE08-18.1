using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class GrowingElementChip : SlotComponent
	{
		public bool isGeneratingSpecificColor
		{
			get
			{
				return this.itemColor != ItemColor.Uncolored && this.itemColor != ItemColor.Unknown;
			}
		}

		public void Init(ItemColor itemColor)
		{
			this.itemColor = itemColor;
		}

		private GrowingElementBehaviour elementBehaviour
		{
			get
			{
				return base.GetComponentBehaviour<GrowingElementBehaviour>();
			}
		}

		public override bool isPlaceBubbleSuspended
		{
			get
			{
				return true;
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

		public override bool isPreventingOtherChipsToFallIntoSlot
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

		public override bool isPreventingGravity
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
				return true;
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
				return true;
			}
		}

		public override bool IsCompatibleWithPickupGoal(Match3Goals.ChipTypeDef chipTypeDef)
		{
			return chipTypeDef.IsEqual(new Match3Goals.ChipTypeDef
			{
				chipType = ChipType.GrowingElementPiece,
				itemColor = ItemColor.Unknown
			});
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
			this.UpdateLevel(destroyParams);
			return result;
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
			this.UpdateLevel(destroyParams);
			return result;
		}

		private void UpdateLevel(SlotDestroyParams destroyParams)
		{
			this.currentLevel++;
			if (this.currentLevel >= 3)
			{
				this.lastConnectedSlot.game.Play(GGSoundSystem.SFXType.GrowingElementFinish);
				this.CreateFlowers();
				this.currentLevel = 0;
			}
			else
			{
				GGSoundSystem.PlayParameters sound = default(GGSoundSystem.PlayParameters);
				sound.soundType = GGSoundSystem.SFXType.GrowingElementGrowFlower;
				sound.variationIndex = this.currentLevel - 1;
				this.lastConnectedSlot.game.Play(sound);
			}
			CollectPointsAction.OnBlockerDestroy(this.lastConnectedSlot, destroyParams);
			this.VisualUpdate();
			if (this.slot != null)
			{
				this.slot.Wobble(Match3Settings.instance.chipWobbleSettings);
			}
		}

		private List<GrowingElementChip.PotentialElementToReceieve> PotentialElementToAttach()
		{
			this.potentialElements.Clear();
			List<Slot> sortedSlotsUpdateList = this.slot.game.board.sortedSlotsUpdateList;
			for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
			{
				Slot slot = sortedSlotsUpdateList[i];
				if (slot.CanAttachGrowingElement())
				{
					Chip slotComponent = slot.GetSlotComponent<Chip>();
					if (slotComponent != null)
					{
						GrowingElementChip.PotentialElementToReceieve item = default(GrowingElementChip.PotentialElementToReceieve);
						item.slot = slot;
						item.chip = slotComponent;
						this.potentialElements.Add(item);
					}
				}
			}
			return this.potentialElements;
		}

		private List<GrowingElementChip.PotentialElementToReceieve> PotentialElementToAttachEmptySlots()
		{
			this.potentialElements.Clear();
			List<Slot> sortedSlotsUpdateList = this.slot.game.board.sortedSlotsUpdateList;
			for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
			{
				Slot slot = sortedSlotsUpdateList[i];
				if (slot.CanAttachGrowingElement())
				{
					Chip slotComponent = slot.GetSlotComponent<Chip>();
					if (slotComponent == null)
					{
						GrowingElementChip.PotentialElementToReceieve item = default(GrowingElementChip.PotentialElementToReceieve);
						item.slot = slot;
						item.chip = slotComponent;
						this.potentialElements.Add(item);
					}
				}
			}
			return this.potentialElements;
		}

		public Slot RandomDesiredSlot()
		{
			List<GrowingElementChip.PotentialElementToReceieve> list = this.PotentialElementToAttach();
			GGUtil.Shuffle<GrowingElementChip.PotentialElementToReceieve>(list);
			if (list.Count > 0)
			{
				return list[0].slot;
			}
			list = this.PotentialElementToAttachEmptySlots();
			GGUtil.Shuffle<GrowingElementChip.PotentialElementToReceieve>(list);
			if (list.Count > 0)
			{
				return list[0].slot;
			}
			return null;
		}

		private void CreateFlowers()
		{
			Match3Game game = this.slot.game;
			GrowingElementBehaviour elementBehaviour = this.elementBehaviour;
			this.selectedElements.Clear();
			int num = 3;
			List<GrowingElementChip.PotentialElementToReceieve> list = this.PotentialElementToAttach();
			GGUtil.Shuffle<GrowingElementChip.PotentialElementToReceieve>(list);
			int num2 = 0;
			while (num2 < list.Count && this.selectedElements.Count < num)
			{
				GrowingElementChip.PotentialElementToReceieve item = list[num2];
				this.selectedElements.Add(item);
				num2++;
			}
			list = this.PotentialElementToAttachEmptySlots();
			GGUtil.Shuffle<GrowingElementChip.PotentialElementToReceieve>(list);
			int num3 = 0;
			while (num3 < list.Count && this.selectedElements.Count < num)
			{
				GrowingElementChip.PotentialElementToReceieve item2 = list[num3];
				this.selectedElements.Add(item2);
				num3++;
			}
			for (int i = 0; i < this.selectedElements.Count; i++)
			{
				GrowingElementChip.PotentialElementToReceieve potentialElementToReceieve = this.selectedElements[i];
				AnimateGrowingElementOnChip animateGrowingElementOnChip = new AnimateGrowingElementOnChip();
				AnimateGrowingElementOnChip.InitArguments initArguments = default(AnimateGrowingElementOnChip.InitArguments);
				initArguments.destinationSlot = potentialElementToReceieve.slot;
				initArguments.game = game;
				initArguments.itemColor = game.board.RandomColor(ItemColor.Unknown);
				initArguments.growingElement = this;
				if (potentialElementToReceieve.chip != null)
				{
					initArguments.itemColor = potentialElementToReceieve.chip.itemColor;
				}
				if (this.isGeneratingSpecificColor)
				{
					initArguments.itemColor = this.itemColor;
					initArguments.forceConvertChip = true;
				}
				initArguments.destinationSlot = null;
				initArguments.worldPositionStart = Vector3.zero;
				if (elementBehaviour != null)
				{
					initArguments.worldPositionStart = elementBehaviour.WorldPositionForElement(i);
				}
				animateGrowingElementOnChip.Init(initArguments);
				game.board.actionManager.AddAction(animateGrowingElementOnChip);
			}
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

		private void VisualUpdate()
		{
			GrowingElementBehaviour elementBehaviour = this.elementBehaviour;
			if (elementBehaviour == null)
			{
				return;
			}
			elementBehaviour.SetLevel(this.currentLevel);
			if (this.currentLevel <= 0)
			{
				elementBehaviour.StopAllAnimations();
				return;
			}
			elementBehaviour.StartAnimationFor(this.currentLevel - 1);
		}

		private ItemColor itemColor = ItemColor.Uncolored;

		private WobbleAnimation wobbleAnimation = new WobbleAnimation();

		private int currentLevel;

		private List<GrowingElementChip.PotentialElementToReceieve> potentialElements = new List<GrowingElementChip.PotentialElementToReceieve>();

		private List<GrowingElementChip.PotentialElementToReceieve> selectedElements = new List<GrowingElementChip.PotentialElementToReceieve>();

		public struct PotentialElementToReceieve
		{
			public Slot slot;

			public Chip chip;
		}
	}
}
