using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class DiscoChipAffector : ChipAffectorBase
	{
		public DiscoBallAffector.Settings settings
		{
			get
			{
				return Match3Settings.instance.discoBallAffectorSettings;
			}
		}

		public override void Clear()
		{
			this.lockContainer.UnlockAll();
			this.boltSlots.Clear();
			for (int i = 0; i < this.affectedSlots.Count; i++)
			{
				Slot slot = this.affectedSlots[i];
				if (slot != null)
				{
					slot.light.RemoveLight(this.distanceLight);
					slot.light.AddLight(this.distanceLight.GetCurrentIntensity(slot.light));
				}
			}
			for (int j = 0; j < this.bolts.Count; j++)
			{
				this.bolts[j].RemoveFromGame();
			}
			this.bolts.Clear();
			if (this.saucerBehaviour != null)
			{
				this.saucerBehaviour.RemoveFromGame();
			}
		}

		public void Init(Slot originSlot, Slot otherSlot, Match3Game game)
		{
			ItemColor itemColor = otherSlot.GetSlotComponent<Chip>().itemColor;
			this.otherSlot = otherSlot;
			List<Slot> list = new List<Slot>();
			List<Slot> sortedSlotsUpdateList = game.board.sortedSlotsUpdateList;
			for (int i = 0; i < sortedSlotsUpdateList.Count; i++)
			{
				Slot slot = sortedSlotsUpdateList[i];
				if (slot != null && slot.CanParticipateInDiscoBombAffectedArea(itemColor, false))
				{
					base.globalLock.LockSlot(slot);
					list.Add(slot);
				}
			}
			this.Init(originSlot, game, list, itemColor);
		}

		public void Init(Slot originSlot, Match3Game game, List<Slot> slots, ItemColor itemColor)
		{
			this.Clear();
			this.timeActive = 0f;
			this.game = game;
			this.originSlot = originSlot;
			base.globalLock.isSlotGravitySuspended = true;
			base.globalLock.isChipGeneratorSuspended = true;
			for (int i = 0; i < slots.Count; i++)
			{
				Slot slot = slots[i];
				base.globalLock.LockSlot(slot);
				this.affectedSlots.Add(slot);
			}
			if (this.otherSlot != null)
			{
				this.affectedSlots.Add(this.otherSlot);
			}
			this.bolts.Clear();
			this.boltSlots.Clear();
			this.distanceLight.intensityMult = 0f;
			float num = 0f;
			if (this.affectedSlots.Count > 0)
			{
				num = this.settings.delayTime / (float)this.affectedSlots.Count;
			}
			for (int j = 0; j < this.affectedSlots.Count; j++)
			{
				Slot slot2 = this.affectedSlots[j];
				if (slot2 != null)
				{
					slot2.light.AddLight(this.distanceLight);
					LightingBolt lightingBolt = game.CreateLightingBoltPowerup();
					lightingBolt.Init(originSlot, slot2);
					this.bolts.Add(lightingBolt);
					lightingBolt.SetEndPositionFromLerp(0f);
					DiscoChipAffector.BoltSlot boltSlot = new DiscoChipAffector.BoltSlot();
					this.boltSlots.Add(boltSlot);
					boltSlot.bolt = lightingBolt;
					boltSlot.slot = slot2;
					boltSlot.delay = (float)j * num;
					foreach (IntVector2 b in IntVector2.upDownLeftRight)
					{
						Slot slot3 = game.GetSlot(slot2.position + b);
						if (slot3 != null && slot3.isDestroyedByMatchingNextTo)
						{
							LightingBolt lightingBolt2 = game.CreateLightingBoltChip();
							lightingBolt2.Init(slot2, slot3);
							lightingBolt2.SetPositionFromSlots();
							lightingBolt2.HideParticle();
							GGUtil.SetActive(lightingBolt2, false);
							this.bolts.Add(lightingBolt2);
							boltSlot.neighbourBolts.Add(lightingBolt2);
						}
					}
				}
			}
			this.saucerBehaviour = game.CreateFlyingSaucer();
			if (this.saucerBehaviour != null)
			{
				this.saucerBehaviour.Init(ChipType.Chip, itemColor);
				this.saucerBehaviour.transform.localPosition = game.LocalPositionOfCenter(originSlot.position);
				GGUtil.SetActive(this.saucerBehaviour, true);
			}
			game.Play(GGSoundSystem.SFXType.DiscoBallElectricity);
		}

		private void SetActive(List<LightingBolt> bolts, bool active)
		{
			for (int i = 0; i < bolts.Count; i++)
			{
				GGUtil.SetActive(bolts[i], active);
			}
		}

		private void UpdateIntensity()
		{
			DiscoBallAffector.Settings settings = this.settings;
			float intensityMult = Mathf.InverseLerp(0f, settings.timeToFullIntensity, this.timeActive);
			if (settings.intensityCurve != null)
			{
				intensityMult = settings.intensityCurve.Evaluate(intensityMult);
			}
			this.distanceLight.intensityMult = intensityMult;
		}

		public override void Update()
		{
			DiscoBallAffector.Settings settings = this.settings;
			this.timeActive += Time.deltaTime;
			this.UpdateIntensity();
			Slot[] slots = this.game.board.slots;
			Vector3 localPositionOfCenter = this.originSlot.localPositionOfCenter;
			this.angle += Time.deltaTime * settings.angleSpeed;
			for (int i = 0; i < this.boltSlots.Count; i++)
			{
				DiscoChipAffector.BoltSlot boltSlot = this.boltSlots[i];
				bool flag = this.time >= boltSlot.delay;
				LightingBolt bolt = boltSlot.bolt;
				GGUtil.SetActive(bolt, flag);
				if (flag)
				{
					float num = Mathf.InverseLerp(0f, settings.outDuration, this.time - boltSlot.delay);
					bolt.SetEndPositionFromLerp(num);
					bool active = num >= 1f;
					this.SetActive(boltSlot.neighbourBolts, active);
					Slot slot = boltSlot.slot;
					if (slot != null)
					{
						Vector3 vector = slot.localPositionOfCenter - localPositionOfCenter;
						vector.z = 0f;
						float d = Mathf.Sin((settings.phaseOffsetMult * vector.sqrMagnitude + this.angle) * 57.29578f) * settings.amplitude;
						Vector3 b = vector.normalized * d;
						slot.offsetPosition = Vector3.Lerp(slot.prevOffsetPosition, b, settings.displacePull * Time.deltaTime);
						slot.prevOffsetPosition = slot.offsetPosition;
						slot.positionIntegrator.SetPosition(slot.offsetPosition);
						slot.offsetScale = Vector3.one;
					}
				}
			}
			this.UpdateCombine();
		}

		public void UpdateCombine()
		{
			this.time += Time.deltaTime;
			if (this.otherSlot == null)
			{
				return;
			}
			Slot slot = this.otherSlot;
			Slot slot2 = this.originSlot;
			DiscoBallAffector.Settings settings = this.settings;
			float t = Mathf.InverseLerp(0f, settings.durationToGetClose, this.time);
			float d = Mathf.Lerp(1f, settings.maxScale, t);
			float t2 = Mathf.Lerp(0f, settings.normalizedClosePosition, t);
			slot.offsetPosition = Vector3.Lerp(slot.localPositionOfCenter, slot2.localPositionOfCenter, t2) - slot.localPositionOfCenter;
			slot2.offsetScale = Vector3.one * d;
		}

		private Slot originSlot;

		private Slot otherSlot;

		private DiscoChipAffector.DistanceLight distanceLight = new DiscoChipAffector.DistanceLight();

		private float angle;

		private float timeActive;

		private Match3Game game;

		public List<Slot> affectedSlots = new List<Slot>();

		public List<LightingBolt> bolts = new List<LightingBolt>();

		public List<DiscoChipAffector.BoltSlot> boltSlots = new List<DiscoChipAffector.BoltSlot>();

		private float time;

		private FlyingSaucerBehaviour saucerBehaviour;

		public class DistanceLight : LightSlotComponent.PermanentLight
		{
			public DiscoBallAffector.Settings settings
			{
				get
				{
					return Match3Settings.instance.discoBallAffectorSettings;
				}
			}

			public override float GetCurrentIntensity(LightSlotComponent component)
			{
				return this.intensityMult * this.settings.lightIntensity;
			}

			public float intensityMult;
		}

		public class BoltSlot
		{
			public float delay;

			public Slot slot;

			public LightingBolt bolt;

			public List<LightingBolt> neighbourBolts = new List<LightingBolt>();
		}
	}
}
