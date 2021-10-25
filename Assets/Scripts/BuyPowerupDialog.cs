using System;
using System.Collections.Generic;
using GGMatch3;
using TMPro;
using UnityEngine;

public class BuyPowerupDialog : MonoBehaviour
{
	public static BuyPowerupDialog instance
	{
		get
		{
			return NavigationManager.instance.GetObject<BuyPowerupDialog>();
		}
	}

	public static void Show(BuyPowerupDialog.InitArguments initArguments)
	{
		BuyPowerupDialog instance = BuyPowerupDialog.instance;
		instance.Init(initArguments);
		GGUtil.SetActive(instance, true);
		GGSoundSystem.Play(GGSoundSystem.SFXType.FlyIn);
	}

	public void Hide()
	{
		GGUtil.Hide(this);
	}

	public void Init(BuyPowerupDialog.InitArguments initArguments)
	{
		this.initArguments = initArguments;
		GGUtil.SetActive(this.widgetsToHide, false);
		BuyPowerupDialog.PowerupDefinition definition = this.GetDefinition(initArguments.powerupType);
		PowerupsDB.PowerupDefinition powerup = initArguments.powerup;
		definition.Show();
		GGUtil.ChangeText(this.titleLabel, powerup.name);
		GGUtil.ChangeText(this.descriptionLabel, powerup.description);
		GGUtil.ChangeText(this.quantityLabel, string.Format("x{0}", powerup.buyQuanitty));
		GGUtil.ChangeText(this.priceLabel, powerup.buyPrice.cost);
	}

	private BuyPowerupDialog.PowerupDefinition GetDefinition(PowerupType powerupType)
	{
		for (int i = 0; i < this.powerups.Count; i++)
		{
			BuyPowerupDialog.PowerupDefinition powerupDefinition = this.powerups[i];
			if (powerupDefinition.powerupType == powerupType)
			{
				return powerupDefinition;
			}
		}
		return null;
	}

	private void OnComplete(bool success)
	{
		if (this.initArguments.onSuccess == null)
		{
			return;
		}
		this.initArguments.onSuccess(success);
	}

	public void ButtonCallback_OnPressed()
	{
		WalletManager walletManager = GGPlayerSettings.instance.walletManager;
		NavigationManager instance = NavigationManager.instance;
		PowerupsDB.PowerupDefinition powerup = this.initArguments.powerup;
		if (!walletManager.CanBuyItemWithPrice(powerup.buyPrice))
		{
			this.Hide();
			this.OnComplete(false);
			instance.GetObject<CurrencyPurchaseDialog>().Show(ScriptableObjectSingleton<OffersDB>.instance, null);
			return;
		}
		powerup.ownedCount += (long)powerup.buyQuanitty;
		walletManager.BuyItem(powerup.buyPrice);
		this.OnComplete(true);
		this.Hide();
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}

	public void ButtonCallback_OnHide()
	{
		this.OnComplete(false);
		this.Hide();
		GGSoundSystem.Play(GGSoundSystem.SFXType.CancelPress);
	}

	[SerializeField]
	private TextMeshProUGUI titleLabel;

	[SerializeField]
	private TextMeshProUGUI descriptionLabel;

	[SerializeField]
	private TextMeshProUGUI priceLabel;

	[SerializeField]
	private TextMeshProUGUI quantityLabel;

	[SerializeField]
	private List<RectTransform> widgetsToHide = new List<RectTransform>();

	[SerializeField]
	private List<BuyPowerupDialog.PowerupDefinition> powerups = new List<BuyPowerupDialog.PowerupDefinition>();

	private BuyPowerupDialog.InitArguments initArguments;

	[Serializable]
	private class PowerupDefinition
	{
		public void Show()
		{
			GGUtil.Show(this.container);
		}

		public PowerupType powerupType;

		public RectTransform container;
	}

	public struct InitArguments
	{
		public PowerupType powerupType
		{
			get
			{
				return this.powerup.type;
			}
		}

		public PowerupsDB.PowerupDefinition powerup;

		public Action<bool> onSuccess;
	}
}
