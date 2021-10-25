using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class ChocolateBorderRenderer : MonoBehaviour
{
	private bool ShouldBeRenderedForChocolate(Slot slot)
	{
		return slot.GetSlotComponent<BasketBlocker>() != null && !slot.isMoving;
	}

	public void DisplayChocolate(Match3Game game)
	{
		List<Slot> sortedSlotsUpdateList = game.board.sortedSlotsUpdateList;
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
		{
			Slot slot = sortedSlotsUpdateList[i];
			bool flag3 = this.ShouldBeRenderedForChocolate(slot);
			if (slot.wasRenderedForChocolateLastFrame != flag3)
			{
				flag = true;
			}
			if (flag3)
			{
				flag2 = true;
			}
		}
		if (!flag)
		{
			return;
		}
		GGUtil.SetActive(this.borderRenderer, flag2);
		GGUtil.SetActive(this.slotsRenderer, flag2);
		if (!flag2)
		{
			return;
		}
		this.tilesSlotsProvider.Init(game, this);
		this.borderRenderer.ShowBorderOnLevel(this.tilesSlotsProvider);
		this.slotsRenderer.ShowSlotsOnLevel(this.tilesSlotsProvider);
	}

	private ChocolateBorderRenderer.ChocolateTilesSlotsProvider tilesSlotsProvider = new ChocolateBorderRenderer.ChocolateTilesSlotsProvider();

	[SerializeField]
	private TilesBorderRenderer borderRenderer;

	[SerializeField]
	private TilesBorderRenderer slotsRenderer;

	public class ChocolateTilesSlotsProvider : TilesSlotsProvider
	{
		public void Init(Match3Game game, ChocolateBorderRenderer borderRenderer)
		{
			this.game = game;
			this.borderRenderer = borderRenderer;
		}

		public int MaxSlots
		{
			get
			{
				return this.game.board.size.x * this.game.board.size.y;
			}
		}

		public Vector2 StartPosition(float slotSize)
		{
			return new Vector2((float)(-(float)this.game.board.size.x) * slotSize * 0.5f, (float)(-(float)this.game.board.size.y) * slotSize * 0.5f);
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
			return this.borderRenderer.ShouldBeRenderedForChocolate(levelSlot);
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

		public ChocolateBorderRenderer borderRenderer;

		private List<TilesSlotsProvider.Slot> allSlots = new List<TilesSlotsProvider.Slot>();
	}
}
