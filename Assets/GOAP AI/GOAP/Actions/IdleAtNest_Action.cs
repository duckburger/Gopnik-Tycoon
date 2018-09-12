using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAtNest_Action : GoapAction
{

    bool completed = false;
    float startTime = 0;
    [SerializeField] GopnikAI gopAI;
    [SerializeField] float actionDuration = 8;
    [SerializeField] float detectionDistance = 4;

    public IdleAtNest_Action()
    {
        addPrecondition("isFree", true);
        addEffect("hasHuntTarget", true);
        name = "IdleAtNest";
        if (gopAI != null)
        {
            target = gopAI.HuntTarget;
        }
       
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, detectionDistance);
    }

    #endregion

    private void Start()
    {
        // First target is in the vicinity of the nest, but not in its exact position
        GetNewIdlingTarget();
    }

    private void GetNewIdlingTarget()
    {
        GopnikNest myNest = this.GetComponent<GopnikAI>().Nest;
        target = myNest.SpawnAndGetRandomIdlePoint();
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        if (CheckForAvailableTargets() == null)
        {
            return true;
        }
        else
        {
            Debug.Log("Found a лох nearby! (Procedural precondition)");
            completed = true;
            startTime = 0;
            return completed;
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

        target = CheckForAvailableTargets();
        if (target != null)
        {
            Debug.Log(name + " - Success! Found a лох nearby!");
            gopAI.HuntTarget = target;
            completed = true;
            startTime = 0;
            return completed;
        }

        // If the work has been completed
        if (Time.time - startTime > actionDuration)
        {
            Debug.Log("Finished action" + name);
            Debug.Log("Checking whether there is a civilian target in the vicinity of " + detectionDistance + " units");
            if (CheckForAvailableTargets() == null)
            {
                Debug.Log(name + " - Couldn't find a лох nearby. Rerouting to a different idle spot");
                completed = false;
                startTime = 0;
                if (target != null)
                {
                    target.SetActive(false);
                    target = null;
                }
                GetNewIdlingTarget();
                return completed;
            }

            target = CheckForAvailableTargets();
            if (target != null)
            {
                Debug.Log(name + " - Success! Found a лох nearby!");
                gopAI.HuntTarget = target;
                completed = true;
                startTime = 0;
                return completed;
            }
            else
            {
                Debug.Log(name + " - Couldn't find a лох nearby. Rerouting to a different idle spot");
                completed = false;
                target.SetActive(false);
                target = null;
                GetNewIdlingTarget();
                startTime = 0;
            }
        }
        return true;
    }

   GameObject CheckForAvailableTargets()
   {
        foreach (Transform civ in Map.Instance.ActiveCivs)
        {
            float distanceToCiv = Vector2.Distance(this.transform.position, civ.transform.position);
            CivStates civStates = civ.GetComponent<CivStates>();
            if (distanceToCiv <= detectionDistance && !civStates.IsAgressive)
            {
                // Set this as the target
                return civ.gameObject;
            }
           

        }
        return null;
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
