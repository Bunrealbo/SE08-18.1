using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class SwapChipsAction : BoardAction
	{
		private SwapChipsAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.swapChipsActionSettings;
			}
		}

		public void Init(SwapChipsAction.SwapChipsParams chipParams)
		{
			this.chipParams = chipParams;
			Slot slot = chipParams.slot1;
			Slot slot2 = chipParams.slot2;
			this.slotLock = this.lockContainer.NewLock();
			this.slotLock.isSlotGravitySuspended = true;
			this.slotLock.isSlotMatchingSuspended = true;
			this.slotLock.LockSlot(slot);
			this.slotLock.LockSlot(slot2);
		}

		public IEnumerator MoveSingleChipAnimation(Slot slotToMove, IntVector2 positionToMoveTo)
		{
			return new SwapChipsAction._003CMoveSingleChipAnimation_003Ed__9(0)
			{
				_003C_003E4__this = this,
				slotToMove = slotToMove,
				positionToMoveTo = positionToMoveTo
			};
		}

		public IEnumerator ScaleDownChipAnimation(Slot slotToMove, IntVector2 positionToMoveTo)
		{
			return new SwapChipsAction._003CScaleDownChipAnimation_003Ed__10(0)
			{
				_003C_003E4__this = this,
				slotToMove = slotToMove,
				positionToMoveTo = positionToMoveTo
			};
		}

		private void SetBrightness(Slot slot, float brightness)
		{
			if (slot == null)
			{
				return;
			}
			Chip slotComponent = slot.GetSlotComponent<Chip>();
			if (slotComponent == null)
			{
				return;
			}
			TransformBehaviour componentBehaviour = slotComponent.GetComponentBehaviour<TransformBehaviour>();
			if (componentBehaviour == null)
			{
				return;
			}
			componentBehaviour.SetBrightness(brightness);
		}

		public IEnumerator MoveAnimation()
		{
			return new SwapChipsAction._003CMoveAnimation_003Ed__12(0)
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

		private SwapChipsAction.SwapChipsParams chipParams;

		private float deltaTime;

		private Lock slotLock;

		private IEnumerator moveAnimation;

		[Serializable]
		public class Settings
		{
			public float duration;

			public AnimationCurve moveCurve;

			public AnimationCurve scaleCurve;

			public float brightness = 2f;
		}

		public struct SwapChipsParams
		{
			public void CallOnComplete()
			{
				if (this.onComplete == null)
				{
					return;
				}
				this.onComplete();
			}

			public Match3Game game;

			public Slot slot1;

			public Slot slot2;

			public bool moveToSpecificPos;

			public IntVector2 positionToMoveSlot1;

			public IntVector2 positionToMoveSlot2;

			public Action onComplete;

			public bool switchSlots;

			public bool scaleDownChip2;
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
				SwapChipsAction swapChipsAction = this._003C_003E4__this;
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
					this._003Cduration_003E5__3 = swapChipsAction.settings.duration;
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
					this._003CstartPosition_003E5__5 = this._003CchipBehaviour_003E5__4.localPosition;
					this._003CendPosition_003E5__6 = swapChipsAction.chipParams.game.LocalPositionOfCenter(this.positionToMoveTo);
				}
				if (this._003Ctime_003E5__2 > this._003Cduration_003E5__3)
				{
					return false;
				}
				this._003Ctime_003E5__2 += swapChipsAction.deltaTime;
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
				num2 = swapChipsAction.settings.moveCurve.Evaluate(num2);
				Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartPosition_003E5__5, this._003CendPosition_003E5__6, num2);
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

			public SwapChipsAction _003C_003E4__this;

			public Slot slotToMove;

			public IntVector2 positionToMoveTo;

			private float _003Ctime_003E5__2;

			private float _003Cduration_003E5__3;

			private TransformBehaviour _003CchipBehaviour_003E5__4;

			private Vector3 _003CstartPosition_003E5__5;

			private Vector3 _003CendPosition_003E5__6;
		}

		private sealed class _003CScaleDownChipAnimation_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CScaleDownChipAnimation_003Ed__10(int _003C_003E1__state)
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
				SwapChipsAction swapChipsAction = this._003C_003E4__this;
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
					this._003Cduration_003E5__3 = swapChipsAction.settings.duration;
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
					this._003CstartPosition_003E5__5 = this._003CchipBehaviour_003E5__4.localPosition;
					this._003CendPosition_003E5__6 = swapChipsAction.chipParams.game.LocalPositionOfCenter(this.positionToMoveTo);
					this._003CchipBehaviour_003E5__4.localPosition = this._003CstartPosition_003E5__5;
					this._003Cdirection_003E5__7 = (this._003CendPosition_003E5__6 - this._003CstartPosition_003E5__5) * 0.5f;
					this._003ChalphDuration_003E5__8 = this._003Cduration_003E5__3 * 0.5f;
				}
				if (this._003Ctime_003E5__2 > this._003Cduration_003E5__3)
				{
					this._003CchipBehaviour_003E5__4.localPosition = this._003CendPosition_003E5__6;
					this._003CchipBehaviour_003E5__4.localScale = Vector3.one;
					return false;
				}
				this._003Ctime_003E5__2 += swapChipsAction.deltaTime;
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__3, this._003Ctime_003E5__2);
				float num3 = swapChipsAction.settings.moveCurve.Evaluate(num2);
				float t = swapChipsAction.settings.scaleCurve.Evaluate(Mathf.PingPong(num2, 0.5f));
				Vector3 localScale = Vector3.LerpUnclamped(Vector3.one, new Vector3(0f, 0f, 1f), t);
				this._003CchipBehaviour_003E5__4.localScale = localScale;
				this._003CchipBehaviour_003E5__4.localScale = Vector3.zero;
				if (num3 < this._003ChalphDuration_003E5__8)
				{
					Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartPosition_003E5__5, this._003CstartPosition_003E5__5 - this._003Cdirection_003E5__7, Mathf.InverseLerp(0f, 0.5f, num3));
					this._003CchipBehaviour_003E5__4.localPosition = localPosition;
				}
				else
				{
					Vector3 localPosition2 = Vector3.Lerp(this._003CendPosition_003E5__6 + this._003Cdirection_003E5__7, this._003CendPosition_003E5__6, Mathf.InverseLerp(0.5f, 1f, num3));
					this._003CchipBehaviour_003E5__4.localPosition = localPosition2;
				}
				this._003CchipBehaviour_003E5__4.localPosition = this._003CstartPosition_003E5__5;
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

			public SwapChipsAction _003C_003E4__this;

			public Slot slotToMove;

			public IntVector2 positionToMoveTo;

			private float _003Ctime_003E5__2;

			private float _003Cduration_003E5__3;

			private TransformBehaviour _003CchipBehaviour_003E5__4;

			private Vector3 _003CstartPosition_003E5__5;

			private Vector3 _003CendPosition_003E5__6;

			private Vector3 _003Cdirection_003E5__7;

			private float _003ChalphDuration_003E5__8;
		}

		private sealed class _003CMoveAnimation_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CMoveAnimation_003Ed__12(int _003C_003E1__state)
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
				SwapChipsAction swapChipsAction = this._003C_003E4__this;
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
					this._003CenumList_003E5__2 = new EnumeratorsList();
					this._003Cslot1_003E5__3 = swapChipsAction.chipParams.slot1;
					this._003Cslot2_003E5__4 = swapChipsAction.chipParams.slot2;
					swapChipsAction.SetBrightness(this._003Cslot1_003E5__3, swapChipsAction.settings.brightness);
					swapChipsAction.SetBrightness(this._003Cslot2_003E5__4, swapChipsAction.settings.brightness);
					if (swapChipsAction.chipParams.moveToSpecificPos)
					{
						this._003CenumList_003E5__2.Add(swapChipsAction.MoveSingleChipAnimation(this._003Cslot1_003E5__3, swapChipsAction.chipParams.positionToMoveSlot1), 0f, null, null, false);
						if (swapChipsAction.chipParams.scaleDownChip2)
						{
							this._003CenumList_003E5__2.Add(swapChipsAction.ScaleDownChipAnimation(this._003Cslot2_003E5__4, swapChipsAction.chipParams.positionToMoveSlot2), 0f, null, null, false);
						}
						else
						{
							this._003CenumList_003E5__2.Add(swapChipsAction.MoveSingleChipAnimation(this._003Cslot2_003E5__4, swapChipsAction.chipParams.positionToMoveSlot2), 0f, null, null, false);
						}
					}
					else if (swapChipsAction.chipParams.switchSlots)
					{
						this._003CenumList_003E5__2.Add(swapChipsAction.MoveSingleChipAnimation(this._003Cslot1_003E5__3, this._003Cslot2_003E5__4.position), 0f, null, null, false);
						if (swapChipsAction.chipParams.scaleDownChip2)
						{
							this._003CenumList_003E5__2.Add(swapChipsAction.ScaleDownChipAnimation(this._003Cslot2_003E5__4, this._003Cslot1_003E5__3.position), 0f, null, null, false);
						}
						else
						{
							this._003CenumList_003E5__2.Add(swapChipsAction.MoveSingleChipAnimation(this._003Cslot2_003E5__4, this._003Cslot1_003E5__3.position), 0f, null, null, false);
						}
					}
					else
					{
						this._003CenumList_003E5__2.Add(swapChipsAction.MoveSingleChipAnimation(this._003Cslot1_003E5__3, this._003Cslot1_003E5__3.position), 0f, null, null, false);
						if (swapChipsAction.chipParams.scaleDownChip2)
						{
							this._003CenumList_003E5__2.Add(swapChipsAction.ScaleDownChipAnimation(this._003Cslot2_003E5__4, this._003Cslot2_003E5__4.position), 0f, null, null, false);
						}
						else
						{
							this._003CenumList_003E5__2.Add(swapChipsAction.MoveSingleChipAnimation(this._003Cslot2_003E5__4, this._003Cslot2_003E5__4.position), 0f, null, null, false);
						}
					}
				}
				if (!this._003CenumList_003E5__2.Update())
				{
					swapChipsAction.slotLock.UnlockAll();
					swapChipsAction.SetBrightness(this._003Cslot1_003E5__3, 1f);
					swapChipsAction.SetBrightness(this._003Cslot2_003E5__4, 1f);
					swapChipsAction.chipParams.CallOnComplete();
					swapChipsAction.isAlive = false;
					return false;
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

			public SwapChipsAction _003C_003E4__this;

			private EnumeratorsList _003CenumList_003E5__2;

			private Slot _003Cslot1_003E5__3;

			private Slot _003Cslot2_003E5__4;
		}
	}
}
