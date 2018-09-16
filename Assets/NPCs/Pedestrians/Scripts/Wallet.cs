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
        currentBalance += amount;
    }
	
    public float Rob()
    {
        // TODO: Make this stat dependent
        if (currentBalance <= 0)
        {
            return 0;
        }
        float amtToSteal = Random.Range(1, currentBalance);
        Mathf.Round(amtToSteal);
        AdjustBalance(-amtToSteal);
        return amtToSteal;
    }
    
}
