using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using System;
using UnityEngine.Events;

[Serializable]
public class OnStateChanged_Event : UnityEvent<ActionType> { }

public enum ActionType
{
    Idling,
    Force,
    Chat,
    Razvod
}

public class AI_CharController : MonoBehaviour, ICharStats
{
    [Header("Action Parent")]
    [SerializeField] GameObject actionParent;
    public GameObject ActionParent
    {
        get
        {
            return this.actionParent;
        }
    }
    [Space(10)]
    [Header("Plug-in modules")]
    [SerializeField] Health healthController;
    [SerializeField] Wallet walletController;
    public Stamina staminaController;
    public ScriptableFloatVar globalBalance;
    
    [Header("Nav Agent")]
    [SerializeField] PolyNavAgent navAgent;
    [Header("Actions")]
    [SerializeField] Queue<AI_Action> actionQueue = new Queue<AI_Action>();

    [Space(10)]
    [Header("ICharStats")]
    [SerializeField] string charName;
    public string GetCharName()
    {
        return this.charName;
    }
    [SerializeField] Sprite charPortrait;
    public Sprite GetPortrait()
    {
        return this.charPortrait;
    }
    [SerializeField] int charisma;
    public int GetStat_Charisma()
    {
        return this.charisma;
    }
    [SerializeField] int cunning;
    public int GetStat_Cunning()
    {
        return this.cunning;
    }
    [SerializeField] int strength;
    public int GetStat_Strength()
    {
        return this.strength;
    }

    public float GetWalletBalance()
    {
        if (this.walletController != null)
        {
            return this.walletController.CurrentBalance;
        }
        return 0;
    }

    [Space(10)]
    [Header("Behaviour")]
    [SerializeField] bool fightToDeath;

    [Header("Events")]
    public OnStateChanged_Event stateChangedEvent;


    bool inDialogue = false;
    bool isBusy = false;
    AI_Action currentAction = null;
    AI_IdleSpot currentIdleSpot = null;
    public AI_IdleSpot CurrentIdleSpot
    {
        get
        {
            return this.currentIdleSpot;
        }
        set
        {
            this.currentIdleSpot = value;
        }
    }

    [HideInInspector]
    public Animator myAnimator;
    public DialogueBubbleDisplayer dialBubbleDisplay;

    [Header("Temp")]
    AttackType currentAttack;
    public AttackType CurrentAttack
    {
        get
        {
            return this.currentAttack;
        }
        set
        {
            this.currentAttack = value;
        }
    }

    void Awake()
    {
        this.myAnimator = this.GetComponent<Animator>();
        this.dialBubbleDisplay = this.GetComponent<DialogueBubbleDisplayer>();
    }

    #region Main Update Loop

    private void Update()
    {
        if (this.navAgent.hasPath)
        {
            this.myAnimator.SetFloat("xInput", this.navAgent.movingDirection.x);
            this.myAnimator.SetFloat("yInput", this.navAgent.movingDirection.y);
        }
        

        if (currentAction != null && !currentAction.Started)
        {
            this.isBusy = true;
            this.currentAction.DoAction();
            return;
        }
        if (currentAction != null && this.currentAction.Completed)
        {
            this.currentAction.Reset();
            this.currentAction = null;
            PickNewAction();
            return;
        }
        if (this.currentAction == null)
        {
            PickNewAction();
            return;
        }
        this.currentAction.DoAction();
    }

    private void PickNewAction()
    {
        if (this.actionQueue.Count > 0)
        {
            this.currentAction = this.actionQueue.Dequeue();
            this.navAgent.stoppingDistance = this.currentAction.ReqTargetProximity;
            stateChangedEvent.Invoke(this.currentAction.ActionType);
            return;
        }
        StartIdlingAction();
    }

    #endregion

    public void AddAction(AI_Action newAction, bool flushAllActions)
    {
        if (newAction == null)
        {
            return;
        }
        if (newAction.HighPriority || flushAllActions)
        {
            DeleteAllActions();
            this.currentAction = newAction;
            return;
        }
        this.actionQueue.Enqueue(newAction);
    }

    public ActionType GetCurrentActionType()
    {
        return this.currentAction.ActionType;
    }

    void DeleteAllActions()
    {
        this.actionQueue.Clear();
        isBusy = false;
        StartIdlingAction(); // Only happens if the character has an idling spot assigned
    }

    void StartIdlingAction()
    {
        if (this.currentIdleSpot != null)
        {
            // Add idling action here
            Act_Idle idlingAction = null;
            if (this.actionParent != null)
            {
                idlingAction = this.actionParent.GetComponent<Act_Idle>();
            }
            else
            {
                Debug.LogError("No action parent connected!");
                return;
            }
            Debug.Log("Setting current action to Idle");
            this.currentAction = idlingAction;
            stateChangedEvent.Invoke(this.currentAction.ActionType);

        }
    }

    public void GetIdlingTarget(out GameObject target, out float reqProximity)
    {
        if (this.currentIdleSpot != null)
        {
            target = this.currentIdleSpot.GetIdlingTarget();
            reqProximity = this.currentIdleSpot.GetReqIdleProximity();
            return;
        }
        target = null;
        reqProximity = 0;
    }

    public void OnAttackConnected(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight < 0.5f)
        {
            return;
        }
        Debug.Log("Attack connected event triggered once");
        if (this.currentAction != null && this.currentAction.ActionType == ActionType.Force)
        {
            this.currentAction.OnAttackConnected(this.currentAttack);
        }
    }

    public void OnAnimationFinished(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight < 0.5f)
        {
            return;
        }
        if (this.currentAction != null)
        {
            Debug.Log("Animation finished triggered once");
            this.currentAction.OnAnimationFinished();
        }
        
    }

    
}