using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class CollectGoalAction : BoardAction
	{
		private CollectGoalAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.collectGoalSettings;
			}
		}

		public void Init(CollectGoalAction.CollectGoalParams collectParams)
		{
			this.collectParams = collectParams;
			this.globalLock = this.lockContainer.NewLock();
			this.globalLock.isSlotGravitySuspended = true;
			this.globalLock.isChipGeneratorSuspended = true;
			if (collectParams.monsterToFeed != null)
			{
				collectParams.monsterToFeed.OnStartCollectAnimation();
			}
			Chip chip = collectParams.chip;
			if (chip != null)
			{
				chip.RemoveFromSlot();
				this.chipLock = new SlotComponentLock();
				this.chipLock.isRemoveFromGameDestroySuspended = true;
				chip.AddLock(this.chipLock);
			}
			this.globalLock.LockSlot(collectParams.chipSlot);
			this.globalLock.LockSlots(collectParams.otherAffectedSlots);
		}

		private IEnumerator ScalePart()
		{
			return new CollectGoalAction._003CScalePart_003Ed__12(0)
			{
				_003C_003E4__this = this
			};
		}

		private IEnumerator BombAffectPart(float distance)
		{
			return new CollectGoalAction._003CBombAffectPart_003Ed__13(0)
			{
				_003C_003E4__this = this,
				distance = distance
			};
		}

		private float TravelDuration(Vector3 startPos, Vector3 endPos)
		{
			float result = this.settings.travelDuration;
			if (this.settings.travelSpeed > 0f)
			{
				result = Vector3.Distance(startPos, endPos) / this.settings.travelSpeed;
			}
			return result;
		}

		private IEnumerator TravelPart()
		{
			return new CollectGoalAction._003CTravelPart_003Ed__15(0)
			{
				_003C_003E4__this = this
			};
		}

		private IEnumerator DoAnimation()
		{
			return new CollectGoalAction._003CDoAnimation_003Ed__16(0)
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

		private CollectGoalAction.CollectGoalParams collectParams;

		private float deltaTime;

		private IEnumerator animationEnum;

		private Lock globalLock;

		private SlotComponentLock chipLock;

		private float lockedTime;

		public bool isUnlocked;

		public struct CollectGoalParams
		{
			public bool isFromRocket
			{
				get
				{
					return this.destroyParams != null && this.destroyParams.isHitByBomb && (this.destroyParams.bombType == ChipType.HorizontalRocket || this.destroyParams.bombType == ChipType.VerticalRocket);
				}
			}

			public TransformBehaviour transformToMove
			{
				get
				{
					if (this.moveTransform != null)
					{
						return this.moveTransform;
					}
					if (this.chip == null)
					{
						return null;
					}
					return this.chip.GetComponentBehaviour<TransformBehaviour>();
				}
			}

			public bool isCollectMoreMovesChip
			{
				get
				{
					return this.collectMoreMovesCount > 0;
				}
			}

			public Match3Goals.GoalBase goal;

			public MonsterElements.MonsterElementPieces monsterToFeed;

			public Chip chip;

			public TransformBehaviour moveTransform;

			public Match3Game game;

			public Slot chipSlot;

			public List<Slot> otherAffectedSlots;

			public bool isExplosion;

			public bool isMagicHat;

			public IntVector2 explosionCentre;

			public SlotDestroyParams destroyParams;

			public float delay;

			public bool skipScale;

			public bool smallScale;

			public int collectMoreMovesCount;
		}

		[Serializable]
		public class Settings
		{
			public bool useParticlesForChip;

			public SpriteSortingSettings sortingLayer = new SpriteSortingSettings();

			public SpriteSortingSettings sortingLayerFly = new SpriteSortingSettings();

			public float timeToLockSlot = 0.2f;

			public float travelDuration = 1f;

			public float travelSpeed;

			public AnimationCurve travelCurve;

			public float scaleUpScale = 1f;

			public bool useSmallEndScale;

			public float smallEndScale;

			public float smallScale;

			public float scaleUpDuration = 1f;

			public AnimationCurve scaleUpCurve;

			public float distanceWithSwap;

			public float distanceWithBomb;

			public float distanceWithMagicHat;

			public AnimationCurve bombCurve;

			public float bombDuration;

			public float orthoDistance;

			public AnimationCurve orthoCurve;

			public bool useScaleCurve;

			public AnimationCurve scaleCurve;
		}

		private sealed class _003CScalePart_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CScalePart_003Ed__12(int _003C_003E1__state)
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
				CollectGoalAction collectGoalAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003CtransformBehaviour_003E5__2 = collectGoalAction.collectParams.transformToMove;
					if (this._003CtransformBehaviour_003E5__2 != null)
					{
						this._003CtransformBehaviour_003E5__2.SetAlpha(1f);
					}
					this._003Csettings_003E5__3 = collectGoalAction.settings;
					this._003CstartScale_003E5__4 = Vector3.one;
					if (this._003CtransformBehaviour_003E5__2 != null)
					{
						this._003CstartScale_003E5__4 = this._003CtransformBehaviour_003E5__2.localScale;
					}
					if (collectGoalAction.collectParams.smallScale)
					{
						this._003CstartScale_003E5__4 = new Vector3(this._003Csettings_003E5__3.smallScale, this._003Csettings_003E5__3.smallScale, 1f);
					}
					if (this._003CtransformBehaviour_003E5__2 != null)
					{
						this._003CtransformBehaviour_003E5__2.localScale = this._003CstartScale_003E5__4;
					}
					this._003CendScale_003E5__5 = new Vector3(this._003Csettings_003E5__3.scaleUpScale, this._003Csettings_003E5__3.scaleUpScale, 1f);
					if (collectGoalAction.collectParams.smallScale && this._003Csettings_003E5__3.useSmallEndScale)
					{
						this._003CendScale_003E5__5 = new Vector3(this._003Csettings_003E5__3.smallEndScale, this._003Csettings_003E5__3.smallEndScale, 1f);
					}
					this._003Ctime_003E5__6 = 0f;
					if (collectGoalAction.collectParams.delay <= 0f)
					{
						goto IL_1A2;
					}
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					if (this._003Ctime_003E5__6 >= this._003Cduration_003E5__7)
					{
						return false;
					}
					goto IL_1EC;
				default:
					return false;
				}
				if (this._003Ctime_003E5__6 < collectGoalAction.collectParams.delay)
				{
					this._003Ctime_003E5__6 += collectGoalAction.deltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				IL_1A2:
				if (collectGoalAction.collectParams.skipScale)
				{
					if (this._003CtransformBehaviour_003E5__2 != null)
					{
						this._003CtransformBehaviour_003E5__2.localScale = this._003CendScale_003E5__5;
					}
					return false;
				}
				this._003Ctime_003E5__6 = 0f;
				this._003Cduration_003E5__7 = this._003Csettings_003E5__3.scaleUpDuration;
				IL_1EC:
				this._003Ctime_003E5__6 += collectGoalAction.deltaTime;
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__7, this._003Ctime_003E5__6);
				num2 = this._003Csettings_003E5__3.scaleUpCurve.Evaluate(num2);
				Vector3 localScale = Vector3.LerpUnclamped(this._003CstartScale_003E5__4, this._003CendScale_003E5__5, num2);
				if (this._003CtransformBehaviour_003E5__2 != null)
				{
					this._003CtransformBehaviour_003E5__2.localScale = localScale;
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

			public CollectGoalAction _003C_003E4__this;

			private TransformBehaviour _003CtransformBehaviour_003E5__2;

			private CollectGoalAction.Settings _003Csettings_003E5__3;

			private Vector3 _003CstartScale_003E5__4;

			private Vector3 _003CendScale_003E5__5;

			private float _003Ctime_003E5__6;

			private float _003Cduration_003E5__7;
		}

		private sealed class _003CBombAffectPart_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CBombAffectPart_003Ed__13(int _003C_003E1__state)
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
				CollectGoalAction collectGoalAction = this._003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					this._003C_003E1__state = -1;
					if (this._003Ctime_003E5__6 >= this._003Cduration_003E5__7)
					{
						return false;
					}
				}
				else
				{
					this._003C_003E1__state = -1;
					this._003CtransformBehaviour_003E5__2 = collectGoalAction.collectParams.transformToMove;
					this._003Csettings_003E5__3 = collectGoalAction.settings;
					Vector3 a = Vector3.zero;
					if (collectGoalAction.collectParams.chipSlot != null)
					{
						a = collectGoalAction.collectParams.chipSlot.localPositionOfCenter;
					}
					if (this._003CtransformBehaviour_003E5__2 != null)
					{
						a = this._003CtransformBehaviour_003E5__2.localPosition;
					}
					this._003Cdirection_003E5__4 = (a - collectGoalAction.collectParams.game.LocalPositionOfCenter(collectGoalAction.collectParams.explosionCentre)).normalized;
					this._003CstartPosition_003E5__5 = a;
					this._003Ctime_003E5__6 = 0f;
					this._003Cduration_003E5__7 = this._003Csettings_003E5__3.bombDuration;
				}
				this._003Ctime_003E5__6 += collectGoalAction.deltaTime;
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__7, this._003Ctime_003E5__6);
				num2 = this._003Csettings_003E5__3.bombCurve.Evaluate(num2);
				if (this._003CtransformBehaviour_003E5__2 != null)
				{
					this._003CtransformBehaviour_003E5__2.localPosition = Vector3.LerpUnclamped(this._003CstartPosition_003E5__5, this._003CstartPosition_003E5__5 + this._003Cdirection_003E5__4 * this.distance, num2);
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

			public CollectGoalAction _003C_003E4__this;

			public float distance;

			private TransformBehaviour _003CtransformBehaviour_003E5__2;

			private CollectGoalAction.Settings _003Csettings_003E5__3;

			private Vector3 _003Cdirection_003E5__4;

			private Vector3 _003CstartPosition_003E5__5;

			private float _003Ctime_003E5__6;

			private float _003Cduration_003E5__7;
		}

		private sealed class _003CTravelPart_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CTravelPart_003Ed__15(int _003C_003E1__state)
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
				CollectGoalAction collectGoalAction = this._003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					this._003C_003E1__state = -1;
					if (this._003Ctime_003E5__7 >= this._003Cduration_003E5__8)
					{
						return false;
					}
				}
				else
				{
					this._003C_003E1__state = -1;
					this._003CtransformBehaviour_003E5__2 = collectGoalAction.collectParams.transformToMove;
					this._003Csettings_003E5__3 = collectGoalAction.settings;
					Vector3 vector = Vector3.zero;
					this._003CstartScale_003E5__4 = Vector3.one;
					if (collectGoalAction.collectParams.chipSlot != null)
					{
						vector = collectGoalAction.collectParams.chipSlot.localPositionOfCenter;
					}
					if (this._003CtransformBehaviour_003E5__2 != null)
					{
						vector = this._003CtransformBehaviour_003E5__2.localPosition;
						this._003CstartScale_003E5__4 = this._003CtransformBehaviour_003E5__2.localScale;
					}
					this._003CstartLocalPos_003E5__5 = vector;
					Match3Game game = collectGoalAction.collectParams.game;
					GoalsPanelGoal goal = game.gameScreen.goalsPanel.GetGoal(collectGoalAction.collectParams.goal);
					this._003CendLocalPos_003E5__6 = this._003CstartLocalPos_003E5__5;
					if (this._003CtransformBehaviour_003E5__2 != null)
					{
						this._003CendLocalPos_003E5__6 = this._003CtransformBehaviour_003E5__2.WorldToLocalPosition(game.gameScreen.goalsPanel.transform.position);
						if (collectGoalAction.collectParams.monsterToFeed != null)
						{
							this._003CendLocalPos_003E5__6 = collectGoalAction.collectParams.monsterToFeed.LocalPositionOfCenter(game);
						}
						else if (collectGoalAction.collectParams.isCollectMoreMovesChip)
						{
							this._003CendLocalPos_003E5__6 = this._003CtransformBehaviour_003E5__2.WorldToLocalPosition(game.gameScreen.goalsPanel.movesCountLabel.transform.position);
						}
						if (goal != null)
						{
							this._003CendLocalPos_003E5__6 = this._003CtransformBehaviour_003E5__2.WorldToLocalPosition(goal.transform.position);
						}
					}
					this._003CendLocalPos_003E5__6.z = 0f;
					this._003Ctime_003E5__7 = 0f;
					this._003Cduration_003E5__8 = collectGoalAction.TravelDuration(this._003CstartLocalPos_003E5__5, this._003CendLocalPos_003E5__6);
				}
				this._003Ctime_003E5__7 += collectGoalAction.deltaTime;
				float time = Mathf.InverseLerp(0f, this._003Cduration_003E5__8, this._003Ctime_003E5__7);
				float t = this._003Csettings_003E5__3.travelCurve.Evaluate(time);
				Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartLocalPos_003E5__5, this._003CendLocalPos_003E5__6, t);
				if (this._003Csettings_003E5__3.orthoDistance != 0f)
				{
					float t2 = this._003Csettings_003E5__3.orthoCurve.Evaluate(time);
					localPosition.y += Mathf.LerpUnclamped(0f, this._003Csettings_003E5__3.orthoDistance, t2);
				}
				if (this._003Csettings_003E5__3.useScaleCurve)
				{
					float d = this._003Csettings_003E5__3.scaleCurve.Evaluate(time);
					if (this._003CtransformBehaviour_003E5__2 != null)
					{
						this._003CtransformBehaviour_003E5__2.localScale = this._003CstartScale_003E5__4 * d;
					}
				}
				if (this._003CtransformBehaviour_003E5__2 != null)
				{
					this._003CtransformBehaviour_003E5__2.localPosition = localPosition;
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

			public CollectGoalAction _003C_003E4__this;

			private TransformBehaviour _003CtransformBehaviour_003E5__2;

			private CollectGoalAction.Settings _003Csettings_003E5__3;

			private Vector3 _003CstartScale_003E5__4;

			private Vector3 _003CstartLocalPos_003E5__5;

			private Vector3 _003CendLocalPos_003E5__6;

			private float _003Ctime_003E5__7;

			private float _003Cduration_003E5__8;
		}

		private sealed class _003CDoAnimation_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoAnimation_003Ed__16(int _003C_003E1__state)
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
				CollectGoalAction collectGoalAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					if (collectGoalAction.collectParams.collectMoreMovesCount > 0)
					{
						collectGoalAction.collectParams.game.Play(GGSoundSystem.SFXType.CollectMoreMovesStart);
					}
					else
					{
						collectGoalAction.collectParams.game.Play(GGSoundSystem.SFXType.CollectGoalStart);
					}
					this._003Cchip_003E5__2 = collectGoalAction.collectParams.chip;
					this._003Cgame_003E5__3 = collectGoalAction.collectParams.game;
					this._003CtransformBehaviour_003E5__4 = collectGoalAction.collectParams.transformToMove;
					if (this._003CtransformBehaviour_003E5__4 != null)
					{
						this._003CtransformBehaviour_003E5__4.SetSortingLayer(collectGoalAction.settings.sortingLayer);
					}
					this._003Canimation_003E5__5 = null;
					if (collectGoalAction.settings.useParticlesForChip && this._003Cchip_003E5__2 != null && this._003Cchip_003E5__2.canFormColorMatches && !collectGoalAction.collectParams.isFromRocket && !collectGoalAction.collectParams.skipScale)
					{
						if (collectGoalAction.collectParams.isExplosion)
						{
							this._003Cgame_003E5__3.particles.CreateParticles(this._003Cchip_003E5__2, Match3Particles.PositionType.OnDestroyChipExplosion, this._003Cchip_003E5__2.chipType, this._003Cchip_003E5__2.itemColor);
						}
						else
						{
							this._003Cgame_003E5__3.particles.CreateParticles(this._003Cchip_003E5__2, Match3Particles.PositionType.OnDestroyChip, this._003Cchip_003E5__2.chipType, this._003Cchip_003E5__2.itemColor);
						}
					}
					this._003CenumList_003E5__6 = new EnumeratorsList();
					this._003CenumList_003E5__6.Add(collectGoalAction.ScalePart(), 0f, null, null, false);
					if (collectGoalAction.collectParams.isExplosion)
					{
						this._003CenumList_003E5__6.Add(collectGoalAction.BombAffectPart(collectGoalAction.settings.distanceWithBomb), 0f, null, null, false);
					}
					else if (collectGoalAction.collectParams.isMagicHat)
					{
						this._003CenumList_003E5__6.Add(collectGoalAction.BombAffectPart(collectGoalAction.settings.distanceWithMagicHat), 0f, null, null, false);
					}
					else
					{
						this._003CenumList_003E5__6.Add(collectGoalAction.BombAffectPart(collectGoalAction.settings.distanceWithSwap), 0f, null, null, false);
					}
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_28D;
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
					this._003CtransformBehaviour_003E5__4.SetSortingLayer(collectGoalAction.settings.sortingLayerFly);
				}
				this._003Canimation_003E5__5 = collectGoalAction.TravelPart();
				IL_28D:
				if (!this._003Canimation_003E5__5.MoveNext())
				{
					if (collectGoalAction.collectParams.moveTransform != null)
					{
						collectGoalAction.collectParams.moveTransform.RemoveFromGame();
					}
					if (this._003Cchip_003E5__2 != null)
					{
						this._003Cchip_003E5__2.RemoveLock(collectGoalAction.chipLock);
						this._003Cchip_003E5__2.RemoveFromGame();
					}
					collectGoalAction.globalLock.UnlockAll();
					this._003Cgame_003E5__3.OnCollectedMoreMoves(collectGoalAction.collectParams.collectMoreMovesCount);
					this._003Cgame_003E5__3.OnPickupGoal(new GoalCollectParams(collectGoalAction.collectParams.goal, collectGoalAction.collectParams.destroyParams));
					if (collectGoalAction.collectParams.monsterToFeed != null)
					{
						collectGoalAction.collectParams.monsterToFeed.OnCollected(this._003Cgame_003E5__3);
					}
					collectGoalAction.isAlive = false;
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

			public CollectGoalAction _003C_003E4__this;

			private Chip _003Cchip_003E5__2;

			private Match3Game _003Cgame_003E5__3;

			private TransformBehaviour _003CtransformBehaviour_003E5__4;

			private IEnumerator _003Canimation_003E5__5;

			private EnumeratorsList _003CenumList_003E5__6;
		}
	}
}
