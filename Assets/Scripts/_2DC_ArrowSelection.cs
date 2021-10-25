using System;
using UnityEngine;

public class _2DC_ArrowSelection : MonoBehaviour
{
	private void Update()
	{
		if (this.timeskip < 1f)
		{
			this.timemult = 1f;
		}
		if (this.timeskip > 1f)
		{
			this.timemult = 1.2f;
		}
		if (this.timeskip > 1.1f)
		{
			this.timemult = 1.4f;
		}
		if (this.timeskip > 1.2f)
		{
			this.timemult = 1.6f;
		}
		if (this.timeskip > 1.3f)
		{
			this.timemult = 1.8f;
		}
		if (this.timeskip > 1.4f)
		{
			this.timemult = 2f;
		}
		if (this.timeskip > 1.5f)
		{
			this.timemult = 2.2f;
		}
		if (this.timeskip > 1.6f)
		{
			this.timemult = 2.5f;
		}
		if (this.timeskip > 1.7f)
		{
			this.timemult = 2.8f;
		}
		if (this.timeskip > 1.8f)
		{
			this.timemult = 3f;
		}
		if (this.timeskip > 1.9f)
		{
			this.timemult = 4f;
		}
		if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
		{
			this.pos = this.cam.transform.position;
			this.pos.x = this.pos.x + -0.2f;
			this.cam.transform.position = this.pos;
		}
		if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
		{
			this.pos = this.cam.transform.position;
			this.pos.x = this.pos.x + 0.2f;
			this.cam.transform.position = this.pos;
		}
	}

	private void OnMouseDrag()
	{
		this.pos = this.cam.transform.position;
		this.pos.x = this.pos.x - this.posx * this.timemult;
		this.cam.transform.position = this.pos;
		this.timeskip += Time.deltaTime;
	}

	private void OnMouseExit()
	{
		this.timeskip = 0f;
	}

	private void OnMouseUp()
	{
		this.timeskip = 0f;
	}

	public Camera cam;

	private Vector3 pos;

	private float x;

	public float posx;

	private float timeskip;

	private float timemult;
}
