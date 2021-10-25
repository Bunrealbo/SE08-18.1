using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetups : MonoBehaviour
{
	public void HideAllCameras()
	{
		for (int i = 0; i < this.cameraSettings.Count; i++)
		{
			GGUtil.Hide(this.cameraSettings[i].originalTransform);
		}
	}

	public CarCamera.Settings GetCarCamera(string cameraName)
	{
		for (int i = 0; i < this.cameraSettings.Count; i++)
		{
			CarCamera.Settings settings = this.cameraSettings[i];
			if (settings.settingsName == cameraName)
			{
				return settings;
			}
		}
		return ScriptableObjectSingleton<CarsDB>.instance.GetCarCamera(this.roomName, cameraName);
	}

	public void LoadFromTransforms()
	{
		this.cameraSettings.Clear();
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			CameraSettings component = transform.GetComponent<CameraSettings>();
			GGUtil.SetActive(transform, false);
			if (!(component == null))
			{
				CarCamera.Settings settings = component.LoadSettings();
				settings.originalTransform = component.transform;
				if (settings == null)
				{
					UnityEngine.Debug.Log("CANT LOAD CAMERA " + transform.name);
				}
				this.cameraSettings.Add(settings);
			}
		}
	}

	[SerializeField]
	public List<CarCamera.Settings> cameraSettings = new List<CarCamera.Settings>();

	[SerializeField]
	private string roomName;
}
