using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class LineChipAffector : ChipAffectorBase
{
	public override void AddToInputAffectorExport(Match3Game.InputAffectorExport inputAffector)
	{
		if (this.originSlot == null)
		{
			return;
		}
		Match3Game.InputAffectorExport.InputAffectorForSlot inputAffectorForSlot = new Match3Game.InputAffectorExport.InputAffectorForSlot();
		inputAffectorForSlot.bolts.AddRange(this.bolts);
		this.bolts.Clear();
		inputAffectorForSlot.slot = this.originSlot;
		inputAffector.affectorExports.Add(inputAffectorForSlot);
	}

	public override void Clear()
	{
		this.affectorDuration = 0f;
		for (int i = 0; i < this.bolts.Count; i++)
		{
			this.bolts[i].RemoveFromGame();
		}
		this.bolts.Clear();
		this.lockContainer.UnlockAll();
	}

	private PowerLineAffector.Settings settings
	{
		get
		{
			return Match3Settings.instance.powerLineAffectorSettings;
		}
	}

	public void Init(Slot originSlot, Match3Game game, IntVector2 direction)
	{
		this.Clear();
		this.originSlot = originSlot;
		this.direction = direction;
		base.globalLock.isSlotGravitySuspended = true;
		base.globalLock.isChipGeneratorSuspended = true;
		IntVector2 position = originSlot.position;
		foreach (Slot slot in game.board.slots)
		{
			if (slot != null)
			{
				IntVector2 intVector = slot.position - position;
				float num;
				if (this.direction.y != 0)
				{
					Mathf.Abs(intVector.y);
					num = (float)Mathf.Abs(intVector.x);
				}
				else
				{
					Mathf.Abs(intVector.x);
					num = (float)Mathf.Abs(intVector.y);
				}
				if (num <= 0f)
				{
					base.globalLock.LockSlot(slot);
					this.affectedSlots.Add(slot);
					if (!Match3Settings.instance.playerInputSettings.disableLightingInAffectors && !Match3Settings.instance.playerInputSettings.useSimpleLineBolts)
					{
						LightingBolt lightingBolt = game.CreateLightingBoltPowerup();
						lightingBolt.Init(originSlot, slot);
						lightingBolt.SetEndPositionFromLerp(0f);
						this.bolts.Add(lightingBolt);
					}
				}
			}
		}
		if (Match3Settings.instance.playerInputSettings.useSimpleLineBolts)
		{
			Slot slot2 = game.LastSlotOnDirection(originSlot, direction);
			Slot slot3 = game.LastSlotOnDirection(originSlot, -direction);
			if (slot2 != null && slot2 != originSlot)
			{
				LightingBolt lightingBolt2 = game.CreateLightingBoltPowerup();
				lightingBolt2.Init(originSlot, slot2);
				lightingBolt2.SetEndPositionFromLerp(0f);
				this.bolts.Add(lightingBolt2);
			}
			if (slot3 != null && slot3 != originSlot)
			{
				LightingBolt lightingBolt3 = game.CreateLightingBoltPowerup();
				lightingBolt3.Init(originSlot, slot3);
				lightingBolt3.SetEndPositionFromLerp(0f);
				this.bolts.Add(lightingBolt3);
			}
		}
	}

	public override void Update()
	{
		this.angle += Time.deltaTime * this.settings.angleSpeed;
		this.affectorDuration += Time.deltaTime;
		float endPositionFromLerp = Mathf.InverseLerp(0f, this.settings.outDuration, this.affectorDuration);
		for (int i = 0; i < this.bolts.Count; i++)
		{
			this.bolts[i].SetEndPositionFromLerp(endPositionFromLerp);
		}
		for (int j = 0; j < this.affectedSlots.Count; j++)
		{
			Slot slot = this.affectedSlots[j];
			this.ApplyShake(slot, this.originSlot.localPositionOfCenter);
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

	public List<Slot> affectedSlots = new List<Slot>();

	public List<LightingBolt> bolts = new List<LightingBolt>();

	private float affectorDuration;

	private IntVector2 direction;

	private Slot originSlot;

	private float angle;
}
