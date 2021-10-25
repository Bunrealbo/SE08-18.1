using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GGMatch3
{
	public class FlyLineRocketAction : BoardAction
	{
		public void Init(FlyLineRocketAction.Params flyParams)
		{
			this.flyParams = flyParams;
			if (flyParams.bombChip != null)
			{
				flyParams.bombChip.RemoveFromSlot();
			}
			FlyRocketPieceAction.Params @params = default(FlyRocketPieceAction.Params);
			@params.game = flyParams.game;
			@params.isHavingCarpet = flyParams.isHavingCarpet;
			@params.canUseScale = flyParams.canUseScale;
			@params.position = flyParams.position;
			@params.prelock = flyParams.prelock;
			@params.isInstant = flyParams.isInstant;
			@params.affectorExport = flyParams.affectorExport;
			if (flyParams.rocketType == ChipType.HorizontalRocket)
			{
				@params.direction = IntVector2.right;
			}
			else
			{
				@params.direction = IntVector2.up;
			}
			FlyRocketPieceAction flyRocketPieceAction = new FlyRocketPieceAction();
			flyRocketPieceAction.Init(@params);
			this.pieceActions.Add(flyRocketPieceAction);
			@params.direction = -@params.direction;
			@params.ignoreOriginSlot = true;
			flyRocketPieceAction = new FlyRocketPieceAction();
			flyRocketPieceAction.Init(@params);
			this.pieceActions.Add(flyRocketPieceAction);
			flyParams.game.Play(GGSoundSystem.SFXType.FlyRocket);
		}

		private IEnumerator DoFly()
		{
			return new FlyLineRocketAction._003CDoFly_003Ed__5(0)
			{
				_003C_003E4__this = this
			};
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			if (this.flyAnimation == null)
			{
				this.flyAnimation = this.DoFly();
			}
			this.flyAnimation.MoveNext();
		}

		private List<FlyRocketPieceAction> pieceActions = new List<FlyRocketPieceAction>();

		private FlyLineRocketAction.Params flyParams;

		private IEnumerator flyAnimation;

		public struct Params
		{
			public Match3Game.InputAffectorExport affectorExport
			{
				get
				{
					if (this.swapParams == null)
					{
						return null;
					}
					return this.swapParams.affectorExport;
				}
			}

			public Chip bombChip;

			public Match3Game game;

			public IntVector2 position;

			public ChipType rocketType;

			public bool prelock;

			public bool canUseScale;

			public bool isHavingCarpet;

			public bool isInstant;

			public SwapParams swapParams;
		}

		private sealed class _003CDoFly_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoFly_003Ed__5(int _003C_003E1__state)
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
				FlyLineRocketAction flyLineRocketAction = this._003C_003E4__this;
				if (num == 0)
				{
					this._003C_003E1__state = -1;
					if (flyLineRocketAction.flyParams.bombChip != null)
					{
						flyLineRocketAction.flyParams.bombChip.RemoveFromGame();
					}
					for (int i = 0; i < flyLineRocketAction.pieceActions.Count; i++)
					{
						FlyRocketPieceAction action = flyLineRocketAction.pieceActions[i];
						flyLineRocketAction.flyParams.game.board.actionManager.AddAction(action);
					}
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				if (num != 1)
				{
					return false;
				}
				this._003C_003E1__state = -1;
				flyLineRocketAction.isAlive = false;
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

			public FlyLineRocketAction _003C_003E4__this;
		}
	}
}
