using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class CarpetSpreadBehaviour : MonoBehaviour
{
	public void Init(Match3Game game, CarpetSpread carpetSpread)
	{
		this.slotsProvider.Init(game, carpetSpread.slots);
	}

	public void RefreshCarpet()
	{
		this.slotsRenderer.ShowSlotsOnLevel(this.slotsProvider);
		for (int i = 0; i < this.borderRenderers.Count; i++)
		{
			this.borderRenderers[i].ShowBorderOnLevel(this.slotsProvider);
		}
	}

	[SerializeField]
	private List<TilesBorderRenderer> borderRenderers = new List<TilesBorderRenderer>();

	[SerializeField]
	private TilesBorderRenderer slotsRenderer;

	private CarpetSpreadBehaviour.RendererTilesSlotsProvider slotsProvider = new CarpetSpreadBehaviour.RendererTilesSlotsProvider();

	public class RendererTilesSlotsProvider : TilesSlotsProvider
	{
		public void Init(Match3Game game, CarpetSpread.SlotData[] slots)
		{
			this.game = game;
			this.slots = slots;
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
			if (position.x >= this.game.board.size.x || position.x < 0 || position.y >= this.game.board.size.y || position.y < 0)
			{
				return new TilesSlotsProvider.Slot(position, false);
			}
			CarpetSpread.SlotData slotData = this.slots[this.game.board.Index(position)];
			return new TilesSlotsProvider.Slot(position, slotData.hasCarpet);
		}

		public List<TilesSlotsProvider.Slot> GetAllSlots()
		{
			this.innerSlots.Clear();
			for (int i = 0; i < this.slots.Length; i++)
			{
				CarpetSpread.SlotData slotData = this.slots[i];
				if (slotData.hasCarpet)
				{
					this.innerSlots.Add(new TilesSlotsProvider.Slot(slotData.position, slotData.hasCarpet));
				}
			}
			return this.innerSlots;
		}

		public CarpetSpread.SlotData[] slots;

		public Match3Game game;

		private List<TilesSlotsProvider.Slot> innerSlots = new List<TilesSlotsProvider.Slot>();
	}
}
