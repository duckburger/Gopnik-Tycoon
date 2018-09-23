using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PolyNav;

public enum GopnikActionType
{ 
    Idling,
    Force,
    Chat,
    Razvod
}

public class GopnikAI : MonoBehaviour, IGoap, ICharStats {

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
            if (chatTarget != null)
            {
                return this.chatTarget.gameObject;
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (value != null)
            {
                this.chatTarget = value.transform;
            }
            else
            {
                this.chatTarget = null;
            }
        }
    }

    [SerializeField] Transform fightTarget;
    public GameObject FightTarget
    {
        get
        {
            return this.fightTarget.gameObject;
        }
        set
        {
            this.fightTarget = value.transform;
        }
    }

    [SerializeField] Transform razvodTarget;
    public GameObject RazvodTarget
    {
        get
        {
            return this.razvodTarget.gameObject;
        }
        set
        {
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
        return this.GetComponent<Wallet>().CurrentBalance();
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

    public void AssignTarget(GameObject target, GopnikActionType actionType)
    {
        switch (actionType)
        {
            case GopnikActionType.Force:
                this.FightTarget = target;
                this.ChatTarget = null;
                this.RazvodTarget = null;
                break;
            case GopnikActionType.Chat:
                this.ChatTarget = target;
                this.FightTarget = null;
                this.RazvodTarget = null;
                break;
            case GopnikActionType.Razvod:
                this.RazvodTarget = target;
                this.FightTarget = null;
                this.ChatTarget = null;
                break;
            default:
                break;
        }
        GetWorldState();
        CreateGoalState();
    }

    public GopnikActionType GetCurrentAction()
    {
        if (fightTarget != null)
        {
            return GopnikActionType.Force;
        }
        else if (chatTarget != null)
        {
            return GopnikActionType.Chat;
        }
        else if (razvodTarget != null)
        {
            return GopnikActionType.Razvod;
        }
        return GopnikActionType.Idling;
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

    public HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        // Dynamically setting the goals!
        HashSet<KeyValuePair<string, object>> goals = new HashSet<KeyValuePair<string, object>>();
        goals.Add(new KeyValuePair<string, object>("patrolArea", true));
        return goals;
    }

   public bool MoveAgent(GoapAction nextAction)
    {
        //if we don't need to move anywhere
        if (previousDestination == (Vector2)nextAction.target.transform.position)
        {
            myAnimator.Play("Idle");
            nextAction.setInRange(true);
            return true;
        }

        myAnimator.Play("Walk");
        navAgent.SetDestination(nextAction.target.transform.position, (bool reachedDestination) =>
        {
            nextAction.setInRange(reachedDestination);
            previousDestination = nextAction.target.transform.position;
        });

        myAnimator.SetFloat("xInput", navAgent.movingDirection.x);
        myAnimator.SetFloat("yInput", navAgent.movingDirection.y);

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
        Debug.Log("Found a plan for " + this.gameObject.name);
        for (int i = 0; i < actions.Count; i++)
        {
            Debug.Log("The action #" + i.ToString() + " for the plan is: " + actions.ToArray()[i]);
        }

    }

    #endregion


}
