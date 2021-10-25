using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class Matches
	{
		public void Init(Match3Board board)
		{
			this.board = board;
			this.islandsMap = new Island[board.size.x * board.size.y];
		}

		public void Clear()
		{
			for (int i = 0; i < this.islandsMap.Length; i++)
			{
				this.islandsMap[i] = null;
			}
			for (int j = 0; j < this.connectionsList.Count; j++)
			{
				Connection item = this.connectionsList[j];
				this.connectionPool.Add(item);
			}
			this.connectionsList.Clear();
			for (int k = 0; k < this.islands.Count; k++)
			{
				Island item2 = this.islands[k];
				this.islandPool.Add(item2);
			}
			this.islands.Clear();
		}

		public Island GetIsland(IntVector2 position)
		{
			int num = this.board.Index(position);
			if (num < 0 || num >= this.islandsMap.Length)
			{
				return null;
			}
			return this.islandsMap[num];
		}

		public void SetIsland(IntVector2 position, Island island)
		{
			int num = this.board.Index(position);
			if (num < 0 || num >= this.islandsMap.Length)
			{
				return;
			}
			this.islandsMap[num] = island;
		}

		private Connection NextConnectionFromPool()
		{
			if (this.connectionPool.Count > 0)
			{
				int index = this.connectionPool.Count - 1;
				Connection result = this.connectionPool[index];
				this.connectionPool.RemoveAt(index);
				return result;
			}
			return new Connection();
		}

		public Island NextIslandFromPool()
		{
			if (this.islandPool.Count > 0)
			{
				int index = this.islandPool.Count - 1;
				Island island = this.islandPool[index];
				this.islandPool.RemoveAt(index);
				island.Clear();
				return island;
			}
			return new Island();
		}

		public int MatchesCount
		{
			get
			{
				return this.islands.Count;
			}
		}

		public void AddCopyOfConnection(Connection c)
		{
			Connection connection = this.NextConnectionFromPool();
			connection.CopyFrom(c);
			this.connectionsList.Add(connection);
		}

		public void UpdateIslandOnMap(Island island)
		{
			for (int i = 0; i < island.connectionsList.Count; i++)
			{
				Connection connection = island.connectionsList[i];
				for (int j = 0; j < connection.slotsList.Count; j++)
				{
					Slot slot = connection.slotsList[j];
					this.SetIsland(slot.position, island);
				}
			}
		}

		public void RemoveIslandOnMap(Island island)
		{
			for (int i = 0; i < island.connectionsList.Count; i++)
			{
				Connection connection = island.connectionsList[i];
				for (int j = 0; j < connection.slotsList.Count; j++)
				{
					Slot slot = connection.slotsList[j];
					if (this.GetIsland(slot.position) == island)
					{
						this.SetIsland(slot.position, null);
					}
				}
			}
		}

		public void RemoveIsland(Island island)
		{
			this.islandPool.Add(island);
			this.islands.Remove(island);
		}

		public void AddIsland(Island island)
		{
			this.islands.Add(island);
		}

		private Island[] islandsMap;

		private Match3Board board;

		public List<Connection> connectionsList = new List<Connection>();

		private List<Connection> connectionPool = new List<Connection>();

		private List<Island> islandPool = new List<Island>();

		public List<Island> islands = new List<Island>();
	}
}
