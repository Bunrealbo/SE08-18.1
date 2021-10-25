using System;
using GGMatch3;
using TMPro;
using UnityEngine;

public class ExperimentsScreen : MonoBehaviour
{
	public void Show()
	{
		NavigationManager.instance.Push(base.gameObject, false);
	}

	public void OnEnable()
	{
		this.Init();
	}

	public void Init()
	{
		this.label.text = GGPlayerSettings.instance.GetName();
	}

	public void ButtonCallback_StartExperiment()
	{
		Match3StagesDB.instance.ResetAll();
		SingletonInit<RoomsBackend>.instance.Reset();
		BehaviourSingleton<EnergyManager>.instance.FillEnergy();
		GGPlayerSettings.instance.ResetEverything();
		GGUIDPrivate.Reset();
		string text = this.label.text;
		UnityEngine.Debug.LogFormat("Tester name is {0}", new object[]
		{
			text
		});
		GGPlayerSettings.instance.SetName(text);
		GGPlayerSettings.instance.Save();
		AWSFirehoseAnalytics awsfirehoseAnalytics = UnityEngine.Object.FindObjectOfType<AWSFirehoseAnalytics>();
		awsfirehoseAnalytics.ResetModel();
		awsfirehoseAnalytics.sessionID = GGUID.NewGuid();
		NavigationManager.instance.Pop(true);
	}

	[SerializeField]
	private TextMeshProUGUI label;
}
