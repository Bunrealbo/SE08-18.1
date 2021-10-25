using System;
using UnityEngine;
using UnityEngine.UI;

public class CarVariationButton : MonoBehaviour
{
	public void Init(CarVariationPanel variationPanel, int variationIndex, CarModelInfo.VariantGroup.Variation variation)
	{
		this.variationIndex = variationIndex;
		this.variationPanel = variationPanel;
		GGUtil.SetColor(this.image, variation.uiSpriteColor);
	}

	public void OnClick()
	{
		GGSoundSystem.Play(GGSoundSystem.SFXType.Flip);
		if (this.variationPanel != null)
		{
			this.variationPanel.ButtonCallback_OnChange(this);
		}
	}

	[SerializeField]
	public Image image;

	[NonSerialized]
	public int variationIndex;

	[NonSerialized]
	private CarVariationPanel variationPanel;
}
