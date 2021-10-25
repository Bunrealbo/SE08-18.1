using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class ExplosionAction : BoardAction
	{
		public void Init(Match3Game game, ExplosionAction.ExplosionSettings settings)
		{
			this.game = game;
			this.settings = settings;
			if (settings.bombChip != null)
			{
				settings.bombChip.RemoveFromSlot();
			}
			this.circles = new List<ExplosionAction.CircleList>();
			this.isHavingCarpet = settings.isHavingCarpet;
			IntVector2 position = settings.position;
			List<Slot> bombArea = game.GetBombArea(position, settings.radius - 1);
			for (int i = 0; i < settings.radius; i++)
			{
				ExplosionAction.CircleList circleList = new ExplosionAction.CircleList();
				circleList.radius = i;
				this.circles.Add(circleList);
				if (settings.isUsingBombAreaOfEffect)
				{
					for (int j = 0; j < bombArea.Count; j++)
					{
						Slot slot = bombArea[j];
						if (slot != null)
						{
							IntVector2 intVector = slot.position - position;
							int a = Mathf.Abs(intVector.x);
							int b = Mathf.Abs(intVector.y);
							if (Mathf.Max(a, b) == i)
							{
								circleList.slotList.Add(slot);
								this.allAffectedSlots.Add(slot);
								if (slot.canCarpetSpreadFromHere)
								{
									this.isHavingCarpet = true;
								}
							}
						}
					}
				}
				else
				{
					for (int k = position.x - i; k <= position.x + i; k++)
					{
						for (int l = position.y - i; l <= position.y + i; l++)
						{
							int a2 = Mathf.Abs(position.x - k);
							int b2 = Mathf.Abs(position.y - l);
							if (Mathf.Max(a2, b2) == i)
							{
								Slot slot2 = game.board.GetSlot(new IntVector2(k, l));
								if (slot2 != null)
								{
									circleList.slotList.Add(slot2);
									this.allAffectedSlots.Add(slot2);
									if (slot2.canCarpetSpreadFromHere)
									{
										this.isHavingCarpet = true;
									}
								}
							}
						}
					}
				}
			}
			this.globalSlotLock = this.lockContainer.NewLock();
			this.globalSlotLock.isAvailableForDiscoBombSuspended = true;
			this.globalSlotLock.isSlotGravitySuspended = true;
			this.globalSlotLock.isChipGeneratorSuspended = true;
			this.globalSlotLock.isAboutToBeDestroyed = true;
			this.globalSlotLock.LockSlots(this.allAffectedSlots);
			game.particles.CreateParticles(game.LocalPositionOfCenter(position), Match3Particles.PositionType.OnExplosion, ChipType.Bomb, ItemColor.Unknown);
			game.Play(GGSoundSystem.SFXType.BombExplode);
		}

		public override void OnStart(ActionManager manager)
		{
			base.OnStart(manager);
		}

		private IEnumerator DoExplosion()
		{
			return new ExplosionAction._003CDoExplosion_003Ed__12(0)
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
			if (this.explosionEnumerator == null)
			{
				this.explosionEnumerator = this.DoExplosion();
				this.explosionEnumerator.MoveNext();
			}
			this.explosionEnumerator.MoveNext();
		}

		private void AffectOuterCircleWithExplosion(IntVector2 center, int radius, float shockWaveOffset)
		{
			Vector3 b = this.game.LocalPositionOfCenter(center);
			for (int i = center.x - radius; i <= center.x + radius; i++)
			{
				for (int j = center.y - radius; j <= center.y + radius; j++)
				{
					int a = Mathf.Abs(center.x - i);
					int b2 = Mathf.Abs(center.y - j);
					if (Mathf.Max(a, b2) == radius)
					{
						Slot slot = this.game.board.GetSlot(new IntVector2(i, j));
						if (slot != null)
						{
							slot.offsetPosition = (slot.localPositionOfCenter - b).normalized * shockWaveOffset;
							slot.positionIntegrator.currentPosition = slot.offsetPosition;
						}
					}
				}
			}
		}

		protected ExplosionAction.ExplosionSettings settings;

		protected Match3Game game;

		private IEnumerator explosionEnumerator;

		private Lock globalSlotLock;

		private List<ExplosionAction.CircleList> circles;

		private List<Slot> allAffectedSlots = new List<Slot>();

		private bool isHavingCarpet;

		public struct ExplosionSettings
		{
			public Chip bombChip;

			public IntVector2 position;

			public int radius;

			public bool isHavingCarpet;

			public bool isUsingBombAreaOfEffect;
		}

		[Serializable]
		public class Settings
		{
			public float delay = 0.1f;

			public float lightIntensity = 0.7f;

			public FloatRange lightIntensityRange = new FloatRange(1.5f, 1f);

			public float lightDuration = 1f;

			public float maxLightDistance = 3f;

			public float shockWaveOffset = 0.02f;

			public float shockWaveOffsetR1 = 0.01f;

			public bool useSecondaryDelay;

			public float secondaryDelay;

			public bool useCameraShake;
		}

		public class CircleList
		{
			public void AddLock(Lock slotLock)
			{
				if (slotLock == null)
				{
					return;
				}
				for (int i = 0; i < this.slotList.Count; i++)
				{
					Slot slot = this.slotList[i];
					slotLock.LockSlot(slot);
				}
			}

			public int radius;

			public List<Slot> slotList = new List<Slot>();
		}

		private sealed class _003CDoExplosion_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoExplosion_003Ed__12(int _003C_003E1__state)
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
				ExplosionAction explosionAction = this._003C_003E4__this;
				if (num == 0)
				{
					this._003C_003E1__state = -1;
					this._003CexplosionSettings_003E5__2 = Match3Settings.instance.explosionSettings;
					this._003Cdelay_003E5__3 = this._003CexplosionSettings_003E5__2.delay;
					this._003CslotLock_003E5__4 = explosionAction.lockContainer.NewLock();
					this._003CslotLock_003E5__4.isSlotGravitySuspended = true;
					this._003CbombParams_003E5__5 = new SlotDestroyParams();
					this._003CbombParams_003E5__5.isHitByBomb = true;
					this._003CbombParams_003E5__5.bombType = ChipType.Bomb;
					this._003CbombParams_003E5__5.isExplosion = true;
					this._003CbombParams_003E5__5.isHavingCarpet = explosionAction.isHavingCarpet;
					this._003CbombParams_003E5__5.explosionCentre = explosionAction.settings.position;
					if (explosionAction.settings.bombChip != null)
					{
						DestroyChipActionGrow destroyChipActionGrow = new DestroyChipActionGrow();
						destroyChipActionGrow.Init(explosionAction.settings.bombChip, explosionAction.settings.bombChip.lastConnectedSlot);
						explosionAction.game.board.actionManager.AddAction(destroyChipActionGrow);
					}
					if (this._003CexplosionSettings_003E5__2.useCameraShake)
					{
						explosionAction.game.ShakeCamera();
					}
					this._003Ci_003E5__6 = 0;
					goto IL_27C;
				}
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
				IL_25C:
				if (this._003Ctime_003E5__7 < this._003Cdelay_003E5__3)
				{
					this._003Ctime_003E5__7 += explosionAction.game.board.currentDeltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				IL_26A:
				int num2 = this._003Ci_003E5__6;
				this._003Ci_003E5__6 = num2 + 1;
				IL_27C:
				if (this._003Ci_003E5__6 >= explosionAction.circles.Count)
				{
					this._003CslotLock_003E5__4.UnlockAll();
					explosionAction.globalSlotLock.UnlockAll();
					explosionAction.AffectOuterCircleWithExplosion(explosionAction.settings.position, explosionAction.settings.radius, this._003CexplosionSettings_003E5__2.shockWaveOffset);
					explosionAction.AffectOuterCircleWithExplosion(explosionAction.settings.position, explosionAction.settings.radius + 1, this._003CexplosionSettings_003E5__2.shockWaveOffsetR1);
					explosionAction.isAlive = false;
					return false;
				}
				ExplosionAction.CircleList circleList = explosionAction.circles[this._003Ci_003E5__6];
				circleList.AddLock(this._003CslotLock_003E5__4);
				bool flag = this._003Ci_003E5__6 == explosionAction.circles.Count - 1;
				bool flag2 = this._003Ci_003E5__6 == 0;
				for (int i = 0; i < circleList.slotList.Count; i++)
				{
					Slot slot = circleList.slotList[i];
					slot.OnDestroySlot(this._003CbombParams_003E5__5);
					slot.light.AddLightWithDuration(this._003CexplosionSettings_003E5__2.lightIntensityRange.Lerp(Mathf.InverseLerp(0f, this._003CexplosionSettings_003E5__2.maxLightDistance, (float)circleList.radius)), this._003CexplosionSettings_003E5__2.lightDuration);
				}
				this._003Ctime_003E5__7 = 0f;
				this._003Cdelay_003E5__3 = this._003CexplosionSettings_003E5__2.delay;
				if (!flag2 && this._003CexplosionSettings_003E5__2.useSecondaryDelay)
				{
					this._003Cdelay_003E5__3 = this._003CexplosionSettings_003E5__2.secondaryDelay;
				}
				if (!flag && this._003Cdelay_003E5__3 > 0f)
				{
					goto IL_25C;
				}
				goto IL_26A;
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

			public ExplosionAction _003C_003E4__this;

			private ExplosionAction.Settings _003CexplosionSettings_003E5__2;

			private float _003Cdelay_003E5__3;

			private Lock _003CslotLock_003E5__4;

			private SlotDestroyParams _003CbombParams_003E5__5;

			private int _003Ci_003E5__6;

			private float _003Ctime_003E5__7;
		}
	}
}
