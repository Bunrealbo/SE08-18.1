using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorMonster : MonoBehaviour
{
	public void Init(LevelEditorVisualizer viz, LevelDefinition level, LevelDefinition.MonsterElement monsterElement)
	{
		for (int i = 0; i < this.elements.Count; i++)
		{
			LevelEditorMonster.ElementDesc elementDesc = this.elements[i];
			GGUtil.SetActive(elementDesc.image, elementDesc.size == monsterElement.size);
		}
		Vector3 localPosition = viz.GetLocalPosition(level, monsterElement.position);
		Vector3 localPosition2 = viz.GetLocalPosition(level, monsterElement.oppositeCornerPosition);
		Vector3 localPosition3 = Vector3.Lerp(localPosition, localPosition2, 0.5f);
		base.transform.localPosition = localPosition3;
		Match3Settings.MonsterColorSettings monsterColorSettings = Match3Settings.instance.GeMonsterColorSettings(monsterElement.itemColor);
		if (monsterColorSettings != null)
		{
			for (int j = 0; j < this.elements.Count; j++)
			{
				this.elements[j].image.material = monsterColorSettings.material;
			}
		}
	}

	[SerializeField]
	private List<LevelEditorMonster.ElementDesc> elements = new List<LevelEditorMonster.ElementDesc>();

	[Serializable]
	public class ElementDesc
	{
		public IntVector2 size;

		public Image image;
	}
}
