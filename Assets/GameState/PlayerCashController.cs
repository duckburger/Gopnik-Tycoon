using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCashController : MonoBehaviour {

    public static PlayerCashController Instance;
    [SerializeField] CashBalanceController cashDataFile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void AdjustBalance(float amount)
    {
        if (cashDataFile != null)
        {
            cashDataFile.AdjustCurrentBalance(amount);
        }
    }

}
