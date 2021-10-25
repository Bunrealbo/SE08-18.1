using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class MonsterElements
	{
		public MonsterElements.MonsterElementPieces GetPieceThatNeedsFeeding(Match3Goals.ChipTypeDef chipTypeDef)
		{
			if (chipTypeDef.chipType != ChipType.Chip)
			{
				return null;
			}
			for (int i = 0; i < this.pieces.Count; i++)
			{
				MonsterElements.MonsterElementPieces monsterElementPieces = this.pieces[i];
				if (monsterElementPieces.isMoreChipsRequiredToCollect && monsterElementPieces.element.itemColor == chipTypeDef.itemColor)
				{
					return monsterElementPieces;
				}
			}
			return null;
		}

		public void Init(Match3Game game, LevelDefinition level)
		{
			LevelDefinition.MonsterElements monsterElements = level.monsterElements;
			for (int i = 0; i < monsterElements.elements.Count; i++)
			{
				LevelDefinition.MonsterElement monsterElement = monsterElements.elements[i];
				MonsterElements.MonsterElementPieces monsterElementPieces = new MonsterElements.MonsterElementPieces();
				monsterElementPieces.element = monsterElement;
				this.pieces.Add(monsterElementPieces);
				MonsterElementBehaviour monsterElementBehaviour = game.CreateMonsterElementBehaviour();
				monsterElementPieces.elementBehaviour = monsterElementBehaviour;
				if (monsterElementBehaviour != null)
				{
					monsterElementBehaviour.Init(monsterElement);
					monsterElementBehaviour.GetComponent<TransformBehaviour>().localPosition = Vector3.Lerp(game.LocalPositionOfCenter(monsterElement.position), game.LocalPositionOfCenter(monsterElement.oppositeCornerPosition), 0.5f);
					monsterElementBehaviour.SetCount(monsterElement.numberToCollect);
				}
				for (int j = 0; j < monsterElement.size.x; j++)
				{
					for (int k = 0; k < monsterElement.size.y; k++)
					{
						Slot slot = game.GetSlot(new IntVector2(j, -k) + monsterElement.position);
						if (slot != null)
						{
							MonsterElementSlotComponent monsterElementSlotComponent = new MonsterElementSlotComponent();
							slot.AddComponent(monsterElementSlotComponent);
							monsterElementPieces.slotComponents.Add(monsterElementSlotComponent);
						}
					}
				}
			}
		}

		public void AddToGoalsAtStart(Match3Goals goals)
		{
			for (int i = 0; i < this.pieces.Count; i++)
			{
				MonsterElements.MonsterElementPieces monsterElementPieces = this.pieces[i];
				goals.GetChipTypeCounter(new Match3Goals.ChipTypeDef
				{
					chipType = ChipType.MonsterBlocker,
					itemColor = monsterElementPieces.element.itemColor
				}).countAtStart++;
			}
		}

		public bool IsCompatibleWithPickupGoal(Slot slot, Match3Goals.ChipTypeDef chipTypeDef)
		{
			if (chipTypeDef.chipType != ChipType.MonsterBlocker)
			{
				return false;
			}
			Match3Goals.ChipTypeDef chipTypeDef2 = default(Match3Goals.ChipTypeDef);
			chipTypeDef2.chipType = ChipType.Chip;
			chipTypeDef2.itemColor = chipTypeDef.itemColor;
			for (int i = 0; i < this.pieces.Count; i++)
			{
				MonsterElements.MonsterElementPieces monsterElementPieces = this.pieces[i];
				if (monsterElementPieces.isMoreChipsRequiredToCollect && monsterElementPieces.element.itemColor == chipTypeDef.itemColor)
				{
					Chip slotComponent = slot.GetSlotComponent<Chip>();
					if (slotComponent != null && slotComponent.IsCompatibleWithPickupGoal(chipTypeDef2))
					{
						return true;
					}
				}
			}
			return false;
		}

		public List<MonsterElements.MonsterElementPieces> pieces = new List<MonsterElements.MonsterElementPieces>();

		public class MonsterElementPieces
		{
			public Vector3 LocalPositionOfCenter(Match3Game game)
			{
				if (this.elementBehaviour != null)
				{
					return this.elementBehaviour.GetComponent<TransformBehaviour>().localPosition;
				}
				return Vector3.Lerp(game.LocalPositionOfCenter(this.element.position), game.LocalPositionOfCenter(this.element.oppositeCornerPosition), 0.5f);
			}

			public bool isCollected
			{
				get
				{
					return this.collectedCount >= this.element.numberToCollect;
				}
			}

			public bool isMoreChipsRequiredToCollect
			{
				get
				{
					return Mathf.Max(this.collectedCount, this.collectedAnimationsStarted) < this.element.numberToCollect;
				}
			}

			public void OnStartCollectAnimation()
			{
				this.collectedAnimationsStarted++;
			}

			public void OnCollected(Match3Game game)
			{
				if (this.isCollected)
				{
					return;
				}
				this.collectedCount++;
				if (this.elementBehaviour != null)
				{
					this.elementBehaviour.SetCount(this.element.numberToCollect - this.collectedCount);
					this.elementBehaviour.DoEatAnimation();
					game.particles.CreateParticles(this.elementBehaviour.transform.localPosition, Match3Particles.PositionType.OnDestroyChip, ChipType.Chip, this.element.itemColor);
				}
				if (!this.isCollected)
				{
					return;
				}
				List<Slot> list = new List<Slot>();
				for (int i = 0; i < this.slotComponents.Count; i++)
				{
					MonsterElementSlotComponent monsterElementSlotComponent = this.slotComponents[i];
					if (monsterElementSlotComponent.lastConnectedSlot != null)
					{
						list.Add(monsterElementSlotComponent.lastConnectedSlot);
					}
					monsterElementSlotComponent.RemoveFromGame();
				}
				Match3Goals.ChipTypeDef chipTypeDef = default(Match3Goals.ChipTypeDef);
				chipTypeDef.chipType = ChipType.MonsterBlocker;
				chipTypeDef.itemColor = this.element.itemColor;
				Match3Goals.GoalBase activeGoal = game.goals.GetActiveGoal(chipTypeDef);
				CollectGoalAction collectGoalAction = new CollectGoalAction();
				CollectGoalAction.CollectGoalParams collectParams = default(CollectGoalAction.CollectGoalParams);
				collectParams.goal = activeGoal;
				collectParams.game = game;
				collectParams.otherAffectedSlots = list;
				if (this.elementBehaviour != null)
				{
					collectParams.moveTransform = this.elementBehaviour.GetComponent<TransformBehaviour>();
					this.elementBehaviour.SetCount(0);
				}
				collectGoalAction.Init(collectParams);
				game.board.actionManager.AddAction(collectGoalAction);
			}

			public LevelDefinition.MonsterElement element;

			public List<MonsterElementSlotComponent> slotComponents = new List<MonsterElementSlotComponent>();

			public MonsterElementBehaviour elementBehaviour;

			public int collectedCount;

			public int collectedAnimationsStarted;
		}
	}
}
