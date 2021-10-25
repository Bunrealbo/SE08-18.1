using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class DestroyAfterAction : BoardAction
	{
		public void Init(DestroyAfterAction.InitArguments initArguments)
		{
			this.globalLock = this.lockContainer.NewLock();
			this.initArguments = initArguments;
			this.globalLock.SuspendAll();
			this.globalLock.LockSlot(initArguments.slot);
			this.enumerator = this.DoAnimate();
		}

		private IEnumerator DoAnimate()
		{
			return new DestroyAfterAction._003CDoAnimate_003Ed__6(0)
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

		private DestroyAfterAction.InitArguments initArguments;

		private float deltaTime;

		private IEnumerator enumerator;

		public struct InitArguments
		{
			public Match3Game game;

			public SlotDestroyParams destroyParams;

			public Slot slot;

			public Chip chip;

			public float delay;
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
				DestroyAfterAction destroyAfterAction = this._003C_003E4__this;
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
					this._003Csettings_003E5__2 = Match3Settings.instance.destroyAfterSettings;
					this._003Ctime_003E5__3 = 0f;
					this._003Cduration_003E5__4 = UnityEngine.Random.Range(this._003Csettings_003E5__2.dulationToHold, this._003Csettings_003E5__2.durationToHoldMax);
					if (destroyAfterAction.initArguments.delay > 0f)
					{
						this._003Cduration_003E5__4 = destroyAfterAction.initArguments.delay;
					}
					this._003Cchip_003E5__5 = destroyAfterAction.initArguments.chip;
				}
				if (this._003Ctime_003E5__3 >= this._003Cduration_003E5__4)
				{
					if (this._003Csettings_003E5__2.useBlink)
					{
						TransformBehaviour componentBehaviour = this._003Cchip_003E5__5.GetComponentBehaviour<TransformBehaviour>();
						if (componentBehaviour != null)
						{
							componentBehaviour.SetBrightness(1f);
						}
					}
					destroyAfterAction.globalLock.UnlockAll();
					destroyAfterAction.initArguments.chip.DestroyBomb(destroyAfterAction.initArguments.destroyParams);
					destroyAfterAction.isAlive = false;
					return false;
				}
				this._003Ctime_003E5__3 += Time.deltaTime;
				if (this._003Csettings_003E5__2.useBlink)
				{
					float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__4, this._003Ctime_003E5__3);
					float t = this._003Csettings_003E5__2.brightnessCurve.Evaluate(time);
					float brightness = Mathf.LerpUnclamped(1f, this._003Csettings_003E5__2.brightness, t);
					TransformBehaviour componentBehaviour2 = this._003Cchip_003E5__5.GetComponentBehaviour<TransformBehaviour>();
					if (componentBehaviour2 != null)
					{
						componentBehaviour2.SetBrightness(brightness);
					}
				}
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

			public DestroyAfterAction _003C_003E4__this;

			private DestroyMatchingIslandBlinkAction.Settings _003Csettings_003E5__2;

			private float _003Ctime_003E5__3;

			private float _003Cduration_003E5__4;

			private Chip _003Cchip_003E5__5;
		}
	}
}
