using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class BombChipAffector : ChipAffectorBase
{
	public override void Clear()
	{
		for (int i = 0; i < this.affectedSlots.Count; i++)
		{
			Slot slot = this.affectedSlots[i];
			slot.offsetPosition = Vector3.zero;
			slot.offsetScale = Vector3.one;
		}
		for (int j = 0; j < this.bolts.Count; j++)
		{
			this.bolts[j].RemoveFromGame();
		}
		this.affectedSlots.Clear();
		this.bolts.Clear();
		this.lockContainer.UnlockAll();
	}

	private SeekingMissleAffector.Settings settings
	{
		get
		{
			return Match3Settings.instance.seekingMissleAffectorSettings;
		}
	}

	public void Init(Slot originSlot, Match3Game game, int radius, bool doPlus, BombChipAffector.PowerupType powerupType)
	{
		this.Clear();
		this.originSlot = originSlot;
		this.game = game;
		this.doPlus = doPlus;
		base.globalLock.isSlotGravitySuspended = true;
		base.globalLock.isChipGeneratorSuspended = true;
		this.radius = radius;
		IntVector2 position = originSlot.position;
		List<Slot> list = game.GetArea(position, radius);
		if (powerupType == BombChipAffector.PowerupType.Bomb)
		{
			list = game.GetBombArea(position, radius);
		}
		else if (powerupType == BombChipAffector.PowerupType.Seeking)
		{
			list = game.GetSeekingMissleArea(position);
		}
		for (int i = 0; i < list.Count; i++)
		{
			Slot slot = list[i];
			if (slot != null)
			{
				IntVector2 intVector = slot.position - position;
				int num = Mathf.Max(Mathf.Abs(intVector.x), Mathf.Abs(intVector.y));
				if (doPlus)
				{
					num = Mathf.Abs(intVector.x) + Mathf.Abs(intVector.y);
				}
				if (num <= radius)
				{
					base.globalLock.LockSlot(slot);
					if (!Match3Settings.instance.playerInputSettings.disableBombLighting || doPlus)
					{
						LightingBolt lightingBolt = game.CreateLightingBoltPowerup();
						lightingBolt.Init(originSlot, slot);
						lightingBolt.SetEndPositionFromLerp(0f);
						this.bolts.Add(lightingBolt);
					}
					this.affectedSlots.Add(slot);
				}
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

	public Slot originSlot;

	public List<Slot> affectedSlots = new List<Slot>();

	public List<LightingBolt> bolts = new List<LightingBolt>();

	private Match3Game game;

	private int radius;

	private bool doPlus;

	private float affectorDuration;

	private float angle;

	public enum PowerupType
	{
		Block,
		Bomb,
		Seeking
	}
}
