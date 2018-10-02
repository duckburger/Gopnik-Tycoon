using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class OnCharAttacked : UnityEvent<GameObject> { }


public class Health : MonoBehaviour {

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 100;
    [Space(10)]
    [Header("Death related")]
    [SerializeField] GameObject deadBody;
    [SerializeField] AudioSource myAudioSource;
    [SerializeField] List<AudioClip> deathSounds = new List<AudioClip>();

    [Header("Events")]
    public OnCharAttacked onAttacked;


    public float CurrHealthPercentage
    {
        get
        {
            return (currentHealth / maxHealth) * 100;
        }
    }

    // Use this for initialization
    void Start()
    {
        this.currentHealth = this.maxHealth;
        this.myAudioSource = this.GetComponent<AudioSource>();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

	public void AdjustHealth(float amount, bool aggression, GameObject attacker = null)
    {
        if (amount < 0)
        {
            Debug.Log(this.gameObject.name + " is hurt for " + Mathf.Abs(amount) + " health!");
        }
        if (Mathf.Abs(amount) > currentHealth)
        {
            Die();
            return;
        }
        currentHealth += amount;
        if (aggression && attacker != null)
        {
            this.onAttacked.Invoke(attacker);
        }
    }

    void Die()
    {
        // Play a sound then spawnt the dead body
        int soundIndex = UnityEngine.Random.Range(0, deathSounds.Count);
        AudioSource bodyAudiosource = Instantiate(this.deadBody, this.transform.position, Quaternion.identity, this.transform.parent).GetComponent<AudioSource>();
        bodyAudiosource.clip = this.deathSounds[soundIndex];
        bodyAudiosource.Play();
        DeseletMyself();
        Destroy(this.gameObject);
    }

    void DeseletMyself()
    {
        SelectableObject me = this.GetComponent<SelectableObject>();
        if (SelectionController.Instance.SelectedObj == me)
        {
            SelectionController.Instance.SelectedObj = null;
        }
    }


	
}
