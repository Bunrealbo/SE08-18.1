using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class BorderTilemapRenderer : MonoBehaviour
	{
		public MeshFilter meshFilter
		{
			get
			{
				if (!this._meshFilter)
				{
					this._meshFilter = base.GetComponent<MeshFilter>();
				}
				return this._meshFilter;
			}
		}

		public MeshRenderer meshRenderer
		{
			get
			{
				if (!this._meshRenderer)
				{
					this._meshRenderer = base.GetComponent<MeshRenderer>();
				}
				return this._meshRenderer;
			}
		}

		private void InitBuffers(int vertexCount, int trisCount)
		{
			if (this.vertexBuffer == null)
			{
				this.vertexBuffer = new List<Vector3>(vertexCount);
				this.uvBuffer = new List<Vector2>(vertexCount);
				this.trisBuffer = new List<int>(trisCount);
			}
		}

		private void Init()
		{
			if (this.mesh == null)
			{
				this.mesh = this.meshFilter.mesh;
			}
			if (this.mesh == null)
			{
				this.mesh = new Mesh();
				this.meshFilter.mesh = this.mesh;
			}
		}

		public static bool IsEmptySlot(TilesSlotsProvider slotsProvider, IntVector2 position)
		{
			return slotsProvider.GetSlot(position).isEmpty;
		}

		public static bool IsOccupiedSlot(TilesSlotsProvider slotsProvider, IntVector2 position)
		{
			return slotsProvider.GetSlot(position).isOccupied;
		}

		public void ShowBorder(TilesSlotsProvider slotsProvider)
		{
			this.Init();
			int maxSlots = slotsProvider.MaxSlots;
			int vertexCount = maxSlots * 8 * 4;
			int trisCount = maxSlots * 8 * 6;
			this.InitBuffers(vertexCount, trisCount);
			this.ClearBuffers();
			Vector2 a = slotsProvider.StartPosition(this.slotSize);
			new Rect(0f, 0f, 1f, 1f);
			List<TilesSlotsProvider.Slot> allSlots = slotsProvider.GetAllSlots();
			int num = 2;
			Vector2 size = new Vector2(this.slotSize / (float)num, this.slotSize / (float)num);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					List<BorderTilemap.SortedTilePiece> list = this.tilemap.SortedTilesForTilePosition(this.tilemap.TilePositionFromOffset(intVector));
					for (int k = 0; k < allSlots.Count; k++)
					{
						TilesSlotsProvider.Slot slot = allSlots[k];
						if (!slot.isEmpty)
						{
							IntVector2 position = slot.position;
							BorderTilemap.TilePiece tilePiece = null;
							BorderTilemap.TilePiece.PositionDefinition positionDefinition = null;
							for (int l = 0; l < list.Count; l++)
							{
								BorderTilemap.SortedTilePiece sortedTilePiece = list[l];
								List<BorderTilemap.TilePiece.Condition> conditions = sortedTilePiece.positionDefinition.conditions;
								bool flag = true;
								for (int m = 0; m < conditions.Count; m++)
								{
									BorderTilemap.TilePiece.Condition condition = conditions[m];
									bool isOccupied = slotsProvider.GetSlot(position + condition.offset).isOccupied;
									if (condition.conditionEnum == BorderTilemap.TilePiece.ConditionEnum.Empty && isOccupied)
									{
										flag = false;
										break;
									}
									if (condition.conditionEnum == BorderTilemap.TilePiece.ConditionEnum.Full && !isOccupied)
									{
										flag = false;
										break;
									}
								}
								if (flag)
								{
									tilePiece = sortedTilePiece.tilePiece;
									positionDefinition = sortedTilePiece.positionDefinition;
									break;
								}
							}
							if (tilePiece != null && positionDefinition != null)
							{
								if (positionDefinition.drawableRects.Count == 0)
								{
									Vector2 position2 = a + new Vector2(((float)position.x + (float)intVector.x * 0.5f) * this.slotSize, ((float)position.y + (float)intVector.y * 0.5f) * this.slotSize);
									Rect pos = new Rect(position2, size);
									Rect uv = tilePiece.UvRect(this.tilemap);
									Vector2 b = new Vector2(uv.width * 0.01f, uv.height * 0.01f);
									uv.min += b;
									uv.max -= b;
									this.DrawRectangle(pos, uv);
								}
								else
								{
									List<BorderTilemap.TilePiece.DrawableRect> drawableRects = positionDefinition.drawableRects;
									for (int n = 0; n < positionDefinition.drawableRects.Count; n++)
									{
										BorderTilemap.TilePiece.DrawableRect drawableRect = positionDefinition.drawableRects[n];
										Vector2 position3 = drawableRect.localImageRect.position;
										Vector2 position4 = a + new Vector2(((float)position.x + 0.5f + position3.x) * this.slotSize, ((float)position.y + 0.5f + position3.y) * this.slotSize);
										Rect pos2 = new Rect(position4, drawableRect.localImageRect.size * this.slotSize);
										this.DrawRectangle(pos2, drawableRect.uvRect);
									}
								}
							}
						}
					}
				}
			}
			this.SetBuffersToToMesh();
		}

		private void ClearBuffers()
		{
			this.vertexBuffer.Clear();
			this.trisBuffer.Clear();
			this.uvBuffer.Clear();
		}

		private void SetBuffersToToMesh()
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.vertexBuffer);
			this.mesh.SetUVs(0, this.uvBuffer);
			this.mesh.SetTriangles(this.trisBuffer, 0, true);
			if (this.setSortingSettings)
			{
				this.sortingSettings.Set(this.meshRenderer);
			}
			this.tilemap.material.mainTexture = this.tilemap.texture;
			this.meshRenderer.sharedMaterial = this.tilemap.material;
		}

		private void DrawRectangle(Rect pos, Rect uv)
		{
			Vector2 min = pos.min;
			Vector2 max = pos.max;
			int count = this.vertexBuffer.Count;
			this.vertexBuffer.Add(new Vector3(min.x, min.y, 0f));
			this.vertexBuffer.Add(new Vector3(max.x, min.y, 0f));
			this.vertexBuffer.Add(new Vector3(max.x, max.y, 0f));
			this.vertexBuffer.Add(new Vector3(min.x, max.y, 0f));
			this.uvBuffer.Add(new Vector2(uv.xMin, uv.yMin));
			this.uvBuffer.Add(new Vector2(uv.xMax, uv.yMin));
			this.uvBuffer.Add(new Vector2(uv.xMax, uv.yMax));
			this.uvBuffer.Add(new Vector2(uv.xMin, uv.yMax));
			this.trisBuffer.Add(count);
			this.trisBuffer.Add(count + 2);
			this.trisBuffer.Add(count + 1);
			this.trisBuffer.Add(count + 2);
			this.trisBuffer.Add(count);
			this.trisBuffer.Add(count + 3);
		}

		[SerializeField]
		public float slotSize = 1f;

		[SerializeField]
		private bool setSortingSettings;

		[SerializeField]
		private SpriteSortingSettings sortingSettings = new SpriteSortingSettings();

		[SerializeField]
		private BorderTilemap tilemap;

		private Mesh mesh;

		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private List<Vector3> vertexBuffer;

		private List<int> trisBuffer;

		private List<Vector2> uvBuffer;
	}
}
