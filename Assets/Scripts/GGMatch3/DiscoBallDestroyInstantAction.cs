using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GGMatch3
{
	public class DiscoBallDestroyInstantAction : BoardAction
	{
		public void Init(DiscoBallDestroyAction.DiscoParams discoParams)
		{
			this.discoParams = discoParams;
			this.slotLock = this.lockContainer.NewLock();
			this.slotLock.SuspendAll();
			this.slotLock.LockSlot(discoParams.originSlot);
			this.stopGeneratorsLock = this.lockContainer.NewLock();
			this.stopGeneratorsLock.isChipGeneratorSuspended = true;
			discoParams.game.AddLockToAllSlots(this.stopGeneratorsLock);
			this.originChip = discoParams.originBomb;
			if (this.originChip != null)
			{
				this.originChip.RemoveFromSlot();
			}
			if (discoParams.otherBomb != null)
			{
				discoParams.otherBomb.RemoveFromGame();
				this.otherBombSlot = discoParams.otherBomb.lastConnectedSlot;
				this.slotLock.LockSlot(this.otherBombSlot);
			}
			Match3Game game = discoParams.game;
			this.slotLock.LockSlots(discoParams.affectedSlotsList);
		}

		private IEnumerator DestroyAnimation()
		{
			return new DiscoBallDestroyInstantAction._003CDestroyAnimation_003Ed__8(0)
			{
				_003C_003E4__this = this
			};
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

		public override void OnUpdate(float deltaTime)
		{
			this.deltaTime = deltaTime;
			base.OnUpdate(deltaTime);
			if (this.destroyAnimation == null)
			{
				this.destroyAnimation = this.DestroyAnimation();
			}
			this.destroyAnimation.MoveNext();
		}

		private DiscoBallDestroyAction.DiscoParams discoParams;

		private Lock slotLock;

		private Lock stopGeneratorsLock;

		private IEnumerator destroyAnimation;

		private Chip originChip;

		private Slot otherBombSlot;

		private float deltaTime;

		private sealed class _003CDestroyAnimation_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDestroyAnimation_003Ed__8(int _003C_003E1__state)
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
				DiscoBallDestroyInstantAction discoBallDestroyInstantAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003CaffectedSlots_003E5__2 = discoBallDestroyInstantAction.discoParams.affectedSlotsList;
					this._003CchipAffector_003E5__3 = new DiscoChipAffector();
					this._003CchipAffector_003E5__3.Init(discoBallDestroyInstantAction.discoParams.originSlot, discoBallDestroyInstantAction.discoParams.game, discoBallDestroyInstantAction.discoParams.affectedSlotsList, discoBallDestroyInstantAction.discoParams.itemColor);
					this._003Ctime_003E5__4 = 0f;
					this._003Cduration_003E5__5 = 1f;
					break;
				case 1:
					this._003C_003E1__state = -1;
					this._003CchipAffector_003E5__3.Update();
					break;
				case 2:
					this._003C_003E1__state = -1;
					discoBallDestroyInstantAction.isAlive = false;
					return false;
				default:
					return false;
				}
				if (this._003Ctime_003E5__4 >= this._003Cduration_003E5__5)
				{
					this._003CchipAffector_003E5__3.Clear();
					bool isHavingCarpet = discoBallDestroyInstantAction.discoParams.isHavingCarpet;
					discoBallDestroyInstantAction.slotLock.UnlockAll();
					discoBallDestroyInstantAction.stopGeneratorsLock.UnlockAll();
					SlotDestroyParams slotDestroyParams = new SlotDestroyParams();
					slotDestroyParams.isHitByBomb = true;
					slotDestroyParams.isHavingCarpet = isHavingCarpet;
					slotDestroyParams.isBombAllowingNeighbourDestroy = true;
					slotDestroyParams.bombType = ChipType.DiscoBall;
					bool flag = !discoBallDestroyInstantAction.discoParams.replaceWithBombs;
					List<Slot> list = new List<Slot>();
					list.AddRange(this._003CaffectedSlots_003E5__2);
					list.Add(discoBallDestroyInstantAction.discoParams.originSlot);
					for (int i = 0; i < list.Count; i++)
					{
						Slot slot = list[i];
						if (slot != null)
						{
							slot.OnDestroySlot(slotDestroyParams);
							discoBallDestroyInstantAction.AffectOuterCircleWithExplosion(slot.position, 1, Match3Settings.instance.discoBallDestroyActionSettings.shockWaveOffset);
							if (flag)
							{
								List<Slot> neigbourSlots = slot.neigbourSlots;
								for (int j = 0; j < neigbourSlots.Count; j++)
								{
									neigbourSlots[j].OnDestroyNeighbourSlot(slot, slotDestroyParams);
								}
							}
						}
					}
					if (discoBallDestroyInstantAction.originChip != null)
					{
						CollectPointsAction.OnChipDestroy(discoBallDestroyInstantAction.originChip, slotDestroyParams);
						DestroyChipActionGrow destroyChipActionGrow = new DestroyChipActionGrow();
						destroyChipActionGrow.Init(discoBallDestroyInstantAction.originChip, discoBallDestroyInstantAction.originChip.lastConnectedSlot);
						discoBallDestroyInstantAction.discoParams.game.board.actionManager.AddAction(destroyChipActionGrow);
					}
					this._003C_003E2__current = null;
					this._003C_003E1__state = 2;
					return true;
				}
				this._003Ctime_003E5__4 += discoBallDestroyInstantAction.deltaTime;
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

			public DiscoBallDestroyInstantAction _003C_003E4__this;

			private List<Slot> _003CaffectedSlots_003E5__2;

			private DiscoChipAffector _003CchipAffector_003E5__3;

			private float _003Ctime_003E5__4;

			private float _003Cduration_003E5__5;
		}
	}
}
