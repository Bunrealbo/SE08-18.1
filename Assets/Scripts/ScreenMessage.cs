using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMessage : MonoBehaviour
{
	public void Init(ScreenMessagePanel panel, string text)
	{
		this.panel = panel;
		this.text.text = text;
	}

	public void Play()
	{
		this.animator.SetTrigger("Play");
	}

	public void OnAnimationEnd()
	{
		this.panel.OnAnimationEnd();
	}

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Text text;

	private ScreenMessagePanel panel;
}
