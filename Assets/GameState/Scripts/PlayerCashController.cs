using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCashController : MonoBehaviour {

    public static PlayerCashController Instance;
    [SerializeField] ScriptableFloatVar cashDataFile;

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

        this.cashDataFile.Reset();
    }

    public void AdjustBalance(float amount)
    {
        if (this.cashDataFile != null)
        {
            this.cashDataFile.AdjustFloatValue(amount);
        }
    }

}
