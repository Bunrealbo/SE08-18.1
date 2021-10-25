using System;
using System.Collections.Generic;
using UnityEngine;

public class Match3ParticleSystem : MonoBehaviour
{
	public void StartParticleSystems()
	{
		this.Init();
		this.started = true;
	}

	public List<ParticleSystem> GetAllParticleSystems()
	{
		this.Init();
		return this.particleSystems;
	}

	private void Init()
	{
		if (this.initialized)
		{
			return;
		}
		this.particleSystems.Clear();
		this.initialized = true;
		this.TryAddParticleSystemFromTransform(base.transform);
	}

	private void TryAddParticleSystemFromTransform(Transform t)
	{
		if (t == null)
		{
			return;
		}
		ParticleSystem component = t.GetComponent<ParticleSystem>();
		if (component != null)
		{
			this.particleSystems.Add(component);
		}
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			if (child.gameObject.activeSelf || this.includeHidden)
			{
				this.TryAddParticleSystemFromTransform(child);
			}
		}
	}

	private void Update()
	{
		if (!this.started)
		{
			return;
		}
		for (int i = 0; i < this.particleSystems.Count; i++)
		{
			if (this.particleSystems[i].IsAlive())
			{
				return;
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[SerializeField]
	private bool includeHidden;

	private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	private bool started;

	private bool initialized;
}
