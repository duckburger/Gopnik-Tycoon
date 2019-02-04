using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCharAttack : MonoBehaviour
{
    [SerializeField] Transform meleePoint;
    [SerializeField] float effectiveDistance = 1f;
    [SerializeField] float attRadius;
    [SerializeField] float attackForce;
    [SerializeField] float attackCoolDownTime = 2.3f;

    [SerializeField] List<AttackData> availableAttacks = new List<AttackData>();
    Animator myAnimator;
    Rigidbody2D myRB;

    bool canAttackAgain = true;
    bool isAttacking = false;

    WaitForSeconds attackCooldown;

    public bool IsAttacking => this.isAttacking;
    public bool CanAttackAgain => this.canAttackAgain;
    public float EffectiveDistance => this.effectiveDistance;

    private void Start()
    {
        this.myAnimator = this.GetComponent<Animator>();
        this.myRB = this.GetComponent<Rigidbody2D>();
        this.attackCooldown = new WaitForSeconds(this.attackCoolDownTime);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !this.isAttacking && this.canAttackAgain && this.gameObject.CompareTag("Player")) // Only respond to controls if this is the player
        {
            Attack();
            this.canAttackAgain = false;
        }
    }

    public void AttackExternal()
    {
        if (!this.isAttacking && this.canAttackAgain) // Only respond to controls if this is the player
        {
            Attack();
            this.canAttackAgain = false;
        }
    }

    IEnumerator StartAttackCooldown()
    {
        yield return this.attackCooldown;
        this.canAttackAgain = true;
    }

    void Attack()
    {
        this.GetComponent<MCharCarry>().DropItem();
        // Choose attack
        if (this.availableAttacks == null || this.availableAttacks.Count <= 0)
        {
            Debug.LogError(this.name + " couldn't find any attacks to use");
            return;
        }
        int attackIndex = Random.Range(0, this.availableAttacks.Count);
        AttackData attackToApply = this.availableAttacks[attackIndex];
        this.myAnimator.SetTrigger(attackToApply.animStateName);
        this.isAttacking = true;
        //this.myRB.velocity = Vector2.zero;
        StartCoroutine(AttackStateTimer());
    }

    IEnumerator AttackStateTimer()
    {
        float animDuration = this.myAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animDuration);
        this.isAttacking = false;
        StartCoroutine(StartAttackCooldown());
        Collider2D[] hits = new Collider2D[20];
        Physics2D.OverlapCircleNonAlloc(this.meleePoint.transform.position, this.attRadius, hits);
        //Debug.Log("Hit " + hits.Length + " objects");
        foreach (Collider2D collider in hits)
        {
            if (collider != null && collider.gameObject != null)
            {
                Health healthController = collider.gameObject.GetComponent<Health>();
                if (collider.gameObject != this.gameObject)
                {
                    if (healthController != null)
                    {
                        healthController.AdjustHealth(-15, true); // TODO: Deplete actual amount of health
                        Vector2 attackDir = collider.gameObject.transform.position - this.gameObject.transform.position;
                        collider.gameObject.GetComponent<Rigidbody2D>().AddForce(attackDir * this.attackForce, ForceMode2D.Impulse);
                        AnimTrigger animTrigger = collider.gameObject.GetComponent<AnimTrigger>();
                        animTrigger?.GetHit(attackDir, this.gameObject);
                    }
                }
            }
           
        }
    }

}
