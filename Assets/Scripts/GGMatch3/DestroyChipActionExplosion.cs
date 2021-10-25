using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class DestroyChipActionExplosion : BoardAction
	{
		public DestroyChipActionExplosion.Settings settings
		{
			get
			{
				return Match3Settings.instance.destroyChipActionExplosionSettings;
			}
		}

		public void Init(Chip chip, Slot slot, IntVector2 bombOriginPostion, SlotDestroyParams slotDestroyParams)
		{
			this.bombOriginPostion = bombOriginPostion;
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
			Vector3 a = chip.lastConnectedSlot.localPositionOfCenter;
			if (componentBehaviour != null)
			{
				componentBehaviour.SetSortingLayer(this.settings.sorting);
				a = componentBehaviour.localPosition;
			}
			this.direction = a - slot.game.LocalPositionOfCenter(bombOriginPostion);
			this.direction.z = 0f;
			this.direction.Normalize();
			this.startPosition = a;
			this.endPosition = this.startPosition + this.direction * this.settings.distance;
			Match3Game game = slot.game;
			if (this.settings.useParticles)
			{
				if (slotDestroyParams.bombType == ChipType.DiscoBall)
				{
					game.particles.CreateParticles(slot, Match3Particles.PositionType.OnDestroyChipDiscoBomb);
					return;
				}
				game.particles.CreateParticles(chip, Match3Particles.PositionType.OnDestroyChipExplosion, chip.chipType, chip.itemColor);
			}
		}

		public override void OnStart(ActionManager manager)
		{
			base.OnStart(manager);
		}

		private IEnumerator DoAnimation()
		{
			return new DestroyChipActionExplosion._003CDoAnimation_003Ed__13(0)
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

		private IntVector2 bombOriginPostion;

		private Vector3 direction;

		private Vector3 startPosition;

		private Vector3 endPosition;

		[Serializable]
		public class Settings
		{
			public bool useParticles;

			public float fromScale = 2f;

			public float duration;

			public float distance = 10f;

			public AnimationCurve animationCurve;

			public bool useScaleCurve;

			public AnimationCurve scaleCurve;

			public bool holdGravityOnChip;

			public float holdGravityDuration;

			public float lightIntensity = 0.6f;

			public SpriteSortingSettings sorting = new SpriteSortingSettings();
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
				DestroyChipActionExplosion destroyChipActionExplosion = this._003C_003E4__this;
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
					this._003Cduration_003E5__3 = destroyChipActionExplosion.settings.duration;
					this._003Ccurve_003E5__4 = destroyChipActionExplosion.settings.animationCurve;
					this._003Cboard_003E5__5 = destroyChipActionExplosion.slot.game.board;
					this._003CchipTransform_003E5__6 = destroyChipActionExplosion.chip.GetComponentBehaviour<TransformBehaviour>();
					this._003CholdGravityDuration_003E5__7 = destroyChipActionExplosion.settings.holdGravityDuration;
				}
				if (this._003Ctime_003E5__2 >= this._003Cduration_003E5__3)
				{
					if (destroyChipActionExplosion.chipLock != null)
					{
						destroyChipActionExplosion.chipLock.UnlockAll();
					}
					destroyChipActionExplosion.chip.RemoveFromGame();
					destroyChipActionExplosion.isAlive = false;
					return false;
				}
				this._003Ctime_003E5__2 += this._003Cboard_003E5__5.currentDeltaTime;
				if (destroyChipActionExplosion.chipLock != null && this._003Ctime_003E5__2 >= this._003CholdGravityDuration_003E5__7)
				{
					destroyChipActionExplosion.chipLock.UnlockAll();
					destroyChipActionExplosion.chipLock = null;
				}
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
				float num3 = num2;
				if (this._003Ccurve_003E5__4 != null)
				{
					num3 = this._003Ccurve_003E5__4.Evaluate(num2);
				}
				float t = num3;
				if (destroyChipActionExplosion.settings.useScaleCurve)
				{
					t = destroyChipActionExplosion.settings.scaleCurve.Evaluate(num2);
				}
				float num4 = Mathf.LerpUnclamped(destroyChipActionExplosion.settings.fromScale, 0f, t);
				if (this._003CchipTransform_003E5__6 != null)
				{
					this._003CchipTransform_003E5__6.scalerTransform.localScale = new Vector3(num4, num4, 1f);
					this._003CchipTransform_003E5__6.localPosition = Vector3.LerpUnclamped(destroyChipActionExplosion.startPosition, destroyChipActionExplosion.endPosition, num3);
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

			public DestroyChipActionExplosion _003C_003E4__this;

			private float _003Ctime_003E5__2;

			private float _003Cduration_003E5__3;

			private AnimationCurve _003Ccurve_003E5__4;

			private Match3Board _003Cboard_003E5__5;

			private TransformBehaviour _003CchipTransform_003E5__6;

			private float _003CholdGravityDuration_003E5__7;
		}
	}
}
