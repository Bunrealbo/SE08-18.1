using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class VariationPanel : MonoBehaviour
{
	public int variationIndexAtStart
	{
		get
		{
			return this._003CvariationIndexAtStart_003Ek__BackingField;
		}
		protected set
		{
			this._003CvariationIndexAtStart_003Ek__BackingField = value;
		}
	}

	public bool isVariationChanged
	{
		get
		{
			return this._003CisVariationChanged_003Ek__BackingField;
		}
		protected set
		{
			this._003CisVariationChanged_003Ek__BackingField = value;
		}
	}

	public void Show(DecorateRoomScreen screen, DecorateRoomSceneVisualItem uiItem, VariationPanel.InitParams initParams)
	{
		this.isVariationChanged = false;
		this.screen = screen;
		this.uiItem = uiItem;
		this.initParams = initParams;
		GGUtil.SetActive(uiItem, true);
		uiItem.HideButton();
		uiItem.ShowMarkers();
		this.buttonPool.Clear();
		VisualObjectBehaviour visualObjectBehaviour = uiItem.visualObjectBehaviour;
		List<VisualObjectVariation> variations = visualObjectBehaviour.variations;
		this.buttonPool.parent.GetComponent<RectTransform>();
		float x = this.buttonPool.prefabSizeDelta.x;
		float num = (float)variations.Count * x + (float)(variations.Count - 1) * this.buttonPadding;
		for (int i = 0; i < variations.Count; i++)
		{
			VariationButton variationButton = this.buttonPool.Next<VariationButton>(false);
			Vector3 zero = Vector3.zero;
			zero.x = -num * 0.5f + ((float)i + 0.5f) * x + (float)i * this.buttonPadding;
			GGUtil.SetActive(variationButton, true);
			variationButton.transform.localPosition = zero;
			variationButton.Init(this, visualObjectBehaviour, i);
		}
		this.buttonPool.HideNotUsed();
		GGUtil.SetActive(this, true);
		visualObjectBehaviour.activeVariation.ScaleAnimation(0f, false);
		this.variationIndexAtStart = uiItem.visualObjectBehaviour.visualObject.ownedVariationIndex;
	}

	public void OnBackgroundClick()
	{
		GGUtil.SetActive(this, false);
		this.screen.VariationPanelCallback_OnClosed(this);
		int ownedVariationIndex = this.uiItem.visualObjectBehaviour.visualObject.ownedVariationIndex;
		if (this.variationIndexAtStart != ownedVariationIndex)
		{
			Analytics.RoomItemChangedEvent roomItemChangedEvent = new Analytics.RoomItemChangedEvent();
			roomItemChangedEvent.variation = this.uiItem.visualObjectBehaviour.visualObject.variations[ownedVariationIndex];
			roomItemChangedEvent.visualObject = this.uiItem.visualObjectBehaviour.visualObject;
			DecorateRoomScreen @object = NavigationManager.instance.GetObject<DecorateRoomScreen>();
			roomItemChangedEvent.screen = @object;
			roomItemChangedEvent.Send();
		}
	}

	public void ButtonCallback_OnChange()
	{
		this.isVariationChanged = true;
	}

	[SerializeField]
	private ComponentPool buttonPool = new ComponentPool();

	[SerializeField]
	private float buttonPadding;

	[NonSerialized]
	private DecorateRoomScreen screen;

	[NonSerialized]
	public VariationPanel.InitParams initParams;

	[NonSerialized]
	public DecorateRoomSceneVisualItem uiItem;

	private int _003CvariationIndexAtStart_003Ek__BackingField;

	private bool _003CisVariationChanged_003Ek__BackingField;

	public struct InitParams
	{
		public bool isPurchased;
	}
}
