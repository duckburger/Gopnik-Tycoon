using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAtNest_Action : GoapAction
{

    bool completed = false;
    float startTime = 0;
    [SerializeField] float actionDuration = 2;
    [SerializeField] float detectionDistance;

    public IdleAtNest_Action()
    {
        addPrecondition("isFree", true);
        addPrecondition("isAtBase", true);
        addEffect("hasHuntTarget", true);
        name = "IdleAtNest";
    }

    private void Start()
    {
        // Getting the nest off the gopnik AI, this nest gets assigned when the the gopniks are first spawned
        target = this.GetComponent<GopnikAI>().Nest;
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

        // If the work has been completed
        if (Time.time - startTime > actionDuration)
        {
            Debug.Log("Finished action" + name);
            Debug.Log("Checking whether there is a civilian target in the vicinity of " + detectionDistance + " units");
            completed = true;
        }
        return true;
    }

   Transform CheckForAvailableTargets()
   {
        foreach (Transform civ in Map.Instance.ActiveCivs)
        {
            float distanceToCiv = Vector2.Distance(this.transform.position, civ.transform.position);
            
        }
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
