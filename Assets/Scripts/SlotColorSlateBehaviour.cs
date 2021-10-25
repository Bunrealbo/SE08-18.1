using System;
using System.Collections.Generic;
using UnityEngine;

public class SlotColorSlateBehaviour : MonoBehaviour
{
	public void Init(string spriteName)
	{
		for (int i = 0; i < this.sprites.Count; i++)
		{
			SlotColorSlateBehaviour.SpriteDescriptor spriteDescriptor = this.sprites[i];
			spriteDescriptor.sprite.gameObject.SetActive(spriteDescriptor.spriteName == spriteName);
		}
	}

	[SerializeField]
	private SpriteRenderer sprite;

	[SerializeField]
	private List<SlotColorSlateBehaviour.SpriteDescriptor> sprites = new List<SlotColorSlateBehaviour.SpriteDescriptor>();

	[Serializable]
	public class SpriteDescriptor
	{
		public string spriteName;

		public SpriteRenderer sprite;
	}
}
