using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class ShowSwapNotPossibleAction : BoardAction
	{
		private ShowSwapNotPossibleAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.showSwapNotPossibleActionSettings;
			}
		}

		public void Init(ShowSwapNotPossibleAction.SwapChipsParams chipParams)
		{
			this.chipParams = chipParams;
			Slot slot = chipParams.slot1;
			this.slotLock = this.lockContainer.NewLock();
			this.slotLock.isSlotGravitySuspended = true;
			this.slotLock.isSlotMatchingSuspended = true;
			this.slotLock.LockSlot(slot);
		}

		public IEnumerator MoveSingleChipAnimation(Slot slotToMove, Vector3 startPosition, Vector3 endPosition, AnimationCurve moveCurve)
		{
			return new ShowSwapNotPossibleAction._003CMoveSingleChipAnimation_003Ed__9(0)
			{
				_003C_003E4__this = this,
				slotToMove = slotToMove,
				startPosition = startPosition,
				endPosition = endPosition,
				moveCurve = moveCurve
			};
		}

		public IEnumerator MoveAnimation()
		{
			return new ShowSwapNotPossibleAction._003CMoveAnimation_003Ed__10(0)
			{
				_003C_003E4__this = this
			};
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.deltaTime = deltaTime;
			if (this.moveAnimation == null)
			{
				this.moveAnimation = this.MoveAnimation();
			}
			this.moveAnimation.MoveNext();
		}

		private ShowSwapNotPossibleAction.SwapChipsParams chipParams;

		private float deltaTime;

		private Lock slotLock;

		private IEnumerator moveAnimation;

		[Serializable]
		public class Settings
		{
			public float duration;

			public float moveDistance;

			public AnimationCurve moveCurve;

			public AnimationCurve moveOutCurve;

			public float brightness = 2f;
		}

		public struct SwapChipsParams
		{
			public Match3Game game;

			public Slot slot1;

			public IntVector2 positionToMoveSlot1;

			public bool wobble;
		}

		private sealed class _003CMoveSingleChipAnimation_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CMoveSingleChipAnimation_003Ed__9(int _003C_003E1__state)
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
				ShowSwapNotPossibleAction showSwapNotPossibleAction = this._003C_003E4__this;
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
					this._003Cduration_003E5__3 = showSwapNotPossibleAction.settings.duration;
					Chip slotComponent = this.slotToMove.GetSlotComponent<Chip>();
					if (slotComponent == null)
					{
						return false;
					}
					this._003CchipBehaviour_003E5__4 = slotComponent.GetComponentBehaviour<TransformBehaviour>();
					if (this._003CchipBehaviour_003E5__4 == null)
					{
						return false;
					}
				}
				if (this._003Ctime_003E5__2 > this._003Cduration_003E5__3)
				{
					return false;
				}
				this._003Ctime_003E5__2 += showSwapNotPossibleAction.deltaTime;
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
				num2 = this.moveCurve.Evaluate(num2);
				Vector3 localPosition = Vector3.LerpUnclamped(this.startPosition, this.endPosition, num2);
				this._003CchipBehaviour_003E5__4.localPosition = localPosition;
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

			public ShowSwapNotPossibleAction _003C_003E4__this;

			public Slot slotToMove;

			public AnimationCurve moveCurve;

			public Vector3 startPosition;

			public Vector3 endPosition;

			private float _003Ctime_003E5__2;

			private float _003Cduration_003E5__3;

			private TransformBehaviour _003CchipBehaviour_003E5__4;
		}

		private sealed class _003CMoveAnimation_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CMoveAnimation_003Ed__10(int _003C_003E1__state)
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
				ShowSwapNotPossibleAction showSwapNotPossibleAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
				{
					this._003C_003E1__state = -1;
					this._003Cslot1_003E5__2 = showSwapNotPossibleAction.chipParams.slot1;
					this._003CstartPos_003E5__3 = this._003Cslot1_003E5__2.localPositionOfCenter;
					Vector3 normalized = (showSwapNotPossibleAction.chipParams.positionToMoveSlot1 - this._003Cslot1_003E5__2.position).ToVector3().normalized;
					this._003CendPos_003E5__4 = this._003CstartPos_003E5__3 + normalized * showSwapNotPossibleAction.settings.moveDistance;
					Chip slotComponent = this._003Cslot1_003E5__2.GetSlotComponent<Chip>();
					this._003CchipBehaviour_003E5__5 = null;
					if (slotComponent != null)
					{
						this._003CchipBehaviour_003E5__5 = slotComponent.GetComponentBehaviour<TransformBehaviour>();
					}
					if (this._003CchipBehaviour_003E5__5 != null)
					{
						this._003CchipBehaviour_003E5__5.SetBrightness(showSwapNotPossibleAction.settings.brightness);
					}
					this._003Canimation_003E5__6 = showSwapNotPossibleAction.MoveSingleChipAnimation(this._003Cslot1_003E5__2, this._003CstartPos_003E5__3, this._003CendPos_003E5__4, showSwapNotPossibleAction.settings.moveCurve);
					break;
				}
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_1B1;
				default:
					return false;
				}
				if (this._003Canimation_003E5__6.MoveNext())
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				Slot slot = showSwapNotPossibleAction.chipParams.game.GetSlot(showSwapNotPossibleAction.chipParams.positionToMoveSlot1);
				if (slot != null && showSwapNotPossibleAction.chipParams.wobble)
				{
					slot.Wobble(Match3Settings.instance.chipWobbleSettings);
				}
				this._003Canimation_003E5__6 = showSwapNotPossibleAction.MoveSingleChipAnimation(this._003Cslot1_003E5__2, this._003CendPos_003E5__4, this._003CstartPos_003E5__3, showSwapNotPossibleAction.settings.moveOutCurve);
				IL_1B1:
				if (!this._003Canimation_003E5__6.MoveNext())
				{
					if (this._003CchipBehaviour_003E5__5 != null)
					{
						this._003CchipBehaviour_003E5__5.SetBrightness(1f);
					}
					showSwapNotPossibleAction.slotLock.UnlockAll();
					showSwapNotPossibleAction.isAlive = false;
					return false;
				}
				this._003C_003E2__current = null;
				this._003C_003E1__state = 2;
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

			public ShowSwapNotPossibleAction _003C_003E4__this;

			private Slot _003Cslot1_003E5__2;

			private Vector3 _003CstartPos_003E5__3;

			private Vector3 _003CendPos_003E5__4;

			private TransformBehaviour _003CchipBehaviour_003E5__5;

			private IEnumerator _003Canimation_003E5__6;
		}
	}
}
