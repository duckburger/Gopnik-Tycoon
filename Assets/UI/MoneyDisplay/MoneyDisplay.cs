using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour {

    [SerializeField] TextMeshProUGUI moneyText;

    public void UpdateMoneyUIText(float newValue)
    {
        moneyText.text = "$" + newValue.ToString("C0");
    }
	
}
