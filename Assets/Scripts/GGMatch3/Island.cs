using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class Island
	{
		public bool isCreatingPowerup
		{
			get
			{
				return this.isDiscoBall || this.isBomb || this.isRocket || this.isSeakingMissle;
			}
		}

		public ChipType powerupToCreate
		{
			get
			{
				if (this.isDiscoBall)
				{
					return ChipType.DiscoBall;
				}
				if (this.isBomb)
				{
					return ChipType.Bomb;
				}
				if (this.isRocket)
				{
					if (this.isHorizontalRocket)
					{
						return ChipType.HorizontalRocket;
					}
					return ChipType.VerticalRocket;
				}
				else
				{
					if (this.isSeakingMissle)
					{
						return ChipType.SeekingMissle;
					}
					return ChipType.Unknown;
				}
			}
		}

		public bool isDiscoBall
		{
			get
			{
				for (int i = 0; i < this.connectionsList.Count; i++)
				{
					Connection connection = this.connectionsList[i];
					if (connection.type != Connection.ConnectionType.Square && connection.slotsList.Count >= 5)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool isBomb
		{
			get
			{
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < this.connectionsList.Count; i++)
				{
					Connection connection = this.connectionsList[i];
					if (connection.type != Connection.ConnectionType.Square)
					{
						if (connection.type == Connection.ConnectionType.Horizontal)
						{
							num = Mathf.Max(num, connection.slotsList.Count);
						}
						if (connection.type == Connection.ConnectionType.Vertical)
						{
							num2 = Mathf.Max(num2, connection.slotsList.Count);
						}
					}
				}
				return num >= 3 && num2 >= 3;
			}
		}

		public bool isRocket
		{
			get
			{
				for (int i = 0; i < this.connectionsList.Count; i++)
				{
					Connection connection = this.connectionsList[i];
					if (connection.type != Connection.ConnectionType.Square && connection.slotsList.Count >= 4)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool isHorizontalRocket
		{
			get
			{
				for (int i = 0; i < this.connectionsList.Count; i++)
				{
					Connection connection = this.connectionsList[i];
					if (connection.type == Connection.ConnectionType.Vertical && connection.slotsList.Count >= 4)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool isSeakingMissle
		{
			get
			{
				for (int i = 0; i < this.connectionsList.Count; i++)
				{
					if (this.connectionsList[i].type == Connection.ConnectionType.Square)
					{
						return true;
					}
				}
				return false;
			}
		}

		public List<Slot> allSlots
		{
			get
			{
				this.slotsList.Clear();
				for (int i = 0; i < this.connectionsList.Count; i++)
				{
					Connection connection = this.connectionsList[i];
					for (int j = 0; j < connection.slotsList.Count; j++)
					{
						Slot item = connection.slotsList[j];
						if (!this.slotsList.Contains(item))
						{
							this.slotsList.Add(item);
						}
					}
				}
				return this.slotsList;
			}
		}

		public bool ContainsPosition(IntVector2 position)
		{
			for (int i = 0; i < this.connectionsList.Count; i++)
			{
				if (this.connectionsList[i].ContainsPosition(position))
				{
					return true;
				}
			}
			return false;
		}

		public void Clear()
		{
			this.connectionsList.Clear();
			this.isFromSwap = false;
		}

		public Connection squareConnection
		{
			get
			{
				for (int i = 0; i < this.connectionsList.Count; i++)
				{
					Connection connection = this.connectionsList[i];
					if (connection.type == Connection.ConnectionType.Square)
					{
						return connection;
					}
				}
				return null;
			}
		}

		public void AddConnection(List<Connection> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Connection c = list[i];
				this.AddConnection(c);
			}
		}

		public void AddConnection(Connection c)
		{
			if (c.type != Connection.ConnectionType.Square)
			{
				this.connectionsList.Add(c);
				return;
			}
			Connection squareConnection = this.squareConnection;
			if (squareConnection == null)
			{
				this.connectionsList.Add(c);
				return;
			}
			if (squareConnection.IsIntersecting(c))
			{
				return;
			}
			this.connectionsList.Add(c);
		}

		public List<Connection> connectionsList = new List<Connection>();

		private List<Slot> slotsList = new List<Slot>();

		public bool isFromSwap;
	}
}
