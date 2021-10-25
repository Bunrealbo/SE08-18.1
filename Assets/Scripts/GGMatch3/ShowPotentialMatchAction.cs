using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class ShowPotentialMatchAction : BoardAction
	{
		private ShowPotentialMatchAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.showPotentialMatchesSettings;
			}
		}

		private void Clear()
		{
			if (this.borderRenderer != null && this.initParams.game != null)
			{
				this.initParams.game.slotsRendererPool.ReturnRenderer(this.borderRenderer);
				this.borderRenderer = null;
			}
			this.matchingSlots.Clear();
			this.swipeSlots.Clear();
			this.animation = null;
		}

		private bool isMatchNoLongerValid
		{
			get
			{
				return !this.isMatchStillValid;
			}
		}

		private bool isMatchStillValid
		{
			get
			{
				if (this.initParams.game.board.isGameEnded)
				{
					return false;
				}
				if (!this.initParams.stayInfiniteTime && (this.initParams.userMoveWhenShow < this.initParams.game.board.userMovesCount || this.initParams.movesCountWhenConveyorTookAction < this.initParams.game.board.moveCountWhenConveyorTookAction || this.initParams.game.isConveyorMoving || !this.initParams.game.isBoardFullySettled))
				{
					return false;
				}
				for (int i = 0; i < this.matchingSlots.Count; i++)
				{
					ShowPotentialMatchAction.SlotChipPair slotChipPair = this.matchingSlots[i];
					if (slotChipPair.chip == null)
					{
						return false;
					}
					if (slotChipPair.slot == null)
					{
						return false;
					}
					if (slotChipPair.isChipChangedSlot)
					{
						return false;
					}
					if (slotChipPair.chip.isRemovedFromGame)
					{
						return false;
					}
					if (slotChipPair.slot.isSlotMatchingSuspended)
					{
						return false;
					}
					if (slotChipPair.slot.isLockedForDiscoBomb)
					{
						return false;
					}
				}
				for (int j = 0; j < this.swipeSlots.Count; j++)
				{
					ShowPotentialMatchAction.SlotChipPair slotChipPair2 = this.swipeSlots[j];
					if (slotChipPair2.isChipChangedSlot)
					{
						return false;
					}
					if (slotChipPair2.slot == null)
					{
						return false;
					}
					if (slotChipPair2.slot.isSlotMatchingSuspended)
					{
						return false;
					}
					if (slotChipPair2.slot.isLockedForDiscoBomb)
					{
						return false;
					}
					if (slotChipPair2.slot.isSlotGravitySuspended)
					{
						return false;
					}
					if (slotChipPair2.slot.isSlotSwapSuspended)
					{
						return false;
					}
				}
				return true;
			}
		}

		public void Init(ShowPotentialMatchAction.InitParams initParams)
		{
			this.Clear();
			this.initParams = initParams;
			Match3Game game = initParams.game;
			this.borderRenderer = game.slotsRendererPool.Next();
			this.listSlotsProvider.Init(game);
			this.listSlotsProvider.allSlots.Clear();
			if (initParams.tutorialMatch != null)
			{
				LevelDefinition.TutorialMatch tutorialMatch = initParams.tutorialMatch;
				for (int i = 0; i < tutorialMatch.matchingSlots.Count; i++)
				{
					IntVector2 position = tutorialMatch.matchingSlots[i];
					Slot slot = game.GetSlot(position);
					if (slot == null)
					{
						UnityEngine.Debug.LogError("(SHOULD NOT HAPPEN!) GETTING EMPTY SLOT FOR SAME COLOR");
					}
					else
					{
						Chip slotComponent = slot.GetSlotComponent<Chip>();
						if (slotComponent == null)
						{
							UnityEngine.Debug.LogError("(SHOULD NOT HAPPEN!) GETTING EMPTY CHIP FOR SAME COLOR");
						}
						else
						{
							ShowPotentialMatchAction.SlotChipPair item = new ShowPotentialMatchAction.SlotChipPair(slot, slotComponent);
							this.matchingSlots.Add(item);
							TilesSlotsProvider.Slot item2 = default(TilesSlotsProvider.Slot);
							item2.isOccupied = true;
							item2.position = slot.position;
							this.listSlotsProvider.allSlots.Add(item2);
						}
					}
				}
				if (!tutorialMatch.matchingSlots.Contains(tutorialMatch.exchangeSlot))
				{
					this.listSlotsProvider.allSlots.Add(new TilesSlotsProvider.Slot(tutorialMatch.exchangeSlot, true));
				}
				if (!tutorialMatch.matchingSlots.Contains(tutorialMatch.slotToSwipe))
				{
					this.listSlotsProvider.allSlots.Add(new TilesSlotsProvider.Slot(tutorialMatch.slotToSwipe, true));
				}
				Slot slot2 = game.GetSlot(tutorialMatch.slotToSwipe);
				Chip slotComponent2 = slot2.GetSlotComponent<Chip>();
				ShowPotentialMatchAction.SlotChipPair item3 = new ShowPotentialMatchAction.SlotChipPair(slot2, slotComponent2);
				item3.moveDirection = (tutorialMatch.exchangeSlot - tutorialMatch.slotToSwipe).ToVector3().normalized;
				this.matchingSlots.Add(item3);
				this.swipeSlots.Add(new ShowPotentialMatchAction.SlotChipPair(slot2, slotComponent2));
				Slot slot3 = game.GetSlot(tutorialMatch.exchangeSlot);
				Chip slotComponent3 = slot3.GetSlotComponent<Chip>();
				this.swipeSlots.Add(new ShowPotentialMatchAction.SlotChipPair(slot3, slotComponent3));
				this.borderRenderer.ShowBorderOnLevel(this.listSlotsProvider);
			}
			else if (initParams.powerupCombine != null)
			{
				List<Slot> list = this.helperList;
				list.Clear();
				list.Add(initParams.powerupCombine.powerupSlot);
				list.Add(initParams.powerupCombine.exchangeSlot);
				for (int j = 0; j < list.Count; j++)
				{
					Slot slot4 = list[j];
					Chip slotComponent4 = slot4.GetSlotComponent<Chip>();
					if (slotComponent4 != null)
					{
						ShowPotentialMatchAction.SlotChipPair item4 = new ShowPotentialMatchAction.SlotChipPair(slot4, slotComponent4);
						if (j == 0 && list.Count > 1)
						{
							item4.moveDirection = (list[1].position - slot4.position).ToVector3();
						}
						this.matchingSlots.Add(item4);
						this.swipeSlots.Add(item4);
						TilesSlotsProvider.Slot item5 = default(TilesSlotsProvider.Slot);
						item5.isOccupied = true;
						item5.position = slot4.position;
						this.listSlotsProvider.allSlots.Add(item5);
					}
				}
				this.borderRenderer.ShowBorderOnLevel(this.listSlotsProvider);
			}
			else if (initParams.powerupActivation != null)
			{
				List<Slot> list2 = this.helperList;
				list2.Clear();
				Slot powerupSlot = initParams.powerupActivation.powerupSlot;
				Slot exchangeSlot = initParams.powerupActivation.exchangeSlot;
				list2.Add(powerupSlot);
				if (initParams.powerupActivation.isSwipe)
				{
					list2.Add(exchangeSlot);
				}
				for (int k = 0; k < list2.Count; k++)
				{
					Slot slot5 = list2[k];
					Chip slotComponent5 = slot5.GetSlotComponent<Chip>();
					if (slotComponent5 != null)
					{
						ShowPotentialMatchAction.SlotChipPair item6 = new ShowPotentialMatchAction.SlotChipPair(slot5, slotComponent5);
						if (k == 0 && list2.Count > 1)
						{
							item6.moveDirection = (list2[1].position - slot5.position).ToVector3();
						}
						this.matchingSlots.Add(item6);
						this.swipeSlots.Add(item6);
						TilesSlotsProvider.Slot item7 = default(TilesSlotsProvider.Slot);
						item7.isOccupied = true;
						item7.position = slot5.position;
						this.listSlotsProvider.allSlots.Add(item7);
					}
				}
				this.borderRenderer.ShowBorderOnLevel(this.listSlotsProvider);
			}
			else if (initParams.potentialMatch != null)
			{
				PotentialMatches.CompoundSlotsSet potentialMatch = initParams.potentialMatch;
				List<PotentialMatches.SlotsSet> slotsSets = potentialMatch.slotsSets;
				for (int l = 0; l < slotsSets.Count; l++)
				{
					List<BoardRepresentation.RepresentationSlot> sameColorSlots = slotsSets[l].sameColorSlots;
					for (int m = 0; m < sameColorSlots.Count; m++)
					{
						BoardRepresentation.RepresentationSlot representationSlot = sameColorSlots[m];
						Slot slot6 = game.GetSlot(representationSlot.position);
						if (slot6 != null)
						{
							Chip slotComponent6 = slot6.GetSlotComponent<Chip>();
							if (slotComponent6 != null)
							{
								bool flag = false;
								for (int n = 0; n < this.matchingSlots.Count; n++)
								{
									if (this.matchingSlots[n].slot == slot6)
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									ShowPotentialMatchAction.SlotChipPair item8 = new ShowPotentialMatchAction.SlotChipPair(slot6, slotComponent6);
									this.matchingSlots.Add(item8);
									TilesSlotsProvider.Slot item9 = default(TilesSlotsProvider.Slot);
									item9.isOccupied = true;
									item9.position = slot6.position;
									this.listSlotsProvider.allSlots.Add(item9);
								}
							}
						}
					}
				}
				this.listSlotsProvider.allSlots.Add(new TilesSlotsProvider.Slot(potentialMatch.positionOfSlotMissingForMatch, true));
				BoardRepresentation.RepresentationSlot swipeSlot = potentialMatch.swipeSlot;
				if (!this.listSlotsProvider.ContainsPosition(swipeSlot.position))
				{
					this.listSlotsProvider.AddSlot(new TilesSlotsProvider.Slot(swipeSlot.position, true));
				}
				Slot slot7 = game.GetSlot(potentialMatch.swipeSlot.position);
				Chip slotComponent7 = slot7.GetSlotComponent<Chip>();
				ShowPotentialMatchAction.SlotChipPair item10 = new ShowPotentialMatchAction.SlotChipPair(slot7, slotComponent7);
				item10.moveDirection = (potentialMatch.positionOfSlotMissingForMatch - slot7.position).ToVector3().normalized;
				this.matchingSlots.Add(item10);
				this.swipeSlots.Add(new ShowPotentialMatchAction.SlotChipPair(slot7, slotComponent7));
				Slot slot8 = game.GetSlot(potentialMatch.positionOfSlotMissingForMatch);
				Chip slotComponent8 = slot8.GetSlotComponent<Chip>();
				this.swipeSlots.Add(new ShowPotentialMatchAction.SlotChipPair(slot8, slotComponent8));
				this.borderRenderer.ShowBorderOnLevel(this.listSlotsProvider);
			}
			base.Reset();
		}

		private IEnumerator DoMoveAnimation()
		{
			return new ShowPotentialMatchAction._003CDoMoveAnimation_003Ed__22(0)
			{
				_003C_003E4__this = this
			};
		}

		private IEnumerator DoSingleAnimation()
		{
			return new ShowPotentialMatchAction._003CDoSingleAnimation_003Ed__23(0)
			{
				_003C_003E4__this = this
			};
		}

		private IEnumerator DoAnimation()
		{
			return new ShowPotentialMatchAction._003CDoAnimation_003Ed__24(0)
			{
				_003C_003E4__this = this
			};
		}

		private void StopAction()
		{
			this.ReturnChipsToPositions();
			this.isAlive = false;
		}

		private void ReturnChipsToPositions()
		{
			for (int i = 0; i < this.matchingSlots.Count; i++)
			{
				ShowPotentialMatchAction.SlotChipPair slotChipPair = this.matchingSlots[i];
				slotChipPair.showMatchActionLocalScale = Vector3.one;
				slotChipPair.showMatchOffset = Vector3.zero;
				slotChipPair.brightness = 1f;
			}
			this.Clear();
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.deltaTime = deltaTime;
			if (!this.initParams.dontStopWhenInvalid && this.isMatchNoLongerValid)
			{
				this.StopAction();
				return;
			}
			if (this.animation == null)
			{
				this.animation = this.DoAnimation();
			}
			this.animation.MoveNext();
		}

		public override void Stop()
		{
			if (!this.isAlive)
			{
				return;
			}
			this.StopAction();
		}

		private ShowPotentialMatchAction.InitParams initParams;

		private List<ShowPotentialMatchAction.SlotChipPair> matchingSlots = new List<ShowPotentialMatchAction.SlotChipPair>();

		private List<ShowPotentialMatchAction.SlotChipPair> swipeSlots = new List<ShowPotentialMatchAction.SlotChipPair>();

		private IEnumerator animation;

		private float deltaTime;

		private TilesBorderRenderer borderRenderer;

		private TilesBorderRenderer maskRenderer;

		private ListSlotsProvider listSlotsProvider = new ListSlotsProvider();

		private List<Slot> helperList = new List<Slot>();

		public struct InitParams
		{
			public Match3Game game;

			public bool stayInfiniteTime;

			public LevelDefinition.TutorialMatch tutorialMatch;

			public PowerupCombines.PowerupCombine powerupCombine;

			public PowerupActivations.PowerupActivation powerupActivation;

			public PotentialMatches.CompoundSlotsSet potentialMatch;

			public int userMoveWhenShow;

			public int movesCountWhenConveyorTookAction;

			public bool dontStopWhenInvalid;
		}

		[Serializable]
		public class Settings
		{
			public ShowPotentialMatchAction.Settings.ShowPotentialMatchTimes GetPotentialTimesAction(ShowPotentialMatchSetting setting, bool hasPowerupCombines, Match3Game game)
			{
				float num = this.idleTimeBeforeShowMatch;
				float num2 = this.boardIdleTimeBeforeShowMatch;
				if (hasPowerupCombines)
				{
					num = this.idleTimeBeforePowerupCombine;
					num2 = this.boardIdleTimeBeforePowerupCombine;
				}
				ShowPotentialMatchAction.Settings.ShowPotentialMatchTimes result = default(ShowPotentialMatchAction.Settings.ShowPotentialMatchTimes);
				result.idleTimeBeforeShowMatch = num;
				result.boardIdleTimeBeforeShowMatch = num2;
				ShowPotentialMatchAction.Settings.ShowSettingsModifier settingsModifier = this.GetSettingsModifier(setting);
				if (settingsModifier != null)
				{
					result = settingsModifier.GetPotentialTimesAction(hasPowerupCombines, game);
				}
				return result;
			}

			private ShowPotentialMatchAction.Settings.ShowSettingsModifier GetSettingsModifier(ShowPotentialMatchSetting setting)
			{
				for (int i = 0; i < this.showSettingsModifiers.Count; i++)
				{
					ShowPotentialMatchAction.Settings.ShowSettingsModifier showSettingsModifier = this.showSettingsModifiers[i];
					if (showSettingsModifier.setting == setting)
					{
						return showSettingsModifier;
					}
				}
				return null;
			}

			public float duration = 1f;

			public float scaleDownDuration = 0.5f;

			public AnimationCurve scaleDownAnimation;

			public float waitBetweenFlashes = 2f;

			public AnimationCurve scaleAnimation;

			public bool useBrightness;

			public float brightnessMax = 1f;

			public AnimationCurve brightnessCurve;

			public float maxScale = 1.2f;

			public int maxFlashes = 10;

			public float stayBigDuration = 0.5f;

			public float idleTimeBeforeShowMatch = 2f;

			public float boardIdleTimeBeforeShowMatch = 2f;

			public float idleTimeBeforePowerupCombine = 1f;

			public float boardIdleTimeBeforePowerupCombine = 0.5f;

			public float moveDuration = 1f;

			public float moveDistance = 0.5f;

			public AnimationCurve moveCurve;

			public float moveDelay;

			[SerializeField]
			private List<ShowPotentialMatchAction.Settings.ShowSettingsModifier> showSettingsModifiers = new List<ShowPotentialMatchAction.Settings.ShowSettingsModifier>();

			public struct ShowPotentialMatchTimes
			{
				public float idleTimeBeforeShowMatch;

				public float boardIdleTimeBeforeShowMatch;
			}

			[Serializable]
			public class ShowSettingsModifier
			{
				public ShowPotentialMatchAction.Settings.ShowPotentialMatchTimes GetPotentialTimesAction(bool hasPowerupCombines, Match3Game game)
				{
					float num = this.idleTimeBeforeShowMatch;
					float num2 = this.boardIdleTimeBeforeShowMatch;
					if (hasPowerupCombines)
					{
						num = this.idleTimeBeforePowerupCombine;
						num2 = this.boardIdleTimeBeforePowerupCombine;
					}
					ShowPotentialMatchAction.Settings.ShowPotentialMatchTimes showPotentialMatchTimes = default(ShowPotentialMatchAction.Settings.ShowPotentialMatchTimes);
					showPotentialMatchTimes.idleTimeBeforeShowMatch = num;
					showPotentialMatchTimes.boardIdleTimeBeforeShowMatch = num2;
					if (this.useMaxTime)
					{
						float t = Mathf.InverseLerp(0f, (float)this.userMovesBeforeMaxTime, (float)game.board.userMovesCount);
						showPotentialMatchTimes.idleTimeBeforeShowMatch = Mathf.Lerp(showPotentialMatchTimes.idleTimeBeforeShowMatch, this.idleTimeBeforeShowMatchMax, t);
						showPotentialMatchTimes.boardIdleTimeBeforeShowMatch = Mathf.Lerp(showPotentialMatchTimes.boardIdleTimeBeforeShowMatch, this.boardIdleTimeBeforeShowMatchMax, t);
					}
					return showPotentialMatchTimes;
				}

				[SerializeField]
				public ShowPotentialMatchSetting setting;

				[SerializeField]
				private float idleTimeBeforeShowMatch;

				[SerializeField]
				private float boardIdleTimeBeforeShowMatch;

				[SerializeField]
				private float idleTimeBeforePowerupCombine;

				[SerializeField]
				private float boardIdleTimeBeforePowerupCombine;

				[SerializeField]
				private bool useMaxTime;

				[SerializeField]
				private int userMovesBeforeMaxTime;

				[SerializeField]
				private float idleTimeBeforeShowMatchMax;

				[SerializeField]
				private float boardIdleTimeBeforeShowMatchMax;
			}
		}

		private struct SlotChipPair
		{
			public TransformBehaviour transform
			{
				get
				{
					if (this.chip == null)
					{
						return null;
					}
					return this.chip.GetComponentBehaviour<TransformBehaviour>();
				}
			}

			public Vector3 showMatchOffset
			{
				set
				{
					TransformBehaviour transform = this.transform;
					if (transform == null)
					{
						return;
					}
					transform.localPotentialMatchOffsetPosition = value;
				}
			}

			public float brightness
			{
				set
				{
					TransformBehaviour transform = this.transform;
					if (transform == null)
					{
						return;
					}
					transform.SetBrightness(value);
				}
			}

			public Vector3 showMatchActionLocalScale
			{
				set
				{
					TransformBehaviour transform = this.transform;
					if (transform == null)
					{
						return;
					}
					transform.showMatchActionLocalScale = value;
				}
			}

			public bool isChipChangedSlot
			{
				get
				{
					return this.chip != null && this.chip.slot != this.slot;
				}
			}

			public SlotChipPair(Slot slot, Chip chip)
			{
				this.slot = slot;
				this.chip = chip;
				this.moveDirection = Vector3.zero;
			}

			public Slot slot;

			public Chip chip;

			public Vector3 moveDirection;
		}

		private sealed class _003CDoMoveAnimation_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoMoveAnimation_003Ed__22(int _003C_003E1__state)
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
				ShowPotentialMatchAction showPotentialMatchAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003Csettings_003E5__2 = showPotentialMatchAction.settings;
					break;
				case 1:
					this._003C_003E1__state = -1;
					goto IL_F6;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_15E;
				default:
					return false;
				}
				IL_35:
				this._003Ctime_003E5__3 = 0f;
				IL_F6:
				if (this._003Ctime_003E5__3 <= this._003Csettings_003E5__2.moveDuration * 2f)
				{
					this._003Ctime_003E5__3 += showPotentialMatchAction.deltaTime;
					float num2 = Mathf.PingPong(this._003Ctime_003E5__3, this._003Csettings_003E5__2.moveDuration);
					if (this._003Csettings_003E5__2.moveCurve != null)
					{
						num2 = this._003Csettings_003E5__2.moveCurve.Evaluate(num2);
					}
					for (int i = 0; i < showPotentialMatchAction.matchingSlots.Count; i++)
					{
						ShowPotentialMatchAction.SlotChipPair slotChipPair = showPotentialMatchAction.matchingSlots[i];
						Vector3 showMatchOffset = Vector3.LerpUnclamped(Vector3.zero, slotChipPair.moveDirection * this._003Csettings_003E5__2.moveDistance, num2);
						slotChipPair.showMatchOffset = showMatchOffset;
					}
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				this._003Ctime_003E5__3 = 0f;
				if (this._003Csettings_003E5__2.moveDelay <= 0f)
				{
					goto IL_35;
				}
				IL_15E:
				if (this._003Ctime_003E5__3 > this._003Csettings_003E5__2.moveDelay)
				{
					goto IL_35;
				}
				this._003Ctime_003E5__3 += showPotentialMatchAction.deltaTime;
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

			public ShowPotentialMatchAction _003C_003E4__this;

			private ShowPotentialMatchAction.Settings _003Csettings_003E5__2;

			private float _003Ctime_003E5__3;
		}

		private sealed class _003CDoSingleAnimation_003Ed__23 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoSingleAnimation_003Ed__23(int _003C_003E1__state)
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
				ShowPotentialMatchAction showPotentialMatchAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003Ctime_003E5__2 = 0f;
					this._003Csettings_003E5__3 = showPotentialMatchAction.settings;
					this._003Cduration_003E5__4 = this._003Csettings_003E5__3.duration;
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_1FB;
				case 3:
					this._003C_003E1__state = -1;
					goto IL_309;
				default:
					return false;
				}
				if (this._003Ctime_003E5__2 <= this._003Cduration_003E5__4)
				{
					this._003Ctime_003E5__2 += showPotentialMatchAction.deltaTime;
					float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__4, this._003Ctime_003E5__2);
					float num3 = num2;
					if (this._003Csettings_003E5__3.scaleAnimation != null)
					{
						num3 = this._003Csettings_003E5__3.scaleAnimation.Evaluate(num3);
					}
					Vector3 showMatchActionLocalScale = Vector3.LerpUnclamped(Vector3.one, new Vector3(this._003Csettings_003E5__3.maxScale, this._003Csettings_003E5__3.maxScale, 1f), num3);
					float t = num2;
					if (this._003Csettings_003E5__3.brightnessCurve != null)
					{
						t = this._003Csettings_003E5__3.brightnessCurve.Evaluate(num2);
					}
					float brightness = Mathf.LerpUnclamped(1f, this._003Csettings_003E5__3.brightnessMax, t);
					for (int i = 0; i < showPotentialMatchAction.matchingSlots.Count; i++)
					{
						ShowPotentialMatchAction.SlotChipPair slotChipPair = showPotentialMatchAction.matchingSlots[i];
						if (slotChipPair.slot.isSlotGravitySuspended)
						{
							slotChipPair.showMatchActionLocalScale = Vector3.one;
							if (this._003Csettings_003E5__3.useBrightness)
							{
								slotChipPair.brightness = 1f;
							}
						}
						else
						{
							slotChipPair.showMatchActionLocalScale = showMatchActionLocalScale;
							if (this._003Csettings_003E5__3.useBrightness)
							{
								slotChipPair.brightness = brightness;
							}
						}
					}
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				this._003Ctime_003E5__2 = 0f;
				this._003Cduration_003E5__4 = this._003Csettings_003E5__3.stayBigDuration;
				IL_1FB:
				if (this._003Ctime_003E5__2 <= this._003Cduration_003E5__4)
				{
					this._003Ctime_003E5__2 += showPotentialMatchAction.deltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 2;
					return true;
				}
				this._003Ctime_003E5__2 = 0f;
				this._003Cduration_003E5__4 = this._003Csettings_003E5__3.scaleDownDuration;
				IL_309:
				if (this._003Ctime_003E5__2 > this._003Cduration_003E5__4)
				{
					return false;
				}
				this._003Ctime_003E5__2 += showPotentialMatchAction.deltaTime;
				float num4 = Mathf.InverseLerp(0f, this._003Cduration_003E5__4, this._003Ctime_003E5__2);
				if (this._003Csettings_003E5__3.scaleDownAnimation != null)
				{
					num4 = this._003Csettings_003E5__3.scaleDownAnimation.Evaluate(num4);
				}
				Vector3 showMatchActionLocalScale2 = Vector3.LerpUnclamped(new Vector3(this._003Csettings_003E5__3.maxScale, this._003Csettings_003E5__3.maxScale, 1f), Vector3.one, num4);
				for (int j = 0; j < showPotentialMatchAction.matchingSlots.Count; j++)
				{
					ShowPotentialMatchAction.SlotChipPair slotChipPair2 = showPotentialMatchAction.matchingSlots[j];
					if (slotChipPair2.slot.isSlotGravitySuspended)
					{
						slotChipPair2.showMatchActionLocalScale = Vector3.one;
					}
					else
					{
						slotChipPair2.showMatchActionLocalScale = showMatchActionLocalScale2;
					}
				}
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

			public ShowPotentialMatchAction _003C_003E4__this;

			private float _003Ctime_003E5__2;

			private ShowPotentialMatchAction.Settings _003Csettings_003E5__3;

			private float _003Cduration_003E5__4;
		}

		private sealed class _003CDoAnimation_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoAnimation_003Ed__24(int _003C_003E1__state)
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
				ShowPotentialMatchAction showPotentialMatchAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003CsingleAnimation_003E5__2 = null;
					this._003CmoveAnimation_003E5__3 = showPotentialMatchAction.DoMoveAnimation();
					this._003Cflashes_003E5__4 = 0;
					break;
				case 1:
					this._003C_003E1__state = -1;
					this._003CmoveAnimation_003E5__3.MoveNext();
					goto IL_BD;
				case 2:
					this._003C_003E1__state = -1;
					break;
				default:
					return false;
				}
				if (this._003CsingleAnimation_003E5__2 == null)
				{
					this._003CsingleAnimation_003E5__2 = showPotentialMatchAction.DoSingleAnimation();
				}
				this._003CmoveAnimation_003E5__3.MoveNext();
				if (this._003CsingleAnimation_003E5__2.MoveNext())
				{
					goto IL_100;
				}
				this._003CsingleAnimation_003E5__2 = null;
				this._003Ctime_003E5__5 = 0f;
				IL_BD:
				if (this._003Ctime_003E5__5 <= showPotentialMatchAction.settings.waitBetweenFlashes)
				{
					this._003Ctime_003E5__5 += showPotentialMatchAction.deltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				int num2 = this._003Cflashes_003E5__4;
				this._003Cflashes_003E5__4 = num2 + 1;
				if (this._003Cflashes_003E5__4 > showPotentialMatchAction.settings.maxFlashes && !showPotentialMatchAction.initParams.stayInfiniteTime)
				{
					showPotentialMatchAction.StopAction();
					return false;
				}
				IL_100:
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

			public ShowPotentialMatchAction _003C_003E4__this;

			private IEnumerator _003CsingleAnimation_003E5__2;

			private IEnumerator _003CmoveAnimation_003E5__3;

			private int _003Cflashes_003E5__4;

			private float _003Ctime_003E5__5;
		}
	}
}
