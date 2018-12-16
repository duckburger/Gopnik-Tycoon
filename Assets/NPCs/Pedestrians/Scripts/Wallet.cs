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

    public float CurrentBalance
    {
        get
        {
            return currentBalance;
        }
        
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

    public bool AdjustBalance(float amount)
    {
        if ((this.currentBalance += amount) < 0)
        {
            Debug.LogError("Cannot take this much, there isn't enough in this wallet");
            return false;
        }
        this.currentBalance += amount;
        this.balanceChagedEvent.Invoke(amount);
        return true;
    }
	
    public float Rob()
    {
        // TODO: Make this stat dependent so gopnik with higher intimidation will rob people for more
        int amtToSteal = (int)UnityEngine.Random.Range(3, currentBalance - 2);
        if (amtToSteal > this.currentBalance)
        {
            Debug.Log("Cannot steal more than the character has in the wallet!");
            return 0;
        }
        Mathf.RoundToInt(amtToSteal);
        Debug.Log("Successful robbery for " + amtToSteal);
        AdjustBalance(-amtToSteal);
        return amtToSteal;
    }
    
}
