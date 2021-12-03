using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsCurrencyScript : MonoBehaviour
{
    private TextMeshProUGUI textCoinsCurrency;

    void Start()
    {
        textCoinsCurrency = gameObject.GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        textCoinsCurrency.text = GGPlayerSettings.instance.walletManager.CurrencyCount(CurrencyType.diamonds).ToString();
    }
}
