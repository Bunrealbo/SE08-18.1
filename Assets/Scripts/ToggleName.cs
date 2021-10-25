using System;
using UnityEngine;

public class ToggleName : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			this.nameEffectText[i].SetActive(true);
		}
	}

	public void ToggleChanged(bool isHide)
	{
		for (int i = 0; i < 4; i++)
		{
			this.nameEffectText[i].SetActive(!isHide);
		}
	}

	public GameObject[] nameEffectText;
}
