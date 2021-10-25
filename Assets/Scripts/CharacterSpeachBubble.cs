using System;
using TMPro;
using UnityEngine;

public class CharacterSpeachBubble : MonoBehaviour
{
	public void SetAlpha(float alpha)
	{
		this.mainCanvasGroup.alpha = alpha;
	}

	public void SetActive(bool active)
	{
		GGUtil.SetActive(this, active);
	}

	public void SetText(string text)
	{
		if(this.label != null)
			this.label.text = text;
	}

	[SerializeField]
	private TextMeshProUGUI label;

	[SerializeField]
	private CanvasGroup mainCanvasGroup;

	[NonSerialized]
	public string characterName;

	[NonSerialized]
	public CharacterAvatar avatar;
}
