using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class CollectPointsAction : BoardAction
	{
		public static void OnBlockerDestroy(Slot slot, SlotDestroyParams destroyParams)
		{
			CollectPointsAction.Settings collectPointsActionSettings = Match3Settings.instance.collectPointsActionSettings;
			if (!collectPointsActionSettings.enabled)
			{
				return;
			}
			if (slot == null)
			{
				return;
			}
			if (destroyParams == null)
			{
				return;
			}
			if (destroyParams.scoreAdded > 0)
			{
				return;
			}
			int blockerCount = collectPointsActionSettings.blockerCount;
			CollectPointsAction.InitArguments initArguments = default(CollectPointsAction.InitArguments);
			Match3Game game = slot.game;
			initArguments.SetCount(blockerCount, 1);
			initArguments.localPosition = slot.localPositionOfCenter;
			initArguments.game = game;
			initArguments.isBlocker = true;
			initArguments.itemColor = ItemColor.Uncolored;
			CollectPointsAction collectPointsAction = new CollectPointsAction();
			collectPointsAction.Init(initArguments);
			game.board.actionManager.AddAction(collectPointsAction);
			destroyParams.scoreAdded++;
		}

		public static void OnIslandDestroy(DestroyMatchingIslandAction.InitArguments arguments)
		{
			CollectPointsAction.Settings collectPointsActionSettings = Match3Settings.instance.collectPointsActionSettings;
			if (!collectPointsActionSettings.enabled)
			{
				return;
			}
			bool flag = arguments.slotWherePowerupIsCreated != null;
			List<Slot> allSlots = arguments.allSlots;
			if (allSlots.Count == 0)
			{
				return;
			}
			CollectPointsAction.slotsWithGoals.Clear();
			Match3Game game = arguments.game;
			bool flag2 = false;
			ItemColor itemColor = ItemColor.Unknown;
			Vector3 vector = Vector3.zero;
			ActionScore a = default(ActionScore);
			for (int i = 0; i < allSlots.Count; i++)
			{
				Slot slot = allSlots[i];
				vector += slot.localPositionOfCenter;
				ActionScore actionScore = game.goals.FreshActionScoreForDestroyingSlot(slot);
				a += actionScore;
				if (actionScore.goalsCount > 0)
				{
					CollectPointsAction.slotsWithGoals.Add(slot);
				}
				Chip slotComponent = slot.GetSlotComponent<Chip>();
				if (slotComponent != null)
				{
					itemColor = slotComponent.itemColor;
				}
			}
			if (!collectPointsActionSettings.enableForGoals && flag2)
			{
				return;
			}
			vector /= (float)allSlots.Count;
			if (arguments.slotWherePowerupIsCreated != null)
			{
				vector = arguments.slotWherePowerupIsCreated.localPositionOfCenter;
			}
			if (CollectPointsAction.slotsWithGoals.Count > 0)
			{
				for (int j = 0; j < CollectPointsAction.slotsWithGoals.Count; j++)
				{
					Slot slot2 = CollectPointsAction.slotsWithGoals[j];
					CollectPointsAction.InitArguments initArguments = default(CollectPointsAction.InitArguments);
					initArguments.SetCount(collectPointsActionSettings.count, 1);
					initArguments.localPosition = slot2.localPositionOfCenter;
					initArguments.game = game;
					initArguments.itemColor = itemColor;
					CollectPointsAction collectPointsAction = new CollectPointsAction();
					collectPointsAction.Init(initArguments);
					game.board.actionManager.AddAction(collectPointsAction);
				}
				return;
			}
			int desiredCount = collectPointsActionSettings.count;
			if (flag)
			{
				desiredCount = collectPointsActionSettings.powerupCount * allSlots.Count;
			}
			if (!collectPointsActionSettings.enableForPowerupCreate && flag)
			{
				return;
			}
			CollectPointsAction.InitArguments initArguments2 = default(CollectPointsAction.InitArguments);
			initArguments2.SetCount(desiredCount, game.board.currentMoveMatches);
			initArguments2.localPosition = vector;
			initArguments2.game = game;
			initArguments2.itemColor = itemColor;
			CollectPointsAction collectPointsAction2 = new CollectPointsAction();
			collectPointsAction2.Init(initArguments2);
			game.board.actionManager.AddAction(collectPointsAction2);
		}

		public static void OnChipDestroy(Chip chip, SlotDestroyParams destroyParams)
		{
			CollectPointsAction.Settings collectPointsActionSettings = Match3Settings.instance.collectPointsActionSettings;
			if (!collectPointsActionSettings.enabled)
			{
				return;
			}
			if (chip == null)
			{
				return;
			}
			if (destroyParams == null)
			{
				return;
			}
			if (!collectPointsActionSettings.enableForGoals && chip.isPartOfActiveGoal)
			{
				return;
			}
			if (destroyParams.isFromSwapOrTap && !collectPointsActionSettings.enableForPowerupBlow && chip.isPowerup)
			{
				return;
			}
			if (chip.chipType == ChipType.Chip && !destroyParams.isHitByBomb)
			{
				return;
			}
			if (chip.isPickupElement)
			{
				return;
			}
			Slot lastConnectedSlot = chip.lastConnectedSlot;
			if (lastConnectedSlot == null)
			{
				return;
			}
			Match3Game game = lastConnectedSlot.game;
			ActionScore actionScore = game.goals.FreshActionScoreForDestroyingSlot(lastConnectedSlot);
			if (chip != null && chip.isPartOfActiveGoal)
			{
				actionScore.goalsCount++;
			}
			if (actionScore.goalsCount <= 0 && destroyParams.chipBlockersDestroyed <= 0 && !chip.isPowerup)
			{
				return;
			}
			CollectPointsAction.InitArguments initArguments = default(CollectPointsAction.InitArguments);
			initArguments.count = collectPointsActionSettings.count;
			if (chip.isPowerup)
			{
				initArguments.SetCount(collectPointsActionSettings.powerupCount, game.board.currentMoveMatches);
			}
			if (actionScore.goalsCount > 0 || actionScore.obstaclesDestroyed > 0)
			{
				initArguments.SetCount(collectPointsActionSettings.count, 1);
			}
			initArguments.localPosition = lastConnectedSlot.localPositionOfCenter;
			initArguments.game = game;
			initArguments.itemColor = chip.itemColor;
			CollectPointsAction collectPointsAction = new CollectPointsAction();
			collectPointsAction.Init(initArguments);
			game.board.actionManager.AddAction(collectPointsAction);
			destroyParams.scoreAdded++;
		}

		public static void OnSlotDestroy(Slot slot, SlotDestroyParams destroyParams)
		{
			CollectPointsAction.Settings collectPointsActionSettings = Match3Settings.instance.collectPointsActionSettings;
			if (!collectPointsActionSettings.enabled)
			{
				return;
			}
			if (destroyParams == null)
			{
				return;
			}
			if (destroyParams.scoreAdded > 0)
			{
				return;
			}
			if (destroyParams.chipsDestroyed > 0)
			{
				return;
			}
			if (destroyParams.chipBlockersDestroyed == 0)
			{
				return;
			}
			CollectPointsAction.InitArguments initArguments = default(CollectPointsAction.InitArguments);
			initArguments.count = collectPointsActionSettings.count;
			initArguments.localPosition = slot.localPositionOfCenter;
			initArguments.game = slot.game;
			initArguments.itemColor = ItemColor.Unknown;
			CollectPointsAction collectPointsAction = new CollectPointsAction();
			collectPointsAction.Init(initArguments);
			slot.game.board.actionManager.AddAction(collectPointsAction);
			destroyParams.scoreAdded++;
		}

		private CollectPointsAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.collectPointsActionSettings;
			}
		}

		public void Init(CollectPointsAction.InitArguments initArguments)
		{
			this.initArguments = initArguments;
		}

		private IEnumerator Move()
		{
			return new CollectPointsAction._003CMove_003Ed__14(0)
			{
				_003C_003E4__this = this
			};
		}

		private IEnumerator Appear()
		{
			return new CollectPointsAction._003CAppear_003Ed__15(0)
			{
				_003C_003E4__this = this
			};
		}

		private IEnumerator DoAnimation()
		{
			return new CollectPointsAction._003CDoAnimation_003Ed__16(0)
			{
				_003C_003E4__this = this
			};
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.deltaTime = deltaTime;
			if (this.animationEnum == null)
			{
				this.animationEnum = this.DoAnimation();
			}
			this.animationEnum.MoveNext();
		}

		private float deltaTime;

		private IEnumerator animationEnum;

		private CollectPointsAction.InitArguments initArguments;

		private static List<Slot> slotsWithGoals = new List<Slot>();

		private TransformBehaviour transformToMove;

		public struct InitArguments
		{
			public void SetCount(int desiredCount, int numMatches)
			{
				this.matchesCount = Mathf.Clamp(numMatches - 1, 0, 5);
				int num = this.matchesCount + 1;
				this.count = desiredCount * num;
			}

			public int count;

			public int matchesCount;

			public Vector3 localPosition;

			public Match3Game game;

			public bool isBlocker;

			public ItemColor itemColor;
		}

		[Serializable]
		public class Settings
		{
			public Color GetColor(ItemColor itemColor)
			{
				for (int i = 0; i < this.itemColors.Count; i++)
				{
					CollectPointsAction.Settings.ColorForItemColor colorForItemColor = this.itemColors[i];
					if (colorForItemColor.itemColor == itemColor)
					{
						return colorForItemColor.color;
					}
				}
				return Color.white;
			}

			public bool enabled;

			public bool enableForPowerupCreate;

			public bool enableForPowerupBlow;

			public bool enableForGoals;

			public int blockerCount = 30;

			public int count = 60;

			public int powerupCount = 100;

			public float fadeFrom;

			public float fadeTo;

			public float scaleFrom;

			public float scaleTo;

			public float scaleToEnd;

			public float scaleToBlocker;

			public int maxMatches;

			public AnimationCurve scaleCurve;

			public float appearDuration;

			public Vector3 travelOffset;

			public float travelDuration;

			public AnimationCurve travelFadeCurve;

			public float finalAlpha;

			[SerializeField]
			private List<CollectPointsAction.Settings.ColorForItemColor> itemColors = new List<CollectPointsAction.Settings.ColorForItemColor>();

			[Serializable]
			public class ColorForItemColor
			{
				[SerializeField]
				public ItemColor itemColor;

				[SerializeField]
				public Color color;
			}
		}

		private sealed class _003CMove_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CMove_003Ed__14(int _003C_003E1__state)
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
				CollectPointsAction collectPointsAction = this._003C_003E4__this;
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
					this._003Csettings_003E5__2 = collectPointsAction.settings;
					this._003CstartPos_003E5__3 = collectPointsAction.transformToMove.localPosition;
					this._003CendPos_003E5__4 = this._003CstartPos_003E5__3 + this._003Csettings_003E5__2.travelOffset;
					this._003Ctime_003E5__5 = 0f;
					this._003Cduration_003E5__6 = this._003Csettings_003E5__2.travelDuration;
				}
				if (this._003Ctime_003E5__5 > this._003Cduration_003E5__6)
				{
					return false;
				}
				this._003Ctime_003E5__5 += collectPointsAction.deltaTime;
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__6, this._003Ctime_003E5__5);
				float t = num2;
				float t2 = this._003Csettings_003E5__2.travelFadeCurve.Evaluate(num2);
				Vector3 localPosition = Vector3.LerpUnclamped(this._003CstartPos_003E5__3, this._003CendPos_003E5__4, t);
				float alpha = Mathf.Lerp(this._003Csettings_003E5__2.fadeTo, this._003Csettings_003E5__2.finalAlpha, t2);
				collectPointsAction.transformToMove.localPosition = localPosition;
				collectPointsAction.transformToMove.SetAlpha(alpha);
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

			public CollectPointsAction _003C_003E4__this;

			private CollectPointsAction.Settings _003Csettings_003E5__2;

			private Vector3 _003CstartPos_003E5__3;

			private Vector3 _003CendPos_003E5__4;

			private float _003Ctime_003E5__5;

			private float _003Cduration_003E5__6;
		}

		private sealed class _003CAppear_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CAppear_003Ed__15(int _003C_003E1__state)
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
				CollectPointsAction collectPointsAction = this._003C_003E4__this;
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
					this._003Csettings_003E5__2 = collectPointsAction.settings;
					this._003CstartScale_003E5__3 = Vector3.one * this._003Csettings_003E5__2.scaleFrom;
					this._003CendScale_003E5__4 = Vector3.one * Mathf.Lerp(this._003Csettings_003E5__2.scaleTo, this._003Csettings_003E5__2.scaleToEnd, Mathf.InverseLerp(0f, (float)this._003Csettings_003E5__2.maxMatches, (float)collectPointsAction.initArguments.matchesCount));
					if (collectPointsAction.initArguments.isBlocker)
					{
						this._003CendScale_003E5__4 = Vector3.one * this._003Csettings_003E5__2.scaleToBlocker;
					}
					this._003Ctime_003E5__5 = 0f;
					this._003Cduration_003E5__6 = this._003Csettings_003E5__2.appearDuration;
				}
				if (this._003Ctime_003E5__5 > this._003Cduration_003E5__6)
				{
					return false;
				}
				this._003Ctime_003E5__5 += collectPointsAction.deltaTime;
				float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__6, this._003Ctime_003E5__5);
				float t = this._003Csettings_003E5__2.scaleCurve.Evaluate(num2);
				float t2 = num2;
				Vector3 localScale = Vector3.LerpUnclamped(this._003CstartScale_003E5__3, this._003CendScale_003E5__4, t);
				float alpha = Mathf.Lerp(this._003Csettings_003E5__2.fadeFrom, this._003Csettings_003E5__2.fadeTo, t2);
				collectPointsAction.transformToMove.localScale = localScale;
				collectPointsAction.transformToMove.SetAlpha(alpha);
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

			public CollectPointsAction _003C_003E4__this;

			private CollectPointsAction.Settings _003Csettings_003E5__2;

			private Vector3 _003CstartScale_003E5__3;

			private Vector3 _003CendScale_003E5__4;

			private float _003Ctime_003E5__5;

			private float _003Cduration_003E5__6;
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
				CollectPointsAction collectPointsAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					collectPointsAction.transformToMove = collectPointsAction.initArguments.game.CreatePointsDisplay();
					if (collectPointsAction.transformToMove == null)
					{
						collectPointsAction.isAlive = false;
						return false;
					}
					collectPointsAction.transformToMove.localPosition = collectPointsAction.initArguments.localPosition;
					collectPointsAction.transformToMove.SetText(collectPointsAction.initArguments.count.ToString());
					collectPointsAction.transformToMove.SetColor(collectPointsAction.settings.GetColor(collectPointsAction.initArguments.itemColor));
					GGUtil.Show(collectPointsAction.transformToMove);
					this._003Canimation_003E5__2 = collectPointsAction.Appear();
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_125;
				default:
					return false;
				}
				if (this._003Canimation_003E5__2.MoveNext())
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				collectPointsAction.initArguments.game.OnScoreAdded(collectPointsAction.initArguments.count);
				this._003Canimation_003E5__2 = collectPointsAction.Move();
				IL_125:
				if (!this._003Canimation_003E5__2.MoveNext())
				{
					collectPointsAction.transformToMove.RemoveFromGame();
					collectPointsAction.isAlive = false;
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

			public CollectPointsAction _003C_003E4__this;

			private IEnumerator _003Canimation_003E5__2;
		}
	}
}
