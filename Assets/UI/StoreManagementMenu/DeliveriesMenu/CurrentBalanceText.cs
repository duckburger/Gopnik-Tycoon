using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentBalanceText : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI balanceText;

    private void Start()
    {
        UpdateCurrentBalanceText(MoneyController.Instance.MainBalance.value);
    }

    public void UpdateCurrentBalanceText(float newValue)
    {
        if (this.balanceText != null)
        {
            balanceText.text = "Current Balance:\n$" + newValue.ToString("C0");
        } 
    }
    
}
