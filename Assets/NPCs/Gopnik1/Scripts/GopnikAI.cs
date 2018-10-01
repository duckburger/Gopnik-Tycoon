using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PolyNav;
using UnityEngine.Events;
using System;



public class GopnikAI : MonoBehaviour, IGoap/*, ICharStats*/ {

    [SerializeField] string charName;
    public string GetCharName()
    {
        return charName;
    }

    [Header("Targets")]
    [SerializeField] Transform chatTarget;
    public GameObject ChatTarget
    {
        get
        {
            if (chatTarget == null)
            {
                return null;
            }
            return this.chatTarget.gameObject;
        }
        set
        {
            if (value == null)
            {
                this.chatTarget = null;
                return;
            }
            this.chatTarget = value.transform;
        }
    }

    [SerializeField] Transform fightTarget;
    public GameObject FightTarget
    {
        get
        {
            if (this.fightTarget == null)
            {
                return null;
            }
            return this.fightTarget.gameObject;
        }
        set
        {   
            if (value == null)
            {
                this.fightTarget = null;
                return;
            }
            this.fightTarget = value.transform;
        }
    }

    [SerializeField] Transform razvodTarget;
    public GameObject RazvodTarget
    {
        get
        {
            if (this.razvodTarget == null)
            {
                return null;
            }
            return this.razvodTarget.gameObject;
        }
        set
        {
            if (value == null)
            {
                return;
            }
            this.razvodTarget = value.transform;
        }
    }

    [Space(10)]
    [Header("Assigned Nest")]
    [SerializeField] GameObject myNest;
    public GopnikNest MyNest
    {
        get
        {
            return myNest.GetComponent<GopnikNest>();
        }
    }

    [Header("Stats")]
    [SerializeField] float stat_strength;
    public float GetStat_Strength()
    {
        return stat_strength;
    }
    [SerializeField] float stat_charisma;
    public float GetStat_Charisma()
    {
        return stat_charisma;
    }
    [SerializeField] float stat_cunning;
    public float GetStat_Cunning()
    {
        return stat_cunning;
    }
    public float GetWalletBalance()
    {
        return this.GetComponent<Wallet>().CurrentBalance;
    }
    [SerializeField] Sprite myPortrait;
    public Sprite GetPortrait()
    {
        return myPortrait;
    }
    public bool IsBusy
    {
        get
        {
            return isBusy;
        } 
    }
    bool isBusy;

    [Space(10)]
    [Header("Float Vars")]
    public ScriptableFloatVar globalBalance;
    [Space(10)]
    PolyNavAgent navAgent;
    Animator myAnimator;
    HuntTargetSensor targetSensor;
    Vector2 previousDestination;
    Health health;
    Vector2 lastDir;
    


    // Use this for initialization
    void Start()
    {
        navAgent = this.GetComponent<PolyNavAgent>();
        health = this.GetComponent<Health>();
        targetSensor = this.GetComponent<HuntTargetSensor>();
        myAnimator = this.GetComponent<Animator>();
    }

    #region OnSpawn Methods

    public void AssingNest(GameObject newNest)
    {
        myNest = newNest;
    }

    #endregion

    #region External Access

    public void AssignTarget(GameObject target, ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Force:
                this.FightTarget = target;
                this.ChatTarget = null;
                this.RazvodTarget = null;
                break;
            case ActionType.Chat:
                this.ChatTarget = target;
                this.FightTarget = null;
                this.RazvodTarget = null;
                break;
            case ActionType.Razvod:
                this.RazvodTarget = target;
                this.FightTarget = null;
                this.ChatTarget = null;
                break;
            default:
                break;
        }
        Debug.Log("Changed gopnik's goal by assigning new target");
        this.GetComponent<GoapAgent>().PushIdleState();
    }

    public ActionType GetCurrentAction()
    {
        if (fightTarget != null)
        {
            return ActionType.Force;
        }
        else if (chatTarget != null)
        {
            return ActionType.Chat;
        }
        else if (razvodTarget != null)
        {
            return ActionType.Razvod;
        }
        return ActionType.Idling;
    }

    #endregion

    #region GOAP Methods
    // Is run constantly to determine the state of the world in the agent's understanding
    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        // Need to have the exact starting preconditions for the first action, otherwise it won't run
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("isChattingTarget", (chatTarget != null)));
        worldData.Add(new KeyValuePair<string, object>("isFightingTarget", (fightTarget != null)));
        worldData.Add(new KeyValuePair<string, object>("isRazvoditTarget", (razvodTarget != null)));
        return worldData;
    }

    public HashSet<KeyValuePair<string, object>> CreateGoalState(string customGoal = "")
    {
        // Dynamically setting the goals!
        HashSet<KeyValuePair<string, object>> goals = new HashSet<KeyValuePair<string, object>>();
        if (string.IsNullOrEmpty(customGoal))
        {
            goals.Add(new KeyValuePair<string, object>("patrolArea", true));
            //sgoals.Add(new KeyValuePair<string, object>("makeMoney", true));
        }
        else
        {

        }
        return goals;
    }

   public bool MoveAgent(GoapAction nextAction)
    {
        //if we don't need to move anywhere
        if (previousDestination == (Vector2)nextAction.target.transform.position)
        {
            this.myAnimator.Play("Idle");
            nextAction.setInRange(true);
            return true;
        }

        this.myAnimator.Play("Walk");
        navAgent.SetDestination(nextAction.target.transform.position, (bool reachedDestination) =>
        {
            nextAction.setInRange(reachedDestination);
            previousDestination = nextAction.target.transform.position;
        });

       this.myAnimator.SetFloat("xInput", navAgent.movingDirection.x);
       this.myAnimator.SetFloat("yInput", navAgent.movingDirection.y);

        if (navAgent.remainingDistance < 1)
        {
            myAnimator.Play("Idle");
            nextAction.setInRange(true);
            return true;
        }
        else
            return false;
    }

    public void ActionsFinished()
    {
        Debug.Log(name + " has completed its goal!");
    }

    public void PlanAborted(GoapAction aborter)
    {
        
    }

    public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        
    }

    public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
        //Debug.Log("Found a plan for " + this.gameObject.name);
        for (int i = 0; i < actions.Count; i++)
        {
            //Debug.Log("The action #" + i.ToString() + " for the plan is: " + actions.ToArray()[i]);
        }

    }

    #endregion


}
