using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFlour : GoapAction {

    bool completed = false;
    float startTime = 0;
    public Inventory windmillInventory;
    [SerializeField] float workDuration = 2; // secs

    public GetFlour()
    {
        addPrecondition("hasFlour", false);
        addPrecondition("hasStock", true);
        addEffect("hasFlour", true);
        name = "GetFlour";
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
            this.GetComponent<Inventory>().flourLevel += 5;
            windmillInventory.flourLevel -= 5;
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
