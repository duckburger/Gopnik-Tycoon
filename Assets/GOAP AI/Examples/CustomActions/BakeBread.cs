using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeBread : GoapAction {

    bool completed = false;
    float startTime = 0;
    [SerializeField] float workDuration = 2; // secs

    public BakeBread()
    {
        addPrecondition("hasFlour", true);
        addEffect("doJob", true);
        name = "BakeBread";
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
            Debug.Log("Starting " + name);
            startTime = Time.time;
        }

        // If the work has been completed
        if (Time.time - startTime > workDuration)
        {
            Debug.Log("Finished " + name);
            this.GetComponent<Inventory>().flourLevel -= 2;
            this.GetComponent<Inventory>().breadLevel += 1;
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
