using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GGMatch3;

public class ShowTutorialMaskAction : BoardAction
{
	public void Init(ShowTutorialMaskAction.Parameters parameters)
	{
		this.parameters = parameters;
		this.provider.Clear();
		this.provider.Init(parameters.game);
		for (int i = 0; i < parameters.match.matchingSlots.Count; i++)
		{
			IntVector2 position = parameters.match.matchingSlots[i];
			this.provider.AddSlot(new TilesSlotsProvider.Slot(position, true));
		}
		if (!parameters.match.matchingSlots.Contains(parameters.match.exchangeSlot))
		{
			this.provider.AddSlot(new TilesSlotsProvider.Slot(parameters.match.exchangeSlot, true));
		}
		if (!parameters.match.matchingSlots.Contains(parameters.match.slotToSwipe))
		{
			this.provider.AddSlot(new TilesSlotsProvider.Slot(parameters.match.slotToSwipe, true));
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		if (this.tutorialEnumerator == null)
		{
			this.tutorialEnumerator = this.DoShowTutorial();
		}
		this.tutorialEnumerator.MoveNext();
	}

	private IEnumerator DoShowTutorial()
	{
		return new ShowTutorialMaskAction._003CDoShowTutorial_003Ed__9(0)
		{
			_003C_003E4__this = this
		};
	}

	public override void Stop()
	{
		base.Stop();
		if (this.parameters.onEnd != null)
		{
			this.parameters.onEnd();
		}
	}

	private ShowTutorialMaskAction.Parameters parameters;

	public bool isBoardSettled;

	private ListSlotsProvider provider = new ListSlotsProvider();

	private ShowPotentialMatchAction showPotentialMatchAction = new ShowPotentialMatchAction();

	private ShowTutorialMaskAction.SlotChipPair swipeChipSlotInfo = new ShowTutorialMaskAction.SlotChipPair();

	private IEnumerator tutorialEnumerator;

	public class Parameters
	{
		public LevelDefinition.TutorialMatch match;

		public Match3Game game;

		public Action onMiddle;

		public Action onEnd;
	}

	public class SlotChipPair
	{
		public void Init(Slot slot, Chip chip)
		{
			this.slot = slot;
			this.chip = chip;
		}

		private Slot slot;

		private Chip chip;
	}

	private sealed class _003CDoShowTutorial_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		[DebuggerHidden]
		public _003CDoShowTutorial_003Ed__9(int _003C_003E1__state)
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
			ShowTutorialMaskAction showTutorialMaskAction = this._003C_003E4__this;
			switch (num)
			{
			case 0:
			{
				this._003C_003E1__state = -1;
				showTutorialMaskAction.isStarted = true;
				Match3Game.TutorialSlotHighlighter tutorialHighlighter = showTutorialMaskAction.parameters.game.tutorialHighlighter;
				tutorialHighlighter.Show(showTutorialMaskAction.provider);
				tutorialHighlighter.ShowGameScreenTutorialMask();
				this._003CswapLock_003E5__2 = showTutorialMaskAction.lockContainer.NewLock();
				this._003CswapLock_003E5__2.isSlotMatchingSuspended = true;
				this._003CtouchLock_003E5__3 = showTutorialMaskAction.lockContainer.NewLock();
				this._003CtouchLock_003E5__3.isSlotTouchingSuspended = true;
				this._003CswipeLock_003E5__4 = showTutorialMaskAction.lockContainer.NewLock();
				this._003CswipeLock_003E5__4.isSlotSwipeSuspended = true;
				for (int i = 0; i < showTutorialMaskAction.parameters.game.board.slots.Length; i++)
				{
					Slot slot = showTutorialMaskAction.parameters.game.board.slots[i];
					if (slot != null && !showTutorialMaskAction.parameters.match.Contains(slot.position))
					{
						this._003CswapLock_003E5__2.LockSlot(slot);
					}
				}
				for (int j = 0; j < showTutorialMaskAction.parameters.match.matchingSlots.Count; j++)
				{
					IntVector2 intVector = showTutorialMaskAction.parameters.match.matchingSlots[j];
					if (!(intVector == showTutorialMaskAction.parameters.match.slotToSwipe))
					{
						Slot slot2 = showTutorialMaskAction.parameters.game.board.GetSlot(intVector);
						this._003CswipeLock_003E5__4.LockSlot(slot2);
					}
				}
				ShowPotentialMatchAction.InitParams initParams = default(ShowPotentialMatchAction.InitParams);
				initParams.game = showTutorialMaskAction.parameters.game;
				initParams.tutorialMatch = showTutorialMaskAction.parameters.match;
				initParams.stayInfiniteTime = true;
				initParams.dontStopWhenInvalid = true;
				this._003Cgame_003E5__5 = showTutorialMaskAction.parameters.game;
				this._003CslotToSwipe_003E5__6 = showTutorialMaskAction.parameters.game.GetSlot(showTutorialMaskAction.parameters.match.slotToSwipe);
				Chip slotComponent = this._003CslotToSwipe_003E5__6.GetSlotComponent<Chip>();
				this._003CexchangeSlot_003E5__7 = showTutorialMaskAction.parameters.game.GetSlot(showTutorialMaskAction.parameters.match.exchangeSlot);
				Chip slotComponent2 = this._003CexchangeSlot_003E5__7.GetSlotComponent<Chip>();
				if (this._003CslotToSwipe_003E5__6 != null && this._003CexchangeSlot_003E5__7 != null)
				{
					TutorialHandController.InitArguments initArguments = default(TutorialHandController.InitArguments);
					initArguments.repeat = true;
					initArguments.startLocalPosition = this._003Cgame_003E5__5.gameScreen.transform.InverseTransformPoint(this._003Cgame_003E5__5.LocalToWorldPosition(this._003CslotToSwipe_003E5__6.localPositionOfCenter));
					initArguments.endLocalPosition = this._003Cgame_003E5__5.gameScreen.transform.InverseTransformPoint(this._003Cgame_003E5__5.LocalToWorldPosition(this._003CexchangeSlot_003E5__7.localPositionOfCenter));
					initArguments.settings = Match3Settings.instance.tutorialSwipeSettings;
					if (this._003CslotToSwipe_003E5__6 == this._003CexchangeSlot_003E5__7)
					{
						initArguments.settings = Match3Settings.instance.tutorialTouchSettings;
					}
					this._003Cgame_003E5__5.gameScreen.tutorialHand.Show(initArguments);
				}
				showTutorialMaskAction.showPotentialMatchAction = new ShowPotentialMatchAction();
				showTutorialMaskAction.showPotentialMatchAction.Init(initParams);
				showTutorialMaskAction.parameters.game.board.actionManager.AddAction(showTutorialMaskAction.showPotentialMatchAction);
				this._003CtutorialLockSwipeSlot_003E5__8 = new SetLock();
				this._003CtutorialLockSwipeSlot_003E5__8.isSwapingSuspended = true;
				this._003CtutorialLockExchangeSlot_003E5__9 = new SetLock();
				this._003CtutorialLockExchangeSlot_003E5__9.isSwapingSuspended = true;
				if (this._003CexchangeSlot_003E5__7 == null || this._003CslotToSwipe_003E5__6 == this._003CexchangeSlot_003E5__7)
				{
					this._003CswipeLock_003E5__4.LockSlot(this._003CslotToSwipe_003E5__6);
				}
				else if (slotComponent.canBeTappedToActivate && slotComponent2.canBeTappedToActivate)
				{
					this._003CtouchLock_003E5__3.LockSlot(this._003CslotToSwipe_003E5__6);
					this._003CtouchLock_003E5__3.LockSlot(this._003CexchangeSlot_003E5__7);
					this._003CswipeLock_003E5__4.LockSlot(this._003CexchangeSlot_003E5__7);
					this._003CtutorialLockSwipeSlot_003E5__8.slots.Add(this._003CexchangeSlot_003E5__7);
					this._003CslotToSwipe_003E5__6.AddSetLock(this._003CtutorialLockSwipeSlot_003E5__8);
				}
				else
				{
					this._003CtutorialLockSwipeSlot_003E5__8.slots.Add(this._003CexchangeSlot_003E5__7);
					this._003CslotToSwipe_003E5__6.AddSetLock(this._003CtutorialLockSwipeSlot_003E5__8);
					this._003CtouchLock_003E5__3.LockSlot(this._003CslotToSwipe_003E5__6);
					this._003CtutorialLockExchangeSlot_003E5__9.slots.Add(this._003CslotToSwipe_003E5__6);
					this._003CexchangeSlot_003E5__7.AddSetLock(this._003CtutorialLockExchangeSlot_003E5__9);
					this._003CtouchLock_003E5__3.LockSlot(this._003CexchangeSlot_003E5__7);
				}
				showTutorialMaskAction.swipeChipSlotInfo.Init(this._003CslotToSwipe_003E5__6, this._003CslotToSwipe_003E5__6.GetSlotComponent<Chip>());
				this._003CstartMove_003E5__10 = showTutorialMaskAction.parameters.game.board.userMovesCount;
				break;
			}
			case 1:
				this._003C_003E1__state = -1;
				break;
			case 2:
				this._003C_003E1__state = -1;
				goto IL_68A;
			default:
				return false;
			}
			if (showTutorialMaskAction.parameters.game.board.userMovesCount <= this._003CstartMove_003E5__10)
			{
				this._003C_003E2__current = null;
				this._003C_003E1__state = 1;
				return true;
			}
			this._003Cgame_003E5__5.gameScreen.tutorialHand.Hide();
			this._003CswapLock_003E5__2.Unlock(this._003CslotToSwipe_003E5__6);
			this._003CtouchLock_003E5__3.Unlock(this._003CslotToSwipe_003E5__6);
			this._003CtouchLock_003E5__3.Unlock(this._003CexchangeSlot_003E5__7);
			this._003CswipeLock_003E5__4.Unlock(this._003CexchangeSlot_003E5__7);
			this._003CslotToSwipe_003E5__6.RemoveSetLock(this._003CtutorialLockSwipeSlot_003E5__8);
			this._003CexchangeSlot_003E5__7.RemoveSetLock(this._003CtutorialLockExchangeSlot_003E5__9);
			for (int k = 0; k < showTutorialMaskAction.parameters.game.board.slots.Length; k++)
			{
				Slot slot3 = showTutorialMaskAction.parameters.game.board.slots[k];
				if (slot3 != null && !showTutorialMaskAction.parameters.match.Contains(slot3.position))
				{
					this._003CswapLock_003E5__2.Unlock(slot3);
				}
			}
			for (int l = 0; l < showTutorialMaskAction.parameters.match.matchingSlots.Count; l++)
			{
				IntVector2 position = showTutorialMaskAction.parameters.match.matchingSlots[l];
				Slot slot4 = showTutorialMaskAction.parameters.game.board.GetSlot(position);
				this._003CswipeLock_003E5__4.Unlock(slot4);
			}
			if (showTutorialMaskAction.parameters.onMiddle != null)
			{
				showTutorialMaskAction.parameters.onMiddle();
			}
			showTutorialMaskAction.showPotentialMatchAction.Stop();
			showTutorialMaskAction.parameters.game.tutorialHighlighter.Hide();
			IL_68A:
			if (showTutorialMaskAction.isBoardSettled)
			{
				if (showTutorialMaskAction.parameters.onEnd != null)
				{
					showTutorialMaskAction.parameters.onEnd();
				}
				showTutorialMaskAction.tutorialEnumerator = null;
				this._003CswapLock_003E5__2.UnlockAll();
				this._003CtouchLock_003E5__3.UnlockAll();
				this._003CswipeLock_003E5__4.UnlockAll();
				showTutorialMaskAction.isAlive = false;
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

		public ShowTutorialMaskAction _003C_003E4__this;

		private Lock _003CswapLock_003E5__2;

		private Lock _003CtouchLock_003E5__3;

		private Lock _003CswipeLock_003E5__4;

		private Match3Game _003Cgame_003E5__5;

		private Slot _003CslotToSwipe_003E5__6;

		private Slot _003CexchangeSlot_003E5__7;

		private SetLock _003CtutorialLockSwipeSlot_003E5__8;

		private SetLock _003CtutorialLockExchangeSlot_003E5__9;

		private int _003CstartMove_003E5__10;
	}
}
