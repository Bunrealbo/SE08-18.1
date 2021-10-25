using System;

namespace GGMatch3
{
	public class BoardAction
	{
		public virtual bool isAlive
		{
			get
			{
				return this._003CisAlive_003Ek__BackingField;
			}
			protected set
			{
				this._003CisAlive_003Ek__BackingField = value;
			}
		}

		public virtual bool isStarted
		{
			get
			{
				return this._003CisStarted_003Ek__BackingField;
			}
			protected set
			{
				this._003CisStarted_003Ek__BackingField = value;
			}
		}

		public virtual void Reset()
		{
			this.isStarted = false;
		}

		public virtual void OnStart(ActionManager manager)
		{
			this.isAlive = true;
			this.isStarted = true;
		}

		public virtual void Stop()
		{
		}

		public virtual void OnUpdate(float deltaTime)
		{
		}

		public LockContainer lockContainer = new LockContainer();

		private bool _003CisAlive_003Ek__BackingField;

		private bool _003CisStarted_003Ek__BackingField;
	}
}
