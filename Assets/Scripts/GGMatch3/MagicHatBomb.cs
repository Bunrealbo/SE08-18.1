using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class MagicHatBomb : SlotComponent
	{
		public override int sortingOrder
		{
			get
			{
				return 10;
			}
		}

		private MagicHat.Settings settings
		{
			get
			{
				return Match3Settings.instance.magicHatSettingsBomb;
			}
		}

		public void Init(MagicHatBehaviour hatBehaviour, int itemCount, ChipType chipType)
		{
			this.hatBehaviour = hatBehaviour;
			this.chipType = chipType;
			if (itemCount <= 0)
			{
				this.hasBombsCount = false;
			}
			this.bombsCount = itemCount;
			if (hatBehaviour != null)
			{
				hatBehaviour.Init(chipType);
				hatBehaviour.SetCountActive(this.hasBombsCount);
				hatBehaviour.SetCount(this.bombsCount - this.bombsFired);
			}
			this.UpdateRocket();
		}

		public override bool IsAvailableForDiscoBombSuspended(bool replaceWithBombs)
		{
			return true;
		}

		public override bool isSlotMatchingSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isAttachGrowingElementSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isPlaceBubbleSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isMoveIntoSlotSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isSlotGravitySuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isSlotSwapSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isPreventingGravity
		{
			get
			{
				return true;
			}
		}

		public override bool isCreatePowerupWithThisSlotSuspended
		{
			get
			{
				return true;
			}
		}

		public override bool isMovingWithConveyor
		{
			get
			{
				return true;
			}
		}

		public override bool isDestroyedByMatchingNextTo
		{
			get
			{
				return true;
			}
		}

		public override bool IsCompatibleWithPickupGoal(Match3Goals.ChipTypeDef chipTypeDef)
		{
			return false;
		}

		private bool isUp
		{
			get
			{
				return this.timeSinceMissleLaunched >= this.settings.delayUp || (this.lastConnectedSlot != null && this.lastConnectedSlot.game.board.lastSettledMove > this.lastBombLaunchedMove);
			}
		}

		public override SlotDestroyResolution OnDestroyNeighbourSlotComponent(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			if (destroyParams.isHitByBomb && !destroyParams.isBombAllowingNeighbourDestroy)
			{
				return result;
			}
			if (!this.isUp)
			{
				return result;
			}
			result.isDestroyed = true;
			result.stopPropagation = true;
			this.FireRocket(slotBeingDestroyed, destroyParams);
			return result;
		}

		public override SlotDestroyResolution OnDestroySlotComponent(SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			result.isDestroyed = true;
			if (destroyParams.isFromTap)
			{
				result.stopPropagation = true;
				return result;
			}
			if (!this.isUp)
			{
				return result;
			}
			if (destroyParams.isHitByBomb && destroyParams.bombType == ChipType.SeekingMissle)
			{
				return result;
			}
			result.stopPropagation = true;
			result.isNeigbourDestroySuspendedForThisChipOnly = true;
			this.FireRocket(this.slot, destroyParams);
			return result;
		}

		private void FireRocket(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			Slot lastConnectedSlot = this.lastConnectedSlot;
			Match3Game game = this.lastConnectedSlot.game;
			List<Slot> list = game.goals.BestSlotsForSeekingMissle(game, lastConnectedSlot);
			if (list == null && list.Count <= 0)
			{
				return;
			}
			this.bombsFired++;
			this.lastBombLaunchedMove = game.board.lastSettledMove;
			if (this.chipType == ChipType.MagicHatSeekingMissle)
			{
				SeekingMissileAction.Parameters parameters = new SeekingMissileAction.Parameters();
				parameters.doCrossExplosion = false;
				parameters.game = game;
				parameters.startSlot = lastConnectedSlot;
				SeekingMissileAction seekingMissileAction = new SeekingMissileAction();
				seekingMissileAction.Init(parameters);
				game.board.actionManager.AddAction(seekingMissileAction);
			}
			else if (this.chipType == ChipType.MagicHatBomb || this.chipType == ChipType.MagicHatRocket)
			{
				ChipType powerup = ChipType.Bomb;
				if (this.chipType == ChipType.MagicHatRocket)
				{
					powerup = ((game.RandomRange(0, 100) > 50) ? ChipType.HorizontalRocket : ChipType.VerticalRocket);
				}
				Slot slotThatCanBeReplacedWithPowerup = PlacePowerupAction.GetSlotThatCanBeReplacedWithPowerup(game, powerup);
				if (slotThatCanBeReplacedWithPowerup != null)
				{
					AnimateCarryPiece animateCarryPiece = new AnimateCarryPiece();
					animateCarryPiece.Init(new AnimateCarryPiece.InitArguments
					{
						chipType = powerup,
						destinationSlot = slotThatCanBeReplacedWithPowerup,
						originPosition = lastConnectedSlot.position,
						game = game
					});
					game.board.actionManager.AddAction(animateCarryPiece);
				}
			}
			game.particles.CreateParticles(lastConnectedSlot.localPositionOfCenter, Match3Particles.PositionType.MagicHatCreate, ChipType.MagicHat, ItemColor.Unknown);
			this.timeSinceMissleLaunched = 0f;
			this.timeSinceUp = 0f;
			game.Play(GGSoundSystem.SFXType.BunnyOutOfHat);
			if (this.hatBehaviour != null)
			{
				this.hatBehaviour.SetCount(this.bombsCount - this.bombsFired);
			}
			if (this.hasBombsCount && this.bombsFired >= this.bombsCount)
			{
				base.RemoveFromGame();
			}
		}

		private void UpdateRocket()
		{
			if (this.hatBehaviour == null)
			{
				return;
			}
			MagicHat.Settings settings = this.settings;
			float time = Mathf.InverseLerp(0f, settings.durationForBunnyUp, this.timeSinceUp);
			float t = settings.positionCurve.Evaluate(time);
			float t2 = settings.scaleCurve.Evaluate(time);
			Vector3 bunnyOffset = Vector3.LerpUnclamped(settings.bunnyOffsetIn, settings.bunnyOffsetOut, t);
			Vector3 bunnyScale = Vector3.LerpUnclamped(settings.bunnyScaleIn, settings.bunnyScaleOut, t2);
			this.hatBehaviour.bunnyOffset = bunnyOffset;
			this.hatBehaviour.bunnyScale = bunnyScale;
		}

		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.timeSinceMissleLaunched += deltaTime;
			if (this.isUp)
			{
				this.timeSinceUp += deltaTime;
			}
			else
			{
				this.timeSinceUp = 0f;
			}
			this.UpdateRocket();
		}

		private MagicHatBehaviour hatBehaviour;

		private float timeSinceMissleLaunched = 100f;

		private int lastBombLaunchedMove = -1;

		private float timeSinceUp;

		private int bombsFired;

		private bool hasBombsCount = true;

		private int bombsCount = 3;

		private ChipType chipType = ChipType.MagicHatSeekingMissle;
	}
}
