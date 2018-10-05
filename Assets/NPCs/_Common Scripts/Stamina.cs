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
            return (int)(currentStamina / maxStamina) * 100;
        }
    }

    // Use this for initialization
    void Start()
    {
        this.currentStamina = this.maxStamina;
        StartCoroutine(Regen());
    }

    IEnumerator Regen()
    {
        while(true)
        {
            if (this.currentStamina < 30)
            {
                this.currentStamina += 5;
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(1f);
        }

    }

    public float GetCurrentStamina()
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
