using System;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
	public void ButtonCallback_OnClick()
	{
		GGSoundSystem.Play(this.soundType);
	}

	[SerializeField]
	private GGSoundSystem.SFXType soundType;
}
