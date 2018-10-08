using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;



public class Act_ForceMug : AI_Action
{
    [SerializeField] List<AttackData> availableAttacks = new List<AttackData>();

    bool isWaiting = false;
    int attacksCompleted = 0;
    float additionalRange = 0.1f;

    // Initializing action
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
        this.charStats = this.transform.parent.GetComponent<ICharStats>();
        this.navAgent = this.transform.parent.GetComponent<PolyNavAgent>();
        this.target = null;
        
        // Additional
        this.isWaiting = false;
        this.attacksCompleted = 0;
    }

    // Will be called from the main char controller
    public override void DoAction()
    {
        if (isWaiting)
        {
            return;
        }
        if (this.target == null)
        {
            Debug.Log("No target for the Force Mug action!");
            this.completed = true;
            this.started = true;
            return;
        }
        if (this.mainCharController != null)
        {
            float dist = Vector2.Distance(this.transform.position, this.target.transform.position) + this.additionalRange;
            // Debug.Log("Distance is " + dist);
            if (this.started && dist <= reqTargetProximity && !this.isWaiting)
            {
                OnReachedTarget(true);
                return;
            }
            this.started = true;
            this.navAgent.SetDestination(this.target.transform.position, OnReachedTarget);
            this.mainCharController.myAnimator.Play("Walk");
        }
        else
        {
            Debug.LogError(this.name + " on " + this.transform.parent.name + " couldn't find the main AI character controller!");
            return;
        }

    }

    void OnReachedTarget(bool reachedTarget)
    {
        if (reachedTarget && !this.isWaiting)
        {
            // Produce dialogue if there is any
            bool hasPreActionText = !string.IsNullOrEmpty(this.phrasePack.GetRandomPhrase(PhraseType.preAction));
            if (this.attacksCompleted == 0 && this.phrasePack != null && hasPreActionText)
            {
                // Show the the dialogue bubble
                string preActionPhrase = this.phrasePack.GetRandomPhrase(PhraseType.preAction);
                this.mainCharController.dialBubbleDisplay.ShowDialogue(preActionPhrase);
            }
            // Choose attack
            if (this.availableAttacks == null || this.availableAttacks.Count <= 0)
            {
                Debug.LogError(this.name + " couldn't find any attacks to use");
                return;
            }
            int attackIndex = Random.Range(0, this.availableAttacks.Count);
            AttackData attackToApply = this.availableAttacks[attackIndex];

            float stamCost = attackToApply.staminaCost;
            float currentStam = this.mainCharController.staminaController.GetCurrentStamina();
            if ((currentStam - stamCost) > 0)
            {
                this.mainCharController.myAnimator.Play(attackToApply.animStateName);
                this.mainCharController.CurrentAttack = attackToApply.type;
                this.mainCharController.staminaController.AdjustStamina(-attackToApply.staminaCost);
                this.isWaiting = true;
                StopAllCoroutines();
                StartCoroutine(AttackTimer(attackToApply.type));
            }
        }
    }

    IEnumerator AttackTimer(AttackType type)
    {
        // Wait for animation to finish
        float timeToWait = this.mainCharController.myAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(timeToWait);
        OnAttackConnected(type);
    }

    public override void OnAttackConnected(AttackType type)
    {
        if (target != null)
        {
            Health targetHealth = this.target.GetComponent<Health>();
            //Debug.Log("Registering damage from a " + type);
            switch (type)
            {
                case AttackType.Punch:
                    targetHealth.AdjustHealth(-10, true, this.mainCharController.gameObject);
                    break;
                case AttackType.Kick:
                    targetHealth.AdjustHealth(-10, true, this.mainCharController.gameObject);
                    break;
                default:
                    break;
            }
            // Push the enemy back
            Rigidbody2D targetRB = this.target.GetComponent<Rigidbody2D>();
            Vector2 hitVector = this.target.transform.position - this.transform.position;
            targetRB.AddForce(hitVector * 2, ForceMode2D.Impulse);
            this.attacksCompleted++;
            float targetHealthPercentage = targetHealth.CurrHealthPercentage;
            if (targetHealthPercentage < 30)
            {
                Wallet targetWallet = this.target.GetComponent<Wallet>();
                float amtToSteal = targetWallet.Rob();
                targetWallet.HasBeenMugged = true;
                this.mainCharController.globalBalance.AddToFloatValue(amtToSteal);

                Vector2 thisScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                FloatingTextDisplay.Instance.SpawnFloatingText(thisScreenPos, "+" + amtToSteal.ToString("C0"));
                //Debug.Log("Robbed the target for " + amtToSteal + ". Finishing the action");
                this.completed = true;
            }

            
            this.isWaiting = false;
        }
    }

   
    

    
}
