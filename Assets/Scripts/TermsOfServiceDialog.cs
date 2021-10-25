using System;
using GGMatch3;
using UnityEngine;

public class TermsOfServiceDialog : MonoBehaviour
{
	public void Show(Action<bool> onComplete)
	{
		this.onComplete = onComplete;
		NavigationManager.instance.Push(base.gameObject, true);
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
	}

	public void ButtonCallback_OnCancel()
	{
		if (this.onComplete != null)
		{
			this.onComplete(false);
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public void ButtonCallback_OnOK()
	{
		if (this.onComplete != null)
		{
			this.onComplete(true);
		}
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	public void ButtonCallback_OnTermsOfService()
	{
		Application.OpenURL("http://www.giraffe-games.com/terms-of-use/");
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	public void ButtonCallback_OnPrivacyPolicy()
	{
		Application.OpenURL("http://www.giraffe-games.com/privacy-policy/");
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	private Action<bool> onComplete;
}
