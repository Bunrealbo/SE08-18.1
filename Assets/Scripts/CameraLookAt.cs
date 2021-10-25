using System;
using UnityEngine;

[ExecuteInEditMode]
public class CameraLookAt : MonoBehaviour
{
	private void Update()
	{
		if (this.lookAt == null)
		{
			return;
		}
		base.transform.rotation = Quaternion.LookRotation(this.lookAt.position - base.transform.position);
	}

	[SerializeField]
	private Transform lookAt;
}
