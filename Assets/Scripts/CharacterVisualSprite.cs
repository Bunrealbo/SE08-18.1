using System;
using UnityEngine;

public class CharacterVisualSprite : MonoBehaviour
{
	public void SetStencilIndex(int stencilIndex)
	{
		if (this.spriteRenderer == null)
		{
			return;
		}
		this.spriteRenderer.material.SetInt("_Stencil", stencilIndex);
	}

	public void Init(VisualSprite visualSpriteBeh, CharacterVisualObjectVariation variation)
	{
		this.visualSpriteBeh = visualSpriteBeh;
		GraphicsSceneConfig.VisualSprite visualSprite = visualSpriteBeh.visualSprite;
		this.variation = variation;
		Sprite sprite = visualSpriteBeh.spriteRenderer.sprite;
		GameObject gameObject = new GameObject("pivot");
		this.pivotTransform = gameObject.transform;
		this.pivotTransform.parent = base.transform;
		this.pivotTransform.localScale = Vector3.one;
		gameObject.layer = base.gameObject.layer;
		GameObject gameObject2 = new GameObject("sprite");
		this.spriteTransform = gameObject2.transform;
		this.spriteTransform.parent = this.pivotTransform;
		this.spriteTransform.localScale = Vector3.one;
		gameObject2.layer = base.gameObject.layer;
		if (sprite != null)
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
		this.pivotTransform.localPosition = visualSpriteBeh.pivotTransform.localPosition;
		this.spriteTransform.localPosition = visualSpriteBeh.spriteTransform.localPosition;
		Material spriteMaterial = variation.visualObjectBehaviour.scene.spriteMaterial;
		if (spriteMaterial != null && this.spriteRenderer != null)
		{
			this.spriteRenderer.sharedMaterial = spriteMaterial;
		}
	}

	[SerializeField]
	private CharacterVisualObjectVariation variation;

	public SpriteRenderer spriteRenderer;

	public VisualSprite visualSpriteBeh;

	public Transform pivotTransform;

	public Transform spriteTransform;
}
