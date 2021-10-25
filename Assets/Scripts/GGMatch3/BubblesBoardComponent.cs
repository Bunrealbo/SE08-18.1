using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class BubblesBoardComponent : BoardComponent
	{
		public void OnBubbleBurst(BubblesPieceBlocker bubble)
		{
			this.isAnyBubbleBurst = true;
		}

		private bool isCleared
		{
			get
			{
				if (this._isCleared)
				{
					return true;
				}
				List<Slot> allSlotsContainingBubbles = this.allSlotsContainingBubbles;
				this._isCleared = (allSlotsContainingBubbles.Count == 0);
				return this._isCleared;
			}
		}

		public void Init(Match3Game game)
		{
			this.game = game;
			this.globalLock = this.lockContainer.NewLock();
			this.globalLock.isAvailableForDiscoBombSuspended = true;
			this.globalLock.isSlotMatchingSuspended = true;
			this.globalLock.isChipGeneratorSuspended = true;
			this.globalLock.isDestroySuspended = true;
			this.globalLock.isSlotGravitySuspended = true;
		}

		private List<Slot> allSlotsContainingBubbles
		{
			get
			{
				this.allSlotsList.Clear();
				List<Slot> sortedSlotsUpdateList = this.game.board.sortedSlotsUpdateList;
				for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
				{
					Slot slot = sortedSlotsUpdateList[i];
					if (slot.GetSlotComponent<BubblesPieceBlocker>() != null)
					{
						this.allSlotsList.Add(slot);
					}
				}
				return this.allSlotsList;
			}
		}

		public bool isWaitingForBubblesToBurst
		{
			get
			{
				return !this.isCleared && this.game.board.userMovesCount > this.movesCountWhenTookAction && !this.isAnyBubbleBurst;
			}
		}

		public void CancelSpread()
		{
			this.movesCountWhenTookAction = this.game.board.userMovesCount;
		}

		public void OnUserMadeMove()
		{
			if (this.isCleared)
			{
				this.CancelSpread();
				return;
			}
			List<Slot> allSlotsContainingBubbles = this.allSlotsContainingBubbles;
			if (allSlotsContainingBubbles.Count == 0)
			{
				this.movesCountWhenTookAction = this.game.board.userMovesCount;
				return;
			}
			bool flag = false;
			for (int i = 0; i < allSlotsContainingBubbles.Count; i++)
			{
				List<Slot> neigbourSlots = allSlotsContainingBubbles[i].neigbourSlots;
				for (int j = 0; j < neigbourSlots.Count; j++)
				{
					Slot slot = neigbourSlots[j];
					if (!slot.isSlotGravitySuspended && !slot.isSlotSwapSuspended && !slot.isDestroySuspended && !slot.isPlaceBubbleSuspended)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.CancelSpread();
				this.isAnyBubbleBurst = false;
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			this.lockContainer.UnlockAll();
			if (this.game.isBubblesSuspended)
			{
				return;
			}
			if (this.game.board.userMovesCount <= this.movesCountWhenTookAction)
			{
				return;
			}
			if (this.game.board.actionManager.ActionCount > 0)
			{
				return;
			}
			if (!this.game.isBoardFullySettled)
			{
				return;
			}
			if (this.isAnyBubbleBurst)
			{
				this.CancelSpread();
				this.isAnyBubbleBurst = false;
				return;
			}
			List<Slot> allSlotsContainingBubbles = this.allSlotsContainingBubbles;
			if (allSlotsContainingBubbles.Count == 0)
			{
				this.CancelSpread();
				return;
			}
			this.slotsWhereCanPlaceNewBubble.Clear();
			for (int i = 0; i < allSlotsContainingBubbles.Count; i++)
			{
				List<Slot> neigbourSlots = allSlotsContainingBubbles[i].neigbourSlots;
				for (int j = 0; j < neigbourSlots.Count; j++)
				{
					Slot slot = neigbourSlots[j];
					if (!slot.isSlotGravitySuspended && !slot.isSlotSwapSuspended && !slot.isDestroySuspended && !slot.isPlaceBubbleSuspended)
					{
						this.slotsWhereCanPlaceNewBubble.Add(slot);
					}
				}
			}
			if (this.slotsWhereCanPlaceNewBubble.Count == 0)
			{
				this.CancelSpread();
				return;
			}
			Slot slot2 = this.slotsWhereCanPlaceNewBubble[UnityEngine.Random.Range(0, this.slotsWhereCanPlaceNewBubble.Count)];
			this.game.AddBubblesToSlot(slot2);
			this.movesCountWhenTookAction = this.game.board.userMovesCount;
			this.game.particles.CreateParticles(slot2.localPositionOfCenter, Match3Particles.PositionType.BubblesCreate);
			this.game.Play(GGSoundSystem.SFXType.SnowCreate);
		}

		private Match3Game game;

		private bool isAnyBubbleBurst;

		private int movesCountWhenTookAction;

		private LockContainer lockContainer = new LockContainer();

		private Lock globalLock;

		private List<Slot> allSlotsList = new List<Slot>();

		private List<Slot> slotsWhereCanPlaceNewBubble = new List<Slot>();

		private bool _isCleared;
	}
}
