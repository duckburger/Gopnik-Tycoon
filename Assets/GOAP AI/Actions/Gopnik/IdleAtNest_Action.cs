using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAtNest_Action : GoapAction
{

    bool completed = false;
    float startTime = 0;
    [SerializeField] GopnikAI gopAI;
    [SerializeField] float actionDuration = 3;
    [SerializeField] HuntTargetSensor targetSensor;

    public IdleAtNest_Action()
    {
        addPrecondition("isFightingTarget", false);
        addEffect("isIdling", true);
        addEffect("patrolArea", true);
        name = "IdleAtNest";
        cost = 60;
    }

    private void Start()
    {
        // First target is in the vicinity of the nest, but not in its exact position
        targetSensor = this.GetComponent<HuntTargetSensor>();
        gopAI = this.GetComponent<GopnikAI>();
    }

    private bool GetNewIdlingTarget()
    {
        GopnikNest myNest = this.GetComponent<GopnikAI>().MyNest;
        target = myNest.SpawnAndGetRandomIdlePoint();
        if (target != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        if (GetNewIdlingTarget())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool isDone()
    {
        return completed;
    }

    public override bool perform(GameObject agent)
    {
        // Takes over while the task is taking place
        if (startTime == 0)
        {
            Debug.Log("Starting action" + name);
            startTime = Time.time;
        }

        // If the work has been completed
        if (Time.time - startTime > actionDuration)
        {
            // We just stand in the idle spot for now, later on gotta animate or something
            Debug.Log("Finished action" + name);
            gopAI.HuntTarget = targetSensor.CheckForAvailableTargets();
            completed = true;
        }
        return true;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override void reset()
    {
        completed = false;
        startTime = 0;
    }
}
