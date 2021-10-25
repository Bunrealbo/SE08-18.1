using System;
using GGMatch3;
using TMPro;
using UnityEngine;

public class StageSelectionScreen : MonoBehaviour
{
	public void Show()
	{
		NavigationManager.instance.Push(base.gameObject, false);
	}

	public void Init()
	{
		Match3StagesDB.Stage currentStage = Match3StagesDB.instance.currentStage;
		int index = currentStage.index;
		string levelName = currentStage.levelReference.levelName;
		string levelDBName = currentStage.levelReference.levelDBName;
		this.stageLabel.text = string.Format(this.stageNameFormat, index, levelName, levelDBName);
	}

	public void OnEnable()
	{
		this.Init();
	}

	public void ButtonCallback_OnLeft()
	{
		int num = Match3StagesDB.instance.stagesPassed;
		num--;
		if (num < 0)
		{
			num = Match3StagesDB.instance.stages.Count - 1;
		}
		Match3StagesDB.instance.stagesPassed = Mathf.Clamp(num, 0, Match3StagesDB.instance.stages.Count - 1);
		this.Init();
	}

	public void ButtonCallback_OnRight()
	{
		Match3StagesDB instance = Match3StagesDB.instance;
		int stagesPassed = instance.stagesPassed;
		instance.stagesPassed = stagesPassed + 1;
		Match3StagesDB.instance.stagesPassed = Mathf.Clamp(Match3StagesDB.instance.stagesPassed, 0, Match3StagesDB.instance.stages.Count - 1);
		this.Init();
	}

	public void ButtonCallback_OnBack()
	{
		NavigationManager.instance.Pop(true);
	}

	[SerializeField]
	private TextMeshProUGUI stageLabel;

	[SerializeField]
	private string stageNameFormat;
}
