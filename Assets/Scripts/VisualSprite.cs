using System;
using UnityEngine;

public class VisualSprite : MonoBehaviour
{
	public void ResetVisually()
	{
		this.pivotTransform.localScale = Vector3.one;
		this.pivotTransform.position = this.pivotTransform.parent.TransformPoint(this.visualSprite.pivotPosition);
	}

	public void Init(GraphicsSceneConfig.VisualSprite visualSprite)
	{
		this.visualSprite = visualSprite;
		Sprite sprite = visualSprite.sprite;
		GameObject gameObject = new GameObject("pivot");
		this.pivotTransform = gameObject.transform;
		this.pivotTransform.parent = base.transform;
		this.pivotTransform.localScale = Vector3.one;
		GameObject gameObject2 = new GameObject("sprite");
		this.spriteTransform = gameObject2.transform;
		this.spriteTransform.parent = this.pivotTransform;
		this.spriteTransform.localScale = Vector3.one;
		if (visualSprite.sprite != null)
		{
			this.spriteRenderer = gameObject2.AddComponent<SpriteRenderer>();
			this.spriteRenderer.sprite = sprite;
			this.spriteRenderer.sortingOrder = visualSprite.depth;
			float num = (float)visualSprite.width / sprite.bounds.size.x;
			float num2 = (float)visualSprite.width / (float)visualSprite.height;
			float num3 = sprite.bounds.size.x / sprite.bounds.size.y;
			if (Mathf.Abs(num3 - num2) > 0.01f || float.IsNaN(num2) || float.IsNaN(num3))
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					visualSprite.spriteName,
					" WRONG ASPECT RATIO ",
					num3,
					" ",
					num2
				}));
			}
			this.spriteTransform.localScale = new Vector3(num, num, 1f);
		}
		Vector3 spritePosition = visualSprite.spritePosition;
		Vector3 pivotPosition = visualSprite.pivotPosition;
		this.pivotTransform.position = base.transform.parent.TransformPoint(pivotPosition);
		base.transform.parent.TransformPoint(spritePosition);
		this.spriteTransform.position = spritePosition;
	}

	public void ResetPositions()
	{
		Vector3 spritePosition = this.visualSprite.spritePosition;
		Vector3 pivotPosition = this.visualSprite.pivotPosition;
		this.pivotTransform.position = base.transform.parent.TransformPoint(pivotPosition);
		base.transform.parent.TransformPoint(spritePosition);
		this.spriteTransform.position = spritePosition;
	}

	public SpriteRenderer spriteRenderer;

	public GraphicsSceneConfig.VisualSprite visualSprite;

	public Transform pivotTransform;

	public Transform spriteTransform;
}
