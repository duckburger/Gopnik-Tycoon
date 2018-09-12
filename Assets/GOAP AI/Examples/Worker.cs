﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Worker : MonoBehaviour, IGoap
{
	NavMeshAgent navMeshAgent;
	Vector3 previousDestination;
	Inventory inv;
    public Inventory windmillInventory;

	void Start()
	{
		navMeshAgent = this.GetComponent<NavMeshAgent>();
		inv = this.GetComponent<Inventory>();
	}

    // Is run constantly
	public HashSet<KeyValuePair<string,object>> GetWorldState () 
	{
		HashSet<KeyValuePair<string,object>> worldData = new HashSet<KeyValuePair<string,object>> ();
        worldData.Add(new KeyValuePair<string, object>("hasStock", (windmillInventory.flourLevel > 4))); ;
        worldData.Add(new KeyValuePair<string, object>("hasFlour", (inv.flourLevel > 1) ));
        worldData.Add(new KeyValuePair<string, object>("hasDelivery", (inv.breadLevel > 4)));
        return worldData;
	}


	public HashSet<KeyValuePair<string,object>> CreateGoalState ()
	{
		HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
        goal.Add(new KeyValuePair<string, object>("doJob", true));
		return goal;
	}


	public bool MoveAgent(GoapAction nextAction) {
		
		//if we don't need to move anywhere
		if(previousDestination == nextAction.target.transform.position)
		{
			nextAction.setInRange(true);
			return true;
		}
		
		navMeshAgent.SetDestination(nextAction.target.transform.position);
		
		if (navMeshAgent.hasPath && navMeshAgent.remainingDistance < 2) {
			nextAction.setInRange(true);
			previousDestination = nextAction.target.transform.position;
			return true;
		} else
			return false;
	}

	void Update()
	{
        // Ensures nav agent doesn't veer off and take very wide corners
		if(navMeshAgent.hasPath)
		{
			Vector3 toTarget = navMeshAgent.steeringTarget - this.transform.position;
         	float turnAngle = Vector3.Angle(this.transform.forward,toTarget);
         	navMeshAgent.acceleration = turnAngle * navMeshAgent.speed;
		}
	}

	public void PlanFailed (HashSet<KeyValuePair<string, object>> failedGoal)
	{

	}

	public void PlanFound (HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
	{

	}

	public void ActionsFinished ()
	{

	}

	public void PlanAborted (GoapAction aborter)
	{

	}
}