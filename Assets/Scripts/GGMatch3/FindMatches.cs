using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class FindMatches
	{
		public IntVector2 size
		{
			get
			{
				return this.board.size;
			}
		}

		public void Init(Match3Board board)
		{
			this.board = board;
			this.matches = new Matches();
			this.matches.Init(board);
		}

		private Slot GetSlot(IntVector2 pos)
		{
			return this.board.GetSlot(pos);
		}

		private bool CanParticipateInMatch(Slot slot)
		{
			if (slot == null)
			{
				return false;
			}
			Chip slotComponent = slot.GetSlotComponent<Chip>();
			return slotComponent != null && slotComponent.canFormColorMatches && !slot.isSlotMatchingSuspended;
		}

		private void AddSlotToLineConnectionIfPossible(Connection currentConnection, int x, int y, bool isLast, Matches matches)
		{
			Slot slot = this.GetSlot(new IntVector2(x, y));
			Chip chip = null;
			if (slot != null)
			{
				chip = slot.GetSlotComponent<Chip>();
			}
			bool flag = this.CanParticipateInMatch(slot);
			if (slot == null || chip == null || slot.isSlotMatchingSuspended || !currentConnection.IsChipAcceptable(chip))
			{
				if (currentConnection.isUsable)
				{
					matches.AddCopyOfConnection(currentConnection);
				}
				currentConnection.Clear();
			}
			if (flag)
			{
				currentConnection.slotsList.Add(slot);
			}
			if (isLast && currentConnection.isUsable)
			{
				matches.AddCopyOfConnection(currentConnection);
			}
			if (isLast)
			{
				currentConnection.Clear();
			}
		}

		public Matches FindAllMatches()
		{
			this.matches.Clear();
			this.currentConnection.Clear();
			for (int i = 0; i < this.size.y; i++)
			{
				this.currentConnection.Clear();
				this.currentConnection.type = Connection.ConnectionType.Horizontal;
				for (int j = 0; j < this.size.x; j++)
				{
					bool isLast = j >= this.size.x - 1;
					this.AddSlotToLineConnectionIfPossible(this.currentConnection, j, i, isLast, this.matches);
				}
			}
			for (int k = 0; k < this.size.x; k++)
			{
				this.currentConnection.Clear();
				this.currentConnection.type = Connection.ConnectionType.Vertical;
				for (int l = 0; l < this.size.y; l++)
				{
					bool isLast2 = l >= this.size.y - 1;
					this.AddSlotToLineConnectionIfPossible(this.currentConnection, k, l, isLast2, this.matches);
				}
			}
			List<Slot> list = new List<Slot>();
			for (int m = 0; m < this.size.x - 1; m++)
			{
				for (int n = 0; n < this.size.y - 1; n++)
				{
					Slot slot = this.GetSlot(new IntVector2(m, n));
					Slot slot2 = this.GetSlot(new IntVector2(m + 1, n));
					Slot slot3 = this.GetSlot(new IntVector2(m, n + 1));
					Slot slot4 = this.GetSlot(new IntVector2(m + 1, n + 1));
					list.Clear();
					list.Add(slot);
					list.Add(slot2);
					list.Add(slot3);
					list.Add(slot4);
					bool flag = true;
					ItemColor itemColor = ItemColor.Unknown;
					for (int num = 0; num < list.Count; num++)
					{
						Slot slot5 = list[num];
						if (slot5 == null)
						{
							flag = false;
							break;
						}
						if (!this.CanParticipateInMatch(slot5))
						{
							flag = false;
							break;
						}
						Chip slotComponent = slot5.GetSlotComponent<Chip>();
						if (slotComponent == null)
						{
							flag = false;
							break;
						}
						if (itemColor == ItemColor.Unknown)
						{
							itemColor = slotComponent.itemColor;
						}
						if (itemColor != slotComponent.itemColor)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						Connection connection = this.currentConnection;
						connection.Clear();
						connection.type = Connection.ConnectionType.Square;
						connection.slotsList.AddRange(list);
						this.matches.AddCopyOfConnection(connection);
					}
				}
			}
			this.connectedIslands.Clear();
			for (int num2 = 0; num2 < this.matches.connectionsList.Count; num2++)
			{
				Connection connection2 = this.matches.connectionsList[num2];
				this.connectedIslands.Clear();
				for (int num3 = 0; num3 < connection2.slotsList.Count; num3++)
				{
					Slot slot6 = connection2.slotsList[num3];
					Island island = this.matches.GetIsland(slot6.position);
					if (island != null && !this.connectedIslands.Contains(island))
					{
						this.connectedIslands.Add(island);
					}
				}
				if (this.connectedIslands.Count == 0)
				{
					Island island2 = this.matches.NextIslandFromPool();
					island2.connectionsList.Add(connection2);
					this.matches.AddIsland(island2);
					this.matches.UpdateIslandOnMap(island2);
				}
				else
				{
					Island island3 = this.connectedIslands[0];
					for (int num4 = 1; num4 < this.connectedIslands.Count; num4++)
					{
						Island island4 = this.connectedIslands[num4];
						island3.AddConnection(island4.connectionsList);
						this.matches.RemoveIsland(island4);
						this.matches.RemoveIslandOnMap(island4);
					}
					island3.AddConnection(connection2);
					this.matches.UpdateIslandOnMap(island3);
				}
			}
			return this.matches;
		}

		public Match3Board board;

		public Matches matches;

		private Connection currentConnection = new Connection();

		private List<Island> connectedIslands = new List<Island>();
	}
}
