using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using System;

public class Act_Wander : AI_Action
{

    public Act_Wander()
    {

    }

    public override void Reset()
    {
        
    }

    public override void DoAction()
    {
        if (this.started || this.completed)
        {
            return;
        }

        this.started = true;
        this.navAgent.SetDestination(BuildingTracker.Instance.GetRandomNearShelfLocation(), OnReachedWanderSpot);
    }

   

    private void OnReachedWanderSpot(bool reachedIt)
    {
        if (reachedIt)
        {
            // Choose a new target OR go fulfill your target
            bool continueWandering = RandomChoice();

            // For now: ALWAYS WONDER
            this.mainCharController.QueueAction(new Act_Wander(), false);
            Reset();
        }
    }

    bool RandomChoice()
    {
        int choice = UnityEngine.Random.Range(0, 2);
        if (choice == 0)
        {
            // Continue wandering
            return false;
        }
        else
        {
            // Start the chosen action
            return true;
        }
    }
}
