using System;
using System.Collections.Generic;
using ProtoModels;
using UnityEngine;

public class OwnedItems
{
	public OwnedItems(string filename)
	{
		this.OwnedItemsFilename = filename;
		this.ReloadModel();
		SingletonInit<FileIOChanges>.instance.OnChange(new FileIOChanges.OnDataChangedDelegate(this.ReloadModel));
	}

	public void ReloadModel()
	{
		if (!ProtoIO.LoadFromFileLocal<ProtoSerializer, OwnedItemsDAO>(this.OwnedItemsFilename, out this.ownedItems))
		{
			this.ownedItems = new OwnedItemsDAO();
			ProtoIO.SaveToFileCS<OwnedItemsDAO>(this.OwnedItemsFilename, this.ownedItems);
		}
		if (this.ownedItems.ownedItems == null)
		{
			this.ownedItems.ownedItems = new List<OwnedItemDAO>();
			ProtoIO.SaveToFileCS<OwnedItemsDAO>(this.OwnedItemsFilename, this.ownedItems);
		}
	}

	public string OwnedItemsFilename
	{
		get
		{
			return this._003COwnedItemsFilename_003Ek__BackingField;
		}
		private set
		{
			this._003COwnedItemsFilename_003Ek__BackingField = value;
		}
	}

	public void AddToOwned(string name, bool canStockpile)
	{
		UnityEngine.Debug.Log("addToOwned");
		if (!canStockpile && this.isOwned(name))
		{
			UnityEngine.Debug.Log("addToOwned - cancelled");
			return;
		}
		OwnedItemDAO ownedItemDAO = new OwnedItemDAO();
		ownedItemDAO.name = name;
		this.ownedItems.ownedItems.Add(ownedItemDAO);
		this.Save();
	}

	public OwnedItemDAO GetOrCreateItemWithName(string name)
	{
		for (int i = 0; i < this.ownedItems.ownedItems.Count; i++)
		{
			OwnedItemDAO ownedItemDAO = this.ownedItems.ownedItems[i];
			if (ownedItemDAO.name == name)
			{
				return ownedItemDAO;
			}
		}
		OwnedItemDAO ownedItemDAO2 = new OwnedItemDAO();
		ownedItemDAO2.name = name;
		this.ownedItems.ownedItems.Add(ownedItemDAO2);
		this.Save();
		return ownedItemDAO2;
	}

	public UsedItemDAO GetOrCreateUsedItemWithName(string name)
	{
		if (this.ownedItems.usedItems == null)
		{
			this.ownedItems.usedItems = new List<UsedItemDAO>();
		}
		for (int i = 0; i < this.ownedItems.usedItems.Count; i++)
		{
			UsedItemDAO usedItemDAO = this.ownedItems.usedItems[i];
			if (usedItemDAO.name == name)
			{
				return usedItemDAO;
			}
		}
		UsedItemDAO usedItemDAO2 = new UsedItemDAO();
		usedItemDAO2.name = name;
		this.ownedItems.usedItems.Add(usedItemDAO2);
		this.Save();
		return usedItemDAO2;
	}

	public bool isOwned(string name)
	{
		OwnedItems._003C_003Ec__DisplayClass14_0 _003C_003Ec__DisplayClass14_ = new OwnedItems._003C_003Ec__DisplayClass14_0();
		_003C_003Ec__DisplayClass14_.name = name;
		return this.ownedItems.ownedItems != null && this.ownedItems.ownedItems.Find(new Predicate<OwnedItemDAO>(_003C_003Ec__DisplayClass14_._003CisOwned_003Eb__0)) != null;
	}

	public OwnedItemDAO GetItemWithName(string name)
	{
		if (this.ownedItems.ownedItems == null)
		{
			return null;
		}
		OwnedItemDAO result = null;
		for (int i = 0; i < this.ownedItems.ownedItems.Count; i++)
		{
			OwnedItemDAO ownedItemDAO = this.ownedItems.ownedItems[i];
			if (ownedItemDAO.name == name)
			{
				result = ownedItemDAO;
				break;
			}
		}
		return result;
	}

	public void Save()
	{
		ProtoIO.SaveToFileCS<OwnedItemsDAO>(this.OwnedItemsFilename, this.ownedItems);
	}

	public static string DefaultOwnedItemsFilename = "ownedItems.bytes";

	public OwnedItemsDAO ownedItems;

	private string _003COwnedItemsFilename_003Ek__BackingField;

	private sealed class _003C_003Ec__DisplayClass14_0
	{
		internal bool _003CisOwned_003Eb__0(OwnedItemDAO o)
		{
			return o.name == this.name;
		}

		public string name;
	}
}
