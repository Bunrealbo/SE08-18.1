using System;
using System.Collections.Generic;
using GGMatch3;

public class ChipAffectorBase
{
	protected Lock globalLock
	{
		get
		{
			if (this.globalLock_ == null)
			{
				this.globalLock_ = this.lockContainer.NewLock();
			}
			return this.globalLock_;
		}
	}

	public virtual bool canFinish
	{
		get
		{
			return true;
		}
	}

	public virtual void ReleaseLocks()
	{
		this.lockContainer.UnlockAllAndSaveToTemporaryList();
	}

	public virtual void ApplyLocks()
	{
		this.lockContainer.LockTemporaryListAndClear();
	}

	public virtual void Clear()
	{
		this.lockContainer.UnlockAll();
	}

	public virtual void Update()
	{
	}

	public virtual void GiveLightingBoltsTo(List<LightingBolt> destinationBolts)
	{
	}

	public virtual void AddToInputAffectorExport(Match3Game.InputAffectorExport inputAffector)
	{
	}

	public virtual void OnAfterDestroy()
	{
	}

	protected LockContainer lockContainer = new LockContainer();

	private Lock globalLock_;
}
