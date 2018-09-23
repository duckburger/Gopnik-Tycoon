using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatWithLokh_Action : GoapAction
{
    bool completed = false;
    float startTime = 0;
    [SerializeField] ICharStats gopStats;
    [SerializeField] float actionDuration = 3;
    [SerializeField] HuntTargetSensor targetSensor;

    public ChatWithLokh_Action()
    {
        addPrecondition("isHuntingTarget", false);
        addPrecondition("isChattingTarget", false);
        addPrecondition("isFightingTarget", false);
        addEffect("makeMoney", true);
        name = "ChatWithLokh";
        cost = 30;
    }

    private void Start()
    {
        targetSensor = this.GetComponent<HuntTargetSensor>();
        target = this.GetComponent<GopnikAI>().ChatTarget;
        gopStats = this.GetComponent<ICharStats>();
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        return target != null;
        //target = this.GetComponent<GopnikAI>().HuntTarget;
        //return true;
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
            float myIntimidation = gopStats.GetStat_Strength();
            float targetIntimidation = target.GetComponent<ICharStats>().GetStat_Strength();

            if (myIntimidation > targetIntimidation)
            {
                Debug.Log("Successful intimidation: " + gameObject.name);
                Wallet targetWalet = target.GetComponent<Wallet>();
                float stolenAmount = targetWalet.Rob();
                if (stolenAmount > 0)
                {
                    targetWalet.HasBeenMugged = true;
                    this.GetComponent<GopnikAI>().globalBalance.AddToFloatValue(stolenAmount);
                    Vector2 thisScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                    FloatingTextDisplay.Instance.SpawnFloatingText(thisScreenPos, "+" + stolenAmount.ToString("C0"));
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
        removeEffect("makeMoney");
        removeEffect("isFightingTarget");
        addEffect("makeMoney", true);
        completed = false;
        startTime = 0;
    }
}
