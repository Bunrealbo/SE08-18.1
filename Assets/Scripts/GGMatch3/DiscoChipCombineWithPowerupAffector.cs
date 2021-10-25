using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGMatch3
{
	public class DiscoChipCombineWithPowerupAffector : ChipAffectorBase
	{
		public DiscoBallAffector.Settings settings
		{
			get
			{
				return Match3Settings.instance.discoBallAffectorSettings;
			}
		}

		public override bool canFinish
		{
			get
			{
				for (int i = 0; i < this.boltSlots.Count; i++)
				{
					DiscoChipCombineWithPowerupAffector.BoltSlot boltSlot = this.boltSlots[i];
					if (boltSlot.shouldReplace && boltSlot.isNotReplaced)
					{
						return false;
					}
					if (boltSlot.action != null && boltSlot.action.isAlive)
					{
						return false;
					}
				}
				return true;
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

		public override void AddToInputAffectorExport(Match3Game.InputAffectorExport inputAffector)
		{
			base.AddToInputAffectorExport(inputAffector);
			DiscoChipCombineWithPowerupAffector.ExecuteAction executeAction = new DiscoChipCombineWithPowerupAffector.ExecuteAction();
			DiscoChipCombineWithPowerupAffector.ExecuteAction.InitArguments initArguments = default(DiscoChipCombineWithPowerupAffector.ExecuteAction.InitArguments);
			initArguments.originSlot = this.originSlot;
			initArguments.powerupSlot = this.otherSlot;
			initArguments.game = this.game;
			initArguments.boltSlots = new List<DiscoChipCombineWithPowerupAffector.BoltSlot>();
			for (int i = 0; i < this.boltSlots.Count; i++)
			{
				DiscoChipCombineWithPowerupAffector.BoltSlot boltSlot = this.boltSlots[i];
				if (boltSlot.shouldReplace)
				{
					initArguments.boltSlots.Add(boltSlot);
				}
			}
			executeAction.Init(initArguments);
			inputAffector.AddAction(executeAction);
		}

		public void Init(Slot originSlot, Slot powerupSlot, ChipType bombType, Match3Game game, List<Slot> slots)
		{
			this.Clear();
			this.bombType = bombType;
			this.timeActive = 0f;
			this.game = game;
			this.originSlot = originSlot;
			this.otherSlot = powerupSlot;
			base.globalLock.SuspendAll();
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
					lightingBolt.HideParticle();
					lightingBolt.Init(originSlot, slot2);
					this.bolts.Add(lightingBolt);
					lightingBolt.SetEndPositionFromLerp(0f);
					DiscoChipCombineWithPowerupAffector.BoltSlot boltSlot = new DiscoChipCombineWithPowerupAffector.BoltSlot();
					this.boltSlots.Add(boltSlot);
					boltSlot.bolt = lightingBolt;
					boltSlot.slot = slot2;
					boltSlot.delay = (float)j * num;
					boltSlot.shouldReplace = (slot2 != this.otherSlot && slot2 != originSlot);
				}
			}
			this.saucerBehaviour = game.CreateFlyingSaucer();
			if (this.saucerBehaviour != null)
			{
				this.saucerBehaviour.Init(bombType, ItemColor.Uncolored);
				this.saucerBehaviour.transform.localPosition = game.LocalPositionOfCenter(originSlot.position);
				GGUtil.SetActive(this.saucerBehaviour, true);
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
				DiscoChipCombineWithPowerupAffector.BoltSlot boltSlot = this.boltSlots[i];
				bool flag = this.time >= boltSlot.delay;
				LightingBolt bolt = boltSlot.bolt;
				GGUtil.SetActive(bolt, flag);
				if (flag)
				{
					float num = Mathf.InverseLerp(0f, settings.outDuration, this.time - boltSlot.delay);
					bolt.SetEndPositionFromLerp(num);
					bool flag2 = num >= 1f;
					if (boltSlot.shouldReplace && boltSlot.isNotReplaced && flag2)
					{
						boltSlot.isReplaced = true;
						this.ReplaceChipWithPowerup(boltSlot);
					}
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

		private void ReplaceChipWithPowerup(DiscoChipCombineWithPowerupAffector.BoltSlot boltSlot)
		{
			Slot slot = boltSlot.slot;
			Chip slotComponent = slot.GetSlotComponent<Chip>();
			SlotDestroyParams slotDestroyParams = new SlotDestroyParams();
			slotDestroyParams.isHitByBomb = true;
			slotDestroyParams.isHavingCarpet = false;
			slotDestroyParams.isBombAllowingNeighbourDestroy = true;
			slotDestroyParams.bombType = ChipType.DiscoBall;
			if (slotComponent != null)
			{
				slotComponent.RemoveFromGameWithPickupGoal(slotDestroyParams);
			}
			ChipType chipType = this.bombType;
			if (chipType == ChipType.HorizontalRocket || chipType == ChipType.VerticalRocket)
			{
				if (this.game.RandomRange(0, 100) >= 50)
				{
					chipType = ChipType.HorizontalRocket;
				}
				else
				{
					chipType = ChipType.VerticalRocket;
				}
			}
			CreatePowerupAction createPowerupAction = new CreatePowerupAction();
			createPowerupAction.Init(new CreatePowerupAction.CreateParams
			{
				positionWherePowerupWillBeCreated = slot.position,
				powerupToCreate = chipType,
				game = this.game
			});
			boltSlot.action = createPowerupAction;
			this.game.board.actionManager.AddAction(createPowerupAction);
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

		private ChipType bombType;

		private DiscoChipCombineWithPowerupAffector.DistanceLight distanceLight = new DiscoChipCombineWithPowerupAffector.DistanceLight();

		private float angle;

		private float timeActive;

		private Match3Game game;

		public List<Slot> affectedSlots = new List<Slot>();

		public List<LightingBolt> bolts = new List<LightingBolt>();

		private FlyingSaucerBehaviour saucerBehaviour;

		public List<DiscoChipCombineWithPowerupAffector.BoltSlot> boltSlots = new List<DiscoChipCombineWithPowerupAffector.BoltSlot>();

		private float time;

		public class ExecuteAction : Match3Game.IAffectorExportAction
		{
			public void Init(DiscoChipCombineWithPowerupAffector.ExecuteAction.InitArguments initArguments)
			{
				this.initArguments = initArguments;
			}

			public void Execute()
			{
				DiscoBallDestroyAction.Settings discoBallDestroyActionSettings = Match3Settings.instance.discoBallDestroyActionSettings;
				Slot originSlot = this.initArguments.originSlot;
				Slot powerupSlot = this.initArguments.powerupSlot;
				List<DiscoChipCombineWithPowerupAffector.BoltSlot> boltSlots = this.initArguments.boltSlots;
				Match3Game game = this.initArguments.game;
				Chip slotComponent = originSlot.GetSlotComponent<Chip>();
				if (slotComponent != null)
				{
					slotComponent.RemoveFromGame();
				}
				bool isHavingCarpet = false;
				if (originSlot.canCarpetSpreadFromHere || powerupSlot.canCarpetSpreadFromHere)
				{
					isHavingCarpet = true;
				}
				SlotDestroyParams slotDestroyParams = new SlotDestroyParams();
				slotDestroyParams.isHitByBomb = true;
				slotDestroyParams.isHavingCarpet = isHavingCarpet;
				slotDestroyParams.isBombAllowingNeighbourDestroy = true;
				slotDestroyParams.bombType = ChipType.DiscoBall;
				powerupSlot.OnDestroySlot(slotDestroyParams);
				for (int i = 0; i < boltSlots.Count; i++)
				{
					Slot slot = boltSlots[i].slot;
					if (slot != null)
					{
						slotDestroyParams.activationDelay = (float)i * Match3Settings.instance.discoBallAffectorSettings.activationDelay;
						slot.OnDestroySlot(slotDestroyParams);
					}
				}
				if (slotComponent != null)
				{
					DestroyChipActionGrow destroyChipActionGrow = new DestroyChipActionGrow();
					destroyChipActionGrow.Init(slotComponent, slotComponent.lastConnectedSlot);
					game.board.actionManager.AddAction(destroyChipActionGrow);
				}
			}

			public void OnCancel()
			{
				this.Execute();
			}

			private DiscoChipCombineWithPowerupAffector.ExecuteAction.InitArguments initArguments;

			public struct InitArguments
			{
				public Match3Game game;

				public Slot originSlot;

				public Slot powerupSlot;

				public List<DiscoChipCombineWithPowerupAffector.BoltSlot> boltSlots;
			}
		}

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
			public bool isNotReplaced
			{
				get
				{
					return !this.isReplaced;
				}
			}

			public float delay;

			public Slot slot;

			public LightingBolt bolt;

			public BoardAction action;

			public bool shouldReplace;

			public bool isReplaced;
		}
	}
}
