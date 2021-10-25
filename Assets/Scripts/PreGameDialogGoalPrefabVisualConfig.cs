using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreGameDialogGoalPrefabVisualConfig : MonoBehaviour
{
	public void SetSprite(Sprite sprite)
	{
		GGUtil.SetSprite(this.image, sprite);
	}

	public void SetLabel(string text)
	{
		GGUtil.ChangeText(this.label, text);
	}

	[SerializeField]
	private Image image;

	[SerializeField]
	private TextMeshProUGUI label;
}
