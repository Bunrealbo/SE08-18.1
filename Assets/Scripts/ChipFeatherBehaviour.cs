using System;
using System.Collections.Generic;
using UnityEngine;

public class ChipFeatherBehaviour : MonoBehaviour
{
	public void Init(ItemColor itemColor)
	{
		for (int i = 0; i < this.colors.Count; i++)
		{
			ChipFeatherBehaviour.ColorSetup colorSetup = this.colors[i];
			if (colorSetup.itemColor == itemColor)
			{
				colorSetup.Apply();
			}
		}
	}

	[SerializeField]
	private List<ChipFeatherBehaviour.ColorSetup> colors = new List<ChipFeatherBehaviour.ColorSetup>();

	[Serializable]
	public class ColorSetup
	{
		public void Apply()
		{
			if (this.spriteRenderer != null)
			{
				this.spriteRenderer.color = this.color;
			}
		}

		public ItemColor itemColor;

		public SpriteRenderer spriteRenderer;

		public Color color = Color.white;
	}
}
