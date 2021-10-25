using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GGMatch3
{
	public class ComboSeekingMissileAction : BoardAction
	{
		public void Init(ComboSeekingMissileAction.Parameters parameters)
		{
			this.parameters = parameters;
			this.sourceLock = this.lockContainer.NewLock();
			this.sourceLock.isSlotGravitySuspended = true;
			this.sourceLock.isChipGeneratorSuspended = true;
			this.sourceLock.LockSlot(parameters.startSlot);
		}

		public override void OnUpdate(float deltaTime)
		{
			if (this.shootRocketsCoroutine == null)
			{
				this.shootRocketsCoroutine = this.DoShootRockets();
			}
			this.shootRocketsCoroutine.MoveNext();
		}

		public IEnumerator DoShootRockets()
		{
			return new ComboSeekingMissileAction._003CDoShootRockets_003Ed__7(0)
			{
				_003C_003E4__this = this
			};
		}

		private ComboSeekingMissileAction.Parameters parameters;

		private Lock sourceLock;

		private IEnumerator shootRocketsCoroutine;

		[Serializable]
		public class Settings
		{
			public float delay = 0.4f;
		}

		public class Parameters
		{
			public int rocketsCount;

			public Match3Game game;

			public Slot startSlot;

			public bool isHavingCarpet;
		}

		private sealed class _003CDoShootRockets_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoShootRockets_003Ed__7(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			bool IEnumerator.MoveNext()
			{
				int num = this._003C_003E1__state;
				ComboSeekingMissileAction comboSeekingMissileAction = this._003C_003E4__this;
				if (num == 0)
				{
					this._003C_003E1__state = -1;
					comboSeekingMissileAction.sourceLock.UnlockAll();
					this._003Ci_003E5__2 = 0;
					goto IL_161;
				}
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
				IL_13A:
				if (this._003Ctime_003E5__5 < this._003Cdelay_003E5__4)
				{
					this._003Ctime_003E5__5 += this._003CseekingMissileParameters_003E5__3.game.board.currentDeltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				IL_148:
				this._003CseekingMissileParameters_003E5__3 = null;
				int num2 = this._003Ci_003E5__2;
				this._003Ci_003E5__2 = num2 + 1;
				IL_161:
				if (this._003Ci_003E5__2 >= comboSeekingMissileAction.parameters.rocketsCount)
				{
					comboSeekingMissileAction.isAlive = false;
					return false;
				}
				bool flag = this._003Ci_003E5__2 == comboSeekingMissileAction.parameters.rocketsCount - 1;
				bool doCrossExplosion = this._003Ci_003E5__2 == 0;
				SeekingMissileAction seekingMissileAction = new SeekingMissileAction();
				this._003CseekingMissileParameters_003E5__3 = new SeekingMissileAction.Parameters();
				this._003CseekingMissileParameters_003E5__3.doCrossExplosion = doCrossExplosion;
				this._003CseekingMissileParameters_003E5__3.startSlot = comboSeekingMissileAction.parameters.startSlot;
				this._003CseekingMissileParameters_003E5__3.game = comboSeekingMissileAction.parameters.game;
				this._003CseekingMissileParameters_003E5__3.isHavingCarpet = comboSeekingMissileAction.parameters.isHavingCarpet;
				seekingMissileAction.Init(this._003CseekingMissileParameters_003E5__3);
				comboSeekingMissileAction.parameters.game.board.actionManager.AddAction(seekingMissileAction);
				if (!flag)
				{
					this._003Cdelay_003E5__4 = Match3Settings.instance.seekingMissileComboSettings.delay;
					this._003Ctime_003E5__5 = 0f;
					goto IL_13A;
				}
				goto IL_148;
			}

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this._003C_003E2__current;
				}
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this._003C_003E2__current;
				}
			}

			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ComboSeekingMissileAction _003C_003E4__this;

			private int _003Ci_003E5__2;

			private SeekingMissileAction.Parameters _003CseekingMissileParameters_003E5__3;

			private float _003Cdelay_003E5__4;

			private float _003Ctime_003E5__5;
		}
	}
}
