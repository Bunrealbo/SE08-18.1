using System;
using System.Collections.Generic;
using JSONData;
using UnityEngine;

[Serializable]
public class GraphicsSceneConfig
{
	public override string ToString()
	{
		return JsonUtility.ToJson(this);
	}

	public int width;

	public int height;

	public List<GraphicsSceneConfig.VisualObject> objects = new List<GraphicsSceneConfig.VisualObject>();

	public List<GraphicsSceneConfig.ValidName> validVisualObjectNames = new List<GraphicsSceneConfig.ValidName>();

	[Serializable]
	public class VisualSprite
	{
		public Vector3 spritePosition
		{
			get
			{
				if (this.sprite == null)
				{
					return Vector3.right * (float)this.left + Vector3.down * (float)this.top;
				}
				Vector2 pivot = this.sprite.pivot;
				pivot.x /= this.sprite.rect.width;
				pivot.y /= this.sprite.rect.height;
				return Vector3.right * Mathf.Lerp((float)this.left, (float)this.right, pivot.x) + Vector3.down * Mathf.Lerp((float)this.bottom, (float)this.top, pivot.y);
			}
		}

		public int width
		{
			get
			{
				return this.right - this.left;
			}
		}

		public int height
		{
			get
			{
				return this.top - this.bottom;
			}
		}

		public string spriteName;

		public string spritePath;

		[NonSerialized]
		public Sprite sprite;

		public bool isShadow;

		public int totalWidth;

		public int totalHeight;

		public int left;

		public int right;

		public int top;

		public int bottom;

		public Vector3 pivotPosition;

		public int depth;

		public int initialDepth;
	}

	[Serializable]
	public class Variation
	{
		public string name;

		public List<GraphicsSceneConfig.VisualSprite> sprites = new List<GraphicsSceneConfig.VisualSprite>();

		public string thumbnailName;

		public Sprite thumbnailSprite;

		public List<GraphicsSceneConfig.Triangle> hitboxTriangles = new List<GraphicsSceneConfig.Triangle>();
	}

	[Serializable]
	public class DrawnShape
	{
		public int depth;

		public ShapeGraphShape shape;
	}

	[Serializable]
	public class Triangle
	{
		public float GetZ(Vector2 pos)
		{
			return this.planeZSetup.GetZ(pos);
		}

		public bool IsInside(Vector2 pos)
		{
			int num = 0;
			GraphicsSceneConfig.Triangle.pointsCache[0] = this.p1;
			GraphicsSceneConfig.Triangle.pointsCache[1] = this.p2;
			GraphicsSceneConfig.Triangle.pointsCache[2] = this.p3;
			Vector2[] array = GraphicsSceneConfig.Triangle.pointsCache;
			for (int i = 0; i < array.Length; i++)
			{
				Vector2 b = array[i];
				Vector2 v = ((i == array.Length - 1) ? array[0] : array[i + 1]) - b;
				Vector2 v2 = pos - b;
				int num2 = Mathf.RoundToInt(Mathf.Sign(Vector3.Cross(v, v2).z));
				if (num != 0 && num != num2)
				{
					return false;
				}
				num = num2;
			}
			return true;
		}

		public int depth;

		public GraphicsSceneConfig.Triangle.PlaneZSetup planeZSetup;

		public Vector2 p1;

		public Vector2 p2;

		public Vector2 p3;

		private static Vector2[] pointsCache = new Vector2[3];

		[Serializable]
		public struct PlaneZSetup
		{
			public float GetZ(Vector2 pos)
			{
				return this.dc - this.ac * pos.x - this.bc * pos.y;
			}

			public float dc;

			public float ac;

			public float bc;
		}
	}

	[Serializable]
	public class VisualObject
	{
		public string animationSettingsName
		{
			get
			{
				if (string.IsNullOrEmpty(this.sceneObjectInfo.animationSettingsName))
				{
					return this.name;
				}
				return this.sceneObjectInfo.animationSettingsName;
			}
		}

		public string displayName
		{
			get
			{
				if (string.IsNullOrEmpty(this.sceneObjectInfo.displayName))
				{
					return GGUtil.FirstCharToUpper(this.name);
				}
				return this.sceneObjectInfo.displayName;
			}
		}

		private RoomsBackend.RoomAccessor roomAccessor
		{
			get
			{
				if (this.roomAccessor_.needsToBeRenewed)
				{
					this.roomAccessor_ = this.roomAccessor_.CreateRenewedAccessor();
				}
				return this.roomAccessor_;
			}
		}

		private RoomsBackend.VisualObjectAccessor visualObjectModel
		{
			get
			{
				if (this.visualObjectModel_ == null)
				{
					this.visualObjectModel_ = this.roomAccessor.GetVisualObject(this.name);
				}
				if (this.visualObjectModel_.needsToBeRenewed)
				{
					this.visualObjectModel_ = this.roomAccessor.GetVisualObject(this.name);
				}
				return this.visualObjectModel_;
			}
		}

		public void Init(RoomsBackend.RoomAccessor roomAccessor)
		{
			this.roomAccessor_ = roomAccessor;
		}

		public GraphicsSceneConfig.Variation ownedVariation
		{
			get
			{
				if (this.ownedVariationIndex < 0 || this.ownedVariationIndex >= this.variations.Count)
				{
					return null;
				}
				return this.variations[this.ownedVariationIndex];
			}
		}

		public GraphicsSceneConfig.VisualObject.HitResult GetHitResult(Vector2 psdPosition)
		{
			GraphicsSceneConfig.VisualObject.HitResult hitResult = default(GraphicsSceneConfig.VisualObject.HitResult);
			List<GraphicsSceneConfig.Triangle> list = this.hitboxTriangles;
			GraphicsSceneConfig.Variation ownedVariation = this.ownedVariation;
			if (ownedVariation != null && ownedVariation.hitboxTriangles.Count > 0)
			{
				list = ownedVariation.hitboxTriangles;
			}
			for (int i = 0; i < list.Count; i++)
			{
				GraphicsSceneConfig.Triangle triangle = list[i];
				if (triangle.IsInside(psdPosition))
				{
					int num = -Mathf.RoundToInt(triangle.GetZ(psdPosition) * 1000f);
					if (!hitResult.isHit || hitResult.hitDepth < num)
					{
						hitResult.isHit = true;
						hitResult.hitDepth = num;
					}
				}
			}
			if (list.Count > 0)
			{
				return hitResult;
			}
			for (int j = 0; j < this.hitboxesList.Count; j++)
			{
				GraphicsSceneConfig.DrawnShape drawnShape = this.hitboxesList[j];
				if (drawnShape.shape.IsInside(psdPosition) && (!hitResult.isHit || hitResult.hitDepth < drawnShape.depth))
				{
					hitResult.isHit = true;
					hitResult.hitDepth = drawnShape.depth;
				}
			}
			if (this.hitboxesList.Count > 0)
			{
				return hitResult;
			}
			hitResult.isHit = this.IsHit(psdPosition);
			hitResult.hitDepth = this.maxDepth;
			return hitResult;
		}

		private bool IsHit(Vector2 psdPostion)
		{
			for (int i = 0; i < this.hitboxes.Count; i++)
			{
				if (this.hitboxes[i].IsInside(psdPostion))
				{
					return true;
				}
			}
			return false;
		}

		public bool isOwned
		{
			get
			{
				return this.visualObjectModel.visualObject.isOwned;
			}
			set
			{
				this.visualObjectModel.visualObject.isOwned = value;
				this.visualObjectModel.Save();
			}
		}

		public bool IsUnlocked(DecoratingScene scene)
		{
			return (this.sceneObjectInfo.groupIndex <= 0 || scene.IsAllElementsPickedUpInGroup(this.sceneObjectInfo.groupIndex - 1)) && this.isUnlocked;
		}

		private bool isUnlocked
		{
			get
			{
				for (int i = 0; i < this.sceneObjectInfo.backwardDependencies.Count; i++)
				{
					string visualObjectName = this.sceneObjectInfo.backwardDependencies[i];
					if (!this.roomAccessor.IsOwned(visualObjectName))
					{
						return false;
					}
				}
				return true;
			}
		}

		public int ownedVariationIndex
		{
			get
			{
				return this.visualObjectModel.visualObject.selectedVariationIndex;
			}
			set
			{
				this.visualObjectModel.visualObject.selectedVariationIndex = value;
				this.visualObjectModel.Save();
			}
		}

		public int depthForMarkerLines
		{
			get
			{
				if (this.sceneObjectInfo.isMarkersAbove)
				{
					return this.maxDepth + 1;
				}
				return this.startingDepth - 1;
			}
		}

		public int maxDepth
		{
			get
			{
				bool flag = false;
				int num = 0;
				for (int i = 0; i < this.variations.Count; i++)
				{
					GraphicsSceneConfig.Variation variation = this.variations[i];
					for (int j = 0; j < variation.sprites.Count; j++)
					{
						GraphicsSceneConfig.VisualSprite visualSprite = variation.sprites[j];
						if (!flag)
						{
							num = visualSprite.depth;
							flag = true;
						}
						else
						{
							num = Mathf.Max(num, visualSprite.depth);
						}
					}
				}
				if (!flag)
				{
					return 0;
				}
				return num;
			}
		}

		public int startingDepth
		{
			get
			{
				bool flag = false;
				int num = 0;
				if (this.hasDefaultVariation)
				{
					GraphicsSceneConfig.Variation variation = this.defaultVariation;
					for (int i = 0; i < variation.sprites.Count; i++)
					{
						GraphicsSceneConfig.VisualSprite visualSprite = variation.sprites[i];
						if (!flag)
						{
							num = visualSprite.depth;
							flag = true;
						}
						else
						{
							num = Mathf.Min(num, visualSprite.depth);
						}
					}
				}
				for (int j = 0; j < this.variations.Count; j++)
				{
					GraphicsSceneConfig.Variation variation2 = this.variations[j];
					for (int k = 0; k < variation2.sprites.Count; k++)
					{
						GraphicsSceneConfig.VisualSprite visualSprite2 = variation2.sprites[k];
						if (!flag)
						{
							num = visualSprite2.depth;
							flag = true;
						}
						else
						{
							num = Mathf.Min(num, visualSprite2.depth);
						}
					}
				}
				if (!flag)
				{
					return 0;
				}
				return num;
			}
		}

		public string name;

		public List<GraphicsSceneConfig.Variation> variations = new List<GraphicsSceneConfig.Variation>();

		private List<GraphicsSceneConfig.Variation> allVariations_ = new List<GraphicsSceneConfig.Variation>();

		public int index;

		public SceneObjectsDB.SceneObjectInfo sceneObjectInfo = new SceneObjectsDB.SceneObjectInfo();

		public List<ShapeGraphShape> hitboxes = new List<ShapeGraphShape>();

		public List<GraphicsSceneConfig.Triangle> hitboxTriangles = new List<GraphicsSceneConfig.Triangle>();

		public List<GraphicsSceneConfig.DrawnShape> hitboxesList = new List<GraphicsSceneConfig.DrawnShape>();

		public List<ShapeGraphShape> dashLines = new List<ShapeGraphShape>();

		public Vector3 iconHandlePosition;

		public bool hasDefaultVariation;

		public GraphicsSceneConfig.Variation defaultVariation;

		private RoomsBackend.RoomAccessor roomAccessor_;

		private RoomsBackend.VisualObjectAccessor visualObjectModel_;

		public struct HitResult
		{
			public bool isHit;

			public int hitDepth;
		}
	}

	public class ValidName
	{
		public List<string> validNames;
	}
}
