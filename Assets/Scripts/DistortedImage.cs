using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistortedImage : Image
{
	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		if (this.fixControlPointsToRectCorners)
		{
			base.rectTransform.GetLocalCorners(this.localCorners);
			this.bl = this.localCorners[0];
			this.tl = this.localCorners[1];
			this.tr = this.localCorners[2];
			this.br = this.localCorners[3];
		}
		if (this.implementation == DistortedImage.Implementation.FillMesh)
		{
			this.FillMeshDistortCorners(toFill);
			return;
		}
		if (this.implementation == DistortedImage.Implementation.CreateMesh2)
		{
			this.CreateMesh2(toFill);
		}
	}

	public void PopulateMesh5(VertexHelper toFill)
	{
		toFill.Clear();
		Vector4 border = base.sprite.border;
		Vector2 size = base.sprite.rect.size;
		float x = border.x / size.x;
		float x2 = 1f - border.z / size.x;
		float y = 1f - border.w / size.y;
		float y2 = border.y / size.y;
		List<Vector2> uvs = new List<Vector2>
		{
			new Vector2(x, y2),
			new Vector2(x, y),
			new Vector2(0f, y),
			new Vector2(0f, y2)
		};
		List<Vector2> uvs2 = new List<Vector2>
		{
			new Vector2(x, y),
			new Vector2(x2, y),
			new Vector2(x2, 1f),
			new Vector2(x, 1f)
		};
		List<Vector2> uvs3 = new List<Vector2>
		{
			new Vector2(1f, y2),
			new Vector2(1f, y),
			new Vector2(x2, y),
			new Vector2(x2, y2)
		};
		List<Vector2> uvs4 = new List<Vector2>
		{
			new Vector2(x, 0f),
			new Vector2(x2, 0f),
			new Vector2(x2, y2),
			new Vector2(x, y2)
		};
		this.PopulateMesh5Side(toFill, this.bl, this.tl, uvs, size.y, true);
		this.PopulateMesh5Side(toFill, this.tl, this.tr, uvs2, size.x, false);
		this.PopulateMesh5Side(toFill, this.br, this.tr, uvs3, size.y, true);
		this.PopulateMesh5Side(toFill, this.bl, this.br, uvs4, size.x, false);
		UIVertex v = default(UIVertex);
		v.position = this.bl;
		v.normal = Vector3.back;
		v.uv0 = new Vector2(x, y2);
		v.color = this.innerColor;
		UIVertex v2 = default(UIVertex);
		v2.position = this.tl;
		v2.normal = Vector3.back;
		v2.uv0 = new Vector2(x, y);
		v2.color = this.innerColor;
		UIVertex v3 = default(UIVertex);
		v3.position = this.tr;
		v3.normal = Vector3.back;
		v3.uv0 = new Vector2(x2, y);
		v3.color = this.innerColor;
		UIVertex v4 = default(UIVertex);
		v4.position = this.br;
		v4.normal = Vector3.back;
		v4.uv0 = new Vector2(x2, y2);
		v4.color = this.innerColor;
		toFill.AddVert(v);
		toFill.AddVert(v2);
		toFill.AddVert(v3);
		toFill.AddVert(v4);
		int currentVertCount = toFill.currentVertCount;
		toFill.AddTriangle(currentVertCount - 4, currentVertCount - 3, currentVertCount - 2);
		toFill.AddTriangle(currentVertCount - 4, currentVertCount - 2, currentVertCount - 1);
	}

	private void PopulateMesh5Side(VertexHelper toFill, Vector3 start, Vector3 end, List<Vector2> uvs, float size, bool isVertical)
	{
		Vector3 normalized = (end - start).normalized;
		Vector3 a = Vector3.Cross(normalized, Vector3.forward);
		int num = Mathf.CeilToInt((end - start).magnitude / size);
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = start + normalized * size * (float)i;
			Vector3 a2 = start + normalized * size * (float)(i + 1);
			float num2 = 1f;
			if (i == num - 1)
			{
				num2 = Vector3.Distance(end, vector) / Vector3.Distance(a2, vector);
				a2 = end + normalized * this.outlineWidth * 0.5f;
			}
			Vector2 b = new Vector2(num2, 1f);
			if (isVertical)
			{
				b.y = num2;
				b.x = 1f;
			}
			UIVertex v = default(UIVertex);
			v.position = vector + a * this.outlineWidth * 0.5f;
			v.uv0 = uvs[0];
			v.normal = Vector3.back;
			v.color = this.color;
			UIVertex v2 = default(UIVertex);
			v2.position = a2 + a * this.outlineWidth * 0.5f;
			v2.uv0 = uvs[0] + Vector2.Scale(uvs[1] - uvs[0], b);
			v2.normal = Vector3.back;
			v2.color = this.color;
			UIVertex v3 = default(UIVertex);
			v3.position = a2 - a * this.outlineWidth * 0.5f;
			v3.uv0 = uvs[3] + Vector2.Scale(uvs[2] - uvs[3], b);
			v3.normal = Vector3.back;
			v3.color = this.color;
			UIVertex v4 = default(UIVertex);
			v4.position = vector - a * this.outlineWidth * 0.5f;
			v4.normal = Vector3.back;
			v4.color = this.color;
			v4.uv0 = uvs[3];
			int currentVertCount = toFill.currentVertCount;
			toFill.AddVert(v);
			toFill.AddVert(v2);
			toFill.AddVert(v3);
			toFill.AddVert(v4);
			toFill.AddTriangle(currentVertCount + 3, currentVertCount + 1, currentVertCount);
			toFill.AddTriangle(currentVertCount + 2, currentVertCount + 1, currentVertCount + 3);
		}
	}

	public void PopulateMesh4(VertexHelper toFill)
	{
		toFill.Clear();
		Vector4 border = base.sprite.border;
		Vector2 size = base.sprite.rect.size;
		float x = border.x / size.x;
		float x2 = 1f - border.z / size.x;
		float y = 1f - border.w / size.y;
		float y2 = border.y / size.y;
		Vector3 normalized = (this.br - this.bl).normalized;
		Vector3 normalized2 = (this.tl - this.bl).normalized;
		List<Vector2> uvs = new List<Vector2>
		{
			new Vector2(0f, 0f),
			new Vector2(x, 0f),
			new Vector2(x, y2),
			new Vector2(0f, y2)
		};
		this.CreateMeshCorner4(toFill, this.bl, uvs, normalized, normalized2);
		normalized = (this.br - this.bl).normalized;
		normalized2 = (this.tr - this.br).normalized;
		List<Vector2> uvs2 = new List<Vector2>
		{
			new Vector2(x2, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, y2),
			new Vector2(x2, y2)
		};
		this.CreateMeshCorner4(toFill, this.br, uvs2, normalized, normalized2);
		normalized = (this.tr - this.tl).normalized;
		normalized2 = (this.tr - this.br).normalized;
		List<Vector2> uvs3 = new List<Vector2>
		{
			new Vector2(x2, y),
			new Vector2(1f, y),
			new Vector2(1f, 1f),
			new Vector2(x2, 1f)
		};
		this.CreateMeshCorner4(toFill, this.tr, uvs3, normalized, normalized2);
		normalized = (this.tr - this.tl).normalized;
		normalized2 = (this.tl - this.bl).normalized;
		List<Vector2> uvs4 = new List<Vector2>
		{
			new Vector2(0f, y),
			new Vector2(x, y),
			new Vector2(x, 1f),
			new Vector2(0f, 1f)
		};
		this.CreateMeshCorner4(toFill, this.tl, uvs4, normalized, normalized2);
		UIVertex uivertex = default(UIVertex);
		toFill.PopulateUIVertex(ref uivertex, 3);
		Vector3 position = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 2);
		Vector3 position2 = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 13);
		Vector3 position3 = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 12);
		Vector3 position4 = uivertex.position;
		List<Vector3> corners = new List<Vector3>
		{
			position2,
			position,
			position4,
			position3
		};
		List<Vector2> uvs5 = new List<Vector2>
		{
			new Vector2(x, y2),
			new Vector2(x, y),
			new Vector2(0f, y),
			new Vector2(0f, y2)
		};
		this.CreateMeshSide4(toFill, corners, uvs5, (this.br - this.bl).normalized, (this.tr - this.tl).normalized, size.y, true);
		uivertex = default(UIVertex);
		toFill.PopulateUIVertex(ref uivertex, 1);
		position = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 4);
		position2 = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 7);
		position3 = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 2);
		position4 = uivertex.position;
		corners = new List<Vector3>
		{
			position,
			position4,
			position3,
			position2
		};
		uvs5 = new List<Vector2>
		{
			new Vector2(x, 0f),
			new Vector2(x2, 0f),
			new Vector2(x2, y2),
			new Vector2(x, y2)
		};
		this.CreateMeshSide4(toFill, corners, uvs5, (this.bl - this.tl).normalized, (this.br - this.tr).normalized, size.x, false);
		uivertex = default(UIVertex);
		toFill.PopulateUIVertex(ref uivertex, 7);
		position = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 6);
		position2 = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 9);
		position3 = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 8);
		position4 = uivertex.position;
		corners = new List<Vector3>
		{
			position,
			position2,
			position4,
			position3
		};
		uvs5 = new List<Vector2>
		{
			new Vector2(1f, y2),
			new Vector2(1f, y),
			new Vector2(x2, y),
			new Vector2(x2, y2)
		};
		this.CreateMeshSide4(toFill, corners, uvs5, (this.br - this.bl).normalized, (this.tr - this.tl).normalized, size.y, true);
		uivertex = default(UIVertex);
		toFill.PopulateUIVertex(ref uivertex, 13);
		position = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 8);
		position2 = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 11);
		position3 = uivertex.position;
		toFill.PopulateUIVertex(ref uivertex, 14);
		position4 = uivertex.position;
		corners = new List<Vector3>
		{
			position,
			position4,
			position2,
			position3
		};
		uvs5 = new List<Vector2>
		{
			new Vector2(x, y),
			new Vector2(x2, y),
			new Vector2(x2, 1f),
			new Vector2(x, 1f)
		};
		this.CreateMeshSide4(toFill, corners, uvs5, (this.tl - this.bl).normalized, (this.tr - this.br).normalized, size.x, false);
		toFill.AddTriangle(2, 8, 7);
		toFill.AddTriangle(2, 13, 8);
	}

	public void CreateMeshCorner4(VertexHelper toFill, Vector3 referencePosition, List<Vector2> uvs, Vector3 horizontalDirection, Vector3 verticalDirection)
	{
		int currentVertCount = toFill.currentVertCount;
		toFill.AddVert(new UIVertex
		{
			position = referencePosition + (-verticalDirection - horizontalDirection) * this.outlineWidth * 0.5f,
			uv0 = uvs[0],
			color = this.color,
			normal = Vector3.back
		});
		toFill.AddVert(new UIVertex
		{
			position = referencePosition + (-verticalDirection + horizontalDirection) * this.outlineWidth * 0.5f,
			uv0 = uvs[1],
			color = this.color,
			normal = Vector3.back
		});
		toFill.AddVert(new UIVertex
		{
			position = referencePosition + (verticalDirection + horizontalDirection) * this.outlineWidth * 0.5f,
			uv0 = uvs[2],
			color = this.color,
			normal = Vector3.back
		});
		toFill.AddVert(new UIVertex
		{
			position = referencePosition + (verticalDirection - horizontalDirection) * this.outlineWidth * 0.5f,
			uv0 = uvs[3],
			color = this.color,
			normal = Vector3.back
		});
		toFill.AddTriangle(currentVertCount + 1, currentVertCount, currentVertCount + 2);
		toFill.AddTriangle(currentVertCount, currentVertCount + 3, currentVertCount + 2);
	}

	public void CreateMeshSide4(VertexHelper toFill, List<Vector3> corners, List<Vector2> uvs, Vector3 tangentStart, Vector3 tangentEnd, float size, bool isVertical)
	{
		Vector3 vector = corners[0] * 0.5f + corners[1] * 0.5f;
		Vector3 vector2 = corners[2] * 0.5f + corners[3] * 0.5f;
		Vector3 normalized = (vector2 - vector).normalized;
		int num = Mathf.CeilToInt((vector2 - vector).magnitude / size);
		float a = Vector3.Distance(corners[0], corners[1]);
		float b = Vector3.Distance(corners[2], corners[3]);
		for (int i = 0; i < num; i++)
		{
			Vector3 vector3 = vector + normalized * size * (float)i;
			Vector3 a2 = vector + normalized * size * (float)(i + 1);
			float num2 = 1f;
			bool flag = i == num - 1;
			if (flag)
			{
				num2 = Vector3.Distance(vector2, vector3) / Vector3.Distance(a2, vector3);
				a2 = vector2;
			}
			float d = Mathf.Lerp(a, b, Vector3.Distance(vector3, vector) / Vector3.Distance(vector, vector2));
			float d2 = Mathf.Lerp(a, b, Vector3.Distance(a2, vector) / Vector3.Distance(vector, vector2));
			Vector2 b2 = new Vector2(num2, 1f);
			if (isVertical)
			{
				b2.y = num2;
				b2.x = 1f;
			}
			UIVertex v = default(UIVertex);
			Vector3 a3 = Vector3.Lerp(tangentStart, tangentEnd, Vector3.Distance(vector3, vector) / size);
			v.position = vector3 + a3 * d * 0.5f;
			v.uv0 = uvs[0];
			v.normal = Vector3.back;
			v.color = this.color;
			UIVertex v2 = default(UIVertex);
			a3 = Vector3.Lerp(tangentStart, tangentEnd, Vector3.Distance(a2, vector) / size);
			if (flag)
			{
				v2.position = corners[3];
			}
			else
			{
				v2.position = a2 + a3 * d2 * 0.5f;
			}
			v2.uv0 = uvs[0] + Vector2.Scale(uvs[1] - uvs[0], b2);
			v2.normal = Vector3.back;
			v2.color = this.color;
			UIVertex v3 = default(UIVertex);
			a3 = Vector3.Lerp(tangentStart, tangentEnd, Vector3.Distance(a2, vector) / size);
			if (flag)
			{
				v3.position = corners[2];
			}
			else
			{
				v3.position = a2 - a3 * d2 * 0.5f;
			}
			v3.uv0 = uvs[3] + Vector2.Scale(uvs[2] - uvs[3], b2);
			v3.normal = Vector3.back;
			v3.color = this.color;
			UIVertex v4 = default(UIVertex);
			a3 = Vector3.Lerp(tangentStart, tangentEnd, Vector3.Distance(vector3, vector) / size);
			v4.position = vector3 - a3 * d * 0.5f;
			v4.normal = Vector3.back;
			v4.color = this.color;
			v4.uv0 = uvs[3];
			int currentVertCount = toFill.currentVertCount;
			toFill.AddVert(v);
			toFill.AddVert(v2);
			toFill.AddVert(v3);
			toFill.AddVert(v4);
			toFill.AddTriangle(currentVertCount + 3, currentVertCount + 1, currentVertCount);
			toFill.AddTriangle(currentVertCount + 2, currentVertCount + 1, currentVertCount + 3);
		}
	}

	private Rect getUVs(Sprite sprite)
	{
		Rect rect = sprite.rect;
		rect.x /= (float)sprite.texture.width;
		rect.width /= (float)sprite.texture.width;
		rect.y /= (float)sprite.texture.height;
		rect.height /= (float)sprite.texture.height;
		return rect;
	}

	public void FillMeshDistortCorners(VertexHelper toFill)
	{
		toFill.Clear();
		Texture2D texture = base.sprite.texture;
		Rect rect = base.sprite.rect;
		Vector4 border = base.sprite.border;
		float x = border.x;
		float z = border.z;
		float y = border.y;
		float w = border.w;
		Rect uvs = this.getUVs(base.sprite);
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
			this.innerVertex = new DistortedImage.VertexProperties[4];
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
			DistortedImage.VertexProperties vertexProperties = this.innerVertex[i];
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
			DistortedImage.VertexProperties vertexProperties2 = this.innerVertex[j];
			DistortedImage.VertexProperties vertexProperties3 = this.innerVertex[(j + 1) % this.innerVertex.Length];
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
			DistortedImage.VertexProperties vertexProperties4 = this.innerVertex[k];
			this.quad[k].position = vertexProperties4.GetVertexBorder(0f, 0f);
			this.quad[k].color = this.innerColor;
			this.quad[k].uv0 = vertexProperties4.borderUv00;
		}
		toFill.AddUIVertexQuad(this.quad);
	}

	public void CreateMesh2(VertexHelper toFill)
	{
		toFill.Clear();
		Vector4 border = base.sprite.border;
		Vector2 size = base.sprite.rect.size;
		float x = border.x / size.x;
		float x2 = 1f - border.z / size.x;
		float y = 1f - border.w / size.y;
		float y2 = border.y / size.y;
		List<Vector2> uvs = new List<Vector2>
		{
			new Vector2(x, y2),
			new Vector2(0f, y2),
			new Vector2(x, 0f),
			new Vector2(0f, 0f)
		};
		this.CreateMeshCorner2(toFill, this.bl, (this.bl - this.tl).normalized, (this.bl - this.br).normalized, uvs);
		List<Vector2> uvs2 = new List<Vector2>
		{
			new Vector2(x2, y2),
			new Vector2(x2, 0f),
			new Vector2(1f, y2),
			new Vector2(1f, 0f)
		};
		this.CreateMeshCorner2(toFill, this.br, (this.br - this.bl).normalized, (this.br - this.tr).normalized, uvs2);
		List<Vector2> uvs3 = new List<Vector2>
		{
			new Vector2(x2, y),
			new Vector2(1f, y),
			new Vector2(x2, 1f),
			new Vector2(1f, 1f)
		};
		this.CreateMeshCorner2(toFill, this.tr, (this.tr - this.br).normalized, (this.tr - this.tl).normalized, uvs3);
		List<Vector2> uvs4 = new List<Vector2>
		{
			new Vector2(x, y),
			new Vector2(x, 1f),
			new Vector2(0f, y),
			new Vector2(0f, 1f)
		};
		this.CreateMeshCorner2(toFill, this.tl, (this.tl - this.tr).normalized, (this.tl - this.bl).normalized, uvs4);
		UIVertex uivertex = default(UIVertex);
		toFill.PopulateUIVertex(ref uivertex, 4);
		Vector3 vector = uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 5);
		vector += uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 0);
		Vector3 vector2 = uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 2);
		vector2 += uivertex.position * 0.5f;
		List<Vector2> uvs5 = new List<Vector2>
		{
			new Vector2(x, 0f),
			new Vector2(x2, 0f),
			new Vector2(x2, y2),
			new Vector2(x, y2)
		};
		this.CreateMeshSide2(toFill, vector, vector2, uvs5, size.x, false);
		uivertex = default(UIVertex);
		toFill.PopulateUIVertex(ref uivertex, 4);
		vector = uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 6);
		vector += uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 8);
		vector2 = uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 9);
		vector2 += uivertex.position * 0.5f;
		uvs5 = new List<Vector2>
		{
			new Vector2(1f, y2),
			new Vector2(1f, y),
			new Vector2(x2, y),
			new Vector2(x2, y2)
		};
		this.CreateMeshSide2(toFill, vector, vector2, uvs5, size.y, true);
		uivertex = default(UIVertex);
		toFill.PopulateUIVertex(ref uivertex, 13);
		vector2 = uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 12);
		vector2 += uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 10);
		vector = uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 8);
		vector += uivertex.position * 0.5f;
		uvs5 = new List<Vector2>
		{
			new Vector2(x, y),
			new Vector2(x2, y),
			new Vector2(x2, 1f),
			new Vector2(x, 1f)
		};
		this.CreateMeshSide2(toFill, vector, vector2, uvs5, size.x, false);
		uivertex = default(UIVertex);
		toFill.PopulateUIVertex(ref uivertex, 1);
		vector = uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 0);
		vector += uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 12);
		vector2 = uivertex.position * 0.5f;
		toFill.PopulateUIVertex(ref uivertex, 14);
		vector2 += uivertex.position * 0.5f;
		uvs5 = new List<Vector2>
		{
			new Vector2(x, y2),
			new Vector2(x, y),
			new Vector2(0f, y),
			new Vector2(0f, y2)
		};
		this.CreateMeshSide2(toFill, vector, vector2, uvs5, size.y, true);
		int currentVertCount = toFill.currentVertCount;
		toFill.PopulateUIVertex(ref uivertex, 4);
		uivertex.color = this.innerColor;
		toFill.AddVert(uivertex);
		toFill.PopulateUIVertex(ref uivertex, 0);
		uivertex.color = this.innerColor;
		toFill.AddVert(uivertex);
		toFill.PopulateUIVertex(ref uivertex, 12);
		uivertex.color = this.innerColor;
		toFill.AddVert(uivertex);
		toFill.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
		currentVertCount = toFill.currentVertCount;
		toFill.PopulateUIVertex(ref uivertex, 4);
		uivertex.color = this.innerColor;
		toFill.AddVert(uivertex);
		toFill.PopulateUIVertex(ref uivertex, 12);
		uivertex.color = this.innerColor;
		toFill.AddVert(uivertex);
		toFill.PopulateUIVertex(ref uivertex, 8);
		uivertex.color = this.innerColor;
		toFill.AddVert(uivertex);
		toFill.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
	}

	public void CreateMeshCorner2(VertexHelper toFill, Vector3 referencePoint, Vector3 direction1, Vector3 direction2, List<Vector2> uvs)
	{
		float num = Vector3.Angle(direction1, direction2);
		float d = this.outlineWidth * 0.5f / Mathf.Sin(0.0174532924f * num);
		float num2 = (360f - 2f * num) * 0.5f;
		float d2 = this.outlineWidth * 0.5f / Mathf.Sin(0.0174532924f * num2);
		Vector3 position = referencePoint + direction1 * d + direction2 * d2;
		Vector3 vector = referencePoint - direction1 * d - direction2 * d2;
		Vector3 a = Vector3.Cross(direction1, Vector3.forward);
		Vector3 a2 = Vector3.Cross(direction2, Vector3.forward);
		Vector3 position2 = vector + a * this.outlineWidth;
		Vector3 position3 = vector - a2 * this.outlineWidth;
		int currentVertCount = toFill.currentVertCount;
		toFill.AddVert(new UIVertex
		{
			position = vector,
			color = this.color,
			normal = Vector3.back,
			uv0 = uvs[0]
		});
		toFill.AddVert(new UIVertex
		{
			position = position2,
			color = this.color,
			normal = Vector3.back,
			uv0 = uvs[1]
		});
		toFill.AddVert(new UIVertex
		{
			position = position3,
			color = this.color,
			normal = Vector3.back,
			uv0 = uvs[2]
		});
		toFill.AddVert(new UIVertex
		{
			position = position,
			color = this.color,
			normal = Vector3.back,
			uv0 = uvs[3]
		});
		toFill.AddTriangle(currentVertCount + 2, currentVertCount + 1, currentVertCount);
		toFill.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount + 1);
	}

	private void CreateMeshSide2(VertexHelper toFill, Vector3 start, Vector3 end, List<Vector2> uvs, float size, bool isVertical)
	{
		Vector3 normalized = (end - start).normalized;
		Vector3 a = Vector3.Cross(normalized, Vector3.forward);
		int num = Mathf.CeilToInt((end - start).magnitude / size);
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = start + normalized * size * (float)i;
			Vector3 a2 = start + normalized * size * (float)(i + 1);
			float num2 = 1f;
			if (i == num - 1)
			{
				num2 = Vector3.Distance(end, vector) / size;
				a2 = end;
			}
			Vector2 b = new Vector2(num2, 1f);
			if (isVertical)
			{
				b.y = num2;
				b.x = 1f;
			}
			UIVertex v = default(UIVertex);
			v.position = vector + a * this.outlineWidth * 0.5f;
			v.uv0 = uvs[0];
			v.normal = Vector3.back;
			v.color = this.color;
			UIVertex v2 = default(UIVertex);
			v2.position = a2 + a * this.outlineWidth * 0.5f;
			v2.uv0 = uvs[0] + Vector2.Scale(uvs[1] - uvs[0], b);
			v2.normal = Vector3.back;
			v2.color = this.color;
			UIVertex v3 = default(UIVertex);
			v3.position = a2 - a * this.outlineWidth * 0.5f;
			v3.uv0 = uvs[3] + Vector2.Scale(uvs[2] - uvs[3], b);
			v3.normal = Vector3.back;
			v3.color = this.color;
			UIVertex v4 = default(UIVertex);
			v4.position = vector - a * this.outlineWidth * 0.5f;
			v4.normal = Vector3.back;
			v4.color = this.color;
			v4.uv0 = uvs[3];
			int currentVertCount = toFill.currentVertCount;
			toFill.AddVert(v);
			toFill.AddVert(v2);
			toFill.AddVert(v3);
			toFill.AddVert(v4);
			toFill.AddTriangle(currentVertCount + 3, currentVertCount + 1, currentVertCount);
			toFill.AddTriangle(currentVertCount + 2, currentVertCount + 1, currentVertCount + 3);
		}
	}

	[SerializeField]
	private DistortedImage.Implementation implementation;

	[SerializeField]
	public Vector3 tl;

	[SerializeField]
	public Vector3 tr;

	[SerializeField]
	public Vector3 bl;

	[SerializeField]
	public Vector3 br;

	[SerializeField]
	public float outlineWidth;

	[SerializeField]
	private Color innerColor;

	[SerializeField]
	private bool fixControlPointsToRectCorners;

	private Vector3[] localCorners = new Vector3[4];

	private UIVertex[] quad;

	private DistortedImage.VertexProperties[] innerVertex;

	public enum Implementation
	{
		FillMesh,
		CreateMesh2
	}

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
