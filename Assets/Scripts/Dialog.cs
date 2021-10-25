using System;
using GGMatch3;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
	public static Dialog instance
	{
		get
		{
			return NavigationManager.instance.GetObject<Dialog>();
		}
	}

	public void Show(string text, string yes, string no, Action<bool> onComplete)
	{
		this.InitYesNo(text, yes, no, onComplete);
		NavigationManager.instance.Push(base.gameObject, true);
	}

	public void Show(string text, string ok, Action<bool> onComplete)
	{
		this.InitOk(text, ok, onComplete);
		NavigationManager.instance.Push(base.gameObject, true);
	}

	private void InitYesNo(string text, string yes, string no, Action<bool> onComplete)
	{
		GGUtil.Hide(this.widgetsToHide);
		this.yesNoStyle.Apply();
		GGUtil.ChangeText(this.yesLabel, yes);
		GGUtil.ChangeText(this.noLabel, no);
		GGUtil.ChangeText(this.textLabel, text);
		this.onComplete = onComplete;
	}

	private void InitOk(string text, string ok, Action<bool> onComplete)
	{
		GGUtil.Hide(this.widgetsToHide);
		this.okStyle.Apply();
		GGUtil.ChangeText(this.okLabel, ok);
		GGUtil.ChangeText(this.textLabel, text);
		this.onComplete = onComplete;
	}

	private void Finish(bool success)
	{
		if (this.onComplete == null)
		{
			NavigationManager.instance.Pop(true);
			return;
		}
		this.onComplete(success);
	}

	public void ButtonCallback_OnYes()
	{
		this.Finish(true);
	}

	public void ButtonCallback_OnNo()
	{
		this.Finish(false);
	}

	[SerializeField]
	private TextMeshProUGUI yesLabel;

	[SerializeField]
	private TextMeshProUGUI noLabel;

	[SerializeField]
	private TextMeshProUGUI okLabel;

	[SerializeField]
	private TextMeshProUGUI textLabel;

	[SerializeField]
	private Transform[] widgetsToHide;

	[SerializeField]
	private VisualStyleSet yesNoStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet okStyle = new VisualStyleSet();

	private Action<bool> onComplete;
}
