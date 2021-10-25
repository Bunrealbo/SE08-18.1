using System;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
	public void Start()
	{
		this.randomRot();
		base.InvokeRepeating("randomRot", 0f, this.rotateEverySecond);
	}

	public void Update()
	{
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.rotTarget, this.lerpCounter * Time.deltaTime);
		this.lerpCounter += 1f;
	}

	public void randomRot()
	{
		this.rotTarget = UnityEngine.Random.rotation;
		this.lerpCounter = 0f;
	}

	private Quaternion rotTarget;

	public float rotateEverySecond = 1f;

	private float lerpCounter;
}
