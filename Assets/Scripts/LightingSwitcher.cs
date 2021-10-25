using System;
using UnityEngine;

public class LightingSwitcher : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.treatments.Length; i++)
		{
			this.treatments[i].DeactivateTreatment(this.sunTransform);
		}
		this.currentIndex = this.startingIndex;
		this.treatments[this.currentIndex].ActivateTreatment(this.sunTransform);
	}

	public void CycleTreatments()
	{
		this.treatments[this.currentIndex].DeactivateTreatment(this.sunTransform);
		this.currentIndex = (this.treatments.Length + this.currentIndex + 1) % this.treatments.Length;
		this.treatments[this.currentIndex].ActivateTreatment(this.sunTransform);
	}

	public int startingIndex;

	public Transform sunTransform;

	public LightingSwitcher.SkyboxLightingPair[] treatments;

	private int currentIndex;

	[Serializable]
	public class SkyboxLightingPair
	{
		public void ActivateTreatment(Transform sunTransform)
		{
			this.lightingGameObject.SetActive(true);
			RenderSettings.skybox = this.skyBoxMaterial;
			this.camera.SetActive(true);
			sunTransform.localEulerAngles = this.sunEulerAngles;
		}

		public void DeactivateTreatment(Transform sunTransform)
		{
			this.camera.SetActive(false);
			this.lightingGameObject.SetActive(false);
			sunTransform.localEulerAngles = this.sunEulerAngles;
		}

		public Material skyBoxMaterial;

		public GameObject lightingGameObject;

		public GameObject camera;

		public Vector3 sunEulerAngles;
	}
}
