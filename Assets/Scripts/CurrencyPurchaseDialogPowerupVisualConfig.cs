using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPurchaseDialogPowerupVisualConfig : MonoBehaviour
{
	public void SetLabel(string text)
	{
		if(this.label != null)
			this.label.text = text;
	}

	[SerializeField]
	private TextMeshProUGUI label;

	[SerializeField]
	private Image image;
}
