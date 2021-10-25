using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class SwipeAffector : PlayerInput.AffectorBase
	{
		public SwipeAffector.Settings settings
		{
			get
			{
				return Match3Settings.instance.swipeAffectorSettings;
			}
		}

		public override float minAffectorDuration
		{
			get
			{
				if (this.initArguments.isCreatingPowerup)
				{
					return this.settings.minAffectorDurationPowerup;
				}
				if (this.initArguments.isMatching)
				{
					return this.settings.minAffectorDuration;
				}
				return 0f;
			}
		}

		public override void AddToSwitchSlotArguments(ref Match3Game.SwitchSlotsArguments switchSlotsArguments)
		{
			base.AddToSwitchSlotArguments(ref switchSlotsArguments);
			switchSlotsArguments.bolts = new List<LightingBolt>();
			this.GiveLightingBoltsTo(switchSlotsArguments.bolts);
		}

		public override void Clear()
		{
			for (int i = 0; i < this.swapAffectedSlots.Count; i++)
			{
				this.swapAffectedSlots[i].light.RemoveLight(this.distanceLight);
			}
			this.swapAffectedSlots.Clear();
			this.affectorDuration = 0f;
			for (int j = 0; j < this.bolts.Count; j++)
			{
				this.bolts[j].RemoveFromGame();
			}
			this.bolts.Clear();
			List<SwipeAffector.AffectedSlot> affectedSlots = this.initArguments.affectedSlots;
			for (int k = 0; k < affectedSlots.Count; k++)
			{
				SwipeAffector.AffectedSlot affectedSlot = affectedSlots[k];
				for (int l = 0; l < affectedSlot.matchingSlotsWithChips.Count; l++)
				{
					Slot slot = affectedSlot.matchingSlotsWithChips[l];
					slot.offsetScale = Vector3.one;
					slot.offsetPosition = Vector3.zero;
				}
			}
		}

		public override void OnBeforeDestroy()
		{
			List<SwipeAffector.AffectedSlot> affectedSlots = this.initArguments.affectedSlots;
			Match3Game game = this.initArguments.input.game;
			SwipeAffector.Settings settings = this.settings;
			for (int i = 0; i < affectedSlots.Count; i++)
			{
				SwipeAffector.AffectedSlot affectedSlot = affectedSlots[i];
				for (int j = 0; j < affectedSlot.matchingSlots.Count; j++)
				{
					Slot slot = affectedSlot.matchingSlots[j];
					for (int k = 0; k < SwipeAffector.directions.Length; k++)
					{
						IntVector2 b = SwipeAffector.directions[k];
						Slot slot2 = game.GetSlot(slot.position + b);
						if (slot2 != null && !this.swapAffectedSlots.Contains(slot2))
						{
							slot2.offsetPosition = (slot2.localPositionOfCenter - slot.localPositionOfCenter).normalized * settings.shockWaveOffset;
							slot2.positionIntegrator.currentPosition = slot2.offsetPosition;
						}
					}
				}
			}
		}

		public override void OnUpdate(PlayerInput.AffectorUpdateParams updateParams)
		{
			SwipeAffector.Settings settings = this.settings;
			this.affectorDuration += Time.deltaTime;
			this.angle += Time.deltaTime * settings.angleSpeed;
			PlayerInput.MouseParams mouseParams = updateParams.mouseParams;
			Slot firstHitSlot = mouseParams.firstHitSlot;
			Slot slotToSwitchWith = mouseParams.slotToSwitchWith;
			List<SwipeAffector.AffectedSlot> affectedSlots = this.initArguments.affectedSlots;
			for (int i = 0; i < affectedSlots.Count; i++)
			{
				SwipeAffector.AffectedSlot affectedSlot = affectedSlots[i];
				Slot slot = affectedSlot.slot;
				int j = 0;
				while (j < affectedSlot.matchingSlotsWithChips.Count)
				{
					Slot slot2 = affectedSlot.matchingSlotsWithChips[j];
					if (affectedSlot.isCreatingPowerup)
					{
						slot2.offsetScale = new Vector3(settings.createPowerupScale, settings.createPowerupScale, 1f);
						if (slot2 != slot)
						{
							slot2.offsetPosition = (slot.localPositionOfCenter - slot2.localPositionOfCenter).normalized * settings.pullOffset;
							goto IL_12F;
						}
						slot2.offsetPosition = Vector3.zero;
					}
					else
					{
						if (affectedSlot.isPartOfActiveGoal)
						{
							slot2.offsetScale = new Vector3(settings.activeGoalUpScale, settings.activeGoalUpScale, 1f);
							goto IL_12F;
						}
						slot2.offsetScale = new Vector3(settings.upScale, settings.upScale, 1f);
						goto IL_12F;
					}
					IL_15B:
					j++;
					continue;
					IL_12F:
					if (!affectedSlot.isPartOfActiveGoal)
					{
						this.ApplyShake(slot2, slot.localPositionOfCenter);
					}
					if (slot2.isSlotGravitySuspended)
					{
						slot2.offsetPosition = Vector3.zero;
						goto IL_15B;
					}
					goto IL_15B;
				}
			}
			for (int k = 0; k < this.bolts.Count; k++)
			{
				this.bolts[k].SetPositionFromSlots();
			}
		}

		private void ApplyShake(Slot slot, Vector3 startLocalPosition)
		{
			Vector3 lhs = slot.localPositionOfCenter - startLocalPosition;
			if (lhs == Vector3.zero)
			{
				lhs = Vector3.right;
			}
			lhs.z = 0f;
			float d = Mathf.Sin((this.settings.phaseOffsetMult * lhs.sqrMagnitude + this.angle) * 57.29578f) * this.settings.amplitude;
			Vector3 b = lhs.normalized * d;
			slot.offsetPosition += b;
		}

		private void GiveLightingBoltsTo(List<LightingBolt> destinationBolts)
		{
			destinationBolts.Clear();
			destinationBolts.AddRange(this.bolts);
			this.bolts.Clear();
		}

		private SwipeAffector.DistanceLight distanceLight;

		public List<LightingBolt> bolts;

		private List<Slot> swapAffectedSlots;

		private SwipeAffector.InitArguments initArguments;

		private float angle;

		private static IntVector2[] directions = new IntVector2[]
		{
			IntVector2.left,
			IntVector2.right,
			IntVector2.up,
			IntVector2.down
		};

		public class DistanceLight : LightSlotComponent.PermanentLight
		{
			public SwipeAffector.Settings settings
			{
				get
				{
					return Match3Settings.instance.swipeAffectorSettings;
				}
			}

			public override float GetCurrentIntensity(LightSlotComponent component)
			{
				return this.intensityMult * this.settings.lightIntensity;
			}

			public float intensityMult;
		}

		[Serializable]
		public class Settings
		{
			public float upScale = 1.2f;

			public float createPowerupScale = 1f;

			public float pullOffset;

			public bool offsetTillMoveEnd;

			public float moveDuration = 0.1f;

			public AnimationCurve moveCurve;

			public float brightness = 1f;

			public float angleSpeed = 100f;

			public float phaseOffsetMult = 1f;

			public float amplitude = 0.05f;

			public float minAffectorDuration = 0.5f;

			public bool ignoreBoltsWithoutPowerup;

			public bool hasMaxAffectorDuration;

			public float maxMaxAffectorDuration = 0.1f;

			public float minAutoMatchDuration = 0.15f;

			public float minAffectorDurationPowerup = 0.5f;

			public float minAffectorDurationMix = 0.5f;

			public float lightIntensity;

			public float shockWaveOffset = 0.1f;

			public bool useParticles;

			public bool autoMatchesProduceLighting;

			public float activeGoalUpScale = 1.4f;

			public bool useAutoMatchDuration;

			public float autoMatchDuration = 0.5f;
		}

		public class AffectedSlot
		{
			public Slot slot;

			public List<Slot> matchingSlotsWithChips;

			public List<Slot> matchingSlots;

			public bool isMatching;

			public bool isCreatingPowerup;

			public bool isPartOfActiveGoal;
		}

		public class InitArguments
		{
			public bool isMatching
			{
				get
				{
					for (int i = 0; i < this.affectedSlots.Count; i++)
					{
						if (this.affectedSlots[i].isMatching)
						{
							return true;
						}
					}
					return false;
				}
			}

			public bool isCreatingPowerup
			{
				get
				{
					for (int i = 0; i < this.affectedSlots.Count; i++)
					{
						SwipeAffector.AffectedSlot affectedSlot = this.affectedSlots[i];
						if (affectedSlot.isMatching && affectedSlot.isCreatingPowerup)
						{
							return true;
						}
					}
					return false;
				}
			}

			public PlayerInput input;

			public List<SwipeAffector.AffectedSlot> affectedSlots;
		}
	}
}
