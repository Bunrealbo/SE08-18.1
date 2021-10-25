using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorBurriedElement : MonoBehaviour
{
	public void Init(LevelEditorVisualizer viz, LevelDefinition level, LevelDefinition.BurriedElement burriedElement)
	{
		for (int i = 0; i < this.elements.Count; i++)
		{
			LevelEditorBurriedElement.BurriedElementDesc burriedElementDesc = this.elements[i];
			GGUtil.SetActive(burriedElementDesc.image, burriedElementDesc.size == burriedElement.size);
		}
		Quaternion localRotation = Quaternion.identity;
		if (burriedElement.orientation == LevelDefinition.BurriedElement.Orientation.Horizontal)
		{
			localRotation = Quaternion.Euler(0f, 0f, 90f);
		}
		Vector3 localPosition = viz.GetLocalPosition(level, burriedElement.position);
		Vector3 localPosition2 = viz.GetLocalPosition(level, burriedElement.oppositeCornerPosition);
		Vector3 localPosition3 = Vector3.Lerp(localPosition, localPosition2, 0.5f);
		base.transform.localPosition = localPosition3;
		base.transform.localRotation = localRotation;
	}

	[SerializeField]
	private List<LevelEditorBurriedElement.BurriedElementDesc> elements = new List<LevelEditorBurriedElement.BurriedElementDesc>();

	[SerializeField]
	private RectTransform rotationRect;

	[Serializable]
	public class BurriedElementDesc
	{
		public IntVector2 size;

		public Image image;
	}
}
