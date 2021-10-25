using System;
using GGMatch3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftBoxDisplay : MonoBehaviour
{
	private void Init()
	{
		GiftsDefinitionDB.GiftDefinition currentGift = ScriptableObjectSingleton<GiftsDefinitionDB>.instance.currentGift;
		if (currentGift == null)
		{
			GGUtil.Hide(this.mainContainer);
			return;
		}
		GGUtil.Show(this.mainContainer);
		GGUtil.SetFill(this.fillSprite, currentGift.progress);
		GiftsDefinitionDB.GiftDefinition.StagesPassedDescriptor stagesPassedDescriptor = currentGift.stagesPassedDescriptor;
		GGUtil.ChangeText(this.countLabel, string.Format("{0}/{1}", stagesPassedDescriptor.currentStagesPassed, stagesPassedDescriptor.stagesNeededToPass));
		GGUtil.SetActive(this.buttonContainer, currentGift.isAvailableToCollect);
	}

	public void ButtonCallback_OnClick()
	{
		GiftsDefinitionDB.GiftDefinition currentGift = ScriptableObjectSingleton<GiftsDefinitionDB>.instance.currentGift;
		if (currentGift == null)
		{
			this.Init();
			return;
		}
		if (!currentGift.isAvailableToCollect)
		{
			this.Init();
			return;
		}
		GiftBoxScreen @object = NavigationManager.instance.GetObject<GiftBoxScreen>();
		GiftBoxScreen.ShowArguments showArguments = default(GiftBoxScreen.ShowArguments);
		showArguments.giftsDefinition = currentGift.gifts;
		showArguments.title = "Gift Box";
		currentGift.ClaimGifts();
		@object.Show(showArguments);
	}

	private void OnEnable()
	{
		this.Init();
	}

	[SerializeField]
	private Transform mainContainer;

	[SerializeField]
	private TextMeshProUGUI countLabel;

	[SerializeField]
	private Image fillSprite;

	[SerializeField]
	private Transform buttonContainer;
}
