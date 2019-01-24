using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCashController : MonoBehaviour {

    public static PlayerCashController Instance;
    [SerializeField] ScriptableFloatVar globalBalance;

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

        this.globalBalance.Reset();
    }

    public void AdjustBalance(float amount)
    {
        if (this.globalBalance != null)
        {
            this.globalBalance.AdjustFloatValue(amount);
        }
    }

}
