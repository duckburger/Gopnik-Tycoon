using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatWithLokh_Action : GoapAction
{
    bool completed = false;
    float startTime = 0;
    [SerializeField] float actionDuration = 2;

    public ChatWithLokh_Action()
    {
        addPrecondition("hasHuntTarget", true);
        addEffect("makeMoney", true);
        name = "ChatWithLokh";
    }

    private void Start()
    {
        target = this.GetComponent<GopnikAI>().HuntTarget;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        if (target == null)
        {
            target = this.GetComponent<GopnikAI>().HuntTarget;
        }
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
           
                completed = true;
                return completed;
                startTime = 0;
            
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
