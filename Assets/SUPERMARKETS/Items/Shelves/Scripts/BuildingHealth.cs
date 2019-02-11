using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BuildingHealth : MonoBehaviour
{

    [SerializeField] HealthBar healthBar;
    [SerializeField] float health;
    public Action<float> onHealthUpdated;

    Animator myAnimator;

    private void Awake()
    {
        this.myAnimator = this.GetComponent<Animator>();
    }

    public void AssignStartingHealth(float amount)
    {
        this.health = amount;
        if (this.healthBar != null)
        {
            this.healthBar.Init(amount);
        }
    }

    public void GotHit(float damage)
    {
        AdjustBuildingHealth(-damage);
    }
 
    public void AdjustBuildingHealth(float amount)
    {
        if (this.health + amount <= 0)
        {
            this.health += amount;
            // Get destroyed or something
            TalkToHealthBar(() => 
            {
                this.GetComponentInParent<ModularBuildingSlot>().CurrentBuilding = null;
                Destroy(this.gameObject); // TODO: Add an animation to this + drop everything contained
            });
        }
        this.health += amount;
        if (this.onHealthUpdated != null)
        {
            this.onHealthUpdated(this.health);
        }

        this.myAnimator?.Play("HitShake");
        TalkToHealthBar();
    }

    private void TalkToHealthBar(Action callback = null)
    {
        if (this.healthBar != null)
        {
            this.healthBar.UpdateBar(this.health, callback);
        }
    }
}
