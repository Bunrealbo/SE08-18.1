using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarSubPartInfo
{
	[SerializeField]
	public Vector3 offset;

	[SerializeField]
	public string displayName;

	[SerializeField]
	public string cameraName;

	[SerializeField]
	public bool showAtStart;

	[SerializeField]
	public bool directControl;

	[SerializeField]
	public bool overrideChangeAnimOffset;

	[SerializeField]
	public bool removing;

	[SerializeField]
	public bool showNutTutorialHand;

	[SerializeField]
	public bool showChangeAnimAfterIn;

	[SerializeField]
	public Vector3 changeAnimOffset = Vector3.up;

	[SerializeField]
	public List<string> toSayBefore = new List<string>();

	[SerializeField]
	public bool hideToSayWhenWorking;

	[SerializeField]
	public List<string> toSayBeforeWork = new List<string>();

	[SerializeField]
	public CarSubPartInfo.RotateSettings rotateSettings = new CarSubPartInfo.RotateSettings();

	[Serializable]
	public class RotateSettings
	{
		public bool enabled;

		public Vector3 axis;

		public float initialAngle;

		public float outAngle;

		public Vector3 forwardDirection;
	}
}
