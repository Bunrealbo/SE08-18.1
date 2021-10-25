using System;
using UnityEngine;

[ExecuteInEditMode]
public class CameraSettings : MonoBehaviour
{
	public CarCamera.Settings LoadSettings()
	{
		if (this.camera == null || this.lookAtTransform == null)
		{
			return null;
		}
		CarCamera.Settings settings = new CarCamera.Settings();
		settings.settingsName = base.transform.name;
		settings.enableRotationCenter = true;
		settings.rotationCenter = this.lookAtTransform.position;
		settings.cameraDistance = -Vector3.Distance(this.lookAtTransform.position, this.camera.transform.position);
		settings.changeAnglesAtStart = this.changeAnglesAtStart;
		settings.horizontalAngleSpeed = 0.2f;
		settings.verticalAngleSpeed = 0.2f;
		settings.verticalAngleRange.min = 0f;
		settings.verticalAngleRange.max = 45f;
		float num = Vector3.Distance(this.lookAtTransform.position, this.camera.transform.position);
		float f = this.camera.transform.position.y - this.lookAtTransform.position.y;
		float startVerticalAngle = 0f;
		if (Mathf.Abs(f) > Mathf.Epsilon && num > 0f)
		{
			startVerticalAngle = Mathf.Sign(f) * Mathf.Asin(Mathf.Abs(f) / num) * 57.29578f;
		}
		settings.startVerticalAngle = startVerticalAngle;
		Vector3 to = this.lookAtTransform.position - this.camera.transform.position;
		to.y = 0f;
		settings.startHorizontalAngle = Vector3.SignedAngle(Vector3.forward, to, Vector3.up);
		settings.fov = this.camera.fieldOfView;
		return settings;
	}

	private void Update()
	{
		if (this.camera == null || this.lookAtTransform == null)
		{
			return;
		}
		this.camera.transform.rotation = Quaternion.LookRotation(this.lookAtTransform.transform.position - this.camera.transform.position);
	}

	[SerializeField]
	private Camera camera;

	[SerializeField]
	private Transform lookAtTransform;

	[SerializeField]
	private bool changeAnglesAtStart = true;
}
