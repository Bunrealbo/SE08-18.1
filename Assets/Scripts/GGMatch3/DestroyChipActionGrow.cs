using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class DestroyChipActionGrow : BoardAction
	{
		public DestroyChipActionGrow.Settings settings
		{
			get
			{
				return Match3Settings.instance.destroyActionGrowSettings;
			}
		}

		public void Init(Chip chip, Slot slot)
		{
			this.chip = chip;
			this.slot = slot;
			if (this.settings.holdGravityOnChip)
			{
				this.chipLock = this.lockContainer.NewLock();
				this.chipLock.isSlotGravitySuspended = true;
				this.chipLock.LockSlot(slot);
			}
			slot.light.AddLight(this.settings.lightIntensity);
			TransformBehaviour componentBehaviour = chip.GetComponentBehaviour<TransformBehaviour>();
			if (componentBehaviour != null)
			{
				componentBehaviour.SetSortingLayer(this.settings.sortingLayer);
			}
		}

		public override void OnStart(ActionManager manager)
		{
			base.OnStart(manager);
		}

		private IEnumerator DoAnimation()
		{
			return new DestroyChipActionGrow._003CDoAnimation_003Ed__9(0)
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

		private Chip chip;

		private Slot slot;

		private Lock chipLock;

		[Serializable]
		public class Settings
		{
			public SpriteSortingSettings sortingLayer = new SpriteSortingSettings();

			public float fromScale = 2f;

			public float toScale;

			public float fromAlpha = 1f;

			public float toAlpha;

			public float duration;

			public AnimationCurve animationCurve;

			public bool holdGravityOnChip;

			public float holdGravityDuration;

			public float lightIntensity = 0.6f;
		}

		private sealed class _003CDoAnimation_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoAnimation_003Ed__9(int _003C_003E1__state)
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
				DestroyChipActionGrow destroyChipActionGrow = this._003C_003E4__this;
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
					this._003Cduration_003E5__3 = destroyChipActionGrow.settings.duration;
					this._003Ccurve_003E5__4 = destroyChipActionGrow.settings.animationCurve;
					this._003Cboard_003E5__5 = destroyChipActionGrow.slot.game.board;
					this._003CchipBeh_003E5__6 = destroyChipActionGrow.chip.GetComponentBehaviour<TransformBehaviour>();
					this._003CholdGravityDuration_003E5__7 = destroyChipActionGrow.settings.holdGravityDuration;
				}
				if (this._003Ctime_003E5__2 >= this._003Cduration_003E5__3)
				{
					if (destroyChipActionGrow.chipLock != null)
					{
						destroyChipActionGrow.chipLock.UnlockAll();
					}
					destroyChipActionGrow.chip.RemoveFromGame();
					destroyChipActionGrow.isAlive = false;
					return false;
				}
				this._003Ctime_003E5__2 += this._003Cboard_003E5__5.currentDeltaTime;
				if (destroyChipActionGrow.chipLock != null && this._003Ctime_003E5__2 >= this._003CholdGravityDuration_003E5__7)
				{
					destroyChipActionGrow.chipLock.UnlockAll();
					destroyChipActionGrow.chipLock = null;
				}
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
				if (this._003Ccurve_003E5__4 != null)
				{
					num2 = this._003Ccurve_003E5__4.Evaluate(num2);
				}
				float num3 = Mathf.LerpUnclamped(destroyChipActionGrow.settings.fromScale, destroyChipActionGrow.settings.toScale, num2);
				float alpha = Mathf.Lerp(destroyChipActionGrow.settings.fromAlpha, destroyChipActionGrow.settings.toAlpha, num2);
				if (this._003CchipBeh_003E5__6 != null)
				{
					this._003CchipBeh_003E5__6.localScale = new Vector3(num3, num3, 1f);
					this._003CchipBeh_003E5__6.SetAlpha(alpha);
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

			public DestroyChipActionGrow _003C_003E4__this;

			private float _003Ctime_003E5__2;

			private float _003Cduration_003E5__3;

			private AnimationCurve _003Ccurve_003E5__4;

			private Match3Board _003Cboard_003E5__5;

			private TransformBehaviour _003CchipBeh_003E5__6;

			private float _003CholdGravityDuration_003E5__7;
		}
	}
}
