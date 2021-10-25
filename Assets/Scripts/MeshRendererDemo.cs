using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererDemo : MonoBehaviour
{
	private void Init()
	{
		if (this.isInitialized)
		{
			return;
		}
		this.isInitialized = true;
		this.meshFilter = base.gameObject.GetComponent<MeshFilter>();
		if (this.meshFilter == null)
		{
			this.meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.meshFilter.mesh = this.mesh;
		}
		int num = 9;
		this.vertexBuffer = new List<Vector3>(num * 4);
		this.trisBuffer = new List<int>(num * 6);
		this.uvBuffer = new List<Vector2>(num * 4);
	}

	private void Start()
	{
		this.Init();
	}

	private int GetIndex(int column, int row, int columns)
	{
		return column + row * (columns + 1);
	}

	public void DoUpdateMesh()
	{
		this.Init();
		this.vertexBuffer.Clear();
		this.trisBuffer.Clear();
		this.uvBuffer.Clear();
		Vector3 vector = new Vector3((float)(-(float)this.columns) * 0.5f * this.distance, (float)(-(float)this.rows) * 0.5f * this.distance, 0f);
		for (int i = 0; i <= this.rows; i++)
		{
			for (int j = 0; j <= this.columns; j++)
			{
				Vector3 vector2 = vector + (float)j * Vector3.right * this.distance + (float)i * Vector3.up * this.distance;
				Vector2 vector3 = UnityEngine.Random.insideUnitCircle * this.distance * 0.1f;
				vector2 += new Vector3(vector3.x, vector3.y, 0f);
				this.vertexBuffer.Add(vector2);
				Vector2 item = new Vector2(Mathf.InverseLerp(0f, (float)this.columns, (float)j), Mathf.InverseLerp(0f, (float)this.rows, (float)i));
				this.uvBuffer.Add(item);
				if (j < this.columns && i < this.rows)
				{
					this.trisBuffer.Add(this.GetIndex(j, i, this.columns));
					this.trisBuffer.Add(this.GetIndex(j + 1, i, this.columns));
					this.trisBuffer.Add(this.GetIndex(j, i + 1, this.columns));
					this.trisBuffer.Add(this.GetIndex(j + 1, i, this.columns));
					this.trisBuffer.Add(this.GetIndex(j + 1, i + 1, this.columns));
					this.trisBuffer.Add(this.GetIndex(j, i + 1, this.columns));
				}
			}
		}
		this.mesh.bounds.SetMinMax(vector, vector + Vector3.right * (float)this.columns * this.distance + Vector3.up * (float)this.rows * this.distance);
		this.mesh.Clear();
		this.mesh.SetVertices(this.vertexBuffer);
		this.mesh.SetUVs(0, this.uvBuffer);
		this.mesh.SetTriangles(this.trisBuffer, 0, false);
	}

	[SerializeField]
	private float distance = 0.2f;

	[SerializeField]
	private int rows = 4;

	[SerializeField]
	private int columns = 5;

	private MeshFilter meshFilter;

	private Mesh mesh;

	private List<Vector3> vertexBuffer;

	private List<int> trisBuffer;

	private List<Vector2> uvBuffer;

	private bool isInitialized;
}
