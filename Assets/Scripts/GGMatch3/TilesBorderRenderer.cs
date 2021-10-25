using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class TilesBorderRenderer : MonoBehaviour
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

		public void ShowBorderOnLevel(LevelDefinition level)
		{
			LevelDefinitionTilesSlotsProvider slotsProvider = new LevelDefinitionTilesSlotsProvider(level);
			this.ShowBorderOnLevel(slotsProvider);
		}

		public void ShowBorderOnLevelDontConnectDiagonalSlots(TilesSlotsProvider slotsProvider)
		{
			this.Init();
			int maxSlots = slotsProvider.MaxSlots;
			int vertexCount = maxSlots * 8 * 4;
			int trisCount = maxSlots * 8 * 6;
			this.InitBuffers(vertexCount, trisCount);
			this.ClearBuffers();
			Vector2 a = slotsProvider.StartPosition(this.slotSize);
			Vector2 size = new Vector2(this.slotSize, this.slotSize);
			new Rect(0f, 0f, 0.5f, 0.5f);
			List<TilesSlotsProvider.Slot> allSlots = slotsProvider.GetAllSlots();
			for (int i = 0; i < allSlots.Count; i++)
			{
				TilesSlotsProvider.Slot slot = allSlots[i];
				if (!slot.isEmpty)
				{
					IntVector2 position = slot.position;
					Vector2 vector = a + new Vector2((float)position.x * this.slotSize, (float)position.y * this.slotSize);
					new Rect(vector, size);
					bool flag = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.up + IntVector2.left);
					bool flag2 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.up + IntVector2.right);
					bool flag3 = !flag;
					bool flag4 = !flag2;
					bool flag5 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.down + IntVector2.left);
					bool flag6 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.down + IntVector2.right);
					bool flag7 = !flag5;
					bool flag8 = !flag6;
					bool flag9 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.left);
					bool flag10 = !flag9;
					bool flag11 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.down);
					bool flag12 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.right);
					bool flag13 = !flag12;
					bool flag14 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.up);
					bool flag15 = !flag14;
					if (flag9)
					{
						Rect pos = new Rect(vector.x - this.borderSize, vector.y + this.borderSize, this.borderSize, this.slotSize - 2f * this.borderSize);
						Rect uv = new Rect(0f, 0.25f, 0.25f, 0.5f);
						if (flag14)
						{
							pos.yMax = vector.y + this.slotSize - this.borderOffset;
						}
						else if (flag)
						{
							pos.yMax = vector.y + this.slotSize;
						}
						else if (flag3)
						{
							pos.yMax += this.borderOffset;
						}
						if (flag11)
						{
							pos.yMin = vector.y + this.borderOffset;
						}
						else if (flag5)
						{
							pos.yMin = vector.y;
						}
						else if (flag7)
						{
							pos.yMin -= this.borderOffset;
						}
						pos.position += Vector2.right * this.borderOffset;
						this.DrawRectangle(pos, uv);
					}
					if (flag12)
					{
						Rect pos2 = new Rect(vector.x + this.slotSize, vector.y + this.borderSize, this.borderSize, this.slotSize - 2f * this.borderSize);
						Rect uv2 = new Rect(0.75f, 0.25f, 0.25f, 0.5f);
						if (flag14)
						{
							pos2.yMax = vector.y + this.slotSize - this.borderOffset;
						}
						else if (flag2)
						{
							pos2.yMax = vector.y + this.slotSize;
						}
						else if (flag4)
						{
							pos2.yMax += this.borderOffset;
						}
						if (flag11)
						{
							pos2.yMin = vector.y + this.borderOffset;
						}
						else if (flag6)
						{
							pos2.yMin = vector.y;
						}
						else if (flag8)
						{
							pos2.yMin -= this.borderOffset;
						}
						pos2.position += Vector2.left * this.borderOffset;
						this.DrawRectangle(pos2, uv2);
					}
					if (flag11)
					{
						Rect pos3 = new Rect(vector.x + this.borderSize, vector.y - this.borderSize, this.slotSize - 2f * this.borderSize, this.borderSize);
						if (flag12)
						{
							pos3.xMax = vector.x + this.slotSize - this.borderOffset;
						}
						else if (flag6)
						{
							pos3.xMax = vector.x + this.slotSize;
						}
						else if (flag8)
						{
							pos3.xMax += this.borderOffset;
						}
						if (flag9)
						{
							pos3.xMin = vector.x + this.borderOffset;
						}
						else if (flag5)
						{
							pos3.xMin = vector.x;
						}
						else if (flag7)
						{
							pos3.xMin -= this.borderOffset;
						}
						pos3.position += Vector2.up * this.borderOffset;
						Rect uv3 = new Rect(0.25f, 0f, 0.5f, 0.25f);
						this.DrawRectangle(pos3, uv3);
					}
					if (flag14)
					{
						Rect pos4 = new Rect(vector.x + this.borderSize, vector.y + this.slotSize, this.slotSize - 2f * this.borderSize, this.borderSize);
						if (flag12)
						{
							pos4.xMax = vector.x + this.slotSize - this.borderOffset;
						}
						else if (flag2)
						{
							pos4.xMax = vector.x + this.slotSize;
						}
						else if (flag4)
						{
							pos4.xMax += this.borderOffset;
						}
						if (flag9)
						{
							pos4.xMin = vector.x + this.borderOffset;
						}
						else if (flag)
						{
							pos4.xMin = vector.x;
						}
						else if (flag3)
						{
							pos4.xMin -= this.borderOffset;
						}
						pos4.position += Vector2.down * this.borderOffset;
						Rect uv4 = new Rect(0.25f, 0.75f, 0.5f, 0.25f);
						this.DrawRectangle(pos4, uv4);
					}
					if (flag14 && flag10 && flag3)
					{
						Rect pos5 = new Rect(vector.x, vector.y + this.slotSize, this.borderSize, this.borderSize);
						pos5.position += new Vector2(-this.borderOffset, -this.borderOffset);
						Rect uv5 = new Rect(0.25f, 0.25f, 0.25f, 0.25f);
						this.DrawRectangle(pos5, uv5);
					}
					if (flag9 && flag15 && flag3)
					{
						Rect pos6 = new Rect(vector.x - this.borderSize, vector.y + this.slotSize - this.borderSize, this.borderSize, this.borderSize);
						pos6.position += new Vector2(this.borderOffset, this.borderOffset);
						Rect uv6 = new Rect(0.5f, 0.5f, 0.25f, 0.25f);
						this.DrawRectangle(pos6, uv6);
					}
					if (flag12 && flag15 && flag4)
					{
						Rect pos7 = new Rect(vector.x + this.slotSize, vector.y + this.slotSize - this.borderSize, this.borderSize, this.borderSize);
						pos7.position += new Vector2(-this.borderOffset, this.borderOffset);
						Rect uv7 = new Rect(0.25f, 0.5f, 0.25f, 0.25f);
						this.DrawRectangle(pos7, uv7);
					}
					if (flag14 && flag13 && flag4)
					{
						Rect pos8 = new Rect(vector.x + this.slotSize - this.borderSize, vector.y + this.slotSize, this.borderSize, this.borderSize);
						pos8.position += new Vector2(this.borderOffset, -this.borderOffset);
						Rect uv8 = new Rect(0.5f, 0.25f, 0.25f, 0.25f);
						this.DrawRectangle(pos8, uv8);
					}
					if (flag14 && flag9)
					{
						Rect pos9 = new Rect(vector.x - this.borderSize, vector.y + this.slotSize, this.borderSize, this.borderSize);
						pos9.position += new Vector2(this.borderOffset, -this.borderOffset);
						Rect uv9 = new Rect(0f, 0.75f, 0.25f, 0.25f);
						this.DrawRectangle(pos9, uv9);
					}
					if (flag14 && flag12)
					{
						Rect pos10 = new Rect(vector.x + this.slotSize, vector.y + this.slotSize, this.borderSize, this.borderSize);
						pos10.position += new Vector2(-this.borderOffset, -this.borderOffset);
						Rect uv10 = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
						this.DrawRectangle(pos10, uv10);
					}
					if (flag11 && flag9)
					{
						Rect pos11 = new Rect(vector.x - this.borderSize, vector.y - this.borderSize, this.borderSize, this.borderSize);
						pos11.position += new Vector2(this.borderOffset, this.borderOffset);
						Rect uv11 = new Rect(0f, 0f, 0.25f, 0.25f);
						this.DrawRectangle(pos11, uv11);
					}
					if (flag11 && flag12)
					{
						Rect pos12 = new Rect(vector.x + this.slotSize, vector.y - this.borderSize, this.borderSize, this.borderSize);
						pos12.position += new Vector2(-this.borderOffset, this.borderOffset);
						Rect uv12 = new Rect(0.75f, 0f, 0.25f, 0.25f);
						this.DrawRectangle(pos12, uv12);
					}
				}
			}
			this.SetBuffersToToMesh();
		}

		public void ShowBorderOnLevel(TilesSlotsProvider slotsProvider)
		{
			if (Application.isEditor)
			{
				this.cachedProviderDebug = slotsProvider;
			}
			if (this.dontConnectDiagonalSlots)
			{
				this.ShowBorderOnLevelDontConnectDiagonalSlots(slotsProvider);
				return;
			}
			this.ShowBorderOnLevelConnectDiagonalSlots(slotsProvider);
		}

		public void ShowBorderOnLevelConnectDiagonalSlots(TilesSlotsProvider slotsProvider)
		{
			this.Init();
			int maxSlots = slotsProvider.MaxSlots;
			int vertexCount = maxSlots * 8 * 4;
			int trisCount = maxSlots * 8 * 6;
			this.InitBuffers(vertexCount, trisCount);
			this.ClearBuffers();
			Vector2 a = slotsProvider.StartPosition(this.slotSize);
			Vector2 size = new Vector2(this.slotSize, this.slotSize);
			new Rect(0f, 0f, 0.5f, 0.5f);
			List<TilesSlotsProvider.Slot> allSlots = slotsProvider.GetAllSlots();
			for (int i = 0; i < allSlots.Count; i++)
			{
				TilesSlotsProvider.Slot slot = allSlots[i];
				if (!slot.isEmpty)
				{
					IntVector2 position = slot.position;
					Vector2 vector = a + new Vector2((float)position.x * this.slotSize, (float)position.y * this.slotSize);
					new Rect(vector, size);
					bool flag = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.up + IntVector2.left);
					bool flag2 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.up + IntVector2.right);
					bool flag3 = !flag;
					bool flag4 = !flag2;
					bool flag5 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.down + IntVector2.left);
					bool flag6 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.down + IntVector2.right);
					bool flag7 = !flag5;
					bool flag8 = !flag6;
					bool flag9 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.left);
					bool flag10 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.down);
					bool flag11 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.right);
					bool flag12 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.up);
					if (flag9)
					{
						Rect pos = new Rect(vector.x - this.borderSize, vector.y + this.borderSize, this.borderSize, this.slotSize - 2f * this.borderSize);
						Rect uv = new Rect(0f, 0.25f, 0.25f, 0.5f);
						if (flag5)
						{
							pos.yMin = vector.y;
						}
						if (flag)
						{
							pos.yMax += this.borderSize;
						}
						if (flag3)
						{
							pos.yMax += this.borderOffset;
						}
						if (flag7)
						{
							pos.yMin -= this.borderOffset;
						}
						if (flag10 && flag5)
						{
							pos.yMin += this.borderOffset;
						}
						if (flag12 && flag)
						{
							pos.yMax -= this.borderOffset;
						}
						pos.position += Vector2.right * this.borderOffset;
						this.DrawRectangle(pos, uv);
					}
					if (flag11)
					{
						Rect pos2 = new Rect(vector.x + this.slotSize, vector.y + this.borderSize, this.borderSize, this.slotSize - 2f * this.borderSize);
						Rect uv2 = new Rect(0.75f, 0.25f, 0.25f, 0.5f);
						if (flag6)
						{
							pos2.yMin = vector.y;
						}
						if (flag2)
						{
							pos2.yMax += this.borderSize;
						}
						if (flag4)
						{
							pos2.yMax += this.borderOffset;
						}
						if (flag8)
						{
							pos2.yMin -= this.borderOffset;
						}
						if (flag10 && flag6)
						{
							pos2.yMin += this.borderOffset;
						}
						if (flag12 && flag2)
						{
							pos2.yMax -= this.borderOffset;
						}
						pos2.position += Vector2.left * this.borderOffset;
						this.DrawRectangle(pos2, uv2);
					}
					if (flag10)
					{
						Rect pos3 = new Rect(vector.x + this.borderSize, vector.y - this.borderSize, this.slotSize - 2f * this.borderSize, this.borderSize);
						if (flag5)
						{
							pos3.xMin = vector.x;
						}
						if (flag6)
						{
							pos3.xMax += this.borderSize;
						}
						if (flag7)
						{
							pos3.xMin -= this.borderOffset;
						}
						if (flag8)
						{
							pos3.xMax += this.borderOffset;
						}
						if (flag5 && flag9)
						{
							pos3.xMin += this.borderOffset;
						}
						if (flag6 && flag11)
						{
							pos3.xMax -= this.borderOffset;
						}
						pos3.position += Vector2.up * this.borderOffset;
						Rect uv3 = new Rect(0.25f, 0f, 0.5f, 0.25f);
						this.DrawRectangle(pos3, uv3);
					}
					if (flag12)
					{
						Rect pos4 = new Rect(vector.x + this.borderSize, vector.y + this.slotSize, this.slotSize - 2f * this.borderSize, this.borderSize);
						if (flag)
						{
							pos4.xMin = vector.x;
						}
						if (flag2)
						{
							pos4.xMax += this.borderSize;
						}
						if (flag3)
						{
							pos4.xMin -= this.borderOffset;
						}
						if (flag4)
						{
							pos4.xMax += this.borderOffset;
						}
						if (flag && flag9)
						{
							pos4.xMin += this.borderOffset;
						}
						if (flag2 && flag11)
						{
							pos4.xMax -= this.borderOffset;
						}
						pos4.position += Vector2.down * this.borderOffset;
						Rect uv4 = new Rect(0.25f, 0.75f, 0.5f, 0.25f);
						this.DrawRectangle(pos4, uv4);
					}
					if (flag12 && flag3)
					{
						Rect pos5 = new Rect(vector.x, vector.y + this.slotSize, this.borderSize, this.borderSize);
						pos5.position += new Vector2(-this.borderOffset, -this.borderOffset);
						Rect uv5 = new Rect(0.25f, 0.25f, 0.25f, 0.25f);
						this.DrawRectangle(pos5, uv5);
					}
					if (flag9 && flag3)
					{
						Rect pos6 = new Rect(vector.x - this.borderSize, vector.y + this.slotSize - this.borderSize, this.borderSize, this.borderSize);
						pos6.position += new Vector2(this.borderOffset, this.borderOffset);
						Rect uv6 = new Rect(0.5f, 0.5f, 0.25f, 0.25f);
						this.DrawRectangle(pos6, uv6);
					}
					if (flag11 && flag4)
					{
						Rect pos7 = new Rect(vector.x + this.slotSize, vector.y + this.slotSize - this.borderSize, this.borderSize, this.borderSize);
						pos7.position += new Vector2(-this.borderOffset, this.borderOffset);
						Rect uv7 = new Rect(0.25f, 0.5f, 0.25f, 0.25f);
						this.DrawRectangle(pos7, uv7);
					}
					if (flag12 && flag4)
					{
						Rect pos8 = new Rect(vector.x + this.slotSize - this.borderSize, vector.y + this.slotSize, this.borderSize, this.borderSize);
						pos8.position += new Vector2(this.borderOffset, -this.borderOffset);
						Rect uv8 = new Rect(0.5f, 0.25f, 0.25f, 0.25f);
						this.DrawRectangle(pos8, uv8);
					}
					if (flag12 && flag && flag9)
					{
						Rect pos9 = new Rect(vector.x - this.borderSize, vector.y + this.slotSize, this.borderSize, this.borderSize);
						pos9.position += new Vector2(this.borderOffset, -this.borderOffset);
						Rect uv9 = new Rect(0f, 0.75f, 0.25f, 0.25f);
						this.DrawRectangle(pos9, uv9);
					}
					if (flag12 && flag2 && flag11)
					{
						Rect pos10 = new Rect(vector.x + this.slotSize, vector.y + this.slotSize, this.borderSize, this.borderSize);
						pos10.position += new Vector2(-this.borderOffset, -this.borderOffset);
						Rect uv10 = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
						this.DrawRectangle(pos10, uv10);
					}
					if (flag10 && flag5 && flag9)
					{
						Rect pos11 = new Rect(vector.x - this.borderSize, vector.y - this.borderSize, this.borderSize, this.borderSize);
						pos11.position += new Vector2(this.borderOffset, this.borderOffset);
						Rect uv11 = new Rect(0f, 0f, 0.25f, 0.25f);
						this.DrawRectangle(pos11, uv11);
					}
					if (flag10 && flag6 && flag11)
					{
						Rect pos12 = new Rect(vector.x + this.slotSize, vector.y - this.borderSize, this.borderSize, this.borderSize);
						pos12.position += new Vector2(-this.borderOffset, this.borderOffset);
						Rect uv12 = new Rect(0.75f, 0f, 0.25f, 0.25f);
						this.DrawRectangle(pos12, uv12);
					}
				}
			}
			this.SetBuffersToToMesh();
		}

		public void ShowSlotsOnLevel(LevelDefinition level)
		{
			LevelDefinitionTilesSlotsProvider slotsProvider = new LevelDefinitionTilesSlotsProvider(level);
			this.ShowSlotsOnLevel(slotsProvider);
		}

		public void ShowSlotsOnLevel(TilesSlotsProvider slotsProvider)
		{
			if (Application.isEditor)
			{
				this.cachedProviderDebug = slotsProvider;
			}
			if (this.dontConnectDiagonalSlots)
			{
				this.ShowSlotsOnLevelDontConnectDiagonalSlots(slotsProvider);
				return;
			}
			this.ShowSlotsOnLevelConnectDiagonalSlots(slotsProvider);
		}

		private void ShowSlotsOnLevelDontConnectDiagonalSlots(TilesSlotsProvider slotsProvider)
		{
			this.Init();
			int maxSlots = slotsProvider.MaxSlots;
			int vertexCount = maxSlots * 4;
			int trisCount = maxSlots * 6;
			this.InitBuffers(vertexCount, trisCount);
			this.ClearBuffers();
			Vector2 a = slotsProvider.StartPosition(this.slotSize);
			Vector2 size = new Vector2(this.slotSize, this.slotSize);
			Rect rect = new Rect(0f, 0f, 1f, 1f);
			float num = this.borderOffset / this.slotSize;
			List<TilesSlotsProvider.Slot> allSlots = slotsProvider.GetAllSlots();
			for (int i = 0; i < allSlots.Count; i++)
			{
				TilesSlotsProvider.Slot slot = allSlots[i];
				if (!slot.isEmpty)
				{
					IntVector2 position = slot.position;
					Vector2 position2 = a + new Vector2((float)position.x * this.slotSize, (float)position.y * this.slotSize);
					Rect uv = rect;
					Rect pos = new Rect(position2, size);
					bool flag = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.left);
					bool flag2 = !flag;
					bool flag3 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.right);
					bool flag4 = !flag3;
					bool flag5 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.up);
					bool flag6 = !flag5;
					bool flag7 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.down);
					bool flag8 = !flag7;
					bool flag9 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.up + IntVector2.left);
					bool flag10 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.up + IntVector2.right);
					bool flag11 = !flag9;
					bool flag12 = !flag10;
					bool flag13 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.down + IntVector2.left);
					bool flag14 = TilesBorderRenderer.IsEmptySlot(slotsProvider, position + IntVector2.down + IntVector2.right);
					bool flag15 = !flag13;
					bool flag16 = !flag14;
					if ((flag9 && flag2 && flag6) || (flag10 && flag4 && flag6) || (flag13 && flag2 && flag8) || (flag14 && flag4 && flag8))
					{
						if (flag2)
						{
							Rect pos2 = new Rect(pos.xMin, pos.yMin + this.borderOffset, this.borderOffset, pos.height - 2f * this.borderOffset);
							Rect uv2 = new Rect(rect.xMin, rect.yMin + num, num, rect.height - 2f * num);
							this.DrawRectangle(pos2, uv2);
						}
						if (flag4)
						{
							Rect pos3 = new Rect(pos.xMax - this.borderOffset, pos.yMin + this.borderOffset, this.borderOffset, pos.height - 2f * this.borderOffset);
							Rect uv3 = new Rect(rect.xMax - num, rect.yMin + num, num, rect.height - 2f * num);
							this.DrawRectangle(pos3, uv3);
						}
						if (flag6)
						{
							Rect pos4 = new Rect(pos.xMin + this.borderOffset, pos.yMax - this.borderOffset, pos.width - 2f * this.borderOffset, this.borderOffset);
							Rect uv4 = new Rect(rect.xMin + num, rect.yMax - num, rect.width - 2f * num, this.borderOffset);
							this.DrawRectangle(pos4, uv4);
						}
						if (flag8)
						{
							Rect pos5 = new Rect(pos.xMin + this.borderOffset, pos.yMin, pos.width - 2f * this.borderOffset, this.borderOffset);
							Rect uv5 = new Rect(rect.xMin + num, rect.yMin, rect.width - 2f * num, this.borderOffset);
							this.DrawRectangle(pos5, uv5);
						}
						if (flag11 && flag6 && flag2)
						{
							Rect pos6 = new Rect(pos.xMin, pos.yMax - this.borderOffset, this.borderOffset, this.borderOffset);
							Rect uv6 = new Rect(rect.xMin, rect.yMax - num, num, num);
							this.DrawRectangle(pos6, uv6);
						}
						if (flag15 && flag8 && flag2)
						{
							Rect pos7 = new Rect(pos.xMin, pos.yMin, this.borderOffset, this.borderOffset);
							Rect uv7 = new Rect(rect.xMin, rect.yMin, num, num);
							this.DrawRectangle(pos7, uv7);
						}
						if (flag12 && flag6 && flag4)
						{
							Rect pos8 = new Rect(pos.xMax - this.borderOffset, pos.yMax - this.borderOffset, this.borderOffset, this.borderOffset);
							Rect uv8 = new Rect(rect.xMax - num, rect.yMax - num, num, num);
							this.DrawRectangle(pos8, uv8);
						}
						if (flag16 && flag8 && flag4)
						{
							Rect pos9 = new Rect(pos.xMax - this.borderOffset, pos.yMin, this.borderOffset, this.borderOffset);
							Rect uv9 = new Rect(rect.xMax - num, rect.yMin, num, num);
							this.DrawRectangle(pos9, uv9);
						}
						Rect pos10 = new Rect(pos.xMin + this.borderOffset, pos.yMin + this.borderOffset, pos.width - 2f * this.borderOffset, pos.height - 2f * this.borderOffset);
						Rect uv10 = new Rect(rect.xMin + num, rect.yMin + num, rect.width - 2f * num, rect.height - 2f * num);
						this.DrawRectangle(pos10, uv10);
					}
					else
					{
						if (flag5)
						{
							pos.yMax -= this.borderOffset;
							uv.yMax -= num;
						}
						if (flag7)
						{
							pos.yMin += this.borderOffset;
							uv.yMin += num;
						}
						if (flag)
						{
							pos.xMin += this.borderOffset;
							uv.xMin += num;
						}
						if (flag3)
						{
							pos.xMax -= this.borderOffset;
							uv.xMax -= num;
						}
						this.DrawRectangle(pos, uv);
					}
				}
			}
			this.SetBuffersToToMesh();
		}

		private void ShowSlotsOnLevelConnectDiagonalSlots(TilesSlotsProvider slotsProvider)
		{
			this.Init();
			int maxSlots = slotsProvider.MaxSlots;
			int vertexCount = maxSlots * 4;
			int trisCount = maxSlots * 6;
			this.InitBuffers(vertexCount, trisCount);
			this.ClearBuffers();
			Vector2 a = slotsProvider.StartPosition(this.slotSize);
			Vector2 size = new Vector2(this.slotSize, this.slotSize);
			Rect uv = new Rect(0f, 0f, 1f, 1f);
			List<TilesSlotsProvider.Slot> allSlots = slotsProvider.GetAllSlots();
			for (int i = 0; i < allSlots.Count; i++)
			{
				TilesSlotsProvider.Slot slot = allSlots[i];
				if (!slot.isEmpty)
				{
					IntVector2 position = slot.position;
					Vector2 position2 = a + new Vector2((float)position.x * this.slotSize, (float)position.y * this.slotSize);
					Rect pos = new Rect(position2, size);
					this.DrawRectangle(pos, uv);
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
		private bool dontConnectDiagonalSlots;

		[SerializeField]
		public float slotSize = 1f;

		[SerializeField]
		private float borderSize = 0.25f;

		[SerializeField]
		private float borderOffset;

		public int sortingLayerId;

		public int sortingOrder;

		[SerializeField]
		private bool setSortingSettings;

		[SerializeField]
		private SpriteSortingSettings sortingSettings = new SpriteSortingSettings();

		private Mesh mesh;

		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private List<Vector3> vertexBuffer;

		private List<int> trisBuffer;

		private List<Vector2> uvBuffer;

		public TilesSlotsProvider cachedProviderDebug;
	}
}
