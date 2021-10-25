using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class BurriedElementBehaviour : SlotComponentBehavoiour
{
	public void Init(LevelDefinition.BurriedElement element)
	{
		for (int i = 0; i < this.elements.Count; i++)
		{
			BurriedElementBehaviour.ElementDescriptor elementDescriptor = this.elements[i];
			bool active = elementDescriptor.size == element.size;
			GGUtil.SetActive(elementDescriptor.image, active);
		}
		Quaternion localRotation = Quaternion.identity;
		if (element.orientation == LevelDefinition.BurriedElement.Orientation.Horizontal)
		{
			localRotation = Quaternion.Euler(0f, 0f, -90f);
		}
		this.rotationTransform.localRotation = localRotation;
	}

	public override void RemoveFromGame()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[SerializeField]
	private List<BurriedElementBehaviour.ElementDescriptor> elements = new List<BurriedElementBehaviour.ElementDescriptor>();

	[SerializeField]
	public Transform rotationTransform;

	[Serializable]
	public class ElementDescriptor
	{
		public IntVector2 size;

		public Transform image;
	}
}
