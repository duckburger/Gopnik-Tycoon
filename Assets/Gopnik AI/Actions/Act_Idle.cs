using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class Act_Idle : AI_Action
{
    [SerializeField] float waitTime = 3f;

    // Initializing action
    private void Awake()
    {
        Reset();
    }

    public override void Reset()
    {
        this.started = false;
        this.completed = false;
        this.highPriority = false;
        this.agressiveStance = false;
        this.staminaCost = 0;
        this.reqTargetProximity = 0.5f;
        this.mainCharController = this.transform.parent.GetComponent<AI_CharController>();
        this.charStats = this.transform.parent.GetComponent<ICharStats>();
        this.navAgent = this.transform.parent.GetComponent<PolyNavAgent>();
        this.target = null;
    }

    // Will be called from the main char controller
    public override void DoAction()
    {
        //base.DoAction(); // Returns if the action has started
        if (this.started || this.completed)
        {
            return;
        }
        // Get the target from the main AI controller
        if (this.mainCharController != null)
        {
            this.started = true;
            Debug.Log("Starting action " + this.actionName);
            Dictionary<GameObject, float> targetAndProximity = new Dictionary<GameObject, float>();
            this.mainCharController.GetIdlingTarget(out this.target, out this.reqTargetProximity); // Asigning target and proximity
            this.mainCharController.myAnimator.Play("Walk");
        }
        else
        {
            Debug.LogError(this.name + " on " + this.transform.parent.name + " couldn't find the main AI character controller!");
            return;
        }
        IdleAtTarget();
    }


    // Moves the character to the target
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
            StopAllCoroutines();
            StartCoroutine(WaitAtTarget());
            this.mainCharController.myAnimator.Play("Idle");
        }
    }

    IEnumerator WaitAtTarget()
    {
        yield return new WaitForSeconds(this.waitTime);
        this.completed = true;
      
    }

   
}
