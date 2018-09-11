﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PolyNav;

public class GopnikAI : MonoBehaviour, IGoap {

    [Header("Targets")]
    [SerializeField] Transform huntTarget;
    [SerializeField] Transform fightTarget;
    [Space(5)]
    [SerializeField] GameObject myNest;
    public GameObject Nest
    {
        get
        {
            return myNest;
        }
    }


    PolyNavAgent navAgent;
    Vector2 previousDestination;
    Health health;

    // Use this for initialization
    void Start()
    {
        navAgent = this.GetComponent<PolyNavAgent>();
        health = this.GetComponent<Health>();
    }

    #region OnSpawn Methods

    public void AssingNest(GameObject newNest)
    {
        myNest = newNest;
    }

    #endregion

    // Is run constantly to determine the state of the world in the agent's understanding
    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("isIdling", (huntTarget == null)));
        //worldData.Add(new KeyValuePair<string, object>("hasHuntTarget", (huntTarget != null)));
        //worldData.Add(new KeyValuePair<string, object>("isFightingTarget", (fightTarget != null)));
        

        return worldData;
    }

    public HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        HashSet<KeyValuePair<string, object>> goals = new HashSet<KeyValuePair<string, object>>();
        goals.Add(new KeyValuePair<string, object>("makeMoney", true));
        return goals;
    }

   public bool MoveAgent(GoapAction nextAction)
    {
        //if we don't need to move anywhere
        if (previousDestination == (Vector2)nextAction.target.transform.position)
        {
            nextAction.setInRange(true);
            return true;
        }

        navAgent.SetDestination(nextAction.target.transform.position, (bool reachedDestination) =>
        {
            nextAction.setInRange(reachedDestination);
            previousDestination = nextAction.target.transform.position;
        });

        if (navAgent.remainingDistance < 1)
        {
            return true;
        }
        else
            return false;
    }

    public void ActionsFinished()
    {
        throw new System.NotImplementedException();
    }

    public void PlanAborted(GoapAction aborter)
    {
        throw new System.NotImplementedException();
    }

    public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        throw new System.NotImplementedException();
    }

    public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
        throw new System.NotImplementedException();
    }

    
	
	
}
