using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 100;

    public int CurrHealthPercentage
    {
        get
        {
            return (int)(currentHealth / maxHealth);
        }
    }

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

	public void AdjustHealth(float amount)
    {
        if (amount > currentHealth)
        {
            Die();
            return;
        }
        currentHealth -= amount;
    }

    void Die()
    {
        // TODO: Make a death script
    }
	
}
