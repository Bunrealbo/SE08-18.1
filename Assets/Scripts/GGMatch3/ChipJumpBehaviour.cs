using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class ChipJumpBehaviour
	{
		public void Init(ChipJumpBehaviour.Settings settings)
		{
			this.settings = settings;
		}

		private void SetOffset(TransformBehaviour t, Vector3 offset)
		{
			if (t == null)
			{
				return;
			}
			t.localOffsetPosition = offset;
		}

		private void SetScale(TransformBehaviour t, Vector3 scale)
		{
			if (t == null)
			{
				return;
			}
			t.showMatchActionLocalScale = scale;
		}

		private void SetRotation(TransformBehaviour t, float angle)
		{
			if (t == null)
			{
				return;
			}
			t.localRotationOffset = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		private IEnumerator DoAnimateJump(Chip chip)
		{
			return new ChipJumpBehaviour._003CDoAnimateJump_003Ed__9(0)
			{
				_003C_003E4__this = this,
				chip = chip
			};
		}

		public void Update(Chip chip, float deltaTime)
		{
			if (!this.isActive)
			{
				return;
			}
			this.deltaTime = deltaTime;
			if (this.animation == null)
			{
				this.animation = this.DoAnimateJump(chip);
			}
			this.animation.MoveNext();
		}

		public bool isActive = true;

		private ChipJumpBehaviour.Settings settings;

		private IEnumerator animation;

		private float deltaTime;

		[Serializable]
		public class Settings
		{
			public Vector3 startOffset;

			public Vector3 offset;

			public bool useStartScale;

			public Vector3 startScale = Vector3.one;

			public Vector3 scale;

			public float rotationAngle;

			public float duration;

			public AnimationCurve animationCurve;

			public float delay;

			public float delayPerX;
		}

		private sealed class _003CDoAnimateJump_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoAnimateJump_003Ed__9(int _003C_003E1__state)
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
				ChipJumpBehaviour chipJumpBehaviour = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003Cgame_003E5__2 = this.chip.slot.game;
					this._003Ct_003E5__3 = this.chip.GetComponentBehaviour<TransformBehaviour>();
					if (chipJumpBehaviour.settings.useStartScale)
					{
						chipJumpBehaviour.SetScale(this._003Ct_003E5__3, chipJumpBehaviour.settings.startScale);
					}
					chipJumpBehaviour.SetOffset(this._003Ct_003E5__3, chipJumpBehaviour.settings.startOffset);
					break;
				case 1:
					this._003C_003E1__state = -1;
					goto IL_17A;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_25B;
				case 3:
					this._003C_003E1__state = -1;
					goto IL_38C;
				default:
					return false;
				}
				IL_8F:
				this._003Ctime_003E5__4 = 0f;
				if (chipJumpBehaviour.settings.delay + chipJumpBehaviour.settings.delayPerX <= 0f)
				{
					goto IL_38C;
				}
				this._003CtotalDuration_003E5__5 = chipJumpBehaviour.settings.delay + chipJumpBehaviour.settings.duration + chipJumpBehaviour.settings.delayPerX * (float)this._003Cgame_003E5__2.board.size.x;
				this._003CinitialDelay_003E5__6 = 0f;
				if (this._003Ct_003E5__3 != null)
				{
					this._003CinitialDelay_003E5__6 = this._003Ct_003E5__3.localPosition.x * chipJumpBehaviour.settings.delayPerX;
				}
				this._003CprevTime_003E5__7 = Mathf.Repeat(this._003Cgame_003E5__2.board.currentTime, this._003CtotalDuration_003E5__5);
				this._003CprevRepetition_003E5__8 = (float)Mathf.FloorToInt(this._003Cgame_003E5__2.board.currentTime / this._003CtotalDuration_003E5__5);
				IL_17A:
				float num2 = Mathf.Repeat(this._003Cgame_003E5__2.board.currentTime, this._003CtotalDuration_003E5__5);
				int num3 = Mathf.FloorToInt(this._003Cgame_003E5__2.board.currentTime / this._003CtotalDuration_003E5__5);
				if (((float)num3 <= this._003CprevRepetition_003E5__8 && this._003CprevTime_003E5__7 > this._003CinitialDelay_003E5__6) || num2 < this._003CinitialDelay_003E5__6)
				{
					this._003CprevTime_003E5__7 = num2;
					this._003CprevRepetition_003E5__8 = (float)num3;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				this._003Ctime_003E5__4 = Mathf.Repeat(this._003Cgame_003E5__2.board.currentTime, this._003CtotalDuration_003E5__5) - this._003CinitialDelay_003E5__6;
				IL_25B:
				if (this._003Ctime_003E5__4 < chipJumpBehaviour.settings.delay)
				{
					this._003Ctime_003E5__4 += chipJumpBehaviour.deltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 2;
					return true;
				}
				this._003Ctime_003E5__4 = Mathf.Repeat(this._003Cgame_003E5__2.board.currentTime, this._003CtotalDuration_003E5__5) - this._003CinitialDelay_003E5__6 - chipJumpBehaviour.settings.delay;
				IL_38C:
				if (this._003Ctime_003E5__4 > chipJumpBehaviour.settings.duration)
				{
					goto IL_8F;
				}
				this._003Ctime_003E5__4 += chipJumpBehaviour.deltaTime;
				ChipJumpBehaviour.Settings settings = chipJumpBehaviour.settings;
				float num4 = Mathf.InverseLerp(0f, settings.duration, this._003Ctime_003E5__4);
				if (settings.animationCurve != null)
				{
					num4 = settings.animationCurve.Evaluate(num4);
				}
				Vector3 offset = Vector3.LerpUnclamped(settings.startOffset, settings.offset, num4);
				Vector3 a = Vector3.one;
				if (settings.useStartScale)
				{
					a = settings.startScale;
				}
				Vector3 scale = Vector3.LerpUnclamped(a, settings.scale, num4);
				float angle = Mathf.LerpUnclamped(0f, settings.rotationAngle, num4);
				chipJumpBehaviour.SetOffset(this._003Ct_003E5__3, offset);
				chipJumpBehaviour.SetScale(this._003Ct_003E5__3, scale);
				chipJumpBehaviour.SetRotation(this._003Ct_003E5__3, angle);
				this._003C_003E2__current = null;
				this._003C_003E1__state = 3;
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

			public Chip chip;

			public ChipJumpBehaviour _003C_003E4__this;

			private Match3Game _003Cgame_003E5__2;

			private TransformBehaviour _003Ct_003E5__3;

			private float _003Ctime_003E5__4;

			private float _003CtotalDuration_003E5__5;

			private float _003CinitialDelay_003E5__6;

			private float _003CprevTime_003E5__7;

			private float _003CprevRepetition_003E5__8;
		}
	}
}
