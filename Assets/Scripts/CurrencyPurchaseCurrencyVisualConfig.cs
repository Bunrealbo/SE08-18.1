using System;
using TMPro;
using UnityEngine;

public class CurrencyPurchaseCurrencyVisualConfig : MonoBehaviour
{
	public void SetLabel(string text)
	{
		this.label.text = text;
	}

	[SerializeField]
	private TextMeshProUGUI label;
}
