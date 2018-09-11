using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : GoapAction {

    bool completed = false;
    float startTime = 0;
    public Inventory windmillInventory;
    [SerializeField] float workDuration = 2; // secs

    public Harvest()
    {
        addEffect("hasWheat", true);
        name = "Harvest";
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
        // Can have task logic
        if (startTime == 0)
        {
            Debug.Log("Starting " + name);
            startTime = Time.time;
        }

        // If the work has been completed
        if (Time.time - startTime > workDuration)
        {
            Debug.Log("Finished " + name);
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
