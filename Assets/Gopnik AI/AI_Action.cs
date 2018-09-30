using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class AI_Action : MonoBehaviour
{
    protected bool started = false;
    public bool Started
    {
        get
        {
            return this.started;
        }
    }
    protected bool completed = false;
    public bool Completed
    {
        get
        {
            return this.completed;
        }
    }
    protected bool highPriority = false;
    public bool HighPriority
    {
        get
        {
            return this.highPriority;
        }
    } // Determines whether the action should interrupt all other actions by default
    protected bool agressiveStance = false;
    public bool AgressiveStance
    {
        get
        {
            return this.agressiveStance;
        }
    }
    protected string actionAnimState;
    public string ActionAnimState
    {
        get
        {
            return this.actionAnimState;
        }
    }

    [SerializeField] Sprite actionIcon;
    public Sprite ActionIcon
    {
        get
        {
            return this.actionIcon;
        }
    }

    [Space(10)]
    [Header("Dialogue")]
    [SerializeField] Dial_PhrasePack phrasePack;
    public Dial_PhrasePack PhrasePack
    {
        get
        {
            return this.phrasePack;
        }
    }

    protected float staminaCost = 0;
    public float StaminaCost
    {
        get
        {
            return this.staminaCost;
        }
    }

    protected GameObject target;
    public GameObject Target
    {
        get
        {
            return this.target;
        }
    }
    protected float reqTargetProximity;
    public float ReqTargetProximity
    {
        get
        {
            return this.reqTargetProximity;
        }
    }

    protected ICharStats charStats; // Used to calculate damage if it's an agressive action (based on strength and so on)
    protected AI_CharController mainCharController;
    protected PolyNavAgent navAgent;

    public virtual void DoAction()
    {
        if (this.started || this.completed)
        {
            return;
        }
        // Action logic
    }

    public virtual void Reset() { }
   
}
