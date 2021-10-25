using System;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	private void Awake()
	{
		CameraShake.myCameraShake = this;
		this.startPos = base.transform.position;
	}

	private void Update()
	{
		if (this.shakeTimer >= 0f)
		{
			Vector2 vector = UnityEngine.Random.insideUnitCircle * this.shakeAmount;
			base.transform.position = new Vector3(base.transform.position.x + vector.x * 0.3f, base.transform.position.y + vector.y, base.transform.position.z);
			this.shakeTimer -= Time.deltaTime;
			return;
		}
		base.transform.position = this.startPos;
	}

	public void ShakeCamera(float shakePwr, float shakeDur)
	{
		this.shakeAmount = shakePwr;
		this.shakeTimer = shakeDur;
	}

	public float shakeTimer;

	public float shakeAmount;

	public static CameraShake myCameraShake;

	private Vector3 startPos;
}
