using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;



public class Act_ForceMug : AI_Action
{
    // Initializing action
    private void Awake()
    {
        Reset();
    }

    public override void Reset()
    {
        this.started = false;
        this.completed = false;
        this.highPriority = true;
        this.agressiveStance = true;
        this.staminaCost = 0; // For this the stamina cost actually depends on attacks themeselves
        this.reqTargetProximity = 0.5f;
        this.mainCharController = this.transform.parent.GetComponent<AI_CharController>();
        this.charStats = this.transform.parent.GetComponent<ICharStats>();
        this.navAgent = this.transform.parent.GetComponent<PolyNavAgent>();
        this.target = null;
    }

    // Will be called from the main char controller
    public override void DoAction()
    {
        base.DoAction(); // Returns if the action has started
        if (this.target == null)
        {
            Debug.Log("No target for the Force Mug action!");
            this.completed = true;
            this.started = true;
            return;
        }
        if (this.mainCharController != null)
        {
            if (this.started && this.navAgent.remainingDistance < reqTargetProximity)
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
        if (reachedTarget)
        {
            // Produce dialogue if there is any
            bool hasPreActionText = !string.IsNullOrEmpty(this.phrasePack.GetRandomPhrase(PhraseType.preAction));
            if (this.phrasePack != null && hasPreActionText)
            {
                // Show the the dialogue bubble
                string preActionPhrase = this.phrasePack.GetRandomPhrase(PhraseType.preAction);
                this.mainCharController.dialBubbleDisplay.ShowDialogue(preActionPhrase);
            }
            // Choose attack
            if (this.mainCharController.staminaController.CurrStaminaPercentage > 70)
            {
                this.mainCharController.myAnimator.Play("Kick");
                this.mainCharController.CurrentAttack = AttackType.Kick;
                this.mainCharController.staminaController.AdjustStamina(-25);
            }
            else
            {
                this.mainCharController.myAnimator.Play("Punch");
                this.mainCharController.CurrentAttack = AttackType.Punch;
                this.mainCharController.staminaController.AdjustStamina(-10);        
            }
        }
    }

    public override void OnAttackConnected(AttackType type)
    {
        Health targetHealth = this.target.GetComponent<Health>();
        if (target != null)
        {
            Debug.Log("Registering damage from a " + type);
            switch (type)
            {
                case AttackType.Punch:
                    targetHealth.AdjustHealth(-15);
                    break;
                case AttackType.Kick:
                    targetHealth.AdjustHealth(-25);
                    break;
                default:
                    break;
            }

            float targetHealthPercentage = targetHealth.CurrHealthPercentage;
            if (targetHealthPercentage < 30)
            {
                Wallet targetWallet = this.target.GetComponent<Wallet>();
                float amtToSteal = targetWallet.Rob();
                targetWallet.HasBeenMugged = true;
                this.mainCharController.globalBalance.AddToFloatValue(amtToSteal);

                Vector2 thisScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                FloatingTextDisplay.Instance.SpawnFloatingText(thisScreenPos, "+" + amtToSteal.ToString("C0"));
                Debug.Log("Robbed the target for " + amtToSteal + ". Finishing the action");
                this.completed = true;
            }

            if(target == null)
            {
                // Target died after this hit, complete this action
                Debug.Log("No target, cancelling the action");
                this.completed = true;
            }
        }
    }

   
    public override void OnAnimationFinished()
    {
        if (this.mainCharController != null)
        {
            this.mainCharController.myAnimator.Play("Idle");
        }
    }
}
