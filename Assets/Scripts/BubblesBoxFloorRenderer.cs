using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class BubblesBoxFloorRenderer : MonoBehaviour
{
	public void Render(Match3Game game)
	{
		if (this.levelRendererPairs == null)
		{
			this.levelRendererPairs = new List<BubblesBoxFloorRenderer.LevelRendererPair>();
			if (this.useHole && this.holeRenderer != null)
			{
				this.holeProvider = new BubblesBoxFloorRenderer.HoleProvider();
				this.holeProvider.Init(game);
				BubblesBoxFloorRenderer.LevelRendererPair levelRendererPair = new BubblesBoxFloorRenderer.LevelRendererPair();
				levelRendererPair.levelProvider = this.holeProvider;
				levelRendererPair.renderer = this.holeRenderer;
				this.levelRendererPairs.Add(levelRendererPair);
			}
			else
			{
				GGUtil.SetActive(this.holeRenderer, false);
			}
			for (int i = 0; i < 2; i++)
			{
				BubblesBoxFloorRenderer.LevelRendererPair levelRendererPair2 = new BubblesBoxFloorRenderer.LevelRendererPair();
				BubblesBoxFloorRenderer.LevelSlotsProvider levelSlotsProvider = new BubblesBoxFloorRenderer.LevelSlotsProvider();
				levelSlotsProvider.Init(game, i + 1);
				levelRendererPair2.levelProvider = levelSlotsProvider;
				levelRendererPair2.renderer = ((i == 0) ? this.level1Renderer : this.level2Renderer);
				this.levelRendererPairs.Add(levelRendererPair2);
			}
		}
		for (int j = 0; j < this.levelRendererPairs.Count; j++)
		{
			BubblesBoxFloorRenderer.LevelRendererPair levelRendererPair3 = this.levelRendererPairs[j];
			if (!levelRendererPair3.isHidden)
			{
				BubblesBoxFloorRenderer.HiddenElementProvider levelProvider = levelRendererPair3.levelProvider;
				int num = levelProvider.CountColoredSlates();
				if (num != levelProvider.lastColoredSlates)
				{
					levelProvider.lastColoredSlates = num;
					levelRendererPair3.renderer.ShowBorder(levelProvider);
					GGUtil.SetActive(levelRendererPair3.renderer, num > 0);
					if (num <= 0)
					{
						levelRendererPair3.isHidden = true;
					}
				}
			}
		}
	}

	[SerializeField]
	private BorderTilemapRenderer holeRenderer;

	[SerializeField]
	private BorderTilemapRenderer level1Renderer;

	[SerializeField]
	private BorderTilemapRenderer level2Renderer;

	[SerializeField]
	private bool useHole;

	private BubblesBoxFloorRenderer.HoleProvider holeProvider;

	private List<BubblesBoxFloorRenderer.LevelRendererPair> levelRendererPairs;

	public class HiddenElementProvider : TilesSlotsProvider
	{
		public virtual int CountColoredSlates()
		{
			return 0;
		}

		public int lastColoredSlates;
	}

	public class LevelSlotsProvider : BubblesBoxFloorRenderer.HiddenElementProvider
	{
		public void Init(Match3Game game, int minLevel)
		{
			this.game = game;
			this.minLevel = minLevel;
		}

		public int CountColoredSlates()
		{
			List<GGMatch3.Slot> sortedSlotsUpdateList = this.game.board.sortedSlotsUpdateList;
			int num = 0;
			for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
			{
				GGMatch3.Slot levelSlot = sortedSlotsUpdateList[i];
				if (this.IsOccupied(levelSlot))
				{
					num++;
				}
			}
			return num;
		}

		public int MaxSlots
		{
			get
			{
				return this.game.board.size.x * this.game.board.size.y;
			}
		}

		public Vector2 StartPosition(float size)
		{
			return new Vector2((float)(-(float)this.game.board.size.x) * size * 0.5f, (float)(-(float)this.game.board.size.y) * size * 0.5f);
		}

		public TilesSlotsProvider.Slot GetSlot(IntVector2 position)
		{
			TilesSlotsProvider.Slot result = default(TilesSlotsProvider.Slot);
			GGMatch3.Slot slot = this.game.GetSlot(position);
			result.position = position;
			if (slot != null)
			{
				result.isOccupied = this.IsOccupied(slot);
			}
			return result;
		}

		private bool IsOccupied(GGMatch3.Slot levelSlot)
		{
			SlotColorSlate slotComponent = levelSlot.GetSlotComponent<SlotColorSlate>();
			return slotComponent != null && slotComponent.blockerLevel >= this.minLevel;
		}

		public List<TilesSlotsProvider.Slot> GetAllSlots()
		{
			this.allSlots.Clear();
			List<GGMatch3.Slot> sortedSlotsUpdateList = this.game.board.sortedSlotsUpdateList;
			for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
			{
				GGMatch3.Slot slot = sortedSlotsUpdateList[i];
				TilesSlotsProvider.Slot item = default(TilesSlotsProvider.Slot);
				item.position = slot.position;
				item.isOccupied = this.IsOccupied(slot);
				this.allSlots.Add(item);
			}
			return this.allSlots;
		}

		public Match3Game game;

		public int minLevel;

		private List<TilesSlotsProvider.Slot> allSlots = new List<TilesSlotsProvider.Slot>();
	}

	public class HoleProvider : BubblesBoxFloorRenderer.HiddenElementProvider
	{
		public int CountColoredSlates()
		{
			return this.GetAllSlots().Count;
		}

		public void Init(Match3Game game)
		{
			this.game = game;
		}

		public int MaxSlots
		{
			get
			{
				return this.GetAllSlots().Count;
			}
		}

		public Vector2 StartPosition(float size)
		{
			return new Vector2((float)(-(float)this.game.board.size.x) * size * 0.5f, (float)(-(float)this.game.board.size.y) * size * 0.5f);
		}

		public TilesSlotsProvider.Slot GetSlot(IntVector2 position)
		{
			return new TilesSlotsProvider.Slot
			{
				position = position,
				isOccupied = this.game.board.burriedElements.ContainsPosition(position)
			};
		}

		public List<TilesSlotsProvider.Slot> GetAllSlots()
		{
			BurriedElements burriedElements = this.game.board.burriedElements;
			this.allSlots.Clear();
			List<BurriedElementPiece> elementPieces = burriedElements.elementPieces;
			for (int i = 0; i < elementPieces.Count; i++)
			{
				LevelDefinition.BurriedElement elementDefinition = elementPieces[i].elementDefinition;
				IntVector2 position = elementDefinition.position;
				IntVector2 oppositeCornerPosition = elementDefinition.oppositeCornerPosition;
				int num = Mathf.Min(position.x, oppositeCornerPosition.x);
				int num2 = Mathf.Max(position.x, oppositeCornerPosition.x);
				int num3 = Mathf.Min(position.y, oppositeCornerPosition.y);
				int num4 = Mathf.Max(position.y, oppositeCornerPosition.y);
				for (int j = num; j <= num2; j++)
				{
					for (int k = num3; k <= num4; k++)
					{
						TilesSlotsProvider.Slot item = default(TilesSlotsProvider.Slot);
						item.position = new IntVector2(j, k);
						item.isOccupied = true;
						this.allSlots.Add(item);
					}
				}
			}
			return this.allSlots;
		}

		private Match3Game game;

		private List<TilesSlotsProvider.Slot> allSlots = new List<TilesSlotsProvider.Slot>();
	}

	private class LevelRendererPair
	{
		public BubblesBoxFloorRenderer.HiddenElementProvider levelProvider;

		public BorderTilemapRenderer renderer;

		public bool isHidden;
	}
}
