using System;
using System.Collections.Generic;
using ProtoModels;
using UnityEngine;

public class InventoryDB : ScriptableObject
{
	public DecoratingRooms model
	{
		get
		{
			if (this._model == null)
			{
				this.ReloadModel();
			}
			return this._model;
		}
	}

	public void ResetAll()
	{
		this._model = new DecoratingRooms();
		this._model.rooms = new List<DecoratingRoom>();
		this.SaveModel();
	}

	public void ReloadModel()
	{
		if (!ProtoIO.LoadFromFileLocal<DecoratingRooms>(this.filename, out this._model))
		{
			this._model = new DecoratingRooms();
			this._model.rooms = new List<DecoratingRoom>();
			this.SaveModel();
		}
		if (this._model.rooms == null)
		{
			this._model.rooms = new List<DecoratingRoom>();
			this.SaveModel();
		}
	}

	public void SaveModel()
	{
		ProtoIO.SaveToFileCS<DecoratingRooms>(this.filename, this._model);
	}

	public static InventoryDB instance
	{
		get
		{
			if (InventoryDB._instance == null)
			{
				InventoryDB._instance = Resources.Load<InventoryDB>("InventoryDB");
				SingletonInit<FileIOChanges>.instance.OnChange(new FileIOChanges.OnDataChangedDelegate(InventoryDB._instance.ReloadModel));
			}
			return InventoryDB._instance;
		}
	}

	[SerializeField]
	public string filename = "inventory.bytes";

	private DecoratingRooms _model;

	private static InventoryDB _instance;
}
