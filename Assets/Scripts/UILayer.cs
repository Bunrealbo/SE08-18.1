using System;
using GGMatch3;
using UnityEngine;

public class UILayer : MonoBehaviour
{
	public virtual void OnGoBack(NavigationManager nav)
	{
		if (!this.escToGoBack)
		{
			return;
		}
		nav.Pop(true);
		GGSoundSystem.Play(this.backSound);
	}

	[SerializeField]
	private bool escToGoBack;

	[SerializeField]
	protected GGSoundSystem.SFXType backSound = GGSoundSystem.SFXType.CancelPress;
}
