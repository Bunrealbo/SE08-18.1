using System;
using System.Collections.Generic;
using GGMatch3;
using UnityEngine;

public class PowerupsPanel : MonoBehaviour
{
	public void Refresh()
	{
		if (this.gameScreen == null)
		{
			return;
		}
		this.Init(this.gameScreen);
	}

	public void ShowArrowsOnAvailablePowerups()
	{
		List<GameObject> usedObjects = this.powerupsPool.usedObjects;
		for (int i = 0; i < usedObjects.Count; i++)
		{
			GameObject gameObject = usedObjects[i];
			if (!(gameObject == null))
			{
				PowerupsPanelPowerup component = gameObject.GetComponent<PowerupsPanelPowerup>();
				if (!(component == null))
				{
					component.ShowArrow();
				}
			}
		}
	}

	public void ReinitPowerups()
	{
		List<GameObject> usedObjects = this.powerupsPool.usedObjects;
		for (int i = 0; i < usedObjects.Count; i++)
		{
			GameObject gameObject = usedObjects[i];
			if (!(gameObject == null))
			{
				PowerupsPanelPowerup component = gameObject.GetComponent<PowerupsPanelPowerup>();
				if (!(component == null))
				{
					component.Init(component.powerup, this);
				}
			}
		}
	}

	public void Init(GameScreen gameScreen)
	{
		this.gameScreen = gameScreen;
		Vector2 prefabSizeDelta = this.powerupsPool.prefabSizeDelta;
		List<PowerupsDB.PowerupDefinition> list = ScriptableObjectSingleton<PowerupsDB>.instance.powerups;
		Vector2 sizeDelta = this.container.sizeDelta;
		Vector3 a = new Vector3(0f, prefabSizeDelta.y * ((float)list.Count * 0.5f - 0.5f), 0f);
		this.powerupsPool.Clear();
		this.powerups.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			PowerupsDB.PowerupDefinition powerup = list[i];
			PowerupsPanelPowerup powerupsPanelPowerup = this.powerupsPool.Next<PowerupsPanelPowerup>(true);
			powerupsPanelPowerup.transform.localPosition = a + Vector3.down * (prefabSizeDelta.y * (float)i);
			powerupsPanelPowerup.Init(powerup, this);
			GGUtil.Show(powerupsPanelPowerup);
			this.powerups.Add(powerupsPanelPowerup);
		}
		this.powerupsPool.HideNotUsed();
	}

	private void OnEnable()
	{
		if (this.gameScreen != null)
		{
			this.Init(this.gameScreen);
		}
	}

	[SerializeField]
	private ComponentPool powerupsPool = new ComponentPool();

	[SerializeField]
	private RectTransform container;

	[NonSerialized]
	public GameScreen gameScreen;

	[NonSerialized]
	private List<PowerupsPanelPowerup> powerups = new List<PowerupsPanelPowerup>();
}
