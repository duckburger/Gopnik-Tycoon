using System.Collections;
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
    [SerializeField] float defaultNavStoppingDistance = 0.1f;
    [Space(10)]
    [SerializeField] CharacterIntentions myIntentions;
    [SerializeField] FoodQuality myPreferredFoodQuality;

    PolyNavAgent navAgent;
    Animator animator;

    QueueNumber myQueueNumber = null;

    // Targets
    [SerializeField] Vector2 target;
    StoreShelf myTargetShelf;
    DeQueue myTargetCashRegisterSlot;

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


    #endregion

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
    void PickUpItemFromShelf()
    {
        if (this.myTargetShelf != null && this.myTargetShelf.CheckIfContainsFoodQuality(myPreferredFoodQuality))
        {
            this.myTargetShelf.TakeFoodOut(this.gameObject, myPreferredFoodQuality);
            Task.current.Succeed();
        }
    }

    [Task]
    void FindCashRegister()
    {
        if (BuildingTracker.Instance != null)
        {

            DeQueue foundCashRegisterSlot = BuildingTracker.Instance.GetCashRegisterWithShortestLine();
            if (foundCashRegisterSlot != null)
            {
                // Get the target to go to
                Vector2 registerLocation = foundCashRegisterSlot.ProvideGeneralBuildingLocation();
                if (registerLocation == Vector2.zero)
                {
                    Task.current.Fail();
                    Debug.Log("Cash register slot gave " + this.gameObject.name + " no general location for some reason. Abandoning task.");
                    return;
                }

                this.myTargetCashRegisterSlot = foundCashRegisterSlot;
                this.navAgent.stoppingDistance = 1f;
                this.navAgent.SetDestination(registerLocation, (bool success) => ResetNavAgent());
                this.animator.Play("Walk");
                Task.current.Succeed();
                return;
            }
        }
    }

    void ResetNavAgent()
    {
        this.navAgent.stoppingDistance = this.defaultNavStoppingDistance;
    }

    [Task]
    void LineUp()
    {
        if (this.myTargetCashRegisterSlot == null)
        {
            Task.current.Fail();
            return;
        }

        if (!this.myTargetCashRegisterSlot.AddToQueue(this.gameObject))
        {
            Task.current.Fail();
            return;
        }
        this.myQueueNumber = this.GetComponent<QueueNumber>();
        Vector2 positionInQueue = this.myTargetCashRegisterSlot.ProvideQueueSpot();
        if (positionInQueue == Vector2.zero)
        {
            Task.current.Fail();
            return;
        }

        this.navAgent.SetDestination(positionInQueue, null);

        Task.current.Succeed();
    }

    [Task]
    void WaitInLine()
    {
        if (this.myQueueNumber == null)
        {
            Debug.LogError("Couldn't find queue number on the queing NPC " + gameObject.name);
            Task.current.Fail();
            return;
        }

        if (this.myQueueNumber.CurrentNumberInQueue == 0)
        {
            // Pay and leave
        }

        if (this.myQueueNumber.CurrentNumberInQueue != this.myQueueNumber.LastNumberInQueue)
        {
            // Advance to the new position!

            
            // Equalize the numbers
        }



        this.myQueueNumber.LastNumberInQueue = this.myQueueNumber.CurrentNumberInQueue;
    }
}


