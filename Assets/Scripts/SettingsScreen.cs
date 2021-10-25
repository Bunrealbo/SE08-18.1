using System;
using GGMatch3;
using UnityEngine;

public class SettingsScreen : MonoBehaviour
{
	private void ResetGameProgress()
	{
		Match3StagesDB.instance.ResetAll();
		SingletonInit<RoomsBackend>.instance.Reset();
		GGPlayerSettings.instance.ResetEverything();
		BehaviourSingleton<EnergyManager>.instance.FillEnergy();
		GGUIDPrivate.Reset();
		AWSFirehoseAnalytics awsfirehoseAnalytics = UnityEngine.Object.FindObjectOfType<AWSFirehoseAnalytics>();
		awsfirehoseAnalytics.ResetModel();
		awsfirehoseAnalytics.sessionID = GGUID.NewGuid();
	}

	public void ButtonCallback_ResetGameProgress()
	{
		this.ResetGameProgress();
		NavigationManager.instance.Pop(true);
	}

	public void ButtonCallback_ShowExperimentsScreen()
	{
		NavigationManager.instance.GetObject<ExperimentsScreen>().Show();
	}

	public void ButtonCallback_ShowStageSelectionScreen()
	{
		NavigationManager.instance.GetObject<StageSelectionScreen>().Show();
	}

	public void ButtonCallback_ClearCache()
	{
		Caching.ClearCache();
	}

	public void ButtonCallback_GiveStars()
	{
		GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.diamonds, 1000);
	}

	public void ButtonCallback_ResetAndGiveStars()
	{
		this.ResetGameProgress();
		GGPlayerSettings.instance.walletManager.AddCurrency(CurrencyType.diamonds, 1000);
		NavigationManager.instance.Pop(true);
	}

	public void OnEnable()
	{
		GGUtil.SetActive(this.experimentsButton, ConfigBase.instance.debug);
		GGUtil.SetActive(this.stagesSelectionButton, ConfigBase.instance.debug);
	}

	[SerializeField]
	private RectTransform experimentsButton;

	[SerializeField]
	private RectTransform stagesSelectionButton;
}
