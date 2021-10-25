using System;
using UnityEngine;

public class FileIOChanges : SingletonInit<FileIOChanges>
{
	private event FileIOChanges.OnDataChangedDelegate onDataChanged;

	public void OnChange(FileIOChanges.OnDataChangedDelegate dataChangeDelegate)
	{
		this.onDataChanged -= dataChangeDelegate;
		this.onDataChanged += dataChangeDelegate;
	}

	public override void Init()
	{
		BehaviourSingletonInit<GGNotificationCenter>.instance.onMessage += this.OnMessage;
	}

	private void OnMessage(string message)
	{
		if (message == "MessageConflictResolved")
		{
			this.ReloadModels();
		}
	}

	private void ReloadModels()
	{
		UnityEngine.Debug.Log("Reload Models");
		if (this.onDataChanged != null)
		{
			this.onDataChanged();
		}
	}

	public delegate void OnDataChangedDelegate();
}
