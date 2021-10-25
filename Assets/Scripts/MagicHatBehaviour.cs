using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;

public class MagicHatBehaviour : SlotComponentBehavoiour
{
	public void Init(ChipType chipType)
	{
		for (int i = 0; i < this.chipLooks.Count; i++)
		{
			MagicHatBehaviour.ChipTypeLook chipTypeLook = this.chipLooks[i];
			GGUtil.SetActive(chipTypeLook.widgetsToShow, chipTypeLook.chipType == chipType);
		}
	}

	public void SetCountActive(bool active)
	{
		GGUtil.SetActive(this.text, active);
		GGUtil.SetActive(this.nonCountObject, !active);
	}

	public void SetCount(int count)
	{
		if (this.text == null)
		{
			return;
		}
		this.text.text = count.ToString();
	}

	public Vector3 bunnyScale
	{
		get
		{
			return this.bunnyTransform.localScale;
		}
		set
		{
			this.bunnyTransform.localScale = value;
		}
	}

	public Vector3 bunnyOffset
	{
		get
		{
			return this.bunnyTransform.localPosition;
		}
		set
		{
			this.bunnyTransform.localPosition = value;
		}
	}

	[SerializeField]
	private Transform bunnyTransform;

	[SerializeField]
	private TextMeshPro text;

	[SerializeField]
	private Transform nonCountObject;

	[SerializeField]
	private List<MagicHatBehaviour.ChipTypeLook> chipLooks = new List<MagicHatBehaviour.ChipTypeLook>();

	[Serializable]
	public class ChipTypeLook
	{
		public ChipType chipType;

		public List<Transform> widgetsToShow = new List<Transform>();
	}
}
