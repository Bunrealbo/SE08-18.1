using System;
using GGMatch3;
using UnityEngine;

public class CurrencyShowButton : MonoBehaviour
{
	public void OnClick()
	{
		NavigationManager.instance.GetObject<CurrencyPurchaseDialog>().Show(ScriptableObjectSingleton<OffersDB>.instance, null);
		GGSoundSystem.Play(GGSoundSystem.SFXType.ButtonPress);
	}
}
