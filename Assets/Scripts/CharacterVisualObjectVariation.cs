using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualObjectVariation : MonoBehaviour
{
	public void SetStencilIndex(int stencilIndex)
	{
		for (int i = 0; i < this.sprites.Count; i++)
		{
			this.sprites[i].SetStencilIndex(stencilIndex);
		}
	}

	public void SetActive(bool isActive)
	{
		GGUtil.SetActive(this, isActive);
	}

	public void Init(CharacterVisualObjectBehaviour visualObjectBehaviour, VisualObjectVariation variation)
	{
		this.visualObjectBehaviour = visualObjectBehaviour;
		this.variation = variation;
		for (int i = 0; i < variation.sprites.Count; i++)
		{
			VisualSprite visualSprite = variation.sprites[i];
			if (!visualSprite.visualSprite.isShadow)
			{
				CharacterVisualSprite item = this.CreateSprite(visualSprite);
				this.sprites.Add(item);
			}
		}
	}

	public CharacterVisualSprite CreateSprite(VisualSprite vSprite)
	{
		CharacterVisualSprite characterVisualSprite = new GameObject(vSprite.visualSprite.spriteName)
		{
			layer = base.gameObject.layer,
			transform = 
			{
				parent = base.transform,
				localPosition = Vector3.zero
			}
		}.AddComponent<CharacterVisualSprite>();
		characterVisualSprite.Init(vSprite, this);
		return characterVisualSprite;
	}

	[SerializeField]
	public CharacterVisualObjectBehaviour visualObjectBehaviour;

	[SerializeField]
	private VisualObjectVariation variation;

	[SerializeField]
	private List<CharacterVisualSprite> sprites = new List<CharacterVisualSprite>();
}
