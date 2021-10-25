using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class CreatePowerupAction : BoardAction
	{
		public CreatePowerupAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.createPowerupActionSettings;
			}
		}

		public void AddChip(Chip chip, LightingBolt bolt)
		{
			CreatePowerupAction.ChipToMoveDescriptor chipToMoveDescriptor = new CreatePowerupAction.ChipToMoveDescriptor();
			chipToMoveDescriptor.chip = chip;
			chipToMoveDescriptor.bolt = bolt;
			this.chipsToMove.Add(chipToMoveDescriptor);
		}

		public void Init(CreatePowerupAction.CreateParams createParams)
		{
			this.createParams = createParams;
			Match3Game game = createParams.game;
			this.slotLock = this.lockContainer.NewLock();
			this.slotLock.isSlotGravitySuspended = true;
			this.slotLock.isChipGeneratorSuspended = true;
			this.powerupCreateLock = this.lockContainer.NewLock();
			this.powerupCreateLock.isDestroySuspended = true;
			this.powerupCreateLock.isSlotGravitySuspended = true;
			this.powerupCreateLock.isSlotMatchingSuspended = true;
			this.powerupCreateLock.isSlotSwipeSuspended = true;
			this.powerupCreateLock.isAttachGrowingElementSuspended = true;
			for (int i = 0; i < this.chipsToMove.Count; i++)
			{
				Chip chip = this.chipsToMove[i].chip;
				chip.RemoveFromSlot();
				this.slots.Add(chip.lastConnectedSlot);
			}
			this.slotLock.LockSlots(this.slots);
			float lightIntensity = this.settings.lightIntensity;
			for (int j = 0; j < this.chipsToMove.Count; j++)
			{
				Slot lastConnectedSlot = this.chipsToMove[j].chip.lastConnectedSlot;
				if (lastConnectedSlot != null)
				{
					lastConnectedSlot.light.AddLightWithDuration(lightIntensity, this.settings.lightDuration);
				}
			}
			if (this.settings.useParticles)
			{
				for (int k = 0; k < this.chipsToMove.Count; k++)
				{
					Chip chip2 = this.chipsToMove[k].chip;
					game.particles.CreateParticles(chip2, Match3Particles.PositionType.OnDestroyChip, chip2.chipType, chip2.itemColor);
				}
			}
			Slot slot = game.GetSlot(createParams.positionWherePowerupWillBeCreated);
			this.powerupCreateLock.LockSlot(slot);
			game.Play(GGSoundSystem.SFXType.CreatePowerup);
		}

		private IEnumerator PowerupCreation()
		{
			return new CreatePowerupAction._003CPowerupCreation_003Ed__14(0)
			{
				_003C_003E4__this = this
			};
		}

		private IEnumerator CreatePowerupAnimation()
		{
			return new CreatePowerupAction._003CCreatePowerupAnimation_003Ed__15(0)
			{
				_003C_003E4__this = this
			};
		}

		private float GetDistance(Chip chip)
		{
			IntVector2 intVector = this.createParams.positionWherePowerupWillBeCreated - chip.lastConnectedSlot.position;
			return (float)Mathf.Max(Mathf.Abs(intVector.x), Mathf.Abs(intVector.y));
		}

		private IEnumerator MoveSingleChip(CreatePowerupAction.ChipToMoveDescriptor chipDescriptor, int index)
		{
			return new CreatePowerupAction._003CMoveSingleChip_003Ed__18(0)
			{
				_003C_003E4__this = this,
				chipDescriptor = chipDescriptor,
				index = index
			};
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.deltaTime = deltaTime;
			if (this.animation == null)
			{
				this.animation = this.CreatePowerupAnimation();
			}
			this.animation.MoveNext();
		}

		private CreatePowerupAction.CreateParams createParams;

		private List<CreatePowerupAction.ChipToMoveDescriptor> chipsToMove = new List<CreatePowerupAction.ChipToMoveDescriptor>();

		private List<Slot> slots = new List<Slot>();

		private float deltaTime;

		private IEnumerator animation;

		private Lock slotLock;

		private Lock powerupCreateLock;

		public struct CreateParams
		{
			public Match3Game game;

			public IntVector2 positionWherePowerupWillBeCreated;

			public ChipType powerupToCreate;

			public int addCoins;
		}

		[Serializable]
		public class Settings
		{
			public bool useParticles;

			public float chipStartScale;

			public float chipEndScale;

			public float chipStartAlpha;

			public float chipEndAlpha;

			public float durationForChip;

			public float durationForGoalMin;

			public float durationForGoalMax = 1f;

			public AnimationCurve positionCurve;

			public float lightIntensity = 0.4f;

			public float lightDuration = 1f;

			public float goalDelay = 0.1f;

			public bool skipScale;

			public float delayForPowerup;

			public float durationForPowerup;

			public AnimationCurve powerupCurve;

			public float startScale = 2f;

			public float startAlpha = 0.5f;

			public int lightRadius = 1;

			public float powerupLigtIntensity = 1f;

			public float additionalKeepLock = 0.3f;

			public float holdGravityDuration = 0.4f;
		}

		public class ChipToMoveDescriptor
		{
			public Chip chip;

			public LightingBolt bolt;
		}

		private sealed class _003CPowerupCreation_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CPowerupCreation_003Ed__14(int _003C_003E1__state)
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
				CreatePowerupAction createPowerupAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003Ctime_003E5__2 = 0f;
					this._003Cgame_003E5__3 = createPowerupAction.createParams.game;
					this._003CpowerupSlot_003E5__4 = this._003Cgame_003E5__3.GetSlot(createPowerupAction.createParams.positionWherePowerupWillBeCreated);
					this._003Cgame_003E5__3.particles.CreateParticles(this._003CpowerupSlot_003E5__4, Match3Particles.PositionType.BombCreate);
					this._003Cgame_003E5__3.particles.CreateParticles(this._003CpowerupSlot_003E5__4, Match3Particles.PositionType.PlacePowerupParticles);
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
                        if (this._003Ctime_003E5__2 <= this._003Cduration_003E5__6)
                        {
                            this._003Ctime_003E5__2 += createPowerupAction.deltaTime;
                            float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__6, this._003Ctime_003E5__2);
                            num2 = this._003Ccurve_003E5__7.Evaluate(num2);
                            Vector3 localScale = Vector3.LerpUnclamped(new Vector3(createPowerupAction.settings.startScale, createPowerupAction.settings.startScale, 1f), Vector3.one, num2);
                            float alpha = Mathf.Lerp(createPowerupAction.settings.startAlpha, 1f, num2);
                            if (this._003CchipTransform_003E5__5 != null)
                            {
                                this._003CchipTransform_003E5__5.localScale = localScale;
                                this._003CchipTransform_003E5__5.SetAlpha(alpha);
                            }
                            this._003C_003E2__current = null;
                            this._003C_003E1__state = 2;
                            return true;
                        }
                        this._003Ctime_003E5__2 = 0f;
                        break;
                    case 3:
					this._003C_003E1__state = -1;
                        if (this._003Ctime_003E5__2 > createPowerupAction.settings.additionalKeepLock)
                        {
                            createPowerupAction.powerupCreateLock.UnlockAll();
                            return false;
                        }
                        break;
                    default:
					return false;
				}
				if (this._003Ctime_003E5__2 <= createPowerupAction.settings.delayForPowerup)
				{
					this._003Ctime_003E5__2 += createPowerupAction.deltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				Chip slotComponent = this._003CpowerupSlot_003E5__4.GetSlotComponent<Chip>();
				if (slotComponent != null)
				{
					slotComponent.RemoveFromGame();
				}
				Chip chip = this._003Cgame_003E5__3.CreatePowerupInSlot(this._003CpowerupSlot_003E5__4, createPowerupAction.createParams.powerupToCreate);
				chip.carriesCoins = createPowerupAction.createParams.addCoins;
				IntVector2 positionWherePowerupWillBeCreated = createPowerupAction.createParams.positionWherePowerupWillBeCreated;
				int lightRadius = createPowerupAction.settings.lightRadius;
				for (int i = positionWherePowerupWillBeCreated.x - lightRadius; i <= positionWherePowerupWillBeCreated.x + lightRadius; i++)
				{
					for (int j = positionWherePowerupWillBeCreated.y - lightRadius; j <= positionWherePowerupWillBeCreated.y + lightRadius; j++)
					{
						Slot slot = this._003Cgame_003E5__3.GetSlot(new IntVector2(i, j));
						if (slot != null)
						{
							slot.light.AddLight(createPowerupAction.settings.powerupLigtIntensity);
						}
					}
				}
				this._003CchipTransform_003E5__5 = chip.GetComponentBehaviour<TransformBehaviour>();
				this._003Ctime_003E5__2 = 0f;
				this._003Cduration_003E5__6 = createPowerupAction.settings.durationForPowerup;
				this._003Ccurve_003E5__7 = createPowerupAction.settings.powerupCurve;
                this._003Ctime_003E5__2 += createPowerupAction.deltaTime;
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

			public CreatePowerupAction _003C_003E4__this;

			private float _003Ctime_003E5__2;

			private Match3Game _003Cgame_003E5__3;

			private Slot _003CpowerupSlot_003E5__4;

			private TransformBehaviour _003CchipTransform_003E5__5;

			private float _003Cduration_003E5__6;

			private AnimationCurve _003Ccurve_003E5__7;
		}

		private sealed class _003CCreatePowerupAnimation_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CCreatePowerupAnimation_003Ed__15(int _003C_003E1__state)
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
				CreatePowerupAction createPowerupAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
				{
					this._003C_003E1__state = -1;
					this._003CenumList_003E5__2 = new EnumeratorsList();
					this._003Ctime_003E5__3 = 0f;
					float lightIntensity = createPowerupAction.settings.lightIntensity;
					float a = 0f;
					for (int i = 0; i < createPowerupAction.chipsToMove.Count; i++)
					{
						CreatePowerupAction.ChipToMoveDescriptor chipToMoveDescriptor = createPowerupAction.chipsToMove[i];
						a = Mathf.Max(a, createPowerupAction.GetDistance(chipToMoveDescriptor.chip));
					}
					for (int j = 0; j < createPowerupAction.chipsToMove.Count; j++)
					{
						CreatePowerupAction.ChipToMoveDescriptor chipDescriptor = createPowerupAction.chipsToMove[j];
						this._003CenumList_003E5__2.Add(createPowerupAction.MoveSingleChip(chipDescriptor, j), 0f, null, null, false);
					}
					this._003CenumList_003E5__2.Add(createPowerupAction.PowerupCreation(), 0f, null, null, false);
					break;
				}
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_16B;
				default:
					return false;
				}
				if (this._003CenumList_003E5__2.Update())
				{
					this._003Ctime_003E5__3 += Time.deltaTime;
					if (this._003Ctime_003E5__3 > createPowerupAction.settings.holdGravityDuration)
					{
						createPowerupAction.slotLock.UnlockAll();
					}
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				IL_16B:
				if (this._003Ctime_003E5__3 >= createPowerupAction.settings.holdGravityDuration)
				{
					createPowerupAction.slotLock.UnlockAll();
					createPowerupAction.powerupCreateLock.UnlockAll();
					createPowerupAction.isAlive = false;
					return false;
				}
				this._003Ctime_003E5__3 += Time.deltaTime;
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

			public CreatePowerupAction _003C_003E4__this;

			private EnumeratorsList _003CenumList_003E5__2;

			private float _003Ctime_003E5__3;
		}

		private sealed class _003CMoveSingleChip_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CMoveSingleChip_003Ed__18(int _003C_003E1__state)
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
				CreatePowerupAction createPowerupAction = this._003C_003E4__this;
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
					this._003Cchip_003E5__2 = this.chipDescriptor.chip;
					this._003Cbolt_003E5__3 = this.chipDescriptor.bolt;
					this._003Cgame_003E5__4 = createPowerupAction.createParams.game;
					this._003CchipTransform_003E5__5 = this._003Cchip_003E5__2.GetComponentBehaviour<TransformBehaviour>();
					this._003CstartPosition_003E5__6 = this._003Cchip_003E5__2.lastConnectedSlot.localPositionOfCenter;
					if (this._003CchipTransform_003E5__5 != null)
					{
						this._003CstartPosition_003E5__6 = this._003CchipTransform_003E5__5.localPosition;
					}
					this._003CendPosition_003E5__7 = createPowerupAction.createParams.game.LocalPositionOfCenter(createPowerupAction.createParams.positionWherePowerupWillBeCreated);
					this._003Ctime_003E5__8 = 0f;
					this._003Cduration_003E5__9 = createPowerupAction.settings.durationForChip;
					this._003CpositionCurve_003E5__10 = createPowerupAction.settings.positionCurve;
				}
				if (this._003Ctime_003E5__8 <= this._003Cduration_003E5__9)
				{
					this._003Ctime_003E5__8 += createPowerupAction.deltaTime;
					float num2 = Mathf.InverseLerp(0f, this._003Cduration_003E5__9, this._003Ctime_003E5__8);
					num2 = this._003CpositionCurve_003E5__10.Evaluate(num2);
					float alpha = Mathf.Lerp(createPowerupAction.settings.chipStartAlpha, createPowerupAction.settings.chipEndAlpha, num2);
					Vector3 vector = Vector3.LerpUnclamped(this._003CstartPosition_003E5__6, this._003CendPosition_003E5__7, num2);
					Vector3 localScale = Vector3.Lerp(new Vector3(createPowerupAction.settings.chipStartScale, createPowerupAction.settings.chipStartScale, 1f), new Vector3(createPowerupAction.settings.chipEndScale, createPowerupAction.settings.chipEndScale, 1f), num2);
					if (this._003CchipTransform_003E5__5 != null)
					{
						this._003CchipTransform_003E5__5.localPosition = vector;
						this._003CchipTransform_003E5__5.localScale = localScale;
						this._003CchipTransform_003E5__5.SetAlpha(alpha);
					}
					if (this._003Cbolt_003E5__3 != null)
					{
						this._003Cbolt_003E5__3.SetEndPosition(vector);
					}
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				if (this._003Cbolt_003E5__3 != null)
				{
					this._003Cbolt_003E5__3.RemoveFromGame();
				}
				Match3Goals.GoalBase goalBase = null;
				if (this._003Cchip_003E5__2.hasGrowingElement)
				{
					Match3Goals.ChipTypeDef chipTypeDef = default(Match3Goals.ChipTypeDef);
					chipTypeDef.chipType = ChipType.GrowingElementPiece;
					chipTypeDef.itemColor = this._003Cchip_003E5__2.itemColor;
					goalBase = this._003Cgame_003E5__4.goals.GetActiveGoal(chipTypeDef);
				}
				if (goalBase == null)
				{
					Match3Goals.ChipTypeDef chipTypeDef2 = Match3Goals.ChipTypeDef.Create(this._003Cchip_003E5__2);
					goalBase = this._003Cgame_003E5__4.goals.GetActiveGoal(chipTypeDef2);
				}
				if (goalBase == null)
				{
					this._003Cchip_003E5__2.RemoveFromGame();
					return false;
				}
				if (this._003Cchip_003E5__2.chipType == ChipType.Chip && this._003Cchip_003E5__2.isFeatherShown)
				{
					Slot slot = this._003Cgame_003E5__4.GetSlot(createPowerupAction.createParams.positionWherePowerupWillBeCreated);
					slot = this._003Cchip_003E5__2.lastConnectedSlot;
					this._003Cgame_003E5__4.particles.CreateParticles(slot.localPositionOfCenter, Match3Particles.PositionType.OnDestroyChip, this._003Cchip_003E5__2.chipType, this._003Cchip_003E5__2.itemColor);
					CollectGoalAction collectGoalAction = new CollectGoalAction();
					collectGoalAction.Init(new CollectGoalAction.CollectGoalParams
					{
						chip = null,
						chipSlot = slot,
						moveTransform = this._003Cgame_003E5__4.CreateChipFeather(slot, this._003Cchip_003E5__2.itemColor),
						game = this._003Cgame_003E5__4,
						goal = goalBase,
						skipScale = createPowerupAction.settings.skipScale,
						smallScale = true
					});
					this._003Cgame_003E5__4.board.actionManager.AddAction(collectGoalAction);
					this._003Cchip_003E5__2.RemoveFromGame();
				}
				else
				{
					CollectGoalAction collectGoalAction2 = new CollectGoalAction();
					collectGoalAction2.Init(new CollectGoalAction.CollectGoalParams
					{
						chip = this._003Cchip_003E5__2,
						chipSlot = this._003Cchip_003E5__2.lastConnectedSlot,
						game = this._003Cgame_003E5__4,
						goal = goalBase,
						delay = (float)this.index * createPowerupAction.settings.goalDelay,
						skipScale = createPowerupAction.settings.skipScale,
						smallScale = true
					});
					this._003Cgame_003E5__4.board.actionManager.AddAction(collectGoalAction2);
				}
				return false;
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

			public CreatePowerupAction.ChipToMoveDescriptor chipDescriptor;

			public CreatePowerupAction _003C_003E4__this;

			public int index;

			private Chip _003Cchip_003E5__2;

			private LightingBolt _003Cbolt_003E5__3;

			private Match3Game _003Cgame_003E5__4;

			private TransformBehaviour _003CchipTransform_003E5__5;

			private Vector3 _003CstartPosition_003E5__6;

			private Vector3 _003CendPosition_003E5__7;

			private float _003Ctime_003E5__8;

			private float _003Cduration_003E5__9;

			private AnimationCurve _003CpositionCurve_003E5__10;
		}
	}
}
