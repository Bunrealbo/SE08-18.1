using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarsCurrencyScript : MonoBehaviour
{

    private TextMeshProUGUI textStarsCurrency;

    void Start()
    {
        textStarsCurrency = gameObject.GetComponent<TextMeshProUGUI>();   
    }

  
    void Update()
    {
        textStarsCurrency.text = GGPlayerSettings.instance.walletManager.CurrencyCount(CurrencyType.diamonds).ToString();
    }
}
