using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSelectionButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler
{
	public void Init(DecorateRoomScreen roomScreen)
	{
		this.roomScreen = roomScreen;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.roomScreen == null)
		{
			return;
		}
		this.roomScreen.ButtonCallback_OnSceneClick();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
	}

	private DecorateRoomScreen roomScreen;
}
