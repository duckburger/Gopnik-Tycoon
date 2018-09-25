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
        addEffect("patrolArea", true);
        name = "PunchTarget";
        cost = 0;
    }

    private void Start()
    {
        targetSensor = this.GetComponent<HuntTargetSensor>();
        target = this.GetComponent<GopnikAI>().ChatTarget;
        gopStats = this.GetComponent<ICharStats>();
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        target = this.GetComponent<GopnikAI>().FightTarget;
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

        this.GetComponent<Animator>().Play("Punch");

        // If the work has been completed
        if (Time.time - startTime > actionDuration)
        {
            Debug.Log("Finished action" + name);
            float targetHealth = 0;
            if (target != null)
            {
                Health targetHealthController = target.GetComponent<Health>();
                targetHealthController.AdjustHealth(-15f);
                targetHealth = targetHealthController.GetCurrentHealth();
            }
            else
            {
                this.GetComponent<GopnikAI>().FightTarget = null;
                startTime = 0;
                return completed;
            }
            

            if (20 > targetHealth)
            {
                Debug.Log("Successful intimidation by force: " + gameObject.name);
                Wallet targetWalet = target.GetComponent<Wallet>();
                float stolenAmount = targetWalet.Rob();
                if (stolenAmount > 0)
                {
                    PlayerCashController.Instance.AdjustBalance(stolenAmount);
                    completed = true;
                    this.target = null;
                    this.GetComponent<GopnikAI>().FightTarget = null;
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
                Debug.Log("Still got health left: " + gameObject.name);
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
