using System;
using UnityEngine;

public class RotateY : MonoBehaviour
{
	public void Update()
	{
		Quaternion rotation = base.transform.rotation;
		Vector3 eulerAngles = rotation.eulerAngles;
		eulerAngles.y += 1f * Time.deltaTime;
		rotation.eulerAngles = eulerAngles;
		base.transform.rotation = rotation;
	}
}
