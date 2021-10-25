using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarSettings
{
	public CarCamera.Settings GetSettings(string name)
	{
		for (int i = 0; i < this.carCameraSettings.Count; i++)
		{
			CarCamera.Settings settings = this.carCameraSettings[i];
			if (settings.settingsName == name)
			{
				return settings;
			}
		}
		return null;
	}

	[SerializeField]
	public string carName;

	[SerializeField]
	private List<CarCamera.Settings> carCameraSettings = new List<CarCamera.Settings>();
}
