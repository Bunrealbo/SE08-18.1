using System;
using UnityEngine;
using UnityEngine.UI;

public class BubbleImage : Image
{
	private Rect getUVs(Sprite sprite)
	{
		Rect rect = sprite.rect;
		rect.x /= (float)sprite.texture.width;
		rect.width /= (float)sprite.texture.width;
		rect.y /= (float)sprite.texture.height;
		rect.height /= (float)sprite.texture.height;
		return rect;
	}

	public BubbleImage.VertexPropertiesList GetVertexPropertiesList()
	{
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
		Rect rect2 = base.rectTransform.rect;
		if (this.vertexPropertiesList.vertexProperties == null)
		{
			this.vertexPropertiesList.vertexProperties = new BubbleImage.VertexProperties[4];
		}
		BubbleImage.VertexProperties[] vertexProperties = this.vertexPropertiesList.vertexProperties;
		vertexProperties[0].vertexOuter = new Vector2(rect2.xMin, rect2.yMin);
		vertexProperties[0].vertexInner = new Vector2(rect2.xMin + x, rect2.yMin + y);
		vertexProperties[0].uvOuter = new Vector2(uvs.xMin, uvs.yMin);
		vertexProperties[0].uvInner = new Vector2(uvs.xMin + num, uvs.yMin + num4);
		vertexProperties[0].settings = this.bottomLeft;
		vertexProperties[0].prevT = Mathf.InverseLerp(rect2.yMin, rect2.yMax, vertexProperties[0].vertexInner.y);
		vertexProperties[0].nextT = Mathf.InverseLerp(rect2.xMin, rect2.xMax, vertexProperties[0].vertexInner.x);
		vertexProperties[1].vertexOuter = new Vector2(rect2.xMax, rect2.yMin);
		vertexProperties[1].vertexInner = new Vector2(rect2.xMax - z, rect2.yMin + y);
		vertexProperties[1].uvOuter = new Vector2(uvs.xMax, uvs.yMin);
		vertexProperties[1].uvInner = new Vector2(uvs.xMax - num2, uvs.yMin + num4);
		vertexProperties[1].settings = this.bottomRight;
		vertexProperties[1].prevT = Mathf.InverseLerp(rect2.xMax, rect2.xMin, vertexProperties[1].vertexInner.x);
		vertexProperties[1].nextT = Mathf.InverseLerp(rect2.yMin, rect2.yMax, vertexProperties[1].vertexInner.y);
		vertexProperties[2].vertexOuter = new Vector2(rect2.xMax, rect2.yMax);
		vertexProperties[2].vertexInner = new Vector2(rect2.xMax - z, rect2.yMax - w);
		vertexProperties[2].uvOuter = new Vector2(uvs.xMax, uvs.yMax);
		vertexProperties[2].uvInner = new Vector2(uvs.xMax - num2, uvs.yMax - num3);
		vertexProperties[2].settings = this.topRight;
		vertexProperties[2].prevT = Mathf.InverseLerp(rect2.yMax, rect2.yMin, vertexProperties[2].vertexInner.y);
		vertexProperties[2].nextT = Mathf.InverseLerp(rect2.xMax, rect2.xMin, vertexProperties[2].vertexInner.x);
		vertexProperties[3].vertexOuter = new Vector2(rect2.xMin, rect2.yMax);
		vertexProperties[3].vertexInner = new Vector2(rect2.xMin + x, rect2.yMax - w);
		vertexProperties[3].uvOuter = new Vector2(uvs.xMin, uvs.yMax);
		vertexProperties[3].uvInner = new Vector2(uvs.xMin + num, uvs.yMax - num3);
		vertexProperties[3].settings = this.topLeft;
		vertexProperties[3].prevT = Mathf.InverseLerp(rect2.xMin, rect2.xMax, vertexProperties[3].vertexInner.x);
		vertexProperties[3].nextT = Mathf.InverseLerp(rect2.yMax, rect2.yMin, vertexProperties[3].vertexInner.y);
		for (int i = 0; i < vertexProperties.Length; i++)
		{
			vertexProperties[i].index = i;
			vertexProperties[i].list = this.vertexPropertiesList;
		}
		return this.vertexPropertiesList;
	}

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		toFill.Clear();
		if (base.sprite == null)
		{
			return;
		}
		BubbleImage.VertexProperties[] vertexProperties = this.GetVertexPropertiesList().vertexProperties;
		if (this.quad == null)
		{
			this.quad = new UIVertex[4];
		}
		Rect rect = base.rectTransform.rect;
		for (int i = 0; i < vertexProperties.Length; i++)
		{
			BubbleImage.VertexProperties vertexProperties2 = vertexProperties[i];
			BubbleImage.VertexProperties vertexProperties3 = vertexProperties[(i + 1) % vertexProperties.Length];
			BubbleImage.VertexProperties vertexProperties4 = vertexProperties[(i + vertexProperties.Length - 1) % vertexProperties.Length];
			Vector2 vertexOuter = vertexProperties4.vertexOuter;
			Vector2 vertexOuter2 = vertexProperties2.vertexOuter;
			Vector2 p = BubbleImage.VertexSettings.ToLocalControlPoint(vertexOuter, vertexOuter2, vertexProperties4.settings.controlPoint2);
			Vector2 p2 = BubbleImage.VertexSettings.ToLocalControlPoint(vertexOuter2, vertexOuter, vertexProperties2.settings.controlPoint1);
			Vector2 vertexOuter3 = vertexProperties2.vertexOuter;
			Vector2 vertexOuter4 = vertexProperties3.vertexOuter;
			Vector2 p3 = BubbleImage.VertexSettings.ToLocalControlPoint(vertexOuter3, vertexOuter4, vertexProperties2.settings.controlPoint2);
			Vector2 p4 = BubbleImage.VertexSettings.ToLocalControlPoint(vertexOuter4, vertexOuter3, vertexProperties3.settings.controlPoint1);
			if (i % 2 == 0)
			{
				int currentVertCount = toFill.currentVertCount;
				toFill.AddVert(new UIVertex
				{
					position = vertexProperties2.vertexInner,
					uv0 = vertexProperties2.uvInner,
					color = this.color
				});
				for (int j = 0; j <= this.sideSubdivisions; j++)
				{
					float num = (float)j / (float)this.sideSubdivisions;
					float t = Mathf.Lerp(0f, vertexProperties2.nextT, num);
					UIVertex v = default(UIVertex);
					Vector2 a = GGMath.CubicBezier(vertexOuter3, p3, p4, vertexOuter4, t) - vertexProperties2.vertexInner;
					float d = Mathf.Lerp(vertexProperties2.settings.maxScale, 1f, num);
					if (vertexProperties2.settings.scaleCurve != null)
					{
						d = Mathf.Lerp(vertexProperties2.settings.maxScale, 1f, vertexProperties2.settings.scaleCurve.Evaluate(num));
					}
					v.position = d * a + vertexProperties2.vertexInner;
					v.uv0 = new Vector2(Mathf.Lerp(vertexProperties2.uvOuter.x, vertexProperties2.uvInner.x, num), vertexProperties2.uvOuter.y);
					v.color = this.color;
					toFill.AddVert(v);
					if (j > 0)
					{
						toFill.AddTriangle(currentVertCount, toFill.currentVertCount - 2, toFill.currentVertCount - 1);
					}
				}
				for (int k = 0; k <= this.sideSubdivisions; k++)
				{
					float num2 = (float)k / (float)this.sideSubdivisions;
					float t2 = Mathf.Lerp(1f, 1f - vertexProperties2.prevT, num2);
					UIVertex v = default(UIVertex);
					Vector2 a2 = GGMath.CubicBezier(vertexOuter, p, p2, vertexOuter2, t2) - vertexProperties2.vertexInner;
					float d2 = Mathf.Lerp(vertexProperties2.settings.maxScale, 1f, num2);
					if (vertexProperties2.settings.scaleCurve != null)
					{
						d2 = Mathf.Lerp(vertexProperties2.settings.maxScale, 1f, vertexProperties2.settings.scaleCurve.Evaluate(num2));
					}
					v.position = d2 * a2 + vertexProperties2.vertexInner;
					v.uv0 = new Vector2(vertexProperties2.uvOuter.x, Mathf.Lerp(vertexProperties2.uvOuter.y, vertexProperties2.uvInner.y, num2));
					v.color = this.color;
					toFill.AddVert(v);
					if (k > 0)
					{
						toFill.AddTriangle(currentVertCount, toFill.currentVertCount - 2, toFill.currentVertCount - 1);
					}
				}
			}
			else
			{
				int currentVertCount2 = toFill.currentVertCount;
				toFill.AddVert(new UIVertex
				{
					position = vertexProperties2.vertexInner,
					uv0 = vertexProperties2.uvInner,
					color = this.color
				});
				for (int l = 0; l <= this.sideSubdivisions; l++)
				{
					float num3 = (float)l / (float)this.sideSubdivisions;
					float t3 = Mathf.Lerp(0f, vertexProperties2.nextT, num3);
					UIVertex v2 = default(UIVertex);
					Vector2 a3 = GGMath.CubicBezier(vertexOuter3, p3, p4, vertexOuter4, t3) - vertexProperties2.vertexInner;
					float d3 = Mathf.Lerp(vertexProperties2.settings.maxScale, 1f, num3);
					if (vertexProperties2.settings.scaleCurve != null)
					{
						d3 = Mathf.Lerp(vertexProperties2.settings.maxScale, 1f, vertexProperties2.settings.scaleCurve.Evaluate(num3));
					}
					v2.position = d3 * a3 + vertexProperties2.vertexInner;
					v2.uv0 = new Vector2(vertexProperties2.uvOuter.x, Mathf.Lerp(vertexProperties2.uvOuter.y, vertexProperties2.uvInner.y, num3));
					v2.color = this.color;
					toFill.AddVert(v2);
					if (l > 0)
					{
						toFill.AddTriangle(currentVertCount2, toFill.currentVertCount - 2, toFill.currentVertCount - 1);
					}
				}
				for (int m = 0; m <= this.sideSubdivisions; m++)
				{
					float num4 = (float)m / (float)this.sideSubdivisions;
					float t4 = Mathf.Lerp(1f, 1f - vertexProperties2.prevT, num4);
					UIVertex v2 = default(UIVertex);
					Vector2 a4 = GGMath.CubicBezier(vertexOuter, p, p2, vertexOuter2, t4) - vertexProperties2.vertexInner;
					float d4 = Mathf.Lerp(vertexProperties2.settings.maxScale, 1f, num4);
					if (vertexProperties2.settings.scaleCurve != null)
					{
						d4 = Mathf.Lerp(vertexProperties2.settings.maxScale, 1f, vertexProperties2.settings.scaleCurve.Evaluate(num4));
					}
					v2.position = d4 * a4 + vertexProperties2.vertexInner;
					v2.uv0 = new Vector2(Mathf.Lerp(vertexProperties2.uvOuter.x, vertexProperties2.uvInner.x, num4), vertexProperties2.uvOuter.y);
					v2.color = this.color;
					toFill.AddVert(v2);
					if (m > 0)
					{
						toFill.AddTriangle(currentVertCount2, toFill.currentVertCount - 2, toFill.currentVertCount - 1);
					}
				}
			}
		}
		for (int n = 0; n < vertexProperties.Length; n++)
		{
			BubbleImage.VertexProperties vertexProperties5 = vertexProperties[n];
			BubbleImage.VertexProperties vertexProperties6 = vertexProperties[(n + 1) % vertexProperties.Length];
			Vector2 vertexOuter5 = vertexProperties5.vertexOuter;
			Vector2 vertexOuter6 = vertexProperties6.vertexOuter;
			Vector2 p5 = BubbleImage.VertexSettings.ToLocalControlPoint(vertexOuter5, vertexOuter6, vertexProperties5.settings.controlPoint2);
			Vector2 p6 = BubbleImage.VertexSettings.ToLocalControlPoint(vertexOuter6, vertexOuter5, vertexProperties6.settings.controlPoint1);
			if (n % 2 == 0)
			{
				for (int num5 = 0; num5 < this.iterations; num5++)
				{
					float t5 = (float)num5 / (float)this.iterations;
					float t6 = (float)(num5 + 1) / (float)this.iterations;
					float t7 = Mathf.Lerp(vertexProperties5.nextT, 1f - vertexProperties6.prevT, t5);
					float t8 = Mathf.Lerp(vertexProperties5.nextT, 1f - vertexProperties6.prevT, t6);
					this.quad[0].position = new Vector2(Mathf.Lerp(vertexProperties5.vertexInner.x, vertexProperties6.vertexInner.x, t5), vertexProperties5.vertexInner.y);
					this.quad[0].uv0 = new Vector2(Mathf.Lerp(vertexProperties5.uvInner.x, vertexProperties6.uvInner.x, t5), vertexProperties5.uvInner.y);
					this.quad[0].color = this.color;
					GGMath.CubicBezier(vertexOuter5, p5, p6, vertexOuter6, t7);
					this.quad[1].position = new Vector2(this.quad[0].position.x, GGMath.CubicBezier(vertexOuter5, p5, p6, vertexOuter6, t7).y);
					this.quad[1].uv0 = new Vector2(this.quad[0].uv0.x, vertexProperties5.uvOuter.y);
					this.quad[1].color = this.color;
					GGMath.CubicBezier(vertexOuter5, p5, p6, vertexOuter6, t8);
					this.quad[2].position = new Vector2(Mathf.Lerp(vertexProperties5.vertexInner.x, vertexProperties6.vertexInner.x, t6), GGMath.CubicBezier(vertexOuter5, p5, p6, vertexOuter6, t8).y);
					this.quad[2].uv0 = new Vector2(Mathf.Lerp(vertexProperties5.uvInner.x, vertexProperties6.uvInner.x, t6), vertexProperties6.uvOuter.y);
					this.quad[2].color = this.color;
					this.quad[3].position = new Vector2(this.quad[2].position.x, vertexProperties6.vertexInner.y);
					this.quad[3].uv0 = new Vector2(this.quad[2].uv0.x, vertexProperties6.uvInner.y);
					this.quad[3].color = this.color;
					this.quad[1].position = GGMath.CubicBezier(vertexOuter5, p5, p6, vertexOuter6, t7);
					this.quad[2].position = GGMath.CubicBezier(vertexOuter5, p5, p6, vertexOuter6, t8);
					toFill.AddUIVertexQuad(this.quad);
				}
			}
			else
			{
				for (int num6 = 0; num6 < this.iterations; num6++)
				{
					float t9 = (float)num6 / (float)this.iterations;
					float t10 = (float)(num6 + 1) / (float)this.iterations;
					float t11 = Mathf.Lerp(vertexProperties5.nextT, 1f - vertexProperties6.prevT, t9);
					float t12 = Mathf.Lerp(vertexProperties5.nextT, 1f - vertexProperties6.prevT, t10);
					this.quad[0].position = new Vector2(vertexProperties5.vertexInner.x, Mathf.Lerp(vertexProperties5.vertexInner.y, vertexProperties6.vertexInner.y, t9));
					this.quad[0].uv0 = new Vector2(vertexProperties5.uvInner.x, Mathf.Lerp(vertexProperties5.uvInner.y, vertexProperties6.uvInner.y, t9));
					this.quad[0].color = this.color;
					this.quad[1].position = new Vector2(GGMath.CubicBezier(vertexOuter5, p5, p6, vertexOuter6, t11).x, this.quad[0].position.y);
					this.quad[1].uv0 = new Vector2(vertexProperties5.uvOuter.x, this.quad[0].uv0.y);
					this.quad[1].color = this.color;
					this.quad[2].position = new Vector2(GGMath.CubicBezier(vertexOuter5, p5, p6, vertexOuter6, t12).x, Mathf.Lerp(vertexProperties5.vertexInner.y, vertexProperties6.vertexInner.y, t10));
					this.quad[2].uv0 = new Vector2(vertexProperties6.uvOuter.x, Mathf.Lerp(vertexProperties5.uvInner.y, vertexProperties6.uvInner.y, t10));
					this.quad[2].color = this.color;
					this.quad[3].position = new Vector2(vertexProperties6.vertexInner.x, this.quad[2].position.y);
					this.quad[3].uv0 = new Vector2(vertexProperties6.uvInner.x, this.quad[2].uv0.y);
					this.quad[3].color = this.color;
					this.quad[1].position = GGMath.CubicBezier(vertexOuter5, p5, p6, vertexOuter6, t11);
					this.quad[2].position = GGMath.CubicBezier(vertexOuter5, p5, p6, vertexOuter6, t12);
					toFill.AddUIVertexQuad(this.quad);
				}
			}
		}
		for (int num7 = 0; num7 < vertexProperties.Length; num7++)
		{
			BubbleImage.VertexProperties vertexProperties7 = vertexProperties[num7];
			this.quad[num7].position = vertexProperties7.vertexInner;
			this.quad[num7].uv0 = vertexProperties7.uvInner;
			this.quad[num7].color = this.color;
		}
		toFill.AddUIVertexQuad(this.quad);
	}

	[SerializeField]
	private BubbleImage.VertexSettings bottomLeft = new BubbleImage.VertexSettings();

	[SerializeField]
	private BubbleImage.VertexSettings bottomRight = new BubbleImage.VertexSettings();

	[SerializeField]
	private BubbleImage.VertexSettings topRight = new BubbleImage.VertexSettings();

	[SerializeField]
	private BubbleImage.VertexSettings topLeft = new BubbleImage.VertexSettings();

	[SerializeField]
	private int iterations = 20;

	[SerializeField]
	private int sideSubdivisions = 20;

	private UIVertex[] quad;

	private BubbleImage.VertexPropertiesList vertexPropertiesList = new BubbleImage.VertexPropertiesList();

	[Serializable]
	public class VertexSettings
	{
		public static Vector2 ToLocalControlPoint(Vector2 fromVertex, Vector2 toVertex, Vector2 controlPoint)
		{
			Vector2 vector = toVertex - fromVertex;
			Vector3 vector2 = Vector3.Cross(vector, Vector3.forward);
			return fromVertex + vector * controlPoint.y + new Vector2(vector2.x, vector2.y) * controlPoint.x;
		}

		public Vector2 controlPoint1 = new Vector2(0f, 0.25f);

		public Vector2 controlPoint2 = new Vector2(0f, 0.25f);

		public float maxScale = 1f;

		public AnimationCurve scaleCurve;
	}

	public struct VertexProperties
	{
		public Vector2 vertexInner;

		public Vector2 vertexOuter;

		public Vector2 uvOuter;

		public Vector2 uvInner;

		public float prevT;

		public float nextT;

		public BubbleImage.VertexSettings settings;

		public int index;

		public BubbleImage.VertexPropertiesList list;
	}

	public class VertexPropertiesList
	{
		public BubbleImage.VertexProperties[] vertexProperties;
	}
}
