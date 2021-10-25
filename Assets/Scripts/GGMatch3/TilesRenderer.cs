using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	[RequireComponent(typeof(MeshFilter))]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshRenderer))]
	public class TilesRenderer : MonoBehaviour
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

		private void InitBuffers(int maxSlots)
		{
			int capacity = maxSlots * 9 * 4;
			int capacity2 = maxSlots * 9 * 6;
			if (this.vertexBuffer == null)
			{
				this.vertexBuffer = new List<Vector3>(capacity);
				this.uvBuffer = new List<Vector2>(capacity);
				this.uv1Buffer = new List<Vector2>(capacity);
				this.uv2Buffer = new List<Vector2>(capacity);
				this.trisBuffer = new List<int>(capacity2);
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

		private Rect SolidRectMask()
		{
			return new Rect
			{
				center = new Vector2(0.21f, 0.21f),
				size = new Vector2(0.125f, 0.125f) * 0.05f
			};
		}

		public static bool IsEmptySlot(LevelDefinition level, IntVector2 position)
		{
			LevelDefinition.SlotDefinition slot = level.GetSlot(position);
			return slot == null || slot.slotType == SlotType.Empty;
		}

		public static bool IsOccupiedSlot(LevelDefinition level, IntVector2 position)
		{
			LevelDefinition.SlotDefinition slot = level.GetSlot(position);
			return slot != null && slot.slotType == SlotType.PlayingSpace;
		}

		public void ShowLevel()
		{
			LevelDefinition levelDefinition = ScriptableObjectSingleton<LevelDB>.instance.levels[0];
			this.Init();
			this.InitBuffers(levelDefinition.size.width * levelDefinition.size.height);
			this.vertexBuffer.Clear();
			this.trisBuffer.Clear();
			this.uvBuffer.Clear();
			this.uv1Buffer.Clear();
			this.uv2Buffer.Clear();
			Vector2 a = new Vector2((float)(-(float)levelDefinition.size.width) * this.slotSize * 0.5f, (float)(-(float)levelDefinition.size.height) * this.slotSize * 0.5f);
			Rect rect = this.SolidRectMask();
			Vector2 size = new Vector2(this.slotSize, this.slotSize);
			Rect uv = new Rect(0f, 0f, 0.5f, 0.5f);
			Rect uv2 = new Rect(0.05f, 0.05f, 0.1f, 0.1f);
			List<LevelDefinition.SlotDefinition> slots = levelDefinition.slots;
			for (int i = 0; i < slots.Count; i++)
			{
				LevelDefinition.SlotDefinition slotDefinition = slots[i];
				if (slotDefinition.slotType != SlotType.Empty)
				{
					IntVector2 position = slotDefinition.position;
					Vector2 vector = a + new Vector2((float)position.x * this.slotSize, (float)position.y * this.slotSize);
					Rect pos = new Rect(vector, size);
					this.DrawRectangle(pos, uv, rect, rect);
					bool flag = TilesRenderer.IsEmptySlot(levelDefinition, position + IntVector2.up + IntVector2.left);
					bool flag2 = TilesRenderer.IsEmptySlot(levelDefinition, position + IntVector2.up + IntVector2.right);
					bool flag3 = !flag;
					bool flag4 = !flag2;
					bool flag5 = TilesRenderer.IsEmptySlot(levelDefinition, position + IntVector2.down + IntVector2.left);
					bool flag6 = TilesRenderer.IsEmptySlot(levelDefinition, position + IntVector2.down + IntVector2.right);
					bool flag7 = TilesRenderer.IsEmptySlot(levelDefinition, position + IntVector2.left);
					bool flag8 = TilesRenderer.IsEmptySlot(levelDefinition, position + IntVector2.down);
					bool flag9 = TilesRenderer.IsEmptySlot(levelDefinition, position + IntVector2.right);
					bool flag10 = TilesRenderer.IsEmptySlot(levelDefinition, position + IntVector2.up);
					if (flag7)
					{
						Rect pos2 = new Rect(vector.x - this.borderSize, vector.y + this.borderSize, this.borderSize, this.slotSize - 2f * this.borderSize);
						Rect rect2 = new Rect(0f, 0.25f, 0.25f, 0.5f);
						if (flag5)
						{
							pos2.yMin = vector.y;
						}
						if (flag)
						{
							pos2.yMax += this.borderSize;
						}
						this.DrawRectangle(pos2, uv2, rect2, rect2);
					}
					if (flag9)
					{
						Rect pos3 = new Rect(vector.x + this.slotSize, vector.y + this.borderSize, this.borderSize, this.slotSize - 2f * this.borderSize);
						Rect rect3 = new Rect(0.75f, 0.25f, 0.25f, 0.5f);
						if (flag6)
						{
							pos3.yMin = vector.y;
						}
						if (flag2)
						{
							pos3.yMax += this.borderSize;
						}
						this.DrawRectangle(pos3, uv2, rect3, rect3);
					}
					if (flag8)
					{
						Rect pos4 = new Rect(vector.x + this.borderSize, vector.y - this.borderSize, this.slotSize - 2f * this.borderSize, this.borderSize);
						if (flag5)
						{
							pos4.xMin = vector.x;
						}
						if (flag6)
						{
							pos4.xMax += this.borderSize;
						}
						Rect rect4 = new Rect(0.25f, 0f, 0.5f, 0.25f);
						this.DrawRectangle(pos4, uv2, rect4, rect4);
					}
					if (flag10)
					{
						Rect pos5 = new Rect(vector.x + this.borderSize, vector.y + this.slotSize, this.slotSize - 2f * this.borderSize, this.borderSize);
						if (flag)
						{
							pos5.xMin = vector.x;
						}
						if (flag2)
						{
							pos5.xMax += this.borderSize;
						}
						Rect rect5 = new Rect(0.25f, 0.75f, 0.5f, 0.25f);
						this.DrawRectangle(pos5, uv2, rect5, rect5);
					}
					if (flag10 && flag3)
					{
						Rect pos6 = new Rect(vector.x, vector.y + this.slotSize, this.borderSize, this.borderSize);
						Rect rect6 = new Rect(0.25f, 0.25f, 0.25f, 0.25f);
						this.DrawRectangle(pos6, uv2, rect6, rect6);
					}
					if (flag7 && flag3)
					{
						Rect pos7 = new Rect(vector.x - this.borderSize, vector.y + this.slotSize - this.borderSize, this.borderSize, this.borderSize);
						Rect rect7 = new Rect(0.5f, 0.5f, 0.25f, 0.25f);
						this.DrawRectangle(pos7, uv2, rect7, rect7);
					}
					if (flag9 && flag4)
					{
						Rect pos8 = new Rect(vector.x + this.slotSize, vector.y + this.slotSize - this.borderSize, this.borderSize, this.borderSize);
						Rect rect8 = new Rect(0.25f, 0.5f, 0.25f, 0.25f);
						this.DrawRectangle(pos8, uv2, rect8, rect8);
					}
					if (flag10 && flag4)
					{
						Rect pos9 = new Rect(vector.x + this.slotSize - this.borderSize, vector.y + this.slotSize, this.borderSize, this.borderSize);
						Rect rect9 = new Rect(0.5f, 0.25f, 0.25f, 0.25f);
						this.DrawRectangle(pos9, uv2, rect9, rect9);
					}
					if (flag10 && flag && flag7)
					{
						Rect pos10 = new Rect(vector.x - this.borderSize, vector.y + this.slotSize, this.borderSize, this.borderSize);
						Rect rect10 = new Rect(0f, 0.75f, 0.25f, 0.25f);
						this.DrawRectangle(pos10, uv2, rect10, rect10);
					}
					if (flag10 && flag2 && flag9)
					{
						Rect pos11 = new Rect(vector.x + this.slotSize, vector.y + this.slotSize, this.borderSize, this.borderSize);
						Rect rect11 = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
						this.DrawRectangle(pos11, uv2, rect11, rect11);
					}
					if (flag8 && flag5 && flag7)
					{
						Rect pos12 = new Rect(vector.x - this.borderSize, vector.y - this.borderSize, this.borderSize, this.borderSize);
						Rect rect12 = new Rect(0f, 0f, 0.25f, 0.25f);
						this.DrawRectangle(pos12, uv2, rect12, rect12);
					}
					if (flag8 && flag6 && flag9)
					{
						Rect pos13 = new Rect(vector.x + this.slotSize, vector.y - this.borderSize, this.borderSize, this.borderSize);
						Rect rect13 = new Rect(0.75f, 0f, 0.25f, 0.25f);
						this.DrawRectangle(pos13, uv2, rect13, rect13);
					}
				}
			}
			this.mesh.Clear();
			this.mesh.SetVertices(this.vertexBuffer);
			this.mesh.SetUVs(0, this.uvBuffer);
			this.mesh.SetUVs(1, this.uv1Buffer);
			this.mesh.SetTriangles(this.trisBuffer, 0, true);
		}

		private void DrawRectangle(Rect pos, Rect uv, Rect uv1, Rect uv2)
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
			this.uv1Buffer.Add(new Vector2(uv1.xMin, uv1.yMin));
			this.uv1Buffer.Add(new Vector2(uv1.xMax, uv1.yMin));
			this.uv1Buffer.Add(new Vector2(uv1.xMax, uv1.yMax));
			this.uv1Buffer.Add(new Vector2(uv1.xMin, uv1.yMax));
			this.uv2Buffer.Add(new Vector2(uv2.xMin, uv2.yMin));
			this.uv2Buffer.Add(new Vector2(uv2.xMax, uv2.yMin));
			this.uv2Buffer.Add(new Vector2(uv2.xMax, uv2.yMax));
			this.uv2Buffer.Add(new Vector2(uv2.xMin, uv2.yMax));
			this.trisBuffer.Add(count);
			this.trisBuffer.Add(count + 2);
			this.trisBuffer.Add(count + 1);
			this.trisBuffer.Add(count + 2);
			this.trisBuffer.Add(count);
			this.trisBuffer.Add(count + 3);
		}

		[SerializeField]
		private float slotSize = 1f;

		[SerializeField]
		private float borderSize = 0.25f;

		public int sortingLayerId;

		public int sortingOrder;

		private Mesh mesh;

		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private List<Vector3> vertexBuffer;

		private List<int> trisBuffer;

		private List<Vector2> uvBuffer;

		private List<Vector2> uv1Buffer;

		private List<Vector2> uv2Buffer;
	}
}
