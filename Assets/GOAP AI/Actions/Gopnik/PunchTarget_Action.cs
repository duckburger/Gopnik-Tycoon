using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchTarget_Action : GoapAction
{
    bool completed = false;
    float startTime = 0;
    [SerializeField] ICharStats gopStats;
    [SerializeField] float actionDuration = 0.5f;
    [SerializeField] HuntTargetSensor targetSensor;

    public PunchTarget_Action()
    {
        addPrecondition("isFightingTarget", true);
        addEffect("makeMoney", true);
        name = "PunchTarget";
        cost = 0;
    }

    private void Start()
    {
        targetSensor = this.GetComponent<HuntTargetSensor>();
        target = this.GetComponent<GopnikAI>().HuntTarget;
        gopStats = this.GetComponent<ICharStats>();
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        if (target == null)
        {
            if (this.GetComponent<GopnikAI>().HuntTarget != null)
            {
                target = this.GetComponent<GopnikAI>().HuntTarget;
            }
            else
            {
                this.GetComponent<GopnikAI>().HuntTarget = targetSensor.CheckForAvailableTargets();
                target = this.GetComponent<GopnikAI>().HuntTarget;
            }
        }
        return target != null;
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

        // TODO: Play punch animation

        // If the work has been completed
        if (Time.time - startTime > actionDuration)
        {
            Debug.Log("Finished action" + name);
            float myIntimidation = gopStats.GetStat_Intimidation();
            float targetIntimidation = target.GetComponent<ICharStats>().GetStat_Intimidation();

            if (myIntimidation > targetIntimidation)
            {
                Debug.Log("Successful intimidation: " + gameObject.name);
                Wallet targetWalet = target.GetComponent<Wallet>();
                float stolenAmount = targetWalet.Rob();
                if (stolenAmount > 0)
                {
                    PlayerCashController.Instance.AdjustBalance(stolenAmount);
                    completed = true;
                    startTime = 0;
                    return completed;
                }
                else
                {
                    Debug.Log("What the fuck!? This лох had no money. What a waste of a gopnik's time!");
                }                
            }
            else
            {
                Debug.Log("Failed intimidation: " + gameObject.name);
                removeEffect("makeMoney");
                this.GetComponent<GopnikAI>().FightTarget = target;
                addEffect("isFightingTarget", true);
                completed = true;
                startTime = 0;
                return completed;
            }
            
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
