using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class BurriedElements
	{
		public void Init(Match3Game game, LevelDefinition level)
		{
			List<LevelDefinition.BurriedElement> elements = level.burriedElements.elements;
			LevelDefinition.ConveyorBelts conveyorBelts = level.GetConveyorBelts();
			for (int i = 0; i < elements.Count; i++)
			{
				LevelDefinition.BurriedElement burriedElement = elements[i];
				if (conveyorBelts.IsPartOfConveyor(burriedElement.position))
				{
					Slot slot = game.GetSlot(burriedElement.position);
					game.AddBurriedElementSlot(slot, burriedElement);
				}
				else
				{
					BurriedElementPiece burriedElementPiece = new BurriedElementPiece();
					burriedElementPiece.Init(game, this, burriedElement);
					this.elementPieces.Add(burriedElementPiece);
				}
			}
		}

		public bool IsCompatibleWithPickupGoal(Slot slot, Match3Goals.ChipTypeDef chipTypeDef)
		{
			if (chipTypeDef.chipType != ChipType.BurriedElement)
			{
				return false;
			}
			for (int i = 0; i < this.elementPieces.Count; i++)
			{
				if (this.elementPieces[i].IsCompatibleWithPickupGoal(slot, chipTypeDef))
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsPosition(IntVector2 position)
		{
			for (int i = 0; i < this.elementPieces.Count; i++)
			{
				if (this.elementPieces[i].ContainsPosition(position))
				{
					return true;
				}
			}
			return false;
		}

		public void AddToGoalsAtStart(Match3Goals goals)
		{
			goals.GetChipTypeCounter(new Match3Goals.ChipTypeDef
			{
				chipType = ChipType.BurriedElement,
				itemColor = ItemColor.Unknown
			}).countAtStart += this.elementPieces.Count;
		}

		public void Remove(BurriedElementPiece piece)
		{
			this.elementPieces.Remove(piece);
		}

		public void WobbleAll()
		{
			for (int i = 0; i < this.elementPieces.Count; i++)
			{
				this.elementPieces[i].Wobble(Match3Settings.instance.chipWobbleSettings);
			}
		}

		public void OnSlateDestroyed(Slot slot, SlotDestroyParams destroyParams)
		{
			this.tempList.Clear();
			this.tempList.AddRange(this.elementPieces);
			for (int i = 0; i < this.tempList.Count; i++)
			{
				this.tempList[i].OnSlateDestroyed(slot, destroyParams);
			}
		}

		public void Update(float deltaTime)
		{
			this.tempList.Clear();
			this.tempList.AddRange(this.elementPieces);
			for (int i = 0; i < this.tempList.Count; i++)
			{
				this.tempList[i].Update(deltaTime);
			}
		}

		private Match3Game game;

		public List<BurriedElementPiece> elementPieces = new List<BurriedElementPiece>();

		private List<BurriedElementPiece> tempList = new List<BurriedElementPiece>();
	}
}
