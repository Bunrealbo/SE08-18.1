using System;
using TMPro;
using UnityEngine;

public class CompletionDialogTask : MonoBehaviour
{
	public void Init(CompletionDialog.InitArguments.Task task, Action<CompletionDialogTask> onComplete)
	{
		this.task = task;
		GGUtil.ChangeText(this.headerLabel, task.name);
		GGUtil.ChangeText(this.priceLabel, task.price);
		this.onComplete = onComplete;
	}

	public void ButtonCallback_OnPress()
	{
		Action<CompletionDialogTask> action = this.onComplete;
		if (action == null)
		{
			return;
		}
		action(this);
	}

	[SerializeField]
	private TextMeshProUGUI headerLabel;

	[SerializeField]
	private TextMeshProUGUI priceLabel;

	private Action<CompletionDialogTask> onComplete;

	[NonSerialized]
	public CompletionDialog.InitArguments.Task task;
}
