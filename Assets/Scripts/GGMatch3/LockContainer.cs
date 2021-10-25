using System;
using System.Collections.Generic;

namespace GGMatch3
{
	public class LockContainer
	{
		public Lock NewLock()
		{
			Lock @lock = new Lock();
			this.locks.Add(@lock);
			return @lock;
		}

		public void UnlockAllAndSaveToTemporaryList()
		{
			for (int i = 0; i < this.locks.Count; i++)
			{
				this.locks[i].UnlockAllAndSaveToTemporaryList();
			}
		}

		public void LockTemporaryListAndClear()
		{
			for (int i = 0; i < this.locks.Count; i++)
			{
				this.locks[i].LockTemporaryListAndClear();
			}
		}

		public void UnlockAll()
		{
			for (int i = 0; i < this.locks.Count; i++)
			{
				this.locks[i].UnlockAll();
			}
		}

		public List<Lock> locks = new List<Lock>();
	}
}
