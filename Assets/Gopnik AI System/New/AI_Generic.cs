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
    [Header("Nav agent settings")]
    [SerializeField] float defaultNavStoppingDistance = 0.2f;
    [Space(10)]
    [SerializeField] CharacterIntentions myIntentions;
    [SerializeField] FoodQuality myPreferredFoodQuality;

    Wallet myWallet;
    MCharCarry myCarryController;
    PolyNavAgent navAgent;
    Animator animator;
    CharacterStats myStats;

    QueueNumber myQueueTicket = null;
    bool isLiningUp = false;
    // Targets
    [SerializeField] Vector2 target;
    StoreShelf myTargetShelf;
    CashRegisterSlot myTargetCashRegisterSlot;

    

    private void Start()
    {
        this.navAgent = this.GetComponent<PolyNavAgent>();
        this.animator = this.GetComponent<Animator>();
        this.myWallet = this.GetComponent<Wallet>();
        this.myCarryController = this.GetComponent<MCharCarry>();
        this.myStats = this.GetComponent<CharacterStats>();
    }


    private void Update()
    {
        //Setting directions for the animator
        if (this.navAgent.hasPath)
        {
            this.animator.SetFloat("xInput", this.navAgent.movingDirection.x);
            this.animator.SetFloat("yInput", this.navAgent.movingDirection.y);
        }
       
    }

    #region Common Tasks


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
    void ChooseRandomPointNearAShelf()
    {
        this.target = BuildingTracker.Instance.GetRandomNearShelfLocation();
        this.navAgent.SetDestination(target, null);
        this.animator.Play("Walk");
        Task.current.Succeed();
    }

    #endregion

    #region Starting Actions

    [Task]
   void ChooseIntentions()
    {
        // For now always choose shop
        this.myIntentions = CharacterIntentions.Shop;
        Task.current.Succeed();
    }

    #endregion

    #region Shopping Actions

    [Task]
    void ChooseFullShelfToShopAt()
    {
        // Choosing a product    to buy logic goes here
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
    bool HasPickedUpEnoughItems()
    {
        if (this.myStats != null && this.myCarryController != null)
        {
            int neededItems = this.myStats.ItemsWanted;
            int currentItems = this.myCarryController.GetCountOfCarriedGoods();
            if (neededItems > currentItems)
            {
                return false;
            }
            else if (neededItems == currentItems || neededItems < currentItems)
            {
                return true;
            }
        }
        return false;
    }

    [Task]
    void PickUpItemFromShelf()
    {
        if (this.myTargetShelf != null && this.myTargetShelf.CheckIfContainsFoodQuality(myPreferredFoodQuality))
        {
            this.myTargetShelf.TakeFoodOut(this.gameObject, myPreferredFoodQuality);
            if (this.myStats != null && this.myCarryController != null)
            {
                int neededItems = this.myStats.ItemsWanted;
                int currentItems = this.myCarryController.GetCountOfCarriedGoods();
                if (neededItems > currentItems)
                {
                    Task.current.Fail();
                    return;
                }
                else if (neededItems == currentItems || neededItems < currentItems)
                {
                    Task.current.Succeed();
                } 
            }
            
        }
    }

    [Task]
    void FindAndGoToCashRegister()
    {
        if (BuildingTracker.Instance != null)
        {

            CashRegisterSlot foundCashRegisterSlot = BuildingTracker.Instance.GetCashRegisterWithShortestLine();
            if (foundCashRegisterSlot != null)
            {
                // Get the target to go to
                Vector2 cashRegisterLocation = foundCashRegisterSlot.ProvideGeneralBuildingLocation();
                if (cashRegisterLocation == Vector2.zero)
                {
                    Task.current.Fail();
                    Debug.Log("Cash register slot gave " + this.gameObject.name + " no general location for some reason. Abandoning task.");
                    return;
                }

                this.myTargetCashRegisterSlot = foundCashRegisterSlot;
                this.navAgent.SetDestination(cashRegisterLocation, null);
                this.animator.Play("Walk");
            }

            if (this.navAgent.remainingDistance <= 3)
            {
                Task.current.Succeed();
                this.animator.Play("Idle");
            }
        }
    }



    [Task]
    void GoToQueuePosition()
    {
        Debug.Log("<color=green>Attempting to line up! </color>" + this.gameObject.name);
        this.animator.Play("Walk");
        if (this.myTargetCashRegisterSlot == null)
        {
            Debug.LogError("Couldn't find attached cash register on " + gameObject.name);
            Task.current.Fail();
            return;
        }

        if (!this.myTargetCashRegisterSlot.AddToQueue(this.gameObject))
        {
            Debug.LogError("Couldn't add " + gameObject.name + " to the cash register queue");
            Task.current.Fail();
            return;
        }

        this.myTargetCashRegisterSlot.AddToQueue(this.gameObject);
        this.myQueueTicket = this.GetComponent<QueueNumber>();
        Vector2 positionInQueue = this.myTargetCashRegisterSlot.ProvideQueueSpot(this.gameObject);
        if (positionInQueue == Vector2.zero)
        {
            this.myTargetCashRegisterSlot.RemoveFromQueue(this.gameObject);
            Task.current.Fail();
            return;
        }

        this.navAgent.SetDestination(positionInQueue, HasReachedQueuePosition);
        Task.current.Succeed();
    }

    private void HasReachedQueuePosition(bool success)
    {
        if (success)
        {
            this.animator.Play("Idle");
            this.isLiningUp = true;
        }
    }

    [Task]
    void TurnToCashRegister()
    {
        Vector3 dirToCash = this.transform.InverseTransformVector(this.myTargetCashRegisterSlot.transform.position);
        if (dirToCash.x > 0)
        {
            // Turn right
            this.animator.SetFloat("xInput", 1);
        }
        else
        {
            // Turn left
            this.animator.SetFloat("xInput", -1);
        }

        Task.current.Succeed();
    }

    [Task]
    bool HasLineAdvanced()
    {
        if (this.myQueueTicket.CurrentNumberInQueue != this.myQueueTicket.LastNumberInQueue)
        {
            this.myQueueTicket.LastNumberInQueue = this.myQueueTicket.CurrentNumberInQueue;
            return true;
        }
        // Equalize the numbers
        return false;
    }

    [Task]
    bool IsLiningUp()
    {
        return this.isLiningUp;
    }

    [Task]
    bool PayAtCash()
    {
        float costOfAllMyFood = this.myCarryController.GetCostOfCarriedGoods();
        if (this.myWallet.AdjustBalance(-costOfAllMyFood))
        {
            this.myTargetCashRegisterSlot.AcceptPayment(costOfAllMyFood);
            FloatingTextDisplay.SpawnFloatingText(this.transform.position, "+$" + costOfAllMyFood);
            Destroy(this.myQueueTicket);
            this.myTargetCashRegisterSlot.RemoveFromQueue(this.gameObject);
            return true;
        }
        return false;
    }

    #endregion

    [Task]
    void WalkOutOfStore()
    {
        Vector2 exitPos = LevelData.CurrentLevel.EntranceExitPoint.position;
        if (this.navAgent != null)
        {
            this.navAgent.SetDestination(exitPos, (bool success) => Destroy(this.gameObject));
            Task.current.Succeed();
            return;
        }
    }

}


