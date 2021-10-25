using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class SelectRoomScreen : MonoBehaviour
{
	private void OnEnable()
	{
		this.Init();
	}

	private void Init()
	{
		this.buttonsPool.Clear();
		List<RoomsDB.Room> rooms = ScriptableObjectSingleton<RoomsDB>.instance.rooms;
		for (int i = 0; i < rooms.Count; i++)
		{
			RoomsDB.Room room = rooms[i];
			this.buttonsPool.Next<SelectRoomScreenButton>(true).Init(room, this);
		}
		this.buttonsPool.HideNotUsed();
	}

	public void SelectRoomScreenButtonCallback_OnRoomSelected(RoomsDB.Room room)
	{
		RoomsBackend instance = SingletonInit<RoomsBackend>.instance;
		int num = ScriptableObjectSingleton<RoomsDB>.instance.IndexOf(room);
		UnityEngine.Debug.Log("ROOM INDEX " + num);
		instance.selectedRoomIndex = num;
		NavigationManager.instance.Pop(true);
	}

	[SerializeField]
	private ComponentPool buttonsPool = new ComponentPool();
}
