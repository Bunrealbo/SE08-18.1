using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[DisallowMultipleComponent]
	public class TiledSpriteRenderer : MonoBehaviour
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

		private void InitBuffers(int maxRectangles)
		{
			int capacity = maxRectangles * 4;
			int capacity2 = maxRectangles * 6;
			if (this.vertexBuffer == null)
			{
				this.vertexBuffer = new List<Vector3>(capacity);
				this.uvBuffer = new List<Vector2>(capacity);
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
			this.sortingSettings.Set(this.meshRenderer);
		}

		public void ClearAndInit(int numRectangles)
		{
			this.Init();
			this.InitBuffers(numRectangles);
		}

		public void DrawRectangle(Rect pos, Rect uv)
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

		public void CopyToMesh()
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.vertexBuffer);
			this.mesh.SetUVs(0, this.uvBuffer);
			this.mesh.SetTriangles(this.trisBuffer, 0, true);
		}

		[SerializeField]
		private SpriteSortingSettings sortingSettings = new SpriteSortingSettings();

		private Mesh mesh;

		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private List<Vector3> vertexBuffer;

		private List<int> trisBuffer;

		private List<Vector2> uvBuffer;
	}
}
