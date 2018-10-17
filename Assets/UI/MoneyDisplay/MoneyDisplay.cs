using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour {

    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] ScriptableFloatVar moneyVar;

    private void Start()
    {
        this.moneyVar.Reset();
    }

    public void UpdateMoneyUIText(float newValue)
    {
        moneyText.text = newValue.ToString("C0");
    }
	
}
