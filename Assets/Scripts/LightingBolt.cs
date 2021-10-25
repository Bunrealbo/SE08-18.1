using System;
using DigitalRuby.ThunderAndLightning;
using GGMatch3;
using UnityEngine;

public class LightingBolt : MonoBehaviour
{
	public void Init(Slot startSlot, Slot endSlot)
	{
		this.startSlot = startSlot;
		this.endSlot = endSlot;
		this.Init(startSlot.localPositionOfCenter, endSlot.localPositionOfCenter);
		this.SetSlotPositions(startSlot.position, endSlot.position);
	}

	public void SetSlotPositions(IntVector2 startSlotPosition, IntVector2 endSlotPosition)
	{
		this.isSlotPositionsSet = true;
		this.startSlotPosition = startSlotPosition;
		this.endSlotPosition = endSlotPosition;
	}

	public void Init(Vector3 localStart, Vector3 localEnd)
	{
		base.transform.localPosition = Vector3.zero;
		this.bolt.Source.transform.localPosition = localStart;
		this.bolt.Destination.transform.localPosition = localEnd;
		Camera camera = NavigationManager.instance.GetCamera();
		this.bolt.Camera = camera;
		GGUtil.SetActive(this, true);
		GGUtil.SetActive(this.bolt, true);
	}

	public void SetEndPositionFromLerp(float n)
	{
		if (this.endSlot == null)
		{
			return;
		}
		this.SetEndPosition(Vector3.Lerp(this.bolt.Source.transform.localPosition, this.endSlot.localPositionOfCenter + this.endSlot.offsetPosition, n));
	}

	public void SetEndPosition(Vector3 localPosition)
	{
		if (this.startSlot == null)
		{
			return;
		}
		this.bolt.Destination.transform.localPosition = localPosition;
	}

	public void SetStartPosition(Vector3 localPosition)
	{
		this.bolt.Source.transform.localPosition = localPosition;
	}

	public void SetPositionFromSlots()
	{
		if (this.startSlot != null)
		{
			this.bolt.Source.transform.localPosition = this.startSlot.localPositionOfCenter + this.startSlot.offsetPosition;
		}
		if (this.endSlot != null)
		{
			this.bolt.Destination.transform.localPosition = this.endSlot.localPositionOfCenter + this.endSlot.offsetPosition;
		}
	}

	public void SetPositionFromChips()
	{
		if (this.startSlot != null)
		{
			Chip slotComponent = this.startSlot.GetSlotComponent<Chip>();
			TransformBehaviour transformBehaviour = null;
			if (slotComponent != null)
			{
				transformBehaviour = slotComponent.GetComponentBehaviour<TransformBehaviour>();
			}
			if (transformBehaviour != null)
			{
				this.bolt.Source.transform.localPosition = transformBehaviour.localPosition;
			}
			else
			{
				this.bolt.Source.transform.localPosition = this.startSlot.localPositionOfCenter + this.startSlot.offsetPosition;
			}
		}
		if (this.endSlot != null)
		{
			Chip slotComponent2 = this.endSlot.GetSlotComponent<Chip>();
			TransformBehaviour transformBehaviour2 = null;
			if (slotComponent2 != null)
			{
				transformBehaviour2 = slotComponent2.GetComponentBehaviour<TransformBehaviour>();
			}
			if (transformBehaviour2 != null)
			{
				this.bolt.Destination.transform.localPosition = transformBehaviour2.localPosition;
				return;
			}
			this.bolt.Destination.transform.localPosition = this.endSlot.localPositionOfCenter + this.endSlot.offsetPosition;
		}
	}

	public void HideParticle()
	{
		GGUtil.Hide(this.particleTransform);
	}

	public void RemoveFromGame()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[SerializeField]
	private LightningBoltPrefabScript bolt;

	[SerializeField]
	private Transform particleTransform;

	[NonSerialized]
	public Slot startSlot;

	[NonSerialized]
	public bool isSlotPositionsSet;

	[NonSerialized]
	public IntVector2 startSlotPosition;

	[NonSerialized]
	public IntVector2 endSlotPosition;

	[NonSerialized]
	public Slot endSlot;
}
