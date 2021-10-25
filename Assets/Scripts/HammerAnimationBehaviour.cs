using System;
using UnityEngine;

public class HammerAnimationBehaviour : MonoBehaviour
{
	public void Init(PowerupType powerupType)
	{
		GGUtil.SetActive(this.hammerIcon, powerupType == PowerupType.Hammer);
		GGUtil.SetActive(this.powerHammerIcon, powerupType == PowerupType.PowerHammer);
	}

	public float animationTime
	{
		get
		{
			return this.animationPlayer[this.animationName].time;
		}
	}

	public float animationNormalizedTime
	{
		get
		{
			if (!this.animationPlayer.isPlaying)
			{
				return 1f;
			}
			return this.animationPlayer[this.animationName].normalizedTime;
		}
	}

	public float timeWhenHammerHit
	{
		get
		{
			return (float)this.frameOfHit / this.animationPlayer[this.animationName].clip.frameRate;
		}
	}

	public void RemoveFromGame()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[SerializeField]
	private Animation animationPlayer;

	[SerializeField]
	private string animationName = "Take 001";

	[SerializeField]
	private int frameOfHit = 30;

	[SerializeField]
	private Transform hammerIcon;

	[SerializeField]
	private Transform powerHammerIcon;
}
