using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class ColorSlotsBorderRenderer : MonoBehaviour
{
	private bool ShouldBeRendered(Slot slot)
	{
		return slot.GetSlotComponent<SlotColorSlate>() != null;
	}

	public void DisplayChocolate(Match3Game game)
	{
		List<Slot> sortedSlotsUpdateList = game.board.sortedSlotsUpdateList;
		int num = 0;
		for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
		{
			Slot slot = sortedSlotsUpdateList[i];
			if (this.ShouldBeRendered(slot))
			{
				num++;
			}
		}
		bool flag = this.lastColoredSlates != num;
		this.lastColoredSlates = num;
		if (!flag)
		{
			return;
		}
		bool flag2 = num > 0;
		for (int j = 0; j < this.borderRenderer.Count; j++)
		{
			GGUtil.SetActive(this.borderRenderer[j], flag2);
		}
		for (int k = 0; k < this.slotsRenderer.Count; k++)
		{
			GGUtil.SetActive(this.slotsRenderer[k], flag2);
		}
		if (!flag2)
		{
			return;
		}
		this.tilesSlotsProvider.Init(game, this);
		for (int l = 0; l < this.borderRenderer.Count; l++)
		{
			this.borderRenderer[l].ShowBorderOnLevel(this.tilesSlotsProvider);
		}
		for (int m = 0; m < this.slotsRenderer.Count; m++)
		{
			this.slotsRenderer[m].ShowSlotsOnLevel(this.tilesSlotsProvider);
		}
	}

	private ColorSlotsBorderRenderer.ColorSlotsTilesSlotsProvider tilesSlotsProvider = new ColorSlotsBorderRenderer.ColorSlotsTilesSlotsProvider();

	[SerializeField]
	private List<TilesBorderRenderer> borderRenderer = new List<TilesBorderRenderer>();

	[SerializeField]
	private List<TilesBorderRenderer> slotsRenderer = new List<TilesBorderRenderer>();

	private int lastColoredSlates;

	public class ColorSlotsTilesSlotsProvider : TilesSlotsProvider
	{
		public void Init(Match3Game game, ColorSlotsBorderRenderer borderRenderer)
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
			return this.borderRenderer.ShouldBeRendered(levelSlot);
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

		public ColorSlotsBorderRenderer borderRenderer;

		private List<TilesSlotsProvider.Slot> allSlots = new List<TilesSlotsProvider.Slot>();
	}
}
