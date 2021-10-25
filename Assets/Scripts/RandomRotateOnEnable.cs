using System;
using UnityEngine;

public class RandomRotateOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		Quaternion localRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(this.minRotation, this.maxRotation), this.rotationAxis);
		base.transform.localRotation = localRotation;
	}

	[SerializeField]
	private Vector3 rotationAxis;

	[SerializeField]
	private float minRotation;

	[SerializeField]
	private float maxRotation;
}
