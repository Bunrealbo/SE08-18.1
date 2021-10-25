using System;
using TMPro;
using UnityEngine;

public class SelectRoomScreenButton : MonoBehaviour
{
	public void Init(RoomsDB.Room room, SelectRoomScreen roomScreen)
	{
		this.room = room;
		this.roomScreen = roomScreen;
		GGUtil.ChangeText(this.roomNameLabel, room.name);
	}

	public void ButtonCallback_OnClick()
	{
		this.roomScreen.SelectRoomScreenButtonCallback_OnRoomSelected(this.room);
	}

	[SerializeField]
	private TextMeshProUGUI roomNameLabel;

	[NonSerialized]
	private RoomsDB.Room room;

	[NonSerialized]
	private SelectRoomScreen roomScreen;
}
