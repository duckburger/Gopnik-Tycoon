using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour {

    [SerializeField] float currentBalance;

    ICharStats charStats;

    private void Start()
    {
        charStats = this.GetComponent<ICharStats>();
    }

    public void AdjustBalance(float amount)
    {
        currentBalance -= currentBalance;
    }
	
    public float Rob()
    {
        // TODO: Make this stat dependent
        float amtToSteal = Random.Range(1, currentBalance);
        AdjustBalance(-amtToSteal);
        return amtToSteal;
    }
    
}
