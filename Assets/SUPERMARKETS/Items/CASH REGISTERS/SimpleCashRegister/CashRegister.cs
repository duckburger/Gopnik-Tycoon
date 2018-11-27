using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : Building
{

    [SerializeField] ScriptableFloatVar globalBalance;

    private void Start()
    {
        BuildingTracker.Instance.AddCashRegisterToTracker(this);
    }

    // TODO: Make this process non automatic so a cashier is required to process payment (or a self serve register?)
    public void AcceptPayment(float amount)
    {
        if (this.globalBalance != null)
        {
            this.globalBalance.AdjustFloatValue(amount);
        }
    }
}
