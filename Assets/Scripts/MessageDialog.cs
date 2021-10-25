using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;

public class MessageDialog : MonoBehaviour
{
	public static MessageDialog instance
	{
		get
		{
			return NavigationManager.instance.GetObject<MessageDialog>();
		}
	}

	private static void SetLabel(TextMeshProUGUI label, string text)
	{
		if (label == null)
		{
			return;
		}
		if (label.text == text)
		{
			return;
		}
		label.text = text;
	}

	public void ShowOk(string header, string text, string ok, Action<bool> onComplete)
	{
		GGUtil.SetActive(this.widgetsToHide, false);
		MessageDialog.SetLabel(this.headerLabel, header);
		MessageDialog.SetLabel(this.textLabel, text);
		MessageDialog.SetLabel(this.okLabel, ok);
		this.okStyle.Apply();
		this.onComplete = onComplete;
		this.ShowOnNav();
	}

	public void ShowYesNo(string header, string text, string yes, string no, Action<bool> onComplete)
	{
		GGUtil.SetActive(this.widgetsToHide, false);
		MessageDialog.SetLabel(this.headerLabel, header);
		MessageDialog.SetLabel(this.textLabel, text);
		MessageDialog.SetLabel(this.yesLabel, yes);
		MessageDialog.SetLabel(this.noLabel, no);
		this.yesNoStyle.Apply();
		this.onComplete = onComplete;
		this.ShowOnNav();
	}

	private void ShowOnNav()
	{
		NavigationManager.instance.Push(base.gameObject, true);
	}

	private void OnComplete(bool isYes)
	{
		if (this.onComplete == null)
		{
			NavigationManager.instance.Pop(true);
			return;
		}
		this.onComplete(isYes);
	}

	public void Callback_OnOk()
	{
		this.OnComplete(true);
	}

	public void Callback_OnYes()
	{
		this.OnComplete(true);
	}

	public void Callback_OnNo()
	{
		this.OnComplete(false);
	}

	[SerializeField]
	private TextMeshProUGUI headerLabel;

	[SerializeField]
	private TextMeshProUGUI textLabel;

	[SerializeField]
	private TextMeshProUGUI okLabel;

	[SerializeField]
	private TextMeshProUGUI yesLabel;

	[SerializeField]
	private TextMeshProUGUI noLabel;

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private VisualStyleSet okStyle = new VisualStyleSet();

	[SerializeField]
	private VisualStyleSet yesNoStyle = new VisualStyleSet();

	private Action<bool> onComplete;
}
