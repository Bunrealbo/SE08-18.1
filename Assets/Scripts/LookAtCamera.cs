using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	public void Start()
	{
		if (this.lookAtCamera == null)
		{
			this.lookAtCamera = Camera.main;
		}
		if (this.lookOnlyOnAwake)
		{
			this.CamLook();
		}
	}

	public void Update()
	{
		if (!this.lookOnlyOnAwake)
		{
			this.CamLook();
		}
	}

	public void CamLook()
	{
		base.transform.LookAt(this.lookAtCamera.transform);
	}

	public Camera lookAtCamera;

	public bool lookOnlyOnAwake;
}
