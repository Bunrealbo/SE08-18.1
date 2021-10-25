using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class DestroyChipAction : BoardAction
	{
		public DestroyChipAction.Settings settings
		{
			get
			{
				DestroyChipAction.Settings settings = Match3Settings.instance.destroyActionSettings;
				if (settings.useRocketDestroySettings && this.initArguments.isFromRocket)
				{
					settings = Match3Settings.instance.destroyActionSettingsRocket;
				}
				return settings;
			}
		}

		public void Init(DestroyChipAction.InitArguments initArguments)
		{
			this.initArguments = initArguments;
			this.chip = initArguments.chip;
			this.slot = initArguments.slot;
			if (this.settings.holdGravityOnChip)
			{
				this.chipLock = this.lockContainer.NewLock();
				this.chipLock.isSlotGravitySuspended = true;
				this.chipLock.LockSlot(this.slot);
			}
			this.slot.light.AddLight(this.settings.lightIntensity);
			Match3Game game = this.slot.game;
		}

		public override void OnStart(ActionManager manager)
		{
			base.OnStart(manager);
		}

		private IEnumerator DoAnimation()
		{
			return new DestroyChipAction._003CDoAnimation_003Ed__11(0)
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

		private DestroyChipAction.InitArguments initArguments;

		[Serializable]
		public class Settings
		{
			public bool useChipAnimation;

			public float fromScale = 2f;

			public float toScale = 1f;

			public float duration;

			public AnimationCurve animationCurve;

			public float fromAlpha = 1f;

			public float toAlpha = 1f;

			public bool holdGravityOnChip;

			public float holdGravityDuration;

			public float lightIntensity = 0.6f;

			public float brightness = 1f;

			public bool useRocketDestroySettings;
		}

		public struct InitArguments
		{
			public bool isFromRocket
			{
				get
				{
					return this.destroyParams != null && this.destroyParams.isHitByBomb && (this.destroyParams.bombType == ChipType.HorizontalRocket || this.destroyParams.bombType == ChipType.VerticalRocket);
				}
			}

			public Chip chip;

			public Slot slot;

			public SlotDestroyParams destroyParams;
		}

		private sealed class _003CDoAnimation_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoAnimation_003Ed__11(int _003C_003E1__state)
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
				DestroyChipAction destroyChipAction = this._003C_003E4__this;
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
					this._003Cduration_003E5__3 = destroyChipAction.settings.duration;
					this._003Ccurve_003E5__4 = destroyChipAction.settings.animationCurve;
					this._003Cboard_003E5__5 = destroyChipAction.slot.game.board;
					this._003CchipTransform_003E5__6 = destroyChipAction.chip.GetComponentBehaviour<TransformBehaviour>();
					Match3Game game = destroyChipAction.slot.game;
					this._003CholdGravityDuration_003E5__7 = destroyChipAction.settings.holdGravityDuration;
					ChipBehaviour componentBehaviour = destroyChipAction.chip.GetComponentBehaviour<ChipBehaviour>();
					GameObject particles;
					if (destroyChipAction.initArguments.isFromRocket)
					{
						particles = game.particles.CreateParticles(destroyChipAction.chip, Match3Particles.PositionType.OnDestroyChipRocket, destroyChipAction.chip.chipType, destroyChipAction.chip.itemColor);
					}
					else
					{
						particles = game.particles.CreateParticles(destroyChipAction.chip, Match3Particles.PositionType.OnDestroyChip, destroyChipAction.chip.chipType, destroyChipAction.chip.itemColor);
					}
					if (componentBehaviour != null)
					{
						componentBehaviour.SetBrightness(destroyChipAction.settings.brightness);
						if (destroyChipAction.settings.useChipAnimation)
						{
							componentBehaviour.StartChipDestroyAnimation(particles);
						}
					}
				}
				if (this._003Ctime_003E5__2 >= this._003Cduration_003E5__3)
				{
					if (destroyChipAction.chipLock != null)
					{
						destroyChipAction.chipLock.UnlockAll();
					}
					destroyChipAction.chip.RemoveFromGame();
					destroyChipAction.isAlive = false;
					return false;
				}
				this._003Ctime_003E5__2 += this._003Cboard_003E5__5.currentDeltaTime;
				if (destroyChipAction.chipLock != null && this._003Ctime_003E5__2 >= this._003CholdGravityDuration_003E5__7)
				{
					destroyChipAction.chipLock.UnlockAll();
					destroyChipAction.chipLock = null;
				}
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
				if (this._003Ccurve_003E5__4 != null)
				{
					num2 = this._003Ccurve_003E5__4.Evaluate(num2);
				}
				float num3 = Mathf.LerpUnclamped(destroyChipAction.settings.fromScale, destroyChipAction.settings.toScale, num2);
				float alpha = Mathf.Lerp(destroyChipAction.settings.fromAlpha, destroyChipAction.settings.toAlpha, num2);
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

			public DestroyChipAction _003C_003E4__this;

			private float _003Ctime_003E5__2;

			private float _003Cduration_003E5__3;

			private AnimationCurve _003Ccurve_003E5__4;

			private Match3Board _003Cboard_003E5__5;

			private TransformBehaviour _003CchipTransform_003E5__6;

			private float _003CholdGravityDuration_003E5__7;
		}
	}
}
