using System;
using System.Collections.Generic;
using GGCloth;
using GGMatch3;
using UnityEngine;

public class SquareClothRenderer : MonoBehaviour
{
	public void SetCloth(SquareCloth cloth)
	{
		this.cloth = cloth;
		this.Init();
	}

	public SquareClothRenderer.ClothTexture GetClothTexture(ItemColor itemColor)
	{
		this.itemColor = itemColor;
		for (int i = 0; i < this.clothTextures.Count; i++)
		{
			SquareClothRenderer.ClothTexture clothTexture = this.clothTextures[i];
			if (clothTexture.itemColor == itemColor)
			{
				return clothTexture;
			}
		}
		return null;
	}

	public SquareClothRenderer.ShadowSprite GetShadowClothTexture(ItemColor itemColor)
	{
		this.itemColor = itemColor;
		for (int i = 0; i < this.shadowTextures.Count; i++)
		{
			SquareClothRenderer.ShadowSprite shadowSprite = this.shadowTextures[i];
			if (shadowSprite.itemColor == itemColor)
			{
				return shadowSprite;
			}
		}
		return null;
	}

	public void UpdateMaterialSettings()
	{
		if (!this.isLocalScaleSaved)
		{
			this.isLocalScaleSaved = true;
			this.localScale = base.transform.localScale;
		}
		if (!Application.isEditor && !ConfigBase.instance.changeChipOnDevice)
		{
			return;
		}
		Match3Settings.ChipChange chipChange = Match3Settings.instance.GetChipChange(this.itemColor);
		if (chipChange == null)
		{
			return;
		}
		Match3Settings.ChipColorSettings colorSettings = Match3Settings.instance.GetColorSettings();
		for (int i = 0; i < this.boxSprites.Count; i++)
		{
			SpriteRenderer spriteRenderer = this.boxSprites[i];
			Material material = spriteRenderer.material;
			material.SetFloat("_ColorHSV_Hue_1", chipChange.boxHue);
			material.SetFloat("_ColorHSV_Saturation_1", chipChange.boxSaturation);
			material.SetFloat("_ColorHSV_Brightness_1", chipChange.boxBrightness);
			spriteRenderer.gameObject.SetActive(colorSettings.hasBoxes);
		}
		Material material2 = this.GetMaterial();
		if (material2 == null)
		{
			return;
		}
		if (!chipChange.change)
		{
			material2.SetFloat("_ColorHSV_Hue_1", 0f);
			material2.SetFloat("_ColorHSV_Saturation_1", 1f);
			material2.SetFloat("_ColorHSV_Brightness_1", 1f);
			return;
		}
		material2.SetFloat("_ColorHSV_Hue_1", chipChange.hue);
		material2.SetFloat("_ColorHSV_Saturation_1", chipChange.saturation);
		material2.SetFloat("_ColorHSV_Brightness_1", chipChange.brightness);
		if (chipChange.textureReplace != null)
		{
			material2.mainTexture = chipChange.textureReplace;
		}
		base.transform.localScale = this.localScale * chipChange.scale;
	}

	public void SetBrightness(float brightness)
	{
		Material material = this.GetMaterial();
		if (material == null)
		{
			return;
		}
		material.SetFloat("_ColorHSV_Brightness_1", brightness);
	}

	public Material GetMaterial()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return null;
		}
		return component.material;
	}

	public void SetColor(Color color)
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		component.material.color = color;
	}

	public void SetAlpha(float alpha)
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		Material material = component.material;
		Color color = material.color;
		color.a = alpha;
		material.color = color;
	}

	public void SetShadowTexture(ItemColor itemColor)
	{
		SquareClothRenderer.ShadowSprite shadowClothTexture = this.GetShadowClothTexture(itemColor);
		if (shadowClothTexture == null)
		{
			return;
		}
		if (this.shadowSprite == null)
		{
			return;
		}
		this.shadowSprite.sprite = shadowClothTexture.texture;
	}

	public void SetClothTexture(ItemColor itemColor)
	{
		this.SetShadowTexture(itemColor);
		SquareClothRenderer.ClothTexture clothTexture = this.GetClothTexture(itemColor);
		if (clothTexture == null)
		{
			return;
		}
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		component.material.mainTexture = clothTexture.texture;
	}

	public void SetSortingLayers(int sortingLayerId, int sortingLayerOrder)
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		component.sortingLayerID = sortingLayerId;
		component.sortingOrder = sortingLayerOrder;
	}

	public void ReinitSortingLayers()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component == null)
		{
			return;
		}
		component.sortingLayerID = this.sortingLayerID;
		component.sortingOrder = this.sortingLayerOrder;
	}

	private void Init()
	{
		if (this.meshFilter == null)
		{
			this.meshFilter = base.gameObject.GetComponent<MeshFilter>();
			this.ReinitSortingLayers();
		}
		if (this.meshFilter == null)
		{
			this.meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		if (this.mesh == null)
		{
			this.mesh = new Mesh();
			this.meshFilter.mesh = this.mesh;
		}
		int rowCount = this.cloth.rowCount;
		int columnCount = this.cloth.columnCount;
		int capacity = rowCount * columnCount * 2;
		int capacity2 = (rowCount + 1) * (columnCount + 1);
		this.vertexBuffer = new List<Vector3>(capacity2);
		this.trisBuffer = new List<int>(capacity);
		this.uvBuffer = new List<Vector2>(capacity2);
	}

	private Vector3 Min(Vector3 a, Vector3 b)
	{
		return new Vector3(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z));
	}

	private Vector3 Max(Vector3 a, Vector3 b)
	{
		return new Vector3(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
	}

	public void DoUpdateMesh()
	{
		if (this.cloth == null || this.mesh == null)
		{
			return;
		}
		this.vertexBuffer.Clear();
		this.trisBuffer.Clear();
		this.uvBuffer.Clear();
		int rowCount = this.cloth.rowCount;
		int columnCount = this.cloth.columnCount;
		PointWorld pointWorld = this.cloth.pointWorld;
		PointMass point = pointWorld.GetPoint(0);
		Vector3 vector = point.currentPosition;
		Vector3 vector2 = point.currentPosition;
		for (int i = 0; i <= rowCount; i++)
		{
			for (int j = 0; j <= columnCount; j++)
			{
				Vector3 vector3 = pointWorld.GetPoint(this.cloth.GetPointIndex(j, i)).currentPosition;
				if (this.cloth.isWorldPosition)
				{
					vector3 -= base.transform.position;
				}
				else if (this.cloth.localPositionTransform != null)
				{
					vector3 -= this.cloth.localPositionTransform.localPosition;
				}
				vector = this.Min(vector, vector3);
				vector2 = this.Max(vector2, vector3);
				this.vertexBuffer.Add(vector3);
				Vector2 item = new Vector2(Mathf.InverseLerp(0f, (float)columnCount, (float)j), Mathf.InverseLerp(0f, (float)rowCount, (float)i));
				this.uvBuffer.Add(item);
				if (j < columnCount && i < rowCount)
				{
					if (!this.invertNormals)
					{
						this.trisBuffer.Add(this.cloth.GetPointIndex(j, i));
						this.trisBuffer.Add(this.cloth.GetPointIndex(j, i + 1));
						this.trisBuffer.Add(this.cloth.GetPointIndex(j + 1, i));
						this.trisBuffer.Add(this.cloth.GetPointIndex(j + 1, i));
						this.trisBuffer.Add(this.cloth.GetPointIndex(j, i + 1));
						this.trisBuffer.Add(this.cloth.GetPointIndex(j + 1, i + 1));
					}
					else
					{
						this.trisBuffer.Add(this.cloth.GetPointIndex(j, i));
						this.trisBuffer.Add(this.cloth.GetPointIndex(j + 1, i));
						this.trisBuffer.Add(this.cloth.GetPointIndex(j, i + 1));
						this.trisBuffer.Add(this.cloth.GetPointIndex(j + 1, i));
						this.trisBuffer.Add(this.cloth.GetPointIndex(j + 1, i + 1));
						this.trisBuffer.Add(this.cloth.GetPointIndex(j, i + 1));
					}
				}
			}
		}
		this.mesh.bounds.SetMinMax(vector, vector2);
		this.mesh.Clear();
		this.mesh.SetVertices(this.vertexBuffer);
		this.mesh.SetUVs(0, this.uvBuffer);
		this.mesh.SetTriangles(this.trisBuffer, 0, false);
	}

	[SerializeField]
	private List<SpriteRenderer> boxSprites = new List<SpriteRenderer>();

	[SerializeField]
	private List<SquareClothRenderer.ClothTexture> clothTextures = new List<SquareClothRenderer.ClothTexture>();

	[SerializeField]
	private List<SquareClothRenderer.ShadowSprite> shadowTextures = new List<SquareClothRenderer.ShadowSprite>();

	[SerializeField]
	private SpriteRenderer shadowSprite;

	[SerializeField]
	public int sortingLayerID;

	[SerializeField]
	public int sortingLayerOrder;

	private MeshFilter meshFilter;

	private Mesh mesh;

	[SerializeField]
	private bool invertNormals;

	private List<Vector3> vertexBuffer;

	private List<int> trisBuffer;

	private List<Vector2> uvBuffer;

	[NonSerialized]
	private SquareCloth cloth;

	[NonSerialized]
	public ItemColor itemColor;

	private bool isLocalScaleSaved;

	private Vector3 localScale;

	[Serializable]
	public class ClothTexture
	{
		public ItemColor itemColor;

		public Texture2D texture;
	}

	[Serializable]
	public class ShadowSprite
	{
		public ItemColor itemColor;

		public Sprite texture;
	}
}
