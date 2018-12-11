using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : Building
{

    [SerializeField] ScriptableFloatVar globalBalance;
    [SerializeField] ScriptableEvent onAdvancedInLine;
    AI_Generic nextPayingCustomer;

    private void Start()
    {
        BuildingTracker.Instance.AddCashRegisterToTracker(this);
    }

    public void RegisterAsNextPayingCustomer(AI_Generic customerController)
    {
        this.nextPayingCustomer = customerController;
    }

    // TODO: Make this process non automatic so a cashier is required to process payment (or a self serve register?)
    public void AcceptPayment()
    {
       if (this.nextPayingCustomer == null)
        {
            return;
        }

        this.nextPayingCustomer.PayAtCash();
        this.nextPayingCustomer = null;
        if (this.onAdvancedInLine != null)
        {
            this.onAdvancedInLine.RaiseWithData(this);
        }
    }
}
