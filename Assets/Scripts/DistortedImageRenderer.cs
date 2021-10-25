using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DistortedImageRenderer
{
	public void PopulateMesh(VertexHelper toFill)
	{
		this.FillMeshDistortCorners(toFill);
	}

	private Rect GetUVs(Sprite sprite)
	{
		Rect rect = sprite.rect;
		rect.x /= (float)sprite.texture.width;
		rect.width /= (float)sprite.texture.width;
		rect.y /= (float)sprite.texture.height;
		rect.height /= (float)sprite.texture.height;
		return rect;
	}

	private void FillMeshDistortCorners(VertexHelper toFill)
	{
		toFill.Clear();
		Texture2D texture = this.sprite.texture;
		Rect rect = this.sprite.rect;
		Vector4 border = this.sprite.border;
		float x = border.x;
		float z = border.z;
		float y = border.y;
		float w = border.w;
		Rect uvs = this.GetUVs(this.sprite);
		int width = texture.width;
		int height = texture.height;
		float num = x / (float)width;
		float num2 = z / (float)width;
		float num3 = w / (float)height;
		float num4 = y / (float)height;
		Vector2 vector = new Vector2(uvs.xMin, uvs.yMin);
		Vector2 vector2 = new Vector2(uvs.xMin + num, uvs.yMin + num4);
		Vector2 vector3 = new Vector2(uvs.xMax, uvs.yMin);
		Vector2 vector4 = new Vector2(uvs.xMax - num2, uvs.yMin + num4);
		Vector2 vector5 = new Vector2(uvs.xMax, uvs.yMax);
		Vector2 vector6 = new Vector2(uvs.xMax - num2, uvs.yMax - num3);
		Vector2 vector7 = new Vector2(uvs.xMin, uvs.yMax);
		Vector2 vector8 = new Vector2(uvs.xMin + num, uvs.yMax - num3);
		float num5 = rect.size.x - x - z;
		float borderLength = num5;
		float num6 = rect.size.y - w - y;
		float borderLength2 = num6;
		if (this.innerVertex == null)
		{
			this.innerVertex = new DistortedImageRenderer.VertexProperties[4];
		}
		this.innerVertex[0].Init(this.bl, this.br, y, num5);
		this.innerVertex[0].InitOuterUV(vector2, vector4, vector, vector3);
		this.innerVertex[0].InitBorderUV(vector2, vector4, new Vector2(vector2.x, vector.y), new Vector2(vector4.x, vector3.y));
		this.innerVertex[1].Init(this.br, this.tr, z, borderLength2);
		this.innerVertex[1].InitOuterUV(vector4, vector6, vector3, vector5);
		this.innerVertex[1].InitBorderUV(vector4, vector6, new Vector2(vector3.x, vector4.y), new Vector2(vector5.x, vector6.y));
		this.innerVertex[2].Init(this.tr, this.tl, w, borderLength);
		this.innerVertex[2].InitOuterUV(vector6, vector8, vector5, vector7);
		this.innerVertex[2].InitBorderUV(vector6, vector8, new Vector2(vector6.x, vector5.y), new Vector2(vector8.x, vector7.y));
		this.innerVertex[3].Init(this.tl, this.bl, x, num6);
		this.innerVertex[3].InitOuterUV(vector8, vector2, vector7, vector);
		this.innerVertex[3].InitBorderUV(vector8, vector2, new Vector2(vector7.x, vector8.y), new Vector2(vector.x, vector2.y));
		if (this.quad == null)
		{
			this.quad = new UIVertex[4];
		}
		for (int i = 0; i < this.innerVertex.Length; i++)
		{
			DistortedImageRenderer.VertexProperties vertexProperties = this.innerVertex[i];
			float num7 = Vector3.Distance(vertexProperties.GetVertexBorder(0f, 0f), vertexProperties.GetVertexBorder(1f, 0f));
			float num8 = num7;
			float num9 = 0f;
			do
			{
				float num10 = Mathf.Min(vertexProperties.borderLength, num8);
				num8 -= vertexProperties.borderLength;
				float x2 = num9 / num7;
				float x3 = Mathf.Clamp01((num9 + num10) / num7);
				float x4 = num10 / vertexProperties.borderLength;
				this.quad[0].position = vertexProperties.GetVertexBorder(x2, 0f);
				this.quad[0].color = this.color;
				this.quad[0].uv0 = vertexProperties.borderUv00;
				this.quad[1].position = vertexProperties.GetVertexBorder(x3, 0f);
				this.quad[1].color = this.color;
				this.quad[1].uv0 = vertexProperties.GetVertexBorderUV(x4, 0f);
				this.quad[2].position = vertexProperties.GetVertexBorder(x3, 1f);
				this.quad[2].color = this.color;
				this.quad[2].uv0 = vertexProperties.GetVertexBorderUV(x4, 1f);
				this.quad[3].position = vertexProperties.GetVertexBorder(x2, 1f);
				this.quad[3].color = this.color;
				this.quad[3].uv0 = vertexProperties.borderUv01;
				toFill.AddUIVertexQuad(this.quad);
				num9 += num10;
			}
			while (num8 > 0f);
		}
		for (int j = 0; j < this.innerVertex.Length; j++)
		{
			DistortedImageRenderer.VertexProperties vertexProperties2 = this.innerVertex[j];
			DistortedImageRenderer.VertexProperties vertexProperties3 = this.innerVertex[(j + 1) % this.innerVertex.Length];
			float d = Vector3.Dot(vertexProperties2.directionToNext, vertexProperties3.orthoDirection) * vertexProperties3.borderWidth;
			Vector3 vector9 = vertexProperties2.GetVertexBorder(1f, 1f) + vertexProperties2.directionToNext * d;
			float num11 = Vector3.Distance(vertexProperties3.GetVertexBorder(0f, 1f), vector9);
			float num12 = num11 * num11;
			float num13 = Vector3.Dot(vertexProperties2.orthoDirection, -vertexProperties3.directionToNext);
			float d2 = Mathf.Sqrt(num12 / (num13 * num13) - num12);
			Vector3 position = vector9 + vertexProperties2.directionToNext * d2 * Mathf.Sign(Vector3.Dot(vertexProperties2.directionToNext, -vertexProperties3.directionToNext));
			this.quad[0].position = vertexProperties2.GetVertexBorder(1f, 0f);
			this.quad[0].color = this.color;
			this.quad[0].uv0 = vertexProperties2.borderUv10;
			this.quad[1].position = vertexProperties2.GetVertexBorder(1f, 1f);
			this.quad[1].color = this.color;
			this.quad[1].uv0 = vertexProperties2.borderUv11;
			this.quad[2].position = position;
			this.quad[2].color = this.color;
			this.quad[2].uv0 = vertexProperties2.uv11;
			this.quad[3].position = vertexProperties3.GetVertexBorder(0f, 1f);
			this.quad[3].color = this.color;
			this.quad[3].uv0 = vertexProperties3.borderUv01;
			toFill.AddUIVertexQuad(this.quad);
		}
		for (int k = 0; k < this.innerVertex.Length; k++)
		{
			DistortedImageRenderer.VertexProperties vertexProperties4 = this.innerVertex[k];
			this.quad[k].position = vertexProperties4.GetVertexBorder(0f, 0f);
			this.quad[k].color = this.innerColor;
			this.quad[k].uv0 = vertexProperties4.borderUv00;
		}
		toFill.AddUIVertexQuad(this.quad);
	}

	[SerializeField]
	public Color color;

	[SerializeField]
	public Sprite sprite;

	public Vector3 tl;

	public Vector3 tr;

	public Vector3 bl;

	public Vector3 br;

	[SerializeField]
	private Color innerColor;

	private UIVertex[] quad;

	private DistortedImageRenderer.VertexProperties[] innerVertex;

	public struct VertexProperties
	{
		public Vector3 GetVertexBorder(float x, float y)
		{
			return Vector3.Lerp(this.vertexInner, this.nextVertexInner, x) + Vector3.Lerp(Vector3.zero, this.orthoDirection * this.borderWidth, y);
		}

		public Vector2 GetVertexBorderUV(float x, float y)
		{
			Vector2 a = this.borderUv01 - this.borderUv00;
			Vector2 a2 = this.borderUv10 - this.borderUv00;
			return this.borderUv00 + a2 * x + a * y;
		}

		public void Init(Vector3 currentPos, Vector3 nextPos, float borderWidth, float borderLength)
		{
			this.vertexInner = currentPos;
			this.borderWidth = borderWidth;
			this.nextVertexInner = nextPos;
			this.borderLength = borderLength;
			this.directionToNext = (nextPos - currentPos).normalized;
			this.orthoDirection = Vector3.Cross(this.directionToNext, Vector3.forward);
		}

		public void InitOuterUV(Vector2 uv00, Vector2 uv10, Vector2 uv01, Vector2 uv11)
		{
			this.uv00 = uv00;
			this.uv10 = uv10;
			this.uv01 = uv01;
			this.uv11 = uv11;
		}

		public void InitBorderUV(Vector2 borderUv00, Vector2 borderUv10, Vector2 borderUv01, Vector2 borderUv11)
		{
			this.borderUv00 = borderUv00;
			this.borderUv10 = borderUv10;
			this.borderUv01 = borderUv01;
			this.borderUv11 = borderUv11;
		}

		public Vector3 vertexInner;

		public Vector3 nextVertexInner;

		public Vector3 directionToNext;

		public Vector3 orthoDirection;

		public float borderWidth;

		public float borderLength;

		public Vector2 uv00;

		public Vector2 uv10;

		public Vector2 uv11;

		public Vector2 uv01;

		public Vector2 borderUv00;

		public Vector2 borderUv10;

		public Vector2 borderUv11;

		public Vector2 borderUv01;
	}
}
