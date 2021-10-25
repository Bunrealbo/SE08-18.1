using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class MatchChipAffector : ChipAffectorBase
{
	public override void AddToInputAffectorExport(Match3Game.InputAffectorExport inputAffector)
	{
		if (this.arguments.originSlot == null)
		{
			return;
		}
		this.SetBrightness(1f);
		this.UpdateMoveBehaviour(1f);
		Match3Game.InputAffectorExport.InputAffectorForSlot inputAffectorForSlot = new Match3Game.InputAffectorExport.InputAffectorForSlot();
		inputAffectorForSlot.bolts.AddRange(this.bolts);
		this.bolts.Clear();
		inputAffectorForSlot.slot = this.arguments.originSlot;
		inputAffector.affectorExports.Add(inputAffectorForSlot);
	}

	public override void GiveLightingBoltsTo(List<LightingBolt> destinationBolts)
	{
		destinationBolts.AddRange(this.bolts);
		this.bolts.Clear();
	}

	public void Init(MatchChipAffector.InitArguments initArguments)
	{
		this.arguments = initArguments;
		Match3Game game = initArguments.game;
		List<Slot> matchingSlots = initArguments.matchingSlots;
		Slot originSlot = initArguments.originSlot;
		Chip slotComponent = originSlot.GetSlotComponent<Chip>();
		if (slotComponent != null)
		{
			this.moveBehaviour = slotComponent.GetComponentBehaviour<TransformBehaviour>();
		}
		if (this.arguments.otherChipToMove != null)
		{
			this.otherChipMoveBehaviour = this.arguments.otherChipToMove.GetComponentBehaviour<TransformBehaviour>();
		}
		if (!Match3Settings.instance.swipeAffectorSettings.ignoreBoltsWithoutPowerup || this.arguments.isCreatingPowerup || !this.arguments.cameFromPositionSet)
		{
			for (int i = 0; i < matchingSlots.Count; i++)
			{
				Slot slot = matchingSlots[i];
				LightingBolt lightingBolt;
				if (initArguments.isCreatingPowerup)
				{
					lightingBolt = game.CreateLightingBoltPowerup();
				}
				else
				{
					lightingBolt = game.CreateLightingBoltChip();
				}
				lightingBolt.Init(originSlot, slot);
				lightingBolt.SetPositionFromSlots();
				this.bolts.Add(lightingBolt);
				GGUtil.SetActive(lightingBolt, true);
				foreach (IntVector2 b in IntVector2.upDownLeftRight)
				{
					Slot slot2 = game.GetSlot(slot.position + b);
					if (slot2 != null && slot2.isDestroyedByMatchingNextTo && !this.arguments.ShouldIgnore(slot2))
					{
						LightingBolt lightingBolt2 = game.CreateLightingBoltChip();
						lightingBolt2.Init(slot, slot2);
						lightingBolt2.SetPositionFromSlots();
						lightingBolt2.HideParticle();
						GGUtil.SetActive(lightingBolt2, true);
						this.bolts.Add(lightingBolt2);
					}
				}
			}
		}
		this.UpdateMoveBehaviour(0f);
		for (int k = 0; k < this.bolts.Count; k++)
		{
			this.bolts[k].SetPositionFromChips();
		}
	}

	private void ShowBolts(bool show)
	{
		for (int i = 0; i < this.bolts.Count; i++)
		{
			GGUtil.SetActive(this.bolts[i], show);
		}
	}

	private void SetBrightness(float brightness)
	{
		List<Slot> matchingSlots = this.arguments.matchingSlots;
		for (int i = 0; i < matchingSlots.Count; i++)
		{
			Chip slotComponent = matchingSlots[i].GetSlotComponent<Chip>();
			if (slotComponent != null)
			{
				ChipBehaviour componentBehaviour = slotComponent.GetComponentBehaviour<ChipBehaviour>();
				if (!(componentBehaviour == null))
				{
					componentBehaviour.SetBrightness(brightness);
				}
			}
		}
	}

	private void UpdateMoveBehaviour(float n)
	{
		if (!this.arguments.cameFromPositionSet)
		{
			this.ShowBolts(true);
			return;
		}
		Vector3 vector = this.arguments.game.LocalPositionOfCenter(this.arguments.cameFromPosition);
		Vector3 localPositionOfCenter = this.arguments.originSlot.localPositionOfCenter;
		if (this.moveBehaviour != null)
		{
			Vector3 localPosition = Vector3.Lerp(vector, localPositionOfCenter, n);
			this.moveBehaviour.localPosition = localPosition;
		}
		if (this.otherChipMoveBehaviour != null)
		{
			Vector3 localPosition2 = Vector3.Lerp(localPositionOfCenter, vector, n);
			this.otherChipMoveBehaviour.localPosition = localPosition2;
		}
		this.ShowBolts(n >= 1f);
	}

	public override void Clear()
	{
		List<Slot> matchingSlots = this.arguments.matchingSlots;
		for (int i = 0; i < matchingSlots.Count; i++)
		{
			Slot slot = matchingSlots[i];
			slot.offsetScale = Vector3.one;
			slot.offsetPosition = Vector3.zero;
			this.SetBrightness(1f);
		}
		for (int j = 0; j < this.bolts.Count; j++)
		{
			this.bolts[j].RemoveFromGame();
		}
		this.bolts.Clear();
	}

	public override void OnAfterDestroy()
	{
	}

	public override void Update()
	{
		SwipeAffector.Settings swipeAffectorSettings = Match3Settings.instance.swipeAffectorSettings;
		base.Update();
		this.duration += Time.deltaTime;
		this.angle += Time.deltaTime * swipeAffectorSettings.angleSpeed;
		float num = Mathf.InverseLerp(0f, swipeAffectorSettings.moveDuration, this.duration);
		float n = swipeAffectorSettings.moveCurve.Evaluate(num);
		this.UpdateMoveBehaviour(n);
		List<Slot> matchingSlots = this.arguments.matchingSlots;
		int i = 0;
		while (i < matchingSlots.Count)
		{
			Slot slot = matchingSlots[i];
			Slot originSlot = this.arguments.originSlot;
			if (!this.arguments.isCreatingPowerup)
			{
				slot.offsetScale = new Vector3(swipeAffectorSettings.upScale, swipeAffectorSettings.upScale, 1f);
				goto IL_120;
			}
			slot.offsetScale = new Vector3(swipeAffectorSettings.createPowerupScale, swipeAffectorSettings.createPowerupScale, 1f);
			if (slot != originSlot)
			{
				slot.offsetPosition = (originSlot.localPositionOfCenter - slot.localPositionOfCenter).normalized * swipeAffectorSettings.pullOffset;
				goto IL_120;
			}
			slot.offsetPosition = Vector3.zero;
			IL_16D:
			i++;
			continue;
			IL_120:
			if (!swipeAffectorSettings.offsetTillMoveEnd || !this.arguments.cameFromPositionSet || num >= 1f)
			{
				this.ApplyShake(slot, originSlot.localPositionOfCenter);
				this.SetBrightness(swipeAffectorSettings.brightness);
			}
			if (slot.isSlotGravitySuspended)
			{
				slot.offsetPosition = Vector3.zero;
				goto IL_16D;
			}
			goto IL_16D;
		}
		for (int j = 0; j < this.bolts.Count; j++)
		{
			this.bolts[j].SetPositionFromChips();
		}
	}

	private void ApplyShake(Slot slot, Vector3 startLocalPosition)
	{
		if (this.bolts.Count == 0)
		{
			return;
		}
		SwipeAffector.Settings swipeAffectorSettings = Match3Settings.instance.swipeAffectorSettings;
		Vector3 lhs = slot.localPositionOfCenter - startLocalPosition;
		lhs.z = 0f;
		if (lhs == Vector3.zero)
		{
			lhs = Vector3.right;
		}
		float d = Mathf.Sin((swipeAffectorSettings.phaseOffsetMult * lhs.sqrMagnitude + this.angle) * 57.29578f) * swipeAffectorSettings.amplitude;
		Vector3 b = lhs.normalized * d;
		slot.offsetPosition += b;
	}

	private float angle;

	private MatchChipAffector.InitArguments arguments;

	public List<LightingBolt> bolts = new List<LightingBolt>();

	private static IntVector2[] directionsDiagonal = new IntVector2[]
	{
		new IntVector2(-1, 1),
		new IntVector2(1, 1),
		new IntVector2(-1, -1),
		new IntVector2(1, -1)
	};

	private static IntVector2[] directions = new IntVector2[]
	{
		IntVector2.left,
		IntVector2.right,
		IntVector2.up,
		IntVector2.down
	};

	private float duration;

	private TransformBehaviour moveBehaviour;

	private TransformBehaviour otherChipMoveBehaviour;

	private static List<Slot> slotsAffectedByExplosion = new List<Slot>();

	public class InitArguments
	{
		public bool ShouldIgnore(Slot slot)
		{
			return this.ignoreSlots.Contains(slot);
		}

		public bool cameFromPositionSet;

		public IntVector2 cameFromPosition;

		public Chip otherChipToMove;

		public Match3Game game;

		public Slot originSlot;

		public bool isCreatingPowerup;

		public List<Slot> matchingSlots = new List<Slot>();

		public List<Slot> ignoreSlots = new List<Slot>();
	}
}
