using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPurchaseDialogEconomyVisualConfig : MonoBehaviour
{
	public void SetLabel(string text)
	{
		for (int i = 0; i < this.labels.Count; i++)
		{
			if(this.labels[i] != null)
				this.labels[i].text = text;
		}
	}

	[SerializeField]
	private Image icon;

	[SerializeField]
	private List<TextMeshProUGUI> labels;
}
