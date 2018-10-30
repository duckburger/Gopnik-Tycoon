using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackSide
{
    Left,
    Right,
    Top,
    Bottom
}

public class FightTarget_Action : GoapAction
{
    bool completed = false;
    float startTime = 0;
    [SerializeField] ICharStats gopStats;
    [SerializeField] float actionDuration = 0.7f;

    Stamina myStamina;
    Animator myAnimator;
    bool hasAttacked = false;
    bool isAttacking = false;

    public FightTarget_Action()
    {
        addPrecondition("isFightingTarget", true);
        addEffect("patrolArea", true);
        name = "FightTarget";
        cost = 0;
    }

    private void Start()
    {
        this.target = this.GetComponent<GopnikAI>().ChatTarget;
        this.gopStats = this.GetComponent<ICharStats>();
        this.myStamina = this.GetComponent<Stamina>();
        this.myAnimator = this.GetComponent<Animator>();
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
        if (!this.hasAttacked && !this.isAttacking)
        {
            this.isAttacking = true;
            if (this.myStamina.CurrStaminaPercentage > 70)
            {
                Debug.Log("Starting kick animation");
                this.myAnimator.Play("Kick");
                this.myStamina.AdjustStamina(-30);
            }
            else
            {
                Debug.Log("Starting Punch animation");
                this.myAnimator.Play("Punch");
                this.myStamina.AdjustStamina(-15);
            }
        }

        if (this.hasAttacked && !this.isAttacking)
        {
            float targetHealth;
            if (target != null)
            {
                Health targetHealthController = target.GetComponent<Health>();
                targetHealth = targetHealthController.GetCurrentHealth();
            }
            else
            {
                this.GetComponent<GopnikAI>().FightTarget = null;
                return false;
            }

            if (20 > targetHealth)
            {
                Debug.Log("Successful intimidation by force: " + gameObject.name);
                Wallet targetWalet = target.GetComponent<Wallet>();
                float stolenAmount = targetWalet.Rob();
                if (stolenAmount > 0)
                {
                    targetWalet.HasBeenMugged = true;
                    this.GetComponent<GopnikAI>().globalBalance.AdjustFloatValue(stolenAmount);

                    Vector2 thisScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                    FloatingTextDisplay.SpawnFloatingText(thisScreenPos, "+" + stolenAmount.ToString("C0"));

                    completed = true;
                    this.target = null;
                    this.GetComponent<GopnikAI>().FightTarget = null;
                    return completed;
                }
                else
                {
                    Debug.Log("What the fuck!? This лох had no money. What a waste of a gopnik's time!");
                    reset();
                    completed = false;
                    return true;
                }
            }
            else
            {
                Debug.Log("Still got health left: " + gameObject.name);
                reset();
                return true;
            }
        }
        return true;
        #region OLD WAY
        // THE OLD WAY
        /*
        // Takes over while the task is taking place
        if (startTime == 0)
        {
            Debug.Log("Starting action" + name);
            startTime = Time.time;
            if (this.myStamina.CurrStaminaPercentage > 70)
            {
                Debug.Log("Starting kick animation");
                this.myAnimator.Play("Kick");
                this.myStamina.AdjustStamina(-30);
            }
            else
            {
                Debug.Log("Starting Punch animation");
                this.myAnimator.Play("Punch");
                this.myStamina.AdjustStamina(-15);
            }
        }

        // If the work has been completed
        if (Time.time - startTime > actionDuration)
        {
            Debug.Log("Completing action " + name);
            float targetHealth = 100;
            if (target != null)
            {
                Health targetHealthController = target.GetComponent<Health>();
                targetHealth = targetHealthController.GetCurrentHealth();
            }
            else
            {
                this.GetComponent<GopnikAI>().FightTarget = null;
                startTime = 0;
                return true;
            }

            if (20 > targetHealth)
            {
                Debug.Log("Successful intimidation by force: " + gameObject.name);
                Wallet targetWalet = target.GetComponent<Wallet>();
                float stolenAmount = targetWalet.Rob();
                if (stolenAmount > 0)
                {
                    targetWalet.HasBeenMugged = true;
                    this.GetComponent<GopnikAI>().globalBalance.AddToFloatValue(stolenAmount);

                    Vector2 thisScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                    FloatingTextDisplay.Instance.SpawnFloatingText(thisScreenPos, "+" + stolenAmount.ToString("C0"));

                    completed = true;
                    this.target = null;
                    this.GetComponent<GopnikAI>().FightTarget = null;
                    startTime = 0;
                    return completed;
                }
                else
                {
                    Debug.Log("What the fuck!? This лох had no money. What a waste of a gopnik's time!");
                    startTime = 0;
                    completed = false;
                    return true;
                }
            }
            else
            {
                Debug.Log("Still got health left: " + gameObject.name);
                startTime = 0;
                return true;
            }
        }
        return true;
        */
        #endregion
    }

    //public void Punch()
    //{
    //    if (target != null)
    //    {
    //        Health targetHealthController = target.GetComponent<Health>();
    //        targetHealthController.AdjustHealth(-15f);
    //        Debug.Log("Finished punching!");
    //        this.hasAttacked = true;
    //        this.isAttacking = false;
    //        return;
    //    }

    //}

    //public void Kick()
    //{
    //    if (target != null)
    //    {
    //        Health targetHealthController = target.GetComponent<Health>();
    //        targetHealthController.AdjustHealth(-30f);
    //        Debug.Log("Finished kicking!");
    //        this.hasAttacked = true;
    //        this.isAttacking = false;
    //        return;
    //    }

    //}




    public override bool requiresInRange()
    {
        return true;
    }

    public override void reset()
    {
        this.completed = false;
        this.hasAttacked = false;
        this.isAttacking = false;
        this.startTime = 0;
    }
}
