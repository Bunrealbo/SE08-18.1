using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class SnowCoverBorderRenderer : MonoBehaviour
{
	public void Render(Match3Game game)
	{
		if (this.levelRendererPairs == null)
		{
			this.levelRendererPairs = new List<SnowCoverBorderRenderer.LevelRendererPair>();
			SnowCoverBorderRenderer.LevelRendererPair levelRendererPair = new SnowCoverBorderRenderer.LevelRendererPair();
			SnowCoverBorderRenderer.LevelSlotsProvider levelSlotsProvider = new SnowCoverBorderRenderer.LevelSlotsProvider();
			levelSlotsProvider.Init(game);
			levelRendererPair.levelProvider = levelSlotsProvider;
			levelRendererPair.renderer = this.levelRenderer;
			this.levelRendererPairs.Add(levelRendererPair);
		}
		for (int i = 0; i < this.levelRendererPairs.Count; i++)
		{
			SnowCoverBorderRenderer.LevelRendererPair levelRendererPair2 = this.levelRendererPairs[i];
			if (!levelRendererPair2.isHidden)
			{
				SnowCoverBorderRenderer.HiddenElementProvider levelProvider = levelRendererPair2.levelProvider;
				int num = levelProvider.CountColoredSlates();
				if (num != levelProvider.lastColoredSlates)
				{
					levelProvider.lastColoredSlates = num;
					levelRendererPair2.renderer.ShowBorder(levelProvider);
					GGUtil.SetActive(levelRendererPair2.renderer, num > 0);
					if (num <= 0)
					{
						levelRendererPair2.isHidden = true;
					}
				}
			}
		}
	}

	[SerializeField]
	private BorderTilemapRenderer levelRenderer;

	private List<SnowCoverBorderRenderer.LevelRendererPair> levelRendererPairs;

	public class HiddenElementProvider : TilesSlotsProvider
	{
		public virtual int CountColoredSlates()
		{
			return 0;
		}

		public int lastColoredSlates;
	}

	public class LevelSlotsProvider : SnowCoverBorderRenderer.HiddenElementProvider
	{
		public void Init(Match3Game game)
		{
			this.game = game;
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
			return levelSlot.GetSlotComponent<SnowCover>() != null;
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

	private class LevelRendererPair
	{
		public SnowCoverBorderRenderer.HiddenElementProvider levelProvider;

		public BorderTilemapRenderer renderer;

		public bool isHidden;
	}
}
