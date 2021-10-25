using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class CrossExplosionAction : BoardAction
	{
		public void Init(CrossExplosionAction.Parameters parameters)
		{
			this.parameters = parameters;
			this.allAffectedSlots = new List<Slot>();
			this.circles = new List<CrossExplosionAction.Circle>();
			CrossExplosionAction.Circle item = new CrossExplosionAction.Circle();
			this.circles.Add(item);
			Slot slot = parameters.game.board.GetSlot(parameters.startPosition);
			if (slot != null)
			{
				this.allAffectedSlots.Add(slot);
			}
			CrossExplosionAction.Circle circle = new CrossExplosionAction.Circle();
			circle.Add(slot);
			this.circles.Add(circle);
			for (int i = 1; i <= parameters.radius; i++)
			{
				CrossExplosionAction.Circle circle2 = new CrossExplosionAction.Circle();
				this.circles.Add(circle2);
				IntVector2 startPosition = parameters.startPosition;
				Match3Board board = parameters.game.board;
				Slot slot2 = board.GetSlot(startPosition + IntVector2.up * i);
				Slot slot3 = board.GetSlot(startPosition + IntVector2.down * i);
				Slot slot4 = board.GetSlot(startPosition + IntVector2.left * i);
				Slot slot5 = board.GetSlot(startPosition + IntVector2.right * i);
				if (slot2 != null)
				{
					circle2.Add(slot2);
				}
				if (slot3 != null)
				{
					circle2.Add(slot3);
				}
				if (slot4 != null)
				{
					circle2.Add(slot4);
				}
				if (slot5 != null)
				{
					circle2.Add(slot5);
				}
				this.allAffectedSlots.AddRange(circle2.slots);
			}
			for (int j = 0; j < this.circles.Count; j++)
			{
				CrossExplosionAction.Circle circle3 = this.circles[j];
				for (int k = 0; k < circle3.slots.Count; k++)
				{
					Slot slot6 = circle3.slots[k];
					if (slot6 != null && slot6.canCarpetSpreadFromHere)
					{
						this.isHavingCarpet = true;
					}
				}
			}
			this.globalSlotLock = this.lockContainer.NewLock();
			this.globalSlotLock.isAvailableForDiscoBombSuspended = true;
			this.globalSlotLock.isSlotGravitySuspended = true;
			this.globalSlotLock.isChipGeneratorSuspended = true;
			this.globalSlotLock.isAboutToBeDestroyed = true;
			this.globalSlotLock.LockSlots(this.allAffectedSlots);
			parameters.game.particles.CreateParticles(parameters.game.LocalPositionOfCenter(parameters.startPosition), Match3Particles.PositionType.OnSeekingMissleExplosion, ChipType.SeekingMissle, ItemColor.Unknown);
		}

		public IEnumerator DoExplosion()
		{
			return new CrossExplosionAction._003CDoExplosion_003Ed__8(0)
			{
				_003C_003E4__this = this
			};
		}

		private void AffectOuterCircleWithExplosion(IntVector2 center, int radius, float shockWaveOffset)
		{
			Match3Game game = this.parameters.game;
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

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			if (!this.isAlive)
			{
				return;
			}
			if (this.explosionCoroutine == null)
			{
				this.explosionCoroutine = this.DoExplosion();
			}
			this.explosionCoroutine.MoveNext();
		}

		public List<Slot> GetAffectedSlots()
		{
			return this.allAffectedSlots;
		}

		private Lock globalSlotLock;

		private List<Slot> allAffectedSlots;

		private CrossExplosionAction.Parameters parameters;

		public List<CrossExplosionAction.Circle> circles = new List<CrossExplosionAction.Circle>();

		private bool isHavingCarpet;

		private IEnumerator explosionCoroutine;

		[Serializable]
		public class Settings
		{
			public float delay = 0.1f;

			public float lightIntensity = 0.7f;

			public FloatRange lightIntensityRange = new FloatRange(1.5f, 1f);

			public float maxLightDistance = 3f;

			public float lockReleaseDelay;

			public float shockWaveOffset = 0.05f;
		}

		public class Circle
		{
			public void Add(Slot slot)
			{
				this.slots.Add(slot);
			}

			public void AddLock(Lock slotLock)
			{
				for (int i = 0; i < this.slots.Count; i++)
				{
					Slot slot = this.slots[i];
					slotLock.LockSlot(slot);
				}
			}

			public float radius;

			public List<Slot> slots = new List<Slot>();
		}

		public class Parameters
		{
			public int radius = 1;

			public IntVector2 startPosition;

			public Match3Game game;

			public Chip bombChip;

			public bool isHavingCarpet;

			public bool explode;
		}

		private sealed class _003CDoExplosion_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoExplosion_003Ed__8(int _003C_003E1__state)
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
				CrossExplosionAction crossExplosionAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003CslotLock_003E5__2 = crossExplosionAction.lockContainer.NewLock();
					this._003CslotLock_003E5__2.isSlotGravitySuspended = true;
					this._003CbombParams_003E5__3 = new SlotDestroyParams();
					this._003CbombParams_003E5__3.isHitByBomb = true;
					this._003CbombParams_003E5__3.isHavingCarpet = crossExplosionAction.parameters.isHavingCarpet;
					this._003CexplosionSettings_003E5__4 = Match3Settings.instance.crossExplosionSettings;
					this._003CbombParams_003E5__3.isExplosion = crossExplosionAction.parameters.explode;
					this._003CbombParams_003E5__3.explosionCentre = crossExplosionAction.parameters.startPosition;
					if (crossExplosionAction.parameters.bombChip != null)
					{
						DestroyChipAction destroyChipAction = new DestroyChipAction();
						destroyChipAction.Init(new DestroyChipAction.InitArguments
						{
							chip = crossExplosionAction.parameters.bombChip,
							slot = crossExplosionAction.parameters.game.board.GetSlot(crossExplosionAction.parameters.startPosition)
						});
						crossExplosionAction.parameters.game.board.actionManager.AddAction(destroyChipAction);
					}
					this._003Ctime_003E5__5 = 0f;
					this._003Ci_003E5__6 = 0;
					goto IL_24C;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_2CF;
				default:
					return false;
				}
				IL_227:
				if (this._003Ctime_003E5__5 < this._003CexplosionSettings_003E5__4.delay)
				{
					this._003Ctime_003E5__5 += crossExplosionAction.parameters.game.board.currentDeltaTime;
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				IL_23A:
				int num2 = this._003Ci_003E5__6;
				this._003Ci_003E5__6 = num2 + 1;
				IL_24C:
				if (this._003Ci_003E5__6 >= crossExplosionAction.circles.Count)
				{
					crossExplosionAction.AffectOuterCircleWithExplosion(crossExplosionAction.parameters.startPosition, crossExplosionAction.parameters.radius, this._003CexplosionSettings_003E5__4.shockWaveOffset);
					this._003Ctime_003E5__5 = 0f;
				}
				else
				{
					bool flag = this._003Ci_003E5__6 == crossExplosionAction.circles.Count - 1;
					CrossExplosionAction.Circle circle = crossExplosionAction.circles[this._003Ci_003E5__6];
					circle.AddLock(this._003CslotLock_003E5__2);
					List<Slot> slots = circle.slots;
					for (int i = 0; i < slots.Count; i++)
					{
						Slot slot = slots[i];
						slot.OnDestroySlot(this._003CbombParams_003E5__3);
						slot.light.AddLight(this._003CexplosionSettings_003E5__4.lightIntensityRange.Lerp(Mathf.InverseLerp(0f, this._003CexplosionSettings_003E5__4.maxLightDistance, circle.radius)));
					}
					this._003Ctime_003E5__5 = 0f;
					if (!flag)
					{
						goto IL_227;
					}
					goto IL_23A;
				}
				IL_2CF:
				if (this._003Ctime_003E5__5 >= this._003CexplosionSettings_003E5__4.lockReleaseDelay)
				{
					this._003CslotLock_003E5__2.UnlockAll();
					crossExplosionAction.globalSlotLock.UnlockAll();
					crossExplosionAction.isAlive = false;
					return false;
				}
				this._003Ctime_003E5__5 += crossExplosionAction.parameters.game.board.currentDeltaTime;
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

			public CrossExplosionAction _003C_003E4__this;

			private Lock _003CslotLock_003E5__2;

			private SlotDestroyParams _003CbombParams_003E5__3;

			private CrossExplosionAction.Settings _003CexplosionSettings_003E5__4;

			private float _003Ctime_003E5__5;

			private int _003Ci_003E5__6;
		}
	}
}
