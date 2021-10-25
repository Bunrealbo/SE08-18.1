using System;
using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		GGSoundSystem.Play(this.soundType);
	}

	[SerializeField]
	private GGSoundSystem.SFXType soundType;
}
