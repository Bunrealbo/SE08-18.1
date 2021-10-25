using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class DiscoBallDestroyAction : BoardAction
	{
		private DiscoBallDestroyAction.Settings settings
		{
			get
			{
				return Match3Settings.instance.discoBallDestroyActionSettings;
			}
		}

		public void Init(DiscoBallDestroyAction.DiscoParams discoParams)
		{
			this.discoParams = discoParams;
			this.allLockedSlots.Add(discoParams.originSlot);
			this.allLockedSlots.AddRange(discoParams.affectedSlotsList);
			this.stopGeneratorsLock = this.lockContainer.NewLock();
			this.stopGeneratorsLock.isChipGeneratorSuspended = true;
			discoParams.game.AddLockToAllSlots(this.stopGeneratorsLock);
			this.slotLock = this.lockContainer.NewLock();
			this.slotLock.isSlotGravitySuspended = true;
			this.slotLock.isChipGravitySuspended = false;
			this.slotLock.isSlotMatchingSuspended = true;
			this.slotLock.isAvailableForDiscoBombSuspended = true;
			this.slotComponentLock = new SlotComponentLock();
			this.slotComponentLock.isDestroySuspended = true;
			this.originChip = discoParams.originBomb;
			if (this.originChip != null)
			{
				this.originChip.RemoveFromSlot();
			}
			if (discoParams.otherBomb != null)
			{
				discoParams.otherBomb.RemoveFromGame();
				this.otherBombSlot = discoParams.otherBomb.lastConnectedSlot;
				this.allLockedSlots.Add(this.otherBombSlot);
			}
			this.slotLock.LockSlots(this.allLockedSlots);
			Match3Game game = discoParams.game;
			this.allAffectedChips.Clear();
			for (int i = 0; i < discoParams.affectedSlotsList.Count; i++)
			{
				Chip slotComponent = discoParams.affectedSlotsList[i].GetSlotComponent<Chip>();
				if (slotComponent != null)
				{
					this.allAffectedChips.Add(slotComponent);
					slotComponent.AddLock(this.slotComponentLock);
				}
			}
		}

		private IEnumerator ShakeChip(Chip chip, float delay, float duration, bool replaceWithBomb)
		{
			return new DiscoBallDestroyAction._003CShakeChip_003Ed__15(0)
			{
				_003C_003E4__this = this,
				chip = chip,
				delay = delay,
				duration = duration,
				replaceWithBomb = replaceWithBomb
			};
		}

		private IEnumerator ShakeSlot(Slot slot, float delay, float duration, bool replaceWithBomb, float lightIntensityWhenStart)
		{
			return new DiscoBallDestroyAction._003CShakeSlot_003Ed__16(0)
			{
				_003C_003E4__this = this,
				slot = slot,
				delay = delay,
				duration = duration,
				replaceWithBomb = replaceWithBomb,
				lightIntensityWhenStart = lightIntensityWhenStart
			};
		}

		private IEnumerator DestroyAnimation()
		{
			return new DiscoBallDestroyAction._003CDestroyAnimation_003Ed__17(0)
			{
				_003C_003E4__this = this
			};
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.deltaTime = deltaTime;
			if (this.destroyAnimation == null)
			{
				this.destroyAnimation = this.DestroyAnimation();
			}
			this.destroyAnimation.MoveNext();
		}

		private void AffectOuterCircleWithExplosion(IntVector2 center, int radius, float shockWaveOffset)
		{
			Match3Game game = this.discoParams.game;
			Vector3 b = game.LocalPositionOfCenter(center);
			for (int i = center.x - radius; i <= center.x + radius; i++)
			{
				for (int j = center.y - radius; j <= center.y + radius; j++)
				{
					int a = Mathf.Abs(center.x - i);
					int b2 = Mathf.Abs(center.y - j);
					if (Mathf.Max(a, b2) == radius)
					{
						Slot slot = game.board.GetSlot(new IntVector2(i, j));
						if (slot != null)
						{
							slot.offsetPosition = (slot.localPositionOfCenter - b).normalized * shockWaveOffset;
							slot.positionIntegrator.currentPosition = slot.offsetPosition;
						}
					}
				}
			}
		}

		private float deltaTime;

		private DiscoBallDestroyAction.DiscoParams discoParams;

		private Lock slotLock;

		private Lock stopGeneratorsLock;

		private SlotComponentLock slotComponentLock;

		private List<Slot> allLockedSlots = new List<Slot>();

		private List<Chip> allAffectedChips = new List<Chip>();

		private IEnumerator destroyAnimation;

		private Chip originChip;

		private Slot otherBombSlot;

		public class DiscoParams
		{
			public bool hasBolts
			{
				get
				{
					return this.bolts != null && this.bolts.Count > 0;
				}
			}

			public void InitWithItemColor(Slot originSlot, Match3Game game, ItemColor itemColor, bool replaceWithBombs)
			{
				this.game = game;
				this.originSlot = originSlot;
				this.itemColor = itemColor;
				foreach (Slot slot in game.board.slots)
				{
					if (slot != null && slot.CanParticipateInDiscoBombAffectedArea(itemColor, replaceWithBombs))
					{
						this.affectedSlotsList.Add(slot);
					}
				}
			}

			public Match3Game game;

			public Slot originSlot;

			public Chip originBomb;

			public List<Slot> affectedSlotsList = new List<Slot>();

			public bool replaceWithBombs;

			public ChipType bombType;

			public Chip otherBomb;

			public bool isInstant;

			public bool isHavingCarpet;

			public List<LightingBolt> bolts;

			public float affectorDuration;

			public ItemColor itemColor;
		}

		[Serializable]
		public class Settings
		{
			public FloatRange amplitudeRange;

			public float angleSpeed;

			public float delayBetweenAffectedSlots = 0.2f;

			public float initialShakeTime = 0.5f;

			public float shakeTimeForSlot = 0.5f;

			public float minTotalDuration = 2f;

			public float touchSlotLightIntensity = 0.6f;

			public float shockWaveOffset = 0.2f;

			public bool useCameraShake;

			public GeneralSettings.CameraShakeSettings cameraShake = new GeneralSettings.CameraShakeSettings();
		}

		private sealed class _003CShakeChip_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CShakeChip_003Ed__15(int _003C_003E1__state)
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
				DiscoBallDestroyAction discoBallDestroyAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003Cbeh_003E5__2 = this.chip.GetComponentBehaviour<TransformBehaviour>();
					if (this._003Cbeh_003E5__2 == null)
					{
						return false;
					}
					this._003Ctime_003E5__3 = 0f;
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_22A;
				default:
					return false;
				}
				if (this._003Ctime_003E5__3 < this.delay)
				{
					this._003Ctime_003E5__3 += discoBallDestroyAction.deltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				Slot lastConnectedSlot = this.chip.lastConnectedSlot;
				if (this.replaceWithBomb)
				{
					this.chip.RemoveFromGame();
					ChipType chipType = discoBallDestroyAction.discoParams.bombType;
					if (chipType == ChipType.HorizontalRocket || chipType == ChipType.VerticalRocket)
					{
						if (discoBallDestroyAction.discoParams.game.RandomRange(0, 100) >= 50)
						{
							chipType = ChipType.HorizontalRocket;
						}
						else
						{
							chipType = ChipType.VerticalRocket;
						}
					}
					this.chip = discoBallDestroyAction.discoParams.game.CreatePowerupInSlot(lastConnectedSlot, chipType);
					this.chip.AddLock(discoBallDestroyAction.slotComponentLock);
					this._003Cbeh_003E5__2 = this.chip.GetComponentBehaviour<TransformBehaviour>();
				}
				this._003Cdirection_003E5__4 = lastConnectedSlot.localPositionOfCenter - discoBallDestroyAction.discoParams.originSlot.localPositionOfCenter;
				if (this._003Cdirection_003E5__4 == Vector3.zero)
				{
					this._003Cdirection_003E5__4 = Vector3.up;
				}
				this._003Cdirection_003E5__4.Normalize();
				this._003CstartPostion_003E5__5 = Vector3.zero;
				this._003Camplitude_003E5__6 = discoBallDestroyAction.settings.amplitudeRange.Random();
				this._003Ctime_003E5__3 = 0f;
				IL_22A:
				if (this._003Ctime_003E5__3 > this.duration)
				{
					if (this._003Cbeh_003E5__2 != null)
					{
						this._003Cbeh_003E5__2.localOffsetPosition = Vector3.zero;
					}
					return false;
				}
				this._003Ctime_003E5__3 += discoBallDestroyAction.deltaTime;
				float num2 = discoBallDestroyAction.settings.angleSpeed * this._003Ctime_003E5__3;
				Vector3 localOffsetPosition = this._003CstartPostion_003E5__5 + this._003Cdirection_003E5__4 * (Mathf.Sin(num2 * 57.29578f) * this._003Camplitude_003E5__6);
				if (this._003Cbeh_003E5__2 != null)
				{
					this._003Cbeh_003E5__2.localOffsetPosition = localOffsetPosition;
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

			public Chip chip;

			public DiscoBallDestroyAction _003C_003E4__this;

			public float delay;

			public bool replaceWithBomb;

			public float duration;

			private TransformBehaviour _003Cbeh_003E5__2;

			private float _003Ctime_003E5__3;

			private Vector3 _003Cdirection_003E5__4;

			private Vector3 _003CstartPostion_003E5__5;

			private float _003Camplitude_003E5__6;
		}

		private sealed class _003CShakeSlot_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CShakeSlot_003Ed__16(int _003C_003E1__state)
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
				DiscoBallDestroyAction discoBallDestroyAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003Ctime_003E5__2 = 0f;
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_2C1;
				default:
					return false;
				}
				if (this._003Ctime_003E5__2 < this.delay)
				{
					this._003Ctime_003E5__2 += discoBallDestroyAction.deltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				Chip chip = this.slot.GetSlotComponent<Chip>();
				if (chip == null)
				{
					return false;
				}
				this._003Cbeh_003E5__3 = chip.GetComponentBehaviour<TransformBehaviour>();
				if (this._003Cbeh_003E5__3 == null)
				{
					return false;
				}
				if (this.replaceWithBomb)
				{
					chip.RemoveFromGame();
					ChipType chipType = discoBallDestroyAction.discoParams.bombType;
					if (chipType == ChipType.HorizontalRocket || chipType == ChipType.VerticalRocket)
					{
						if (discoBallDestroyAction.discoParams.game.RandomRange(0, 100) >= 50)
						{
							chipType = ChipType.HorizontalRocket;
						}
						else
						{
							chipType = ChipType.VerticalRocket;
						}
					}
					chip = discoBallDestroyAction.discoParams.game.CreatePowerupInSlot(this.slot, chipType);
					chip.AddLock(discoBallDestroyAction.slotComponentLock);
					this._003Cbeh_003E5__3 = chip.GetComponentBehaviour<TransformBehaviour>();
				}
				IntensityChange intensityChange = default(IntensityChange);
				intensityChange = intensityChange.Duration(this.duration);
				intensityChange = intensityChange.EaseCurve(GGMath.Ease.Linear);
				IntensityChange change = intensityChange.IntensityRange(this.lightIntensityWhenStart, this.lightIntensityWhenStart);
				intensityChange = default(IntensityChange);
				intensityChange = intensityChange.Delay(this.delay);
				intensityChange = intensityChange.Duration(0.5f);
				intensityChange = intensityChange.EaseCurve(GGMath.Ease.EaseOutCubic);
				IntensityChange change2 = intensityChange.IntensityRange(this.lightIntensityWhenStart, 0f);
				this.slot.light.AddIntensityChange(change);
				this.slot.light.AddIntensityChange(change2);
				this._003Cdirection_003E5__4 = this.slot.localPositionOfCenter - discoBallDestroyAction.discoParams.originSlot.localPositionOfCenter;
				if (this._003Cdirection_003E5__4 == Vector3.zero)
				{
					this._003Cdirection_003E5__4 = Vector3.up;
				}
				this._003Cdirection_003E5__4.Normalize();
				this._003CstartPostion_003E5__5 = Vector3.zero;
				this._003Camplitude_003E5__6 = discoBallDestroyAction.settings.amplitudeRange.Random();
				this._003Ctime_003E5__2 = 0f;
				IL_2C1:
				if (this._003Ctime_003E5__2 > this.duration)
				{
					if (this._003Cbeh_003E5__3 != null)
					{
						this._003Cbeh_003E5__3.localOffsetPosition = Vector3.zero;
					}
					return false;
				}
				this._003Ctime_003E5__2 += discoBallDestroyAction.deltaTime;
				float num2 = discoBallDestroyAction.settings.angleSpeed * this._003Ctime_003E5__2;
				Vector3 localOffsetPosition = this._003CstartPostion_003E5__5 + this._003Cdirection_003E5__4 * (Mathf.Sin(num2 * 57.29578f) * this._003Camplitude_003E5__6);
				if (this._003Cbeh_003E5__3 != null)
				{
					this._003Cbeh_003E5__3.localOffsetPosition = localOffsetPosition;
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

			public DiscoBallDestroyAction _003C_003E4__this;

			public float delay;

			public Slot slot;

			public bool replaceWithBomb;

			public float duration;

			public float lightIntensityWhenStart;

			private float _003Ctime_003E5__2;

			private TransformBehaviour _003Cbeh_003E5__3;

			private Vector3 _003Cdirection_003E5__4;

			private Vector3 _003CstartPostion_003E5__5;

			private float _003Camplitude_003E5__6;
		}

		private sealed class _003CDestroyAnimation_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDestroyAnimation_003Ed__17(int _003C_003E1__state)
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
				DiscoBallDestroyAction discoBallDestroyAction = this._003C_003E4__this;
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
					this._003Csettings_003E5__2 = discoBallDestroyAction.settings;
					this._003CaffectedSlots_003E5__3 = discoBallDestroyAction.discoParams.affectedSlotsList;
					float num2 = this._003Csettings_003E5__2.initialShakeTime;
					if (this._003CaffectedSlots_003E5__3.Count > 0)
					{
						num2 += this._003Csettings_003E5__2.delayBetweenAffectedSlots * (float)this._003CaffectedSlots_003E5__3.Count + this._003Csettings_003E5__2.shakeTimeForSlot;
					}
					num2 = Mathf.Max(this._003Csettings_003E5__2.minTotalDuration, num2);
					if (discoBallDestroyAction.discoParams.hasBolts && discoBallDestroyAction.discoParams.isInstant)
					{
						num2 = Mathf.Max(0f, num2 * 0.5f - discoBallDestroyAction.discoParams.affectorDuration);
					}
					else if (discoBallDestroyAction.discoParams.isInstant)
					{
						num2 = 0f;
					}
					this._003CenumList_003E5__4 = new EnumeratorsList();
					this._003CenumList_003E5__4.Add(discoBallDestroyAction.ShakeChip(discoBallDestroyAction.discoParams.originBomb, 0f, num2, false), 0f, null, null, false);
					this._003CisHavingCarpet_003E5__5 = discoBallDestroyAction.discoParams.isHavingCarpet;
					for (int i = 0; i < this._003CaffectedSlots_003E5__3.Count; i++)
					{
						Slot slot = this._003CaffectedSlots_003E5__3[i];
						float num3 = (float)i * this._003Csettings_003E5__2.delayBetweenAffectedSlots;
						if (discoBallDestroyAction.discoParams.isInstant)
						{
							num3 = 0f;
						}
						if (slot.canCarpetSpreadFromHere)
						{
							this._003CisHavingCarpet_003E5__5 = true;
						}
						this._003CenumList_003E5__4.Add(discoBallDestroyAction.ShakeSlot(slot, num3, num2 - num3, discoBallDestroyAction.discoParams.replaceWithBombs, this._003Csettings_003E5__2.touchSlotLightIntensity), 0f, null, null, false);
					}
				}
				if (!this._003CenumList_003E5__4.Update())
				{
					for (int j = 0; j < discoBallDestroyAction.discoParams.affectedSlotsList.Count; j++)
					{
						Chip slotComponent = discoBallDestroyAction.discoParams.affectedSlotsList[j].GetSlotComponent<Chip>();
						if (slotComponent != null)
						{
							slotComponent.RemoveLock(discoBallDestroyAction.slotComponentLock);
						}
					}
					SlotDestroyParams slotDestroyParams = new SlotDestroyParams();
					slotDestroyParams.isHitByBomb = true;
					slotDestroyParams.isHavingCarpet = this._003CisHavingCarpet_003E5__5;
					slotDestroyParams.isBombAllowingNeighbourDestroy = true;
					slotDestroyParams.bombType = ChipType.DiscoBall;
					bool flag = !discoBallDestroyAction.discoParams.replaceWithBombs;
					List<Slot> list = new List<Slot>();
					list.AddRange(this._003CaffectedSlots_003E5__3);
					list.Add(discoBallDestroyAction.discoParams.originSlot);
					if (this._003Csettings_003E5__2.useCameraShake && !discoBallDestroyAction.discoParams.replaceWithBombs)
					{
						discoBallDestroyAction.discoParams.game.ShakeCamera(this._003Csettings_003E5__2.cameraShake);
					}
					for (int k = 0; k < list.Count; k++)
					{
						Slot slot2 = list[k];
						if (slot2 != null)
						{
							slot2.OnDestroySlot(slotDestroyParams);
							discoBallDestroyAction.AffectOuterCircleWithExplosion(slot2.position, 1, Match3Settings.instance.discoBallDestroyActionSettings.shockWaveOffset);
							if (flag)
							{
								List<Slot> neigbourSlots = slot2.neigbourSlots;
								for (int l = 0; l < neigbourSlots.Count; l++)
								{
									neigbourSlots[l].OnDestroyNeighbourSlot(slot2, slotDestroyParams);
								}
							}
						}
					}
					discoBallDestroyAction.discoParams.game.Play(GGSoundSystem.SFXType.DiscoBallExplode);
					if (discoBallDestroyAction.originChip != null)
					{
						DestroyChipActionGrow destroyChipActionGrow = new DestroyChipActionGrow();
						destroyChipActionGrow.Init(discoBallDestroyAction.originChip, discoBallDestroyAction.originChip.lastConnectedSlot);
						discoBallDestroyAction.discoParams.game.board.actionManager.AddAction(destroyChipActionGrow);
					}
					DiscoBallAffector.RemoveFromGame(discoBallDestroyAction.discoParams.bolts);
					discoBallDestroyAction.stopGeneratorsLock.UnlockAll();
					discoBallDestroyAction.slotLock.UnlockAll();
					discoBallDestroyAction.isAlive = false;
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

			public DiscoBallDestroyAction _003C_003E4__this;

			private DiscoBallDestroyAction.Settings _003Csettings_003E5__2;

			private List<Slot> _003CaffectedSlots_003E5__3;

			private EnumeratorsList _003CenumList_003E5__4;

			private bool _003CisHavingCarpet_003E5__5;
		}
	}
}
