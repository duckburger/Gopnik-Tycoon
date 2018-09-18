using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PolyNav;

public class GopnikAI : MonoBehaviour, IGoap, ICharStats {

    [Header("Targets")]
    [SerializeField] Transform huntTarget;
    public GameObject HuntTarget
    {
        get
        {
            if (huntTarget != null)
            {
                return huntTarget.gameObject;
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
                huntTarget = value.transform;
            }
            else
            {
                huntTarget = null;
            }
        }
    }

    [SerializeField] Transform fightTarget;
    public GameObject FightTarget
    {
        get
        {
            return fightTarget.gameObject;
        }
        set
        {
            fightTarget = value.transform;
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
    [SerializeField] float stat_intimidation;
    public float GetStat_Intimidation()
    {
        return stat_intimidation;
    }
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

    #region GOAP Methods
    // Is run constantly to determine the state of the world in the agent's understanding
    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        // Need to have the exact starting preconditions for the first action, otherwise it won't run
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("hasHuntTarget", (huntTarget != null)));
        worldData.Add(new KeyValuePair<string, object>("isFightingTarget", (fightTarget != null)));
        return worldData;
    }

    public HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        // Dynamically setting the goals!
        HashSet<KeyValuePair<string, object>> goals = new HashSet<KeyValuePair<string, object>>();
        if (targetSensor.CheckForAvailableTargets() == null)
        {
            goals.Add(new KeyValuePair<string, object>("patrolArea", true));
        }
        else
        {
            goals.Add((new KeyValuePair<string, object>("makeMoney", true)));
        }
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
