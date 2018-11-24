using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : Building
{



    private void Start()
    {
        BuildingTracker.Instance.AddCashRegisterToTracker(this);
    }


    public void AcceptPayment(float amount)
    {

    }
}
