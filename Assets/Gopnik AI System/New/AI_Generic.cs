﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using System;
using PolyNav;

[Serializable]
public enum CharacterIntentions
{
    Shop,
    Trouble,
    Shoplifting
}

public class AI_Generic : MonoBehaviour
{
    // Add character data
    [SerializeField] Vector2 target;
    StoreShelf myTargetShelf;
    [SerializeField] CharacterIntentions myIntentions;
    [SerializeField] FoodQuality myPreferredFoodQuality;

    PolyNavAgent navAgent;
    Animator animator;
    

    private void Start()
    {
        this.navAgent = this.GetComponent<PolyNavAgent>();
        this.animator = this.GetComponent<Animator>();
    }


    private void Update()
    {
        if (this.navAgent.hasPath)
        {
            this.animator.SetFloat("xInput", this.navAgent.movingDirection.x);
            this.animator.SetFloat("yInput", this.navAgent.movingDirection.y);
        }
       
    }

    [Task]
   void ChooseIntentions()
    {
        // For now always choose shop
        this.myIntentions = CharacterIntentions.Shop;
        Task.current.Succeed();
    }

    [Task]
    void ChooseRandomPointNearAShelf()
    {
        this.target = BuildingTracker.Instance.GetRandomNearShelfLocation();
        this.navAgent.SetDestination(target, null);
        this.animator.Play("Walk");
        Task.current.Succeed();
    }

    [Task]
    void ChooseFullShelfToShopAt()
    {
        // Choosing a product to buy logic goes here
        StoreShelf shelfToShop = BuildingTracker.Instance.FindShelfByFoodQuality(myPreferredFoodQuality);
        if (shelfToShop != null)
        {
            this.myTargetShelf = shelfToShop;
        }
        else
        {
            Task.current.Fail();
            return;
        }
        this.target = new Vector2(shelfToShop.gameObject.transform.position.x, shelfToShop.gameObject.transform.position.y - 1.5f);
        this.navAgent.SetDestination(target, null);
        this.animator.Play("Walk");
        Task.current.Succeed();
        return;
    }


    [Task]
    void GoToTarget()
    {
        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("t = {0:0.00}", Time.time);
        }

        if (navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
        {
            this.animator.Play("Idle");
            Task.current.Succeed();
        }
    }

    [Task]
    void GoIdle()
    {
        this.animator.Play("Idle");
        Task.current.Succeed();
    }

    [Task]
    void PickUpItemFromShelf()
    {
        if (this.myTargetShelf != null && this.myTargetShelf.CheckIfContainsFoodQuality(myPreferredFoodQuality))
        {
            this.myTargetShelf.TakeFoodOut(this.gameObject, myPreferredFoodQuality);
            Task.current.Succeed();
        }
    }
}
