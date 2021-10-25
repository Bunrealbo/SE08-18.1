using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class SeekingMissleAffector : PlayerInput.AffectorBase
{
	public override void Clear()
	{
		this.affectorDuration = 0f;
		for (int i = 0; i < this.bolts.Count; i++)
		{
			this.bolts[i].RemoveFromGame();
		}
		this.bolts.Clear();
		Slot.RemoveLocks(this.lockedSlots, this.globalLock);
	}

	private SeekingMissleAffector.Settings settings
	{
		get
		{
			return Match3Settings.instance.seekingMissleAffectorSettings;
		}
	}

	public override float minAffectorDuration
	{
		get
		{
			return this.settings.minAffectorDuration;
		}
	}

	public override void OnUpdate(PlayerInput.AffectorUpdateParams updateParams)
	{
		this.angle += Time.deltaTime * this.settings.angleSpeed;
		this.affectorDuration += Time.deltaTime;
		float endPositionFromLerp = Mathf.InverseLerp(0f, this.settings.outDuration, this.affectorDuration);
		for (int i = 0; i < this.bolts.Count; i++)
		{
			this.bolts[i].SetEndPositionFromLerp(endPositionFromLerp);
		}
		Slot slot = this.input.game.GetSlot(this.startPosition);
		for (int j = 0; j < this.affectedSlots.Count; j++)
		{
			Slot slot2 = this.affectedSlots[j];
			this.ApplyShake(slot2, slot.localPositionOfCenter);
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

	public IntVector2 startPosition;

	public List<Slot> affectedSlots;

	public List<Slot> lockedSlots;

	public List<LightingBolt> bolts;

	public Lock globalLock;

	private PlayerInput input;

	private float angle;

	[Serializable]
	public class Settings
	{
		public float minAffectorDuration = 0.25f;

		public float outDuration = 0.1f;

		public float angleSpeed = 100f;

		public float phaseOffsetMult = 1f;

		public float amplitude = 0.05f;
	}
}
