using System;
using System.Collections.Generic;
using ProtoModels;

public class RoomsBackend : SingletonInit<RoomsBackend>
{
	public RoomDecoration model
	{
		get
		{
			if (this.model_ == null)
			{
				this.ReloadModel();
			}
			return this.model_;
		}
	}

	public int selectedRoomIndex
	{
		get
		{
			return this.model_.selectedRoomIndex;
		}
		set
		{
			this.model_.selectedRoomIndex = value;
			this.Save();
		}
	}

	public List<RoomDecoration.Room> rooms
	{
		get
		{
			if (this.model.rooms == null)
			{
				this.model.rooms = new List<RoomDecoration.Room>();
			}
			return this.model.rooms;
		}
	}

	public RoomsBackend.RoomAccessor GetRoom(string roomName)
	{
		for (int i = 0; i < this.roomAccessors.Count; i++)
		{
			RoomsBackend.RoomAccessor roomAccessor = this.roomAccessors[i];
			if (roomAccessor.room.name == roomName)
			{
				return roomAccessor;
			}
		}
		RoomsBackend.RoomAccessor roomAccessor2 = new RoomsBackend.RoomAccessor(this.GetOrCreateRoomModel(roomName), this);
		this.roomAccessors.Add(roomAccessor2);
		return roomAccessor2;
	}

	private RoomDecoration.Room GetOrCreateRoomModel(string roomName)
	{
		RoomDecoration.Room room = this.GetRoomModel(roomName);
		if (room != null)
		{
			return room;
		}
		room = new RoomDecoration.Room();
		room.name = roomName;
		this.rooms.Add(room);
		this.Save();
		return room;
	}

	public void Save()
	{
		ProtoIO.SaveToFileCS<RoomDecoration>(this.filename, this.model);
	}

	private RoomDecoration.Room GetRoomModel(string roomName)
	{
		List<RoomDecoration.Room> rooms = this.rooms;
		for (int i = 0; i < rooms.Count; i++)
		{
			RoomDecoration.Room room = rooms[i];
			if (room.name == roomName)
			{
				return room;
			}
		}
		return null;
	}

	private void RenewRoomAccessors()
	{
		for (int i = 0; i < this.roomAccessors.Count; i++)
		{
			this.roomAccessors[i].SetNeedsToBeRenewed();
		}
		this.roomAccessors.Clear();
	}

	public void Reset()
	{
		this.model.rooms.Clear();
		this.Save();
		this.RenewRoomAccessors();
	}

	public void ReloadModel()
	{
		if (!ProtoIO.LoadFromFileLocal<RoomDecoration>(this.filename, out this.model_))
		{
			this.model_ = new RoomDecoration();
		}
		this.RenewRoomAccessors();
	}

	public override void Init()
	{
		base.Init();
		this.ReloadModel();
		SingletonInit<FileIOChanges>.instance.OnChange(new FileIOChanges.OnDataChangedDelegate(this.ReloadModel));
	}

	public string filename = "r.bytes";

	private RoomDecoration model_;

	private List<RoomsBackend.RoomAccessor> roomAccessors = new List<RoomsBackend.RoomAccessor>();

	public class RoomAccessor
	{
		public void SetNeedsToBeRenewed()
		{
			this.needsToBeRenewed = true;
			for (int i = 0; i < this.visualObjectAccessors.Count; i++)
			{
				this.visualObjectAccessors[i].needsToBeRenewed = true;
			}
		}

		public bool isPassed
		{
			get
			{
				return this.room.isPassed;
			}
			set
			{
				this.room.isPassed = value;
				this.Save();
			}
		}

		public RoomsBackend.RoomAccessor CreateRenewedAccessor()
		{
			return this.roomsBackend.GetRoom(this.room.name);
		}

		public RoomAccessor(RoomDecoration.Room room, RoomsBackend roomsBackend)
		{
			this.room = room;
			this.roomsBackend = roomsBackend;
		}

		public RoomsBackend.VisualObjectAccessor GetVisualObject(string visualObjectName)
		{
			for (int i = 0; i < this.visualObjectAccessors.Count; i++)
			{
				RoomsBackend.VisualObjectAccessor visualObjectAccessor = this.visualObjectAccessors[i];
				if (visualObjectAccessor.visualObject.name == visualObjectName)
				{
					return visualObjectAccessor;
				}
			}
			RoomsBackend.VisualObjectAccessor visualObjectAccessor2 = new RoomsBackend.VisualObjectAccessor(this.GetOrCreateVisualObjectModel(visualObjectName), this);
			this.visualObjectAccessors.Add(visualObjectAccessor2);
			return visualObjectAccessor2;
		}

		public RoomsBackend.VariantGroupAccessor GetVariantGroup(string variantGroupName)
		{
			for (int i = 0; i < this.variantGroupAccessors.Count; i++)
			{
				RoomsBackend.VariantGroupAccessor variantGroupAccessor = this.variantGroupAccessors[i];
				if (variantGroupAccessor.variantGroup.name == variantGroupName)
				{
					return variantGroupAccessor;
				}
			}
			RoomsBackend.VariantGroupAccessor variantGroupAccessor2 = new RoomsBackend.VariantGroupAccessor(this.GetOrCreateVariantGroupModel(variantGroupName), this);
			this.variantGroupAccessors.Add(variantGroupAccessor2);
			return variantGroupAccessor2;
		}

		private List<RoomDecoration.VisualObject> visualObjects
		{
			get
			{
				if (this.room.visualObjects == null)
				{
					this.room.visualObjects = new List<RoomDecoration.VisualObject>();
				}
				return this.room.visualObjects;
			}
		}

		private RoomDecoration.VisualObject GetOrCreateVisualObjectModel(string visualObjectName)
		{
			RoomDecoration.VisualObject visualObject = this.GetVisualObjectModel(visualObjectName);
			if (visualObject != null)
			{
				return visualObject;
			}
			visualObject = new RoomDecoration.VisualObject();
			visualObject.name = visualObjectName;
			this.visualObjects.Add(visualObject);
			this.Save();
			return visualObject;
		}

		private RoomDecoration.VisualObject GetVisualObjectModel(string visualObjectName)
		{
			List<RoomDecoration.VisualObject> visualObjects = this.visualObjects;
			for (int i = 0; i < visualObjects.Count; i++)
			{
				RoomDecoration.VisualObject visualObject = visualObjects[i];
				if (visualObject.name == visualObjectName)
				{
					return visualObject;
				}
			}
			return null;
		}

		private List<RoomDecoration.VariantGroup> variantGroups
		{
			get
			{
				if (this.room.variantGroups == null)
				{
					this.room.variantGroups = new List<RoomDecoration.VariantGroup>();
				}
				return this.room.variantGroups;
			}
		}

		private RoomDecoration.VariantGroup GetVariantGroupModel(string variantGroupName)
		{
			List<RoomDecoration.VariantGroup> variantGroups = this.variantGroups;
			for (int i = 0; i < variantGroups.Count; i++)
			{
				RoomDecoration.VariantGroup variantGroup = variantGroups[i];
				if (variantGroup.name == variantGroupName)
				{
					return variantGroup;
				}
			}
			return null;
		}

		private RoomDecoration.VariantGroup GetOrCreateVariantGroupModel(string variantGroupName)
		{
			RoomDecoration.VariantGroup variantGroup = this.GetVariantGroupModel(variantGroupName);
			if (variantGroup != null)
			{
				return variantGroup;
			}
			variantGroup = new RoomDecoration.VariantGroup();
			variantGroup.name = variantGroupName;
			this.variantGroups.Add(variantGroup);
			this.Save();
			return variantGroup;
		}

		public bool IsOwned(string visualObjectName)
		{
			RoomDecoration.VisualObject visualObjectModel = this.GetVisualObjectModel(visualObjectName);
			return visualObjectModel != null && visualObjectModel.isOwned;
		}

		public void Save()
		{
			this.roomsBackend.Save();
		}

		public bool needsToBeRenewed;

		public RoomsBackend roomsBackend;

		public RoomDecoration.Room room;

		private List<RoomsBackend.VisualObjectAccessor> visualObjectAccessors = new List<RoomsBackend.VisualObjectAccessor>();

		private List<RoomsBackend.VariantGroupAccessor> variantGroupAccessors = new List<RoomsBackend.VariantGroupAccessor>();
	}

	public class VariantGroupAccessor
	{
		public VariantGroupAccessor(RoomDecoration.VariantGroup variantGroup, RoomsBackend.RoomAccessor roomAccessor)
		{
			this.variantGroup = variantGroup;
			this.roomAccessor = roomAccessor;
		}

		public void Save()
		{
			this.roomAccessor.Save();
		}

		public bool needsToBeRenewed;

		public RoomDecoration.VariantGroup variantGroup;

		public RoomsBackend.RoomAccessor roomAccessor;
	}

	public class VisualObjectAccessor
	{
		public VisualObjectAccessor(RoomDecoration.VisualObject visualObject, RoomsBackend.RoomAccessor roomAccessor)
		{
			this.visualObject = visualObject;
			this.roomAccessor = roomAccessor;
		}

		public void Save()
		{
			this.roomAccessor.Save();
		}

		public bool needsToBeRenewed;

		public RoomDecoration.VisualObject visualObject;

		public RoomsBackend.RoomAccessor roomAccessor;
	}
}
