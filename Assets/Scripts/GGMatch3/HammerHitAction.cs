using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GGMatch3
{
	public class HammerHitAction : BoardAction
	{
		public void Init(HammerHitAction.InitArguments initArguments)
		{
			this.initArguments = initArguments;
		}

		private IEnumerator DoAnimation()
		{
			return new HammerHitAction._003CDoAnimation_003Ed__4(0)
			{
				_003C_003E4__this = this
			};
		}

		private void ActivatePowerup()
		{
			Match3Game game = this.initArguments.game;
			PowerupPlacementHandler.PlacementCompleteArguments completeArguments = this.initArguments.completeArguments;
			PowerupsDB.PowerupDefinition powerup = completeArguments.initArguments.powerup;
			Slot targetSlot = completeArguments.targetSlot;
			if (powerup.type == PowerupType.Hammer)
			{
				targetSlot.OnDestroySlot(new SlotDestroyParams
				{
					isNeigbourDestroySuspended = true,
					isHavingCarpet = (game.board.carpet.isCarpetPossible && !targetSlot.isBlockingCarpetSpread)
				});
				game.particles.CreateParticles(targetSlot, Match3Particles.PositionType.OnHammerHit);
				game.Play(GGSoundSystem.SFXType.HammerHit);
				return;
			}
			if (powerup.type == PowerupType.PowerHammer)
			{
				FlyCrossRocketAction flyCrossRocketAction = new FlyCrossRocketAction();
				flyCrossRocketAction.Init(new FlyCrossRocketAction.FlyParams
				{
					game = game,
					prelockAll = true,
					originPosition = targetSlot.position,
					rows = 1,
					columns = 1,
					useDelayBetweenRowsAndColumns = false
				});
				game.board.actionManager.AddAction(flyCrossRocketAction);
				game.particles.CreateParticles(targetSlot, Match3Particles.PositionType.OnHammerPowerHit);
				game.Play(GGSoundSystem.SFXType.PowerHammerHit);
			}
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			if (this.animationAction == null)
			{
				this.animationAction = this.DoAnimation();
			}
			this.animationAction.MoveNext();
		}

		private HammerHitAction.InitArguments initArguments;

		private IEnumerator animationAction;

		public struct InitArguments
		{
			public Match3Game game;

			public PowerupPlacementHandler.PlacementCompleteArguments completeArguments;
		}

		private sealed class _003CDoAnimation_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
		{
			[DebuggerHidden]
			public _003CDoAnimation_003Ed__4(int _003C_003E1__state)
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
				HammerHitAction hammerHitAction = this._003C_003E4__this;
				switch (num)
				{
				case 0:
					this._003C_003E1__state = -1;
					this._003Cgame_003E5__2 = hammerHitAction.initArguments.game;
					this._003Cgame_003E5__2.Play(GGSoundSystem.SFXType.HammerStart);
					this._003Cgame_003E5__2.board.powerupAnimationsInProgress++;
					this._003ChammerBehaviour_003E5__3 = this._003Cgame_003E5__2.CreateHammerAnimationBehaviour();
					if (this._003ChammerBehaviour_003E5__3 != null)
					{
						this._003ChammerBehaviour_003E5__3.Init(hammerHitAction.initArguments.completeArguments.initArguments.powerup.type);
						Slot targetSlot = hammerHitAction.initArguments.completeArguments.targetSlot;
						this._003ChammerBehaviour_003E5__3.transform.localPosition = targetSlot.localPositionOfCenter;
					}
					break;
				case 1:
					this._003C_003E1__state = -1;
					break;
				case 2:
					this._003C_003E1__state = -1;
					goto IL_139;
				default:
					return false;
				}
				if (!(this._003ChammerBehaviour_003E5__3 == null) && this._003ChammerBehaviour_003E5__3.animationTime < this._003ChammerBehaviour_003E5__3.timeWhenHammerHit && this._003ChammerBehaviour_003E5__3.animationNormalizedTime < 1f)
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 1;
					return true;
				}
				hammerHitAction.ActivatePowerup();
				this._003Cgame_003E5__2.board.powerupAnimationsInProgress--;
				IL_139:
				if (!(this._003ChammerBehaviour_003E5__3 == null) && this._003ChammerBehaviour_003E5__3.animationNormalizedTime < 1f)
				{
					this._003C_003E2__current = null;
					this._003C_003E1__state = 2;
					return true;
				}
				if (this._003ChammerBehaviour_003E5__3 != null)
				{
					this._003ChammerBehaviour_003E5__3.RemoveFromGame();
				}
				hammerHitAction.isAlive = false;
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

			public HammerHitAction _003C_003E4__this;

			private Match3Game _003Cgame_003E5__2;

			private HammerAnimationBehaviour _003ChammerBehaviour_003E5__3;
		}
	}
}
