using System;
using System.Collections.Generic;
using UnityEngine;

public class CarVariationPanel : MonoBehaviour
{
	public CarModelInfo.VariantGroup variantGroup
	{
		get
		{
			return this.initParams.variantGroup;
		}
	}

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

	public int selectedVariation
	{
		get
		{
			return this._003CselectedVariation_003Ek__BackingField;
		}
		set
		{
			this._003CselectedVariation_003Ek__BackingField = value;
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

	public void Show(CarVariationPanel.InitParams initParams)
	{
		this.isVariationChanged = false;
		this.initParams = initParams;
		CarModelInfo.VariantGroup variantGroup = initParams.variantGroup;
		this.variationIndexAtStart = variantGroup.selectedVariationIndex;
		this.selectedVariation = this.variationIndexAtStart;
		this.initParams = initParams;
		GGUtil.SetActive(this.backgroundClickArea, initParams.showBackground);
		if (initParams.inputHandler != null)
		{
			initParams.inputHandler.onClick -= this.OnInputHandlerClick;
			initParams.inputHandler.onClick += this.OnInputHandlerClick;
		}
		this.buttonPool.Clear();
		List<CarModelInfo.VariantGroup.Variation> variations = variantGroup.variations;
		float x = this.buttonPool.prefabSizeDelta.x;
		float num = (float)variations.Count * x + (float)(variations.Count - 1) * this.buttonPadding;
		for (int i = 0; i < variations.Count; i++)
		{
			CarModelInfo.VariantGroup.Variation variation = variations[i];
			CarVariationButton carVariationButton = this.buttonPool.Next<CarVariationButton>(false);
			Vector3 zero = Vector3.zero;
			zero.x = -num * 0.5f + ((float)i + 0.5f) * x + (float)i * this.buttonPadding;
			GGUtil.SetActive(carVariationButton, true);
			carVariationButton.transform.localPosition = zero;
			carVariationButton.Init(this, i, variation);
		}
		this.buttonPool.HideNotUsed();
		GGUtil.SetActive(this, true);
	}

	private void OnInputHandlerClick(Vector2 position)
	{
		this.OnBackgroundClick();
	}

	public void OnBackgroundClick()
	{
		GGUtil.SetActive(this, false);
		this.initParams.CallOnClosed(this);
	}

	public void ButtonCallback_OnChange(CarVariationButton button)
	{
		this.selectedVariation = button.variationIndex;
		this.initParams.variantGroup.selectedVariationIndex = this.selectedVariation;
		this.initParams.screen.scene.carModel.RefreshVariations();
		this.initParams.CallOnChange(this);
		this.isVariationChanged = true;
	}

	private void OnDisable()
	{
		if (this.initParams.inputHandler != null)
		{
			this.initParams.inputHandler.onClick -= this.OnInputHandlerClick;
		}
	}

	[SerializeField]
	private ComponentPool buttonPool = new ComponentPool();

	[SerializeField]
	private float buttonPadding;

	[SerializeField]
	private Transform backgroundClickArea;

	[NonSerialized]
	public CarVariationPanel.InitParams initParams;

	private int _003CvariationIndexAtStart_003Ek__BackingField;

	private int _003CselectedVariation_003Ek__BackingField;

	private bool _003CisVariationChanged_003Ek__BackingField;

	public struct InitParams
	{
		public void CallOnClosed(CarVariationPanel panel)
		{
			if (this.onClosed != null)
			{
				this.onClosed(panel);
			}
		}

		public void CallOnChange(CarVariationPanel panel)
		{
			Action<CarVariationPanel> action = this.onChange;
			if (action == null)
			{
				return;
			}
			action(panel);
		}

		public bool isPurchased;

		public AssembleCarScreen screen;

		public CarModelInfo.VariantGroup variantGroup;

		public Action<CarVariationPanel> onClosed;

		public Action<CarVariationPanel> onChange;

		public bool showBackground;

		public InputHandler inputHandler;
	}
}
