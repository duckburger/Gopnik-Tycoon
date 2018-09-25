using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class WalletBalanceChanged_Event : UnityEvent<float>
{

}

public class Wallet : MonoBehaviour {

    [SerializeField] float currentBalance;

    public WalletBalanceChanged_Event balanceChagedEvent;

    public float CurrentBalance()
    {
        return currentBalance;
    }

    ICharStats charStats;
    bool hasBeenMugged;
    public bool HasBeenMugged
    {
        get
        {
            return hasBeenMugged;
        }
        set
        {
            if (value.GetType() == typeof(bool))
            {
                hasBeenMugged = value;
            }
        }
    }


    private void Start()
    {
        this.charStats = this.GetComponent<ICharStats>();
    }

    public void AdjustBalance(float amount)
    {
        if ((this.currentBalance += amount) < 0)
        {
            Debug.LogError("Cannot take this much, there isn't enough in this wallet");
            return;
        }
        this.currentBalance += amount;
        this.balanceChagedEvent.Invoke(this.currentBalance);
    }
	
    public float Rob()
    {
        // TODO: Make this stat dependent so gopnik with higher intimidation will 
        if (currentBalance <= 3)
        {
            return 0;
        }
        float amtToSteal = UnityEngine.Random.Range(3, currentBalance);
        Mathf.Round(amtToSteal);
        AdjustBalance(-amtToSteal);
        return amtToSteal;
    }
    
}
