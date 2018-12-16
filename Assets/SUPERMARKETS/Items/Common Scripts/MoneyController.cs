using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    public static MoneyController Instance;

    [SerializeField] ScriptableFloatVar mainBalance;
    public ScriptableFloatVar MainBalance
    {
        get
        {
            return this.mainBalance;
        }
    }

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

    public static void AdjustMainBalance(float amount)
    {
        if (MoneyController.Instance.MainBalance != null)
        {
            MoneyController.Instance.MainBalance.AdjustFloatValue(amount);
        }
    }
}
