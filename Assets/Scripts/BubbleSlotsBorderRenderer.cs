using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class BubbleSlotsBorderRenderer : MonoBehaviour
{
	public void Render(Match3Game game)
	{
		this.soapSlotsProvider.Init(game);
		int num = this.soapSlotsProvider.CountSoapSlates();
		if (num == this.lastBubbleCount)
		{
			return;
		}
		this.lastBubbleCount = num;
		this.tilemapRenderer.ShowBorder(this.soapSlotsProvider);
	}

	[SerializeField]
	private BorderTilemapRenderer tilemapRenderer;

	protected BubbleSlotsBorderRenderer.BubbleSlotsProvider soapSlotsProvider = new BubbleSlotsBorderRenderer.BubbleSlotsProvider();

	private int lastBubbleCount;

	protected class BubbleSlotsProvider : TilesSlotsProvider
	{
		public void Init(Match3Game game)
		{
			this.game = game;
		}

		public int CountSoapSlates()
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
			return levelSlot.GetSlotComponent<BubblesPieceBlocker>() != null;
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

		private List<TilesSlotsProvider.Slot> allSlots = new List<TilesSlotsProvider.Slot>();
	}
}
