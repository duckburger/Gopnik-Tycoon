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

    // MAIN STATES
    [Task]
    public bool isShopping = false;
    [Task]
    public bool isCausingTrouble = false;
    [Task]
    public bool isShoplifting = false;


    // Add character data
    [Header("Nav agent settings")]
    [SerializeField] float defaultNavStoppingDistance = 0.2f;
    [Space(10)]
    [SerializeField] CharacterIntentions myIntentions;
    [SerializeField] FoodQuality myPreferredFoodQuality;

    Wallet myWallet;
    MCharCarry myCarryController;
    MCharAttack myAttackController;
    DialogueBubbleDisplayer dialogueBubbleDisplayer;

    PolyNavAgent navAgent;

    Animator animator;
    CharacterStats myStats;

    QueueNumber myQueueTicket = null;

    bool hasPaidForGroceries = false;

    // COMBAT
    [SerializeField] GameObject combatTarget;
    Health combatTargetHealth;
    [SerializeField] int buildingsDestroyed = 0;
    int buildingsDamaged = 0;

    // SHOPLIFTING
    float amountStolen = 0f;

    // PATHFINDING
    [SerializeField] Vector2 target;
    Queue<Vector2> targetQueue = new Queue<Vector2>();
    Vector2 savedTarget;
    StoreShelf myTargetShelf;
    CashRegisterSlot myTargetCashRegisterSlot = null;
    CashRegister myCashRegister = null;
    NavMeshPortal myTargetNavPortal = null;

    [Task]
    public bool isLiningUp = false;
    [Task]
    public bool inCombat = false;
    [Task]
    public bool CanAttack => this.myAttackController.CanAttackAgain;
    [Task]
    public bool HasDestroyedEnoughBuildings => this.buildingsDestroyed >= this.myStats.BuildingsDestroyedWanted;
    [Task]
    public bool CurrentTargetAlive => this.combatTargetHealth.CurrHealthPercentage > 0;
    [Task]
    public bool HasShopliftedEnough => this.amountStolen >= this.myStats.AmountStolenWanted;

    public PolyNavAgent NavAgent
    {
        get => this.navAgent;
        set => this.navAgent = value;
    }
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
        this.myAttackController = this.GetComponent<MCharAttack>();
        this.myStats = this.GetComponent<CharacterStats>();
        this.dialogueBubbleDisplayer = this.GetComponent<DialogueBubbleDisplayer>();
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

    #region Starting Actions

    [Task]
    void ChooseIntentions()
    {
        // For now always choose shop
        this.myIntentions = CharacterIntentions.Trouble;
        this.isCausingTrouble = true;
        this.isShoplifting = false;
        this.isShopping = false;
        Task.current.Succeed();
    }

    #endregion

    #region Common Tasks

    [Task]
    void GoToTarget()
    {
        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("t = {0:0.00}", Time.time);
        }
        if (!Task.current.isStarting)
        {
            CheckIfReachedTarget();
        }        
        // If the given target is not on the current nav map..
        if (this.target != null && this.navAgent.map != null && !this.navAgent.map.PointIsValid(this.target) && this.savedTarget == Vector2.zero)
        {
            if (NavmeshPortalManager.Instance.FindNextNavPortal(this.target, this.navAgent.map) != null)
            {
                if (Task.current.isStarting)
                {
                    this.animator.Play("Walk");
                }                
                NavMeshPortal portal = NavmeshPortalManager.Instance.FindNextNavPortal(this.target, this.navAgent.map);
                this.savedTarget = this.target; // Heading to the portal first
                this.myTargetNavPortal = portal;
            }
        }
        else if (this.target != null && this.navAgent.map != null && this.navAgent.map.PointIsValid(this.target))
        {
            this.navAgent.SetDestination(this.target, null);
            if (CheckIfReachedTarget())
            {
                Task.current.Succeed();
                return;
            }
            if (Task.current.isStarting)
            {
                this.animator.Play("Walk");
            }
        }

        if (this.myTargetNavPortal != null)
        {
            this.target = this.myTargetNavPortal.transform.position;
            this.navAgent.SetDestination(this.target, null);
            return;
        }

        CheckIfReachedTarget();
    }

    private bool CheckIfReachedTarget()
    {
        if (navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
        {
            Task.current.Succeed();
            return true;
        }
        return false;
    }

    public void AdvanceToTarget()
    {
        this.target = this.savedTarget;
        this.savedTarget = Vector2.zero;
        this.myTargetNavPortal = null;
        this.navAgent.SetDestination(this.target, null);
    }

    [Task]
    void GoIdle()
    {
        if (Task.current.isStarting && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            this.animator.SetTrigger("Idle");
        }
        Task.current.Succeed();
    }

    [Task]
    void ChooseRandomPointNearAShelf()
    {
        this.target = BuildingTracker.Instance.GetRandomNearShelfLocation();
        this.navAgent.SetDestination(target, null);
        if (Task.current.isStarting)
        {
            this.animator.Play("Walk");
        }
        Task.current.Succeed();
    }

    #endregion

    #region Shopping Actions

    [Task]
    void ChooseFullShelfToShopAt()
    {
        // Choosing a product    to buy logic goes here
        StoreShelf shelfToShop = BuildingTracker.Instance.FindShelfByFoodQuality(this.myStats.PreferredFoodQuality);
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
        if (Task.current.isStarting)
        {
            this.animator.Play("Walk");
        }
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
        if (this.myTargetShelf != null && this.myTargetShelf.CheckIfContainsFoodQuality(this.myStats.PreferredFoodQuality))
        {
            this.myTargetShelf.TakeFoodOut(this.gameObject, this.myStats.PreferredFoodQuality);
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
                if (Task.current.isStarting)
                {
                    this.animator.Play("Walk");
                }
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

        //this.myTargetCashRegisterSlot.AddToQueue(this.gameObject);
        this.myQueueTicket = this.GetComponent<QueueNumber>();
        Vector2 positionInQueue = this.myTargetCashRegisterSlot.ProvideQueueSpot(this.gameObject);
        if (positionInQueue == Vector2.zero)
        {
            this.myTargetCashRegisterSlot.RemoveFromQueue(this.gameObject);
            Task.current.Fail();
            return;
        }

        this.navAgent.SetDestination(positionInQueue, (bool success) => 
        {
            if (success)
            {
                this.animator.SetTrigger("Idle");
                this.isLiningUp = true;
                Task.current?.Succeed();
            }
        });
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

    #region Casuing Trouble Actions

    [Task]
    public void ChooseBuildingToDamage()
    {
        if (BuildingTracker.Instance.GetRandomShelf() == null)
        {
            Task.current.Fail();
        }
        Building chosenBuilding = BuildingTracker.Instance.GetRandomShelf();
        this.target = chosenBuilding.transform.position + new Vector3(0f, -0.5f, 0f);
        this.combatTarget = chosenBuilding.gameObject;
        this.combatTargetHealth = this.combatTarget.GetComponent<BuildingHealth>();
        Task.current.Succeed();
    }



    #endregion

    [Task]
    void WalkOutOfStore()
    {
        Vector2 exitPos = LevelData.CurrentLevel.EntranceExitPoint.position;
        if (this.navAgent != null)
        {
            this.navAgent.SetDestination(exitPos, (bool success) =>
            {
                Task.current?.Succeed();
                Destroy(this.gameObject);                
            });
            if (Task.current != null && Task.current.isStarting)
            {
                this.animator.Play("Walk");
            }
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

    #region Combat

    public void ReactToAttack(GameObject attacker)
    {
        // TODO: Add decision making logic here to decided whether to flee, or fight back
        if (!this.inCombat || this.combatTarget != attacker)
        {
            this.inCombat = true;
            this.navAgent.Stop();
            this.animator.SetTrigger("Idle");
            this.dialogueBubbleDisplayer?.ShowDialogue("Hey what the fuck, dude!");
        }

        // TODO: Add an aggro system
       this.combatTarget = attacker;
       this.combatTargetHealth = this.combatTarget?.GetComponent<Health>();
    }

    [Task]
    void Attack()
    {
        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("t = {0:0.00}", Time.time);
        }
        if (Task.current.isStarting)
        {
            this.navAgent.Stop();
            this.animator.SetTrigger("Idle");
        }
        this.myAttackController?.AttackExternal(() => CountDestroyedBuildings());
    }


    void CountDestroyedBuildings()
    {
        if (HasEnemyDied())
        {
            this.buildingsDestroyed++;
        }
    }

    bool HasEnemyDied()
    {
        if (this.combatTargetHealth != null)
        {
            if (this.combatTargetHealth.GetCurrentHealth() <= 0)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    [Task]
    bool CombatTargetInRange()
    {
        if (this.combatTarget == null)
        {
            return false;
        }
        if (Vector2.Distance(this.transform.position, this.combatTarget.transform.position) <= this.myAttackController?.EffectiveDistance)
        {
            return true;
        }
        return false;
    }

    [Task]
    void ChaseCombatTarget()
    {
        if (this.combatTarget == null)
        {
            Task.current.Fail();
            return;
        }    
        if (Task.current.isStarting || Task.current.item != null && this.target != (Vector2)this.combatTarget.transform.position)
        {
            if (this.navAgent.map.PointIsValid(this.combatTarget.transform.position))
            {
                this.target = this.combatTarget.transform.position;
            }
            else
            {
                this.target = this.combatTarget.transform.position + new Vector3(0f, -0.5f, 0f);
            }
            
            this.navAgent?.SetDestination(this.combatTarget.transform.position, null);
            GoToTarget();
            Task.current.Succeed();
            return;
        }
    }

    #endregion

}


