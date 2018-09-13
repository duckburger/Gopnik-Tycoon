using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Gopnik/CashBalanceController")]
public class CashBalanceController : ScriptableObject
{
    [SerializeField] float currentBalance;
    public float CurrentBalance
    {
        get
        {
            return currentBalance;
        }
    }
    [SerializeField] float defBalance;

    public void AdjustCurrentBalance(float amount)
    {
        currentBalance += amount;
    }

    public void ResetDefBalance()
    {
        currentBalance = defBalance;
    }
	
}
