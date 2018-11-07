using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public enum ActivityType
{
    None = 0,
    Shop,
    CauseTrouble,
}

public class Act_PickObjective : AI_Action
{

    private void Awake()
    {
        Reset();
    }

    public override void Reset()
    {
        // Base
        this.started = false;
        this.completed = false;
        this.highPriority = true;
        this.agressiveStance = true;
        this.staminaCost = 0; // For this the stamina cost actually depends on attacks themeselves
        this.reqTargetProximity = 0.7f;
        this.mainCharController = this.transform.parent.GetComponent<AI_CharController>();
    }

    public override void DoAction()
    {
        if (this.started || this.completed)
        {
            return;
        }

        // Pick activity to do
        ActivityType chosenActivity = ActivityType.Shop;

        this.mainCharController.currentObjective = chosenActivity;
        // Proceed to wandering

        this.mainCharController.QueueAction(new Act_Wander(), false);
        this.completed = true;
        Reset();
    }
}
