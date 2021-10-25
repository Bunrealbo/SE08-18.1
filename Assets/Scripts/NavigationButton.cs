using System;
using GGMatch3;
using UnityEngine;

public class NavigationButton : MonoBehaviour
{
	public void OnClick()
	{
		NavigationManager instance = NavigationManager.instance;
		if (instance == null)
		{
			return;
		}
		if (this.action == NavigationButton.ActionType.Pop)
		{
			GGSoundSystem.Play(this.pressSound);
			instance.Pop(true);
			return;
		}
		bool isModal = this.action == NavigationButton.ActionType.PushModal;
		NavigationManager.ObjectDefinition objectByName = instance.GetObjectByName(this.screenName);
		if (objectByName == null)
		{
			return;
		}
		instance.Push(objectByName.gameObject, isModal);
		GGSoundSystem.Play(this.pressSound);
	}

	[SerializeField]
	private NavigationButton.ActionType action;

	[SerializeField]
	private string screenName;

	[SerializeField]
	private GGSoundSystem.SFXType pressSound;

	public enum ActionType
	{
		Push,
		PushModal,
		Pop
	}
}
