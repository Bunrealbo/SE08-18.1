using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class CompletionDialog : UILayer
{
	public void Show(CompletionDialog.InitArguments init)
	{
		this.initArguments = init;
		NavigationManager.instance.Push(base.gameObject, init.showModal);
	}

	private void OnEnable()
	{
		this.Init(this.initArguments);
	}

	public void Init(CompletionDialog.InitArguments init)
	{
		this.initArguments = init;
		List<CompletionDialog.InitArguments.Task> tasks = init.tasks;
		Vector2 prefabSizeDelta = this.tasksPool.prefabSizeDelta;
		UnityEngine.Debug.Log("Size " + prefabSizeDelta.x);
		float num = (float)tasks.Count * prefabSizeDelta.x + (float)(tasks.Count - 1) * this.padding;
		Vector2 a = new Vector2(-num * 0.5f, 0f);
		this.tasksPool.Clear();
		for (int i = 0; i < tasks.Count; i++)
		{
			CompletionDialog.InitArguments.Task task = tasks[i];
			Vector2 v = a + Vector2.right * ((float)i * (prefabSizeDelta.x + this.padding) + prefabSizeDelta.x * 0.5f);
			CompletionDialogTask completionDialogTask = this.tasksPool.Next<CompletionDialogTask>(false);
			completionDialogTask.transform.localPosition = v;
			completionDialogTask.Init(task, new Action<CompletionDialogTask>(this.CompleteDialogTask_OnPressed));
			GGUtil.Show(completionDialogTask);
		}
		this.tasksPool.HideNotUsed();
	}

	private void CompleteDialogTask_OnPressed(CompletionDialogTask task)
	{
		Action<CompletionDialog.InitArguments.Task> onComplete = this.initArguments.onComplete;
		if (onComplete == null)
		{
			return;
		}
		onComplete(task.task);
	}

	public override void OnGoBack(NavigationManager nav)
	{
		GGSoundSystem.Play(this.backSound);
		Action onCancel = this.initArguments.onCancel;
		if (onCancel == null)
		{
			return;
		}
		onCancel();
	}

	[SerializeField]
	private ComponentPool tasksPool = new ComponentPool();

	[SerializeField]
	private float padding = 20f;

	[NonSerialized]
	private CompletionDialog.InitArguments initArguments;

	public class InitArguments
	{
		public bool showModal;

		public List<CompletionDialog.InitArguments.Task> tasks = new List<CompletionDialog.InitArguments.Task>();

		public Action<CompletionDialog.InitArguments.Task> onComplete;

		public Action onCancel;

		public struct Task
		{
			public string name;

			public int price;

			public CarModelPart part;
		}
	}
}
