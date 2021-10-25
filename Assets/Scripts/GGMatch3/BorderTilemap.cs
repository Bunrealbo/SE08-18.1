using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class BorderTilemap : ScriptableObject
	{
		public List<BorderTilemap.SortedTilePiece> SortedTilesForTilePosition(TilePosition tilePosition)
		{
			this.sortedTiles.Clear();
			for (int i = 0; i < this.tiles.Count; i++)
			{
				BorderTilemap.TilePiece tilePiece = this.tiles[i];
				for (int j = 0; j < tilePiece.positions.Count; j++)
				{
					BorderTilemap.TilePiece.PositionDefinition positionDefinition = tilePiece.positions[j];
					if (positionDefinition.position == tilePosition)
					{
						BorderTilemap.SortedTilePiece item = default(BorderTilemap.SortedTilePiece);
						item.positionDefinition = positionDefinition;
						item.tilePiece = tilePiece;
						this.sortedTiles.Add(item);
					}
				}
			}
			return this.sortedTiles;
		}

		public TilePosition TilePositionFromOffset(IntVector2 direction)
		{
			if (direction.x == 0 && direction.y == 0)
			{
				return TilePosition.BottomLeft;
			}
			if (direction.x == 1 && direction.y == 0)
			{
				return TilePosition.BottomRight;
			}
			if (direction.x == 0 && direction.y == 1)
			{
				return TilePosition.TopLeft;
			}
			if (direction.x == 1 && direction.y == 1)
			{
				return TilePosition.TopRight;
			}
			return TilePosition.TopLeft;
		}

		public int maxTextureWidth = 1024;

		public BorderTilemap.BorderDefinition border = new BorderTilemap.BorderDefinition();

		public int spritePadding;

		public List<BorderTilemap.TilePiece> tiles = new List<BorderTilemap.TilePiece>();

		public Vector2 uvSize;

		public Texture2D texture;

		public Material material;

		private List<BorderTilemap.SortedTilePiece> sortedTiles = new List<BorderTilemap.SortedTilePiece>();

		[Serializable]
		public class BorderDefinition
		{
			public int leftOffset;

			public int leftWidth;

			public int rightOffset;

			public int rightWidth;

			public int topOffset;

			public int topWidth;

			public int bottomOffset;

			public int bottomWidth;
		}

		[Serializable]
		public class TilePiece
		{
			public Rect UvRect(BorderTilemap tilemap)
			{
				return new Rect((float)this.rowColumnInTileMap.x * tilemap.uvSize.x, (float)this.rowColumnInTileMap.y * tilemap.uvSize.y, tilemap.uvSize.x, tilemap.uvSize.y);
			}

			public IntVector2 positionInOriginal;

			public List<BorderTilemap.TilePiece.PositionDefinition> positions = new List<BorderTilemap.TilePiece.PositionDefinition>();

			public IntVector2 rowColumnInTileMap;

			public enum ConditionEnum
			{
				Full,
				Empty
			}

			[Serializable]
			public class Condition
			{
				public IntVector2 offset;

				public BorderTilemap.TilePiece.ConditionEnum conditionEnum;
			}

			[Serializable]
			public class DrawableRect
			{
				public Rect localImageRect;

				public Rect uvRect;
			}

			[Serializable]
			public class PositionDefinition
			{
				public TilePosition position;

				public List<BorderTilemap.TilePiece.Condition> conditions = new List<BorderTilemap.TilePiece.Condition>();

				public List<BorderTilemap.TilePiece.DrawableRect> drawableRects = new List<BorderTilemap.TilePiece.DrawableRect>();
			}
		}

		public struct SortedTilePiece
		{
			public BorderTilemap.TilePiece tilePiece;

			public BorderTilemap.TilePiece.PositionDefinition positionDefinition;
		}
	}
}
