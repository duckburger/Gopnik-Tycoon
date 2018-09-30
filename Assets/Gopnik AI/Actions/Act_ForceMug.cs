using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class Act_ForceMug : AI_Action
{

    // Initializing action
    private void Awake()
    {
        Reset();
    }

    public override void Reset()
    {
        this.started = false;
        this.completed = false;
        this.highPriority = true;
        this.agressiveStance = true;
        this.staminaCost = 0;
        this.reqTargetProximity = 1.7f;
        this.mainCharController = this.transform.parent.GetComponent<AI_CharController>();
        this.charStats = this.transform.parent.GetComponent<ICharStats>();
        this.navAgent = this.transform.parent.GetComponent<PolyNavAgent>();
        this.target = null;
    }

    // Will be called from the main char controller
    public override void DoAction()
    {
        base.DoAction(); // Returns if the action has started
        // Get the target from the main AI controller
        if (this.mainCharController != null)
        {
            this.started = true;
            Debug.Log("Starting action " + this.name);
            Dictionary<GameObject, float> targetAndProximity = new Dictionary<GameObject, float>();
            this.mainCharController.GetIdlingTarget(out this.target, out this.reqTargetProximity); // Asigning target and proximity
        }
        else
        {
            Debug.LogError(this.name + " on " + this.transform.parent.name + " couldn't find the main AI character controller!");
            return;
        }
        IdleAtTarget();
    }


    void IdleAtTarget()
    {
        if (this.target != null && this.navAgent != null)
        {
            this.navAgent.stoppingDistance = this.reqTargetProximity;
            this.navAgent.SetDestination(this.target.transform.position, CompleteAction);
            return;
        }
    }

    void CompleteAction(bool reachedDestination)
    {
        if (reachedDestination)
        {
        }
    }


}
