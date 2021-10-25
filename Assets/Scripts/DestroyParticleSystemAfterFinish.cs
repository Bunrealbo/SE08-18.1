using System;
using UnityEngine;

public class DestroyParticleSystemAfterFinish : MonoBehaviour
{
	private void Start()
	{
		if (this.ps == null)
		{
			this.ps = base.GetComponent<ParticleSystem>();
		}
	}

	private void OnDisable()
	{
		this.DoDestroy();
	}

	private void DoDestroy()
	{
		if (this.isDestroying)
		{
			return;
		}
		this.isDestroying = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		if (this.ps == null)
		{
			return;
		}
		if (this.ps.IsAlive())
		{
			return;
		}
		this.DoDestroy();
	}

	[SerializeField]
	private ParticleSystem ps;

	private bool isDestroying;
}
