using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsCurrency : MonoBehaviour
{
    private TextMeshProUGUI coinsCurrency;

    void Start()
    {
        coinsCurrency = gameObject.GetComponent<TextMeshProUGUI>();
    }

    
    void Update()
    {
        coinsCurrency.text = GGPlayerSettings.instance.walletManager.CurrencyCount(CurrencyType.coins).ToString();
    }
}
