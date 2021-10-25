using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class DestroyMatchingIslandAction : BoardAction
	{
		public void Init(DestroyMatchingIslandAction.InitArguments initArguments)
		{
			this.globalLock = this.lockContainer.NewLock();
			this.initArguments = initArguments;
			this.globalLock.SuspendAll();
			this.globalLock.LockSlots(initArguments.allSlots);
			this.enumerator = this.DoAnimate();
		}

		private IEnumerator DoAnimate()
		{
			return new DestroyMatchingIslandAction._003CDoAnimate_003Ed__6(0)
			{
				_003C_003E4__this = this
			};
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.deltaTime = deltaTime;
			if (this.enumerator != null && !this.enumerator.MoveNext())
			{
				this.enumerator = null;
			}
		}

		private Lock globalLock;

		private DestroyMatchingIslandAction.InitArguments initArguments;

		private float deltaTime;

		private IEnumerator enumerator;

		public struct InitArguments
		{
			public Match3Game game;

			public List<Slot> allSlots;

			public SlotDestroyParams slotDestroyParams;

			public Slot slotWherePowerupIsCreated;

			public ChipType powerupToCreate;

			public BoltCollection boltCollection;

			public int matchComboIndex;
		}

		private sealed class _003CDoAnimate_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoAnimate_003Ed__6(int _003C_003E1__state)
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
				DestroyMatchingIslandAction destroyMatchingIslandAction = this._003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					this._003C_003E1__state = -1;
				}
				else
				{
					this._003C_003E1__state = -1;
					MatchChipAffector.InitArguments initArguments = new MatchChipAffector.InitArguments();
					initArguments.game = destroyMatchingIslandAction.initArguments.game;
					initArguments.matchingSlots.AddRange(destroyMatchingIslandAction.initArguments.allSlots);
					if (destroyMatchingIslandAction.initArguments.slotWherePowerupIsCreated != null)
					{
						initArguments.originSlot = destroyMatchingIslandAction.initArguments.slotWherePowerupIsCreated;
					}
					else
					{
						initArguments.originSlot = destroyMatchingIslandAction.initArguments.allSlots[0];
					}
					initArguments.ignoreSlots.AddRange(destroyMatchingIslandAction.initArguments.allSlots);
					this._003CchipAffector_003E5__2 = new MatchChipAffector();
					this._003CchipAffector_003E5__2.Init(initArguments);
					this._003Ctime_003E5__3 = 0f;
					this._003Cduration_003E5__4 = Match3Settings.instance.swipeAffectorSettings.minAffectorDuration;
					if (Match3Settings.instance.swipeAffectorSettings.useAutoMatchDuration)
					{
						this._003Cduration_003E5__4 = Mathf.Max(this._003Cduration_003E5__4, Match3Settings.instance.swipeAffectorSettings.autoMatchDuration);
					}
				}
				if (this._003Ctime_003E5__3 >= this._003Cduration_003E5__4)
				{
					this._003CchipAffector_003E5__2.GiveLightingBoltsTo(destroyMatchingIslandAction.initArguments.boltCollection.bolts);
					this._003CchipAffector_003E5__2.OnAfterDestroy();
					this._003CchipAffector_003E5__2.Clear();
					destroyMatchingIslandAction.globalLock.UnlockAll();
					destroyMatchingIslandAction.initArguments.game.FinishDestroySlots(destroyMatchingIslandAction.initArguments);
					destroyMatchingIslandAction.isAlive = false;
					return false;
				}
				this._003Ctime_003E5__3 += Time.deltaTime;
				this._003CchipAffector_003E5__2.Update();
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
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

			public DestroyMatchingIslandAction _003C_003E4__this;

			private MatchChipAffector _003CchipAffector_003E5__2;

			private float _003Ctime_003E5__3;

			private float _003Cduration_003E5__4;
		}
	}
}
