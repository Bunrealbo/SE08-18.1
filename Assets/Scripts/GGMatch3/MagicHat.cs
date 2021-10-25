using System;
using UnityEngine;

namespace GGMatch3
{
	public class MagicHat : SlotComponent
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

		public void Init(MagicHatBehaviour hatBehaviour)
		{
			this.hatBehaviour = hatBehaviour;
			this.UpdateBunny();
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
			return chipTypeDef.chipType == ChipType.BunnyChip;
		}

		public override SlotDestroyResolution OnDestroyNeighbourSlotComponent(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			SlotDestroyResolution result = default(SlotDestroyResolution);
			if (destroyParams.isHitByBomb && !destroyParams.isBombAllowingNeighbourDestroy)
			{
				return result;
			}
			result.isDestroyed = true;
			result.stopPropagation = true;
			this.CreateBunny(slotBeingDestroyed, destroyParams);
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
			result.stopPropagation = true;
			result.isNeigbourDestroySuspendedForThisChipOnly = true;
			this.CreateBunny(this.slot, destroyParams);
			return result;
		}

		private void CreateBunny(Slot slotBeingDestroyed, SlotDestroyParams destroyParams)
		{
			Slot lastConnectedSlot = this.lastConnectedSlot;
			Match3Game game = this.lastConnectedSlot.game;
			Chip chip = game.CreateCharacterInSlot(lastConnectedSlot, ChipType.BunnyChip, 0);
			chip.RemoveFromSlot();
			Match3Goals.ChipTypeDef chipTypeDef = Match3Goals.ChipTypeDef.Create(chip);
			Match3Goals.GoalBase activeGoal = game.goals.GetActiveGoal(chipTypeDef);
			CollectGoalAction collectGoalAction = new CollectGoalAction();
			collectGoalAction.Init(new CollectGoalAction.CollectGoalParams
			{
				chip = chip,
				chipSlot = lastConnectedSlot,
				game = game,
				goal = activeGoal,
				isMagicHat = true,
				explosionCentre = lastConnectedSlot.position + IntVector2.down,
				destroyParams = destroyParams
			});
			game.board.actionManager.AddAction(collectGoalAction);
			game.particles.CreateParticles(lastConnectedSlot.localPositionOfCenter, Match3Particles.PositionType.MagicHatCreate, ChipType.MagicHat, ItemColor.Unknown);
			this.timeSinceBunnyCollected = 0f;
			game.Play(GGSoundSystem.SFXType.BunnyOutOfHat);
		}

		private void UpdateBunny()
		{
			if (this.hatBehaviour == null)
			{
				return;
			}
			MagicHat.Settings settings = this.settings;
			float time = Mathf.InverseLerp(0f, settings.durationForBunnyUp, this.timeSinceBunnyCollected - settings.delayUp);
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
			this.timeSinceBunnyCollected += deltaTime;
			this.UpdateBunny();
		}

		private MagicHatBehaviour hatBehaviour;

		private float timeSinceBunnyCollected = 100f;

		[Serializable]
		public class Settings
		{
			public float durationForBunnyUp;

			public Vector3 bunnyOffsetIn;

			public Vector3 bunnyOffsetOut;

			public Vector3 bunnyScaleIn;

			public Vector3 bunnyScaleOut;

			public AnimationCurve positionCurve;

			public AnimationCurve scaleCurve;

			public float delayUp;
		}
	}
}
