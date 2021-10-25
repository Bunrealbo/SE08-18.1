using System;
using System.Collections.Generic;
using UnityEngine;

public class CarVisualItemButton : MonoBehaviour
{
	public void Init(CarModelPart part, AssembleCarScreen screen)
	{
		this.part = part;
		this.screen = screen;
		CarPartInfo partInfo = part.partInfo;
		GGUtil.SetActive(this.widgetsToHide, false);
		bool active = partInfo.isUnlocked && !partInfo.isOwned;
		GGUtil.SetActive(this.buyButtonContanier, active);
		part.SetActiveIfOwned();
		this.SetPositionOfBuyButton();
	}

	private void SetPositionOfBuyButton()
	{
		Vector3 localPosition = this.screen.TransformWorldCarPointToLocalUIPosition(this.part.buttonHandlePosition);
		localPosition.z = 0f;
		this.buyButtonContanier.localPosition = localPosition;
	}

	private void Update()
	{
		if (this.part == null)
		{
			return;
		}
		this.SetPositionOfBuyButton();
	}

	public void HideButton()
	{
		GGUtil.SetActive(this.buyButtonContanier, false);
	}

	public void ButtonCallback_OnBuyButton()
	{
		this.screen.VisualItemCallback_OnBuyItemPressed(this);
	}

	[SerializeField]
	private List<Transform> widgetsToHide = new List<Transform>();

	[SerializeField]
	private Transform buyButtonContanier;

	[NonSerialized]
	public CarModelPart part;

	[NonSerialized]
	private AssembleCarScreen screen;
}
