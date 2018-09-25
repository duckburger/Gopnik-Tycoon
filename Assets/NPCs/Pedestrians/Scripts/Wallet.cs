using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour {

    [SerializeField] float currentBalance;
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
        charStats = this.GetComponent<ICharStats>();
    }

    public void AdjustBalance(float amount)
    {
        currentBalance += amount;
    }
	
    public float Rob()
    {
        // TODO: Make this stat dependent so gopnik with higher intimidation will 
        if (currentBalance <= 3)
        {
            return 0;
        }
        float amtToSteal = Random.Range(3, currentBalance);
        Mathf.Round(amtToSteal);
        AdjustBalance(-amtToSteal);
        return amtToSteal;
    }
    
}
