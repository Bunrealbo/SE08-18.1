using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteGroup : MonoBehaviour
{
	private void OnDidApplyAnimationProperties()
	{
		for (int i = 0; i < this.sprites.Count; i++)
		{
			SpriteRenderer spriteRenderer = this.sprites[i];
			Color color = spriteRenderer.color;
			color.a = this.alphaValue;
			spriteRenderer.color = color;
		}
	}

	[SerializeField]
	private List<SpriteRenderer> sprites = new List<SpriteRenderer>();

	[SerializeField]
	private float alphaValue;
}
