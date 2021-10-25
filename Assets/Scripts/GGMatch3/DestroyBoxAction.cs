using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class DestroyBoxAction : BoardAction
	{
		public DestroyBoxAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.destroyBoxActionSettings;
			}
		}

		private SlotComponent chip
		{
			get
			{
				return this.initArguments.chip;
			}
		}

		private Slot slot
		{
			get
			{
				return this.initArguments.slot;
			}
		}

		public void Init(DestroyBoxAction.InitArguments initArguments)
		{
			this.initArguments = initArguments;
			if (this.settings.holdGravityOnChip)
			{
				this.chipLock = this.lockContainer.NewLock();
				this.chipLock.isSlotGravitySuspended = true;
				this.chipLock.LockSlot(this.slot);
			}
		}

		public override void OnStart(ActionManager manager)
		{
			base.OnStart(manager);
		}

		private IEnumerator DoAnimation()
		{
			return new DestroyBoxAction._003CDoAnimation_003Ed__13(0)
			{
				_003C_003E4__this = this
			};
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			if (!this.isAlive)
			{
				return;
			}
			if (this.animation == null)
			{
				this.animation = this.DoAnimation();
			}
			this.animation.MoveNext();
		}

		private IEnumerator animation;

		private Lock chipLock;

		private DestroyBoxAction.InitArguments initArguments;

		[Serializable]
		public class Settings
		{
			public float fromScale = 2f;

			public float toScale = 1f;

			public float duration;

			public AnimationCurve animationCurve;

			public float fromAlpha = 1f;

			public float toAlpha = 1f;

			public bool holdGravityOnChip;

			public float holdGravityDuration;
		}

		public struct InitArguments
		{
			public SlotComponent chip;

			public Slot slot;
		}

		private sealed class _003CDoAnimation_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoAnimation_003Ed__13(int _003C_003E1__state)
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
				DestroyBoxAction destroyBoxAction = this._003C_003E4__this;
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
					this._003Ctime_003E5__2 = 0f;
					this._003Cduration_003E5__3 = destroyBoxAction.settings.duration;
					this._003Ccurve_003E5__4 = destroyBoxAction.settings.animationCurve;
					this._003Cboard_003E5__5 = destroyBoxAction.slot.game.board;
					this._003CchipTransform_003E5__6 = destroyBoxAction.chip.GetComponentBehaviour<TransformBehaviour>();
					Match3Game game = destroyBoxAction.slot.game;
					int level = 0;
					game.particles.CreateParticles(destroyBoxAction.slot.localPositionOfCenter, Match3Particles.PositionType.BoxDestroy, ChipType.Box, level);
					this._003CholdGravityDuration_003E5__7 = destroyBoxAction.settings.holdGravityDuration;
				}
				if (this._003Ctime_003E5__2 >= this._003Cduration_003E5__3)
				{
					if (destroyBoxAction.chipLock != null)
					{
						destroyBoxAction.chipLock.UnlockAll();
					}
					destroyBoxAction.chip.RemoveFromGame();
					destroyBoxAction.isAlive = false;
					return false;
				}
				this._003Ctime_003E5__2 += this._003Cboard_003E5__5.currentDeltaTime;
				if (destroyBoxAction.chipLock != null && this._003Ctime_003E5__2 >= this._003CholdGravityDuration_003E5__7)
				{
					destroyBoxAction.chipLock.UnlockAll();
					destroyBoxAction.chipLock = null;
				}
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
				if (this._003Ccurve_003E5__4 != null)
				{
					num2 = this._003Ccurve_003E5__4.Evaluate(num2);
				}
				float num3 = Mathf.LerpUnclamped(destroyBoxAction.settings.fromScale, destroyBoxAction.settings.toScale, num2);
				float alpha = Mathf.Lerp(destroyBoxAction.settings.fromAlpha, destroyBoxAction.settings.toAlpha, num2);
				if (this._003CchipTransform_003E5__6 != null)
				{
					this._003CchipTransform_003E5__6.localScale = new Vector3(num3, num3, 1f);
					this._003CchipTransform_003E5__6.SetAlpha(alpha);
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

			public DestroyBoxAction _003C_003E4__this;

			private float _003Ctime_003E5__2;

			private float _003Cduration_003E5__3;

			private AnimationCurve _003Ccurve_003E5__4;

			private Match3Board _003Cboard_003E5__5;

			private TransformBehaviour _003CchipTransform_003E5__6;

			private float _003CholdGravityDuration_003E5__7;
		}
	}
}
