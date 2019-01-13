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
    [SerializeField] float defaultNavStoppingDistance = 0.2f;
    [Space(10)]
    [SerializeField] CharacterIntentions myIntentions;
    [SerializeField] FoodQuality myPreferredFoodQuality;

    Wallet myWallet;
    MCharCarry myCarryController;
    PolyNavAgent navAgent;
    public PolyNavAgent NavAgent 
    {
         get
         {
             return this.navAgent;
         }
          
          set
          {
              this.navAgent = value;
          }
    }
    Animator animator;
    CharacterStats myStats;

    QueueNumber myQueueTicket = null;
    bool isLiningUp = false;
    bool hasPaidForGroceries = false;
    // Targets
    Queue<Vector2> targetQueue = new Queue<Vector2>();
    [SerializeField] Vector2 target;
    public Vector2 Target
    {
        get
        {
            return this.target;
        }
        set
        {
            this.target = value;
        }
    }
    Vector2 savedTarget; // Used for continuous pathing
    StoreShelf myTargetShelf;
    CashRegisterSlot myTargetCashRegisterSlot = null;
    CashRegister myCashRegister = null;
    NavMeshPortal myTargetNavPortal = null;
    public NavMeshPortal MyTargetNavPortal
    {
        get
        {
            return this.myTargetNavPortal;
        }
    }

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

        // If the given target is not on the current nav map..
        if (this.target != null && this.navAgent.map != null && !this.navAgent.map.PointIsValid(this.target) && this.savedTarget == Vector2.zero)
        {
            if (NavmeshPortalManager.Instance.FindNextNavPortal(this.target, this.navAgent.map) != null)
            {
                NavMeshPortal portal = NavmeshPortalManager.Instance.FindNextNavPortal(this.target, this.navAgent.map);
                this.savedTarget = this.target; // Heading to the portal first
                this.myTargetNavPortal = portal;
            }
        }

        if (this.myTargetNavPortal != null)
        {
            this.target = this.myTargetNavPortal.transform.position;
            this.navAgent.SetDestination(this.target, null);
            return;
        }

        if (navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
        {
            this.animator.Play("Idle");
            Task.current.Succeed();
        }
    }

    public void AdvanceToTargetAfterReachingPortal()
    {
        // TODO: Check whether the saved target is on the newly assigned navmap
        this.target = this.savedTarget;
        this.savedTarget = Vector2.zero;
        this.myTargetNavPortal = null;
        this.navAgent.SetDestination(this.target, null);
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
    bool CheckForExistenceOfCashRegisters()
    {
        return BuildingTracker.Instance.CheckIfThereAreCashRegisters();
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
                Vector2 cashRegisterLocation = foundCashRegisterSlot.ProvideLastPersonInQueueLocation();
                if (cashRegisterLocation == Vector2.zero)
                {
                    Task.current.Fail();
                    Debug.Log("Cash register slot gave " + this.gameObject.name + " no general location for some reason. Abandoning task.");
                    return;
                }

                this.myTargetCashRegisterSlot = foundCashRegisterSlot;
                this.myCashRegister = this.myTargetCashRegisterSlot.GetCurrentCashRegister();
                this.navAgent.SetDestination(cashRegisterLocation, null);
                this.animator.Play("Walk");
            }

            if (this.navAgent.remainingDistance <= 3)
            {
                Task.current.Succeed();
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
    void TurnDownwards()
    {
        this.animator.SetFloat("yInput", -1);
        this.animator.SetFloat("xInput", 0);
        Task.current.Succeed();
    }

    /// <summary>
    /// Makes the customer let the cash register know that they will be the next one paying
    /// </summary>
    [Task]
    void RegisterForPayment()
    {
        if (this.myTargetCashRegisterSlot == null)
        {
            Task.current.Fail();
            return;
        }

        CashRegister currentRegisterInSlot = this.myTargetCashRegisterSlot.GetCurrentCashRegister();
        if (currentRegisterInSlot == null)
        {
            return;
        }

        currentRegisterInSlot.RegisterAsNextPayingCustomer(this);
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
        return false;
    }

    [Task]
    bool HasPaidForGroceries()
    {
        return this.hasPaidForGroceries;
    }

    [Task]
    bool FirstInLine()
    {
        {
            if (this.myQueueTicket != null && this.myQueueTicket.CurrentNumberInQueue == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    [Task]
    bool IsLiningUp()
    {
        return this.isLiningUp;
    }

    [Task]
    public bool PayAtCash()
    {
        float costOfAllMyFood = this.myCarryController.GetCostOfCarriedGoods();
        if (this.myWallet.AdjustBalance(-costOfAllMyFood))
        {
            if (MoneyController.Instance != null)
            {
                MoneyController.AdjustMainBalance(costOfAllMyFood);
            }
            FloatingTextDisplay.SpawnFloatingText(this.transform.position, "+$" + costOfAllMyFood);
            Destroy(this.myQueueTicket);
            this.myTargetCashRegisterSlot.RemoveFromQueue(this.gameObject);
            this.myTargetCashRegisterSlot = null;
            this.myCashRegister = null;
            this.hasPaidForGroceries = true;
            this.isLiningUp = false;
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
            this.animator.Play("Walk");
            Task.current.Succeed();
            return;
        }
    }


    #region Accepting Events

    public void AdvanceInLine(object obj)
    {
        if (this.myQueueTicket == null || this.myCashRegister == null)
        {
            return;
        }

        CashRegister registerToCompare = obj as CashRegister;
        if (registerToCompare != null && registerToCompare == this.myCashRegister)
        {
            // Advance in line
            if (this.myQueueTicket.CurrentNumberInQueue > 0)
            {
                this.myQueueTicket.CurrentNumberInQueue--;
            }
            Vector2 myNewSpot = this.myTargetCashRegisterSlot.ProvideQueueSpot(this.gameObject);
            this.navAgent.SetDestination(myNewSpot, null);
            this.animator.Play("Walk");
        }
        
    }

    #endregion
}


