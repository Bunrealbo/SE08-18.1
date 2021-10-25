using System;
using GGMatch3;
using UnityEngine;
using UnityEngine.UI;

public class InGameSettingsScreen : MonoBehaviour
{
	private void OnEnable()
	{
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
		this.Init();
	}

	private void Init()
	{
		float alpha = (float)(GGPlayerSettings.instance.Model.isTestUser ? 1 : 0);
		GGUtil.SetAlpha(this.testerImage, alpha);
	}

	public void ButtonCallback_OnExit()
	{
		NavigationManager.instance.Pop(true);
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	public void ButtonCallback_OnRate()
	{
		GGSupportMenu.instance.showRateApp(ConfigBase.instance.platformRateProvider);
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	public void ButtonCallback_OnTesterClick()
	{
		float num = Time.unscaledTime - this.lastClickTime;
		this.lastClickTime = Time.unscaledTime;
		if (num > this.waitSeconds)
		{
			this.clicks = 0;
		}
		this.clicks++;
		if (this.clicks > this.tries)
		{
			GGPlayerSettings instance = GGPlayerSettings.instance;
			instance.Model.isTestUser = !instance.Model.isTestUser;
			instance.Save();
			this.clicks = 0;
			this.Init();
		}
	}

	public void ButtonCallback_OnTermsOfService()
	{
		Application.OpenURL("http://www.giraffe-games.com/terms-of-use/");
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	public void ButtonCallback_PrivacyPolicy()
	{
		Application.OpenURL("http://www.giraffe-games.com/privacy-policy/");
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	[SerializeField]
	private Image testerImage;

	[SerializeField]
	private int tries = 10;

	[SerializeField]
	private float waitSeconds = 1f;

	private float lastClickTime;

	private int clicks;
}
