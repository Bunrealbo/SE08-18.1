using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputFollower : MonoBehaviour
{
	public void SetActive(bool active)
	{
		for (int i = 0; i < this.particles.Count; i++)
		{
			ParticleSystem particleSystem = this.particles[i];
			if (!(particleSystem == null))
			{
				GGUtil.SetActive(particleSystem.transform, active);
			}
		}
		for (int j = 0; j < this.trails.Count; j++)
		{
			TrailRenderer trailRenderer = this.trails[j];
			if (!(trailRenderer == null))
			{
				GGUtil.SetActive(trailRenderer, active);
			}
		}
	}

	public void Clear()
	{
		for (int i = 0; i < this.particles.Count; i++)
		{
			ParticleSystem particleSystem = this.particles[i];
			if (!(particleSystem == null))
			{
				particleSystem.Clear();
			}
		}
		for (int j = 0; j < this.trails.Count; j++)
		{
			TrailRenderer trailRenderer = this.trails[j];
			if (!(trailRenderer == null))
			{
				trailRenderer.Clear();
			}
		}
	}

	[SerializeField]
	private List<ParticleSystem> particles = new List<ParticleSystem>();

	[SerializeField]
	private List<TrailRenderer> trails = new List<TrailRenderer>();
}
