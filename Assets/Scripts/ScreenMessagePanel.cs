using System;
using UnityEngine;

public class ScreenMessagePanel : MonoBehaviour
{
	public void Play(string text)
	{
		GGUtil.SetActive(this, true);
		this.message.Init(this, text);
		this.message.Play();
	}

	public void OnAnimationEnd()
	{
		GGUtil.SetActive(this, false);
	}

	[SerializeField]
	private ScreenMessage message;
}
