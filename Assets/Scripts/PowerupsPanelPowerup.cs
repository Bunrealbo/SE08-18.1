using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupsPanelPowerup : MonoBehaviour
{
	public void ShowArrow()
	{
		if (this.powerup == null)
		{
			return;
		}
		bool active = this.powerup.ownedCount > 0L;
		GGUtil.SetActive(this.arrowAnimator, active);
	}

	public void Init(PowerupsDB.PowerupDefinition powerup, PowerupsPanel panel)
	{
		this.panel = panel;
		this.powerup = powerup;
		GGUtil.SetActive(this.objectsToHide, false);
		PowerupsPanelPowerup.PowerupDefinition definition = this.GetDefinition(powerup);
		if (definition != null)
		{
			definition.Show();
		}
		long ownedCount = powerup.ownedCount;
		if (ownedCount > 0L)
		{
			this.activeStyle.Apply();
		}
		else
		{
			this.notActiveStyle.Apply();
		}
		if (ownedCount <= 0L)
		{
			GGUtil.ChangeText(this.countLabel, "-");
		}
		else
		{
			GGUtil.ChangeText(this.countLabel, ownedCount);
		}
		bool flag = powerup.ownedCount > 0L && powerup.usedCount == 0L;
		bool active = false;
		GGUtil.SetActive(this.arrowAnimator, active);
	}

	private PowerupsPanelPowerup.PowerupDefinition GetDefinition(PowerupsDB.PowerupDefinition powerup)
	{
		for (int i = 0; i < this.powerups.Count; i++)
		{
			PowerupsPanelPowerup.PowerupDefinition powerupDefinition = this.powerups[i];
			if (powerupDefinition.powerupType == powerup.type)
			{
				return powerupDefinition;
			}
		}
		return null;
	}

	public void ButtonCallback_OnPressed()
	{
		GGUtil.SetActive(this.arrowAnimator, false);
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
		this.panel.gameScreen.Callback_ShowActivatePowerup(this);
	}

	[SerializeField]
	private List<RectTransform> objectsToHide = new List<RectTransform>();

	[SerializeField]
	private List<PowerupsPanelPowerup.PowerupDefinition> powerups = new List<PowerupsPanelPowerup.PowerupDefinition>();

	[SerializeField]
	private Transform arrowAnimator;

	[SerializeField]
	private TextMeshProUGUI countLabel;

	[SerializeField]
	private PowerupsPanelPowerup.ItemStyleSet activeStyle = new PowerupsPanelPowerup.ItemStyleSet();

	[SerializeField]
	private PowerupsPanelPowerup.ItemStyleSet notActiveStyle = new PowerupsPanelPowerup.ItemStyleSet();

	[NonSerialized]
	private PowerupsPanel panel;

	[NonSerialized]
	public PowerupsDB.PowerupDefinition powerup;

	[Serializable]
	public class PowerupDefinition
	{
		public void Show()
		{
			GGUtil.Show(this.container);
		}

		public PowerupType powerupType;

		public RectTransform container;
	}

	[Serializable]
	public class ItemStyleSet
	{
		public void Apply()
		{
			for (int i = 0; i < this.styleChanges.Count; i++)
			{
				this.styleChanges[i].Apply();
			}
		}

		[SerializeField]
		private List<PowerupsPanelPowerup.ItemStyle> styleChanges = new List<PowerupsPanelPowerup.ItemStyle>();
	}

	[Serializable]
	public class ItemStyle
	{
		public void Apply()
		{
			GGUtil.SetColor(this.image, this.color);
		}

		[SerializeField]
		private Image image;

		[SerializeField]
		private Color color;
	}
}
