using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] float currentStamina;
    [SerializeField] float maxStamina = 100;

    public int CurrStaminaPercentage
    {
        get
        {
            return (int)(currentStamina / maxStamina);
        }
    }

    // Use this for initialization
    void Start()
    {
        this.currentStamina = this.maxStamina;
    }

    public float GetCurrentHealth()
    {
        return currentStamina;
    }

    public void AdjustStamina(float amount)
    {
        if (Mathf.Abs(amount) > currentStamina)
        {
            currentStamina = 0;
            return;
        }
        currentStamina += amount;
    }

   
}
