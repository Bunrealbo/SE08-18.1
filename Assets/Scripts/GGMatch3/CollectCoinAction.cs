using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class CollectCoinAction : BoardAction
	{
		private CollectGoalAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.collectGoalSettings;
			}
		}

		public void Init(CollectCoinAction.InitArguments initArguments)
		{
			this.initArguments = initArguments;
			this.globalLock = this.lockContainer.NewLock();
			this.globalLock.isSlotGravitySuspended = true;
			this.globalLock.isChipGeneratorSuspended = true;
			this.globalLock.LockSlot(initArguments.chipSlot);
		}

		private IEnumerator ScalePart(TransformBehaviour transformBehaviour)
		{
			return new CollectCoinAction._003CScalePart_003Ed__10(0)
			{
				_003C_003E4__this = this,
				transformBehaviour = transformBehaviour
			};
		}

		private IEnumerator TravelPart(TransformBehaviour transformBehaviour)
		{
			return new CollectCoinAction._003CTravelPart_003Ed__11(0)
			{
				_003C_003E4__this = this,
				transformBehaviour = transformBehaviour
			};
		}

		private IEnumerator DoAnimation()
		{
			return new CollectCoinAction._003CDoAnimation_003Ed__12(0)
			{
				_003C_003E4__this = this
			};
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.deltaTime = deltaTime;
			this.lockedTime += deltaTime;
			if (this.animationEnum == null)
			{
				this.animationEnum = this.DoAnimation();
			}
			this.animationEnum.MoveNext();
			bool flag = this.lockedTime > this.settings.timeToLockSlot;
			if (!this.isUnlocked && flag)
			{
				this.isUnlocked = true;
				this.globalLock.UnlockAll();
			}
		}

		private CollectCoinAction.InitArguments initArguments;

		private float deltaTime;

		private IEnumerator animationEnum;

		private Lock globalLock;

		private float lockedTime;

		public bool isUnlocked;

		public struct InitArguments
		{
			public Match3Game game;

			public Slot chipSlot;

			public float delay;
		}

		private sealed class _003CScalePart_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CScalePart_003Ed__10(int _003C_003E1__state)
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
				CollectCoinAction collectCoinAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					if (this.transformBehaviour != null)
					{
						this.transformBehaviour.SetAlpha(1f);
					}
					this._003Csettings_003E5__2 = collectCoinAction.settings;
					this._003CstartScale_003E5__3 = Vector3.one;
					if (this.transformBehaviour != null)
					{
						this._003CstartScale_003E5__3 = this.transformBehaviour.localScale;
					}
					if (this.transformBehaviour != null)
					{
						this.transformBehaviour.localScale = this._003CstartScale_003E5__3;
					}
					this._003CendScale_003E5__4 = new Vector3(this._003Csettings_003E5__2.scaleUpScale, this._003Csettings_003E5__2.scaleUpScale, 1f);
					this._003Ctime_003E5__5 = 0f;
					if (collectCoinAction.initArguments.delay <= 0f)
					{
						goto IL_11E;
					}
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					if (this._003Ctime_003E5__5 >= this._003Cduration_003E5__6)
					{
						return false;
					}
					goto IL_13A;
				default:
					return false;
				}
				if (this._003Ctime_003E5__5 < collectCoinAction.initArguments.delay)
				{
					this._003Ctime_003E5__5 += collectCoinAction.deltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				IL_11E:
				this._003Ctime_003E5__5 = 0f;
				this._003Cduration_003E5__6 = this._003Csettings_003E5__2.scaleUpDuration;
				IL_13A:
				this._003Ctime_003E5__5 += collectCoinAction.deltaTime;
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__6, this._003Ctime_003E5__5);
				num2 = this._003Csettings_003E5__2.scaleUpCurve.Evaluate(num2);
				Vector3 localScale = Vector3.LerpUnclamped(this._003CstartScale_003E5__3, this._003CendScale_003E5__4, num2);
				if (this.transformBehaviour != null)
				{
					this.transformBehaviour.localScale = localScale;
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

			public TransformBehaviour transformBehaviour;

			public CollectCoinAction _003C_003E4__this;

			private CollectGoalAction.Settings _003Csettings_003E5__2;

			private Vector3 _003CstartScale_003E5__3;

			private Vector3 _003CendScale_003E5__4;

			private float _003Ctime_003E5__5;

			private float _003Cduration_003E5__6;
		}

		private sealed class _003CTravelPart_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CTravelPart_003Ed__11(int _003C_003E1__state)
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
				CollectCoinAction collectCoinAction = this._003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					this._003C_003E1__state = -1;
					if (this._003Ctime_003E5__5 >= this._003Cduration_003E5__6)
					{
						return false;
					}
				}
				else
				{
					this._003C_003E1__state = -1;
					this._003Csettings_003E5__2 = collectCoinAction.settings;
					Vector3 vector = Vector3.zero;
					if (collectCoinAction.initArguments.chipSlot != null)
					{
						vector = collectCoinAction.initArguments.chipSlot.localPositionOfCenter;
					}
					if (this.transformBehaviour != null)
					{
						vector = this.transformBehaviour.localPosition;
					}
					this._003CstartLocalPos_003E5__3 = vector;
					Match3Game game = collectCoinAction.initArguments.game;
					RectTransform coinsTransform = game.gameScreen.goalsPanel.coinsTransform;
					this._003CendLocalPos_003E5__4 = this._003CstartLocalPos_003E5__3;
					if (this.transformBehaviour != null)
					{
						this._003CendLocalPos_003E5__4 = this.transformBehaviour.WorldToLocalPosition(game.gameScreen.goalsPanel.transform.position);
						if (coinsTransform != null)
						{
							this._003CendLocalPos_003E5__4 = this.transformBehaviour.WorldToLocalPosition(coinsTransform.transform.position);
						}
					}
					this._003CendLocalPos_003E5__4.z = 0f;
					this._003Ctime_003E5__5 = 0f;
					this._003Cduration_003E5__6 = this._003Csettings_003E5__2.travelDuration;
				}
				this._003Ctime_003E5__5 += collectCoinAction.deltaTime;
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__6, this._003Ctime_003E5__5);
				num2 = this._003Csettings_003E5__2.travelCurve.Evaluate(num2);
				Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartLocalPos_003E5__3, this._003CendLocalPos_003E5__4, num2);
				if (this.transformBehaviour != null)
				{
					this.transformBehaviour.localPosition = localPosition;
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

			public CollectCoinAction _003C_003E4__this;

			public TransformBehaviour transformBehaviour;

			private CollectGoalAction.Settings _003Csettings_003E5__2;

			private Vector3 _003CstartLocalPos_003E5__3;

			private Vector3 _003CendLocalPos_003E5__4;

			private float _003Ctime_003E5__5;

			private float _003Cduration_003E5__6;
		}

		private sealed class _003CDoAnimation_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoAnimation_003Ed__12(int _003C_003E1__state)
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
				CollectCoinAction collectCoinAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003Cgame_003E5__2 = collectCoinAction.initArguments.game;
					this._003Ccoin_003E5__3 = this._003Cgame_003E5__2.CreateCoin();
					this._003CtransformBehaviour_003E5__4 = this._003Ccoin_003E5__3;
					if (this._003CtransformBehaviour_003E5__4 != null)
					{
						this._003CtransformBehaviour_003E5__4.localPosition = collectCoinAction.initArguments.chipSlot.localPositionOfCenter;
						this._003CtransformBehaviour_003E5__4.SetSortingLayer(collectCoinAction.settings.sortingLayer);
						GGUtil.Show(this._003CtransformBehaviour_003E5__4);
					}
					this._003Canimation_003E5__5 = null;
					this._003Cgame_003E5__2.Play(GGSoundSystem.SFXType.CoinCollectStart);
					this._003CenumList_003E5__6 = new EnumeratorsList();
					this._003CenumList_003E5__6.Add(collectCoinAction.ScalePart(this._003CtransformBehaviour_003E5__4), 0f, null, null, false);
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_155;
				default:
					return false;
				}
				if (this._003CenumList_003E5__6.Update())
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				if (this._003CtransformBehaviour_003E5__4 != null)
				{
					this._003CtransformBehaviour_003E5__4.SetSortingLayer(collectCoinAction.settings.sortingLayerFly);
				}
				this._003Canimation_003E5__5 = collectCoinAction.TravelPart(this._003CtransformBehaviour_003E5__4);
				IL_155:
				if (!this._003Canimation_003E5__5.MoveNext())
				{
					this._003Cgame_003E5__2.Play(GGSoundSystem.SFXType.CoinCollect);
					if (this._003Ccoin_003E5__3 != null)
					{
						this._003Ccoin_003E5__3.RemoveFromGame();
					}
					collectCoinAction.globalLock.UnlockAll();
					this._003Cgame_003E5__2.OnCollectCoin();
					collectCoinAction.isAlive = false;
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

			public CollectCoinAction _003C_003E4__this;

			private Match3Game _003Cgame_003E5__2;

			private TransformBehaviour _003Ccoin_003E5__3;

			private TransformBehaviour _003CtransformBehaviour_003E5__4;

			private IEnumerator _003Canimation_003E5__5;

			private EnumeratorsList _003CenumList_003E5__6;
		}
	}
}
