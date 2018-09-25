using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForHuntTarget_Action : GoapAction
{
    bool completed = false;
    float startTime = 0;
    [SerializeField] GopnikAI gopAI;
    [SerializeField] float actionDuration = 0.1f;
    [SerializeField] HuntTargetSensor targetSensor;

    public CheckForHuntTarget_Action()
    {
        addPrecondition("isHuntingTarget", false);
        addPrecondition("isIdling", true);
        addEffect("isHuntTarget", true);
        name = "CheckForHuntTarget";
        cost = 10;
    }

    private void Start()
    {
        targetSensor = this.GetComponent<HuntTargetSensor>();
        gopAI = this.GetComponent<GopnikAI>();
    }


    public override bool checkProceduralPrecondition(GameObject agent)
    {
        return true;
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

        if (targetSensor.CheckForAvailableTargets() == null)
        {
            completed = false;
            return false;
        }

        // If the work has been completed
        if (Time.time - startTime > actionDuration)
        {
            Debug.Log("Finished action" + name);
            GameObject huntTarget = targetSensor.CheckForAvailableTargets();
            if (huntTarget != null)
            {
                Debug.Log(name + " - Success! Found a лох nearby!");
                gopAI.ChatTarget = huntTarget;
                completed = true;
                startTime = 0;
                return completed;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public override bool requiresInRange()
    {
        return false;
    }

    public override void reset()
    {
        completed = false;
        startTime = 0;
    }
}
