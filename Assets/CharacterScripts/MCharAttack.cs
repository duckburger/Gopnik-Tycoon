using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCharAttack : MonoBehaviour
{
    [SerializeField] Transform meleePoint;
    [SerializeField] float attRadius;
    [SerializeField] float attackForce;

    [SerializeField] List<AttackData> availableAttacks = new List<AttackData>();
    Animator myAnimator;
    Rigidbody2D myRB;

    bool isAttacking = false;
    public bool IsAttacking
    {
        get
        {
            return this.isAttacking;
        }
    }
    private void Start()
    {
        this.myAnimator = this.GetComponent<Animator>();
        this.myRB = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !this.isAttacking)
        {
            Attack();
        }
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
        this.myAnimator.Play(attackToApply.animStateName);
        this.isAttacking = true;
        this.myRB.velocity = Vector2.zero;
        StartCoroutine(AttackStateTimer());
    }

    IEnumerator AttackStateTimer()
    {
        float animDuration = this.myAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animDuration);
        this.isAttacking = false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(this.meleePoint.position, this.attRadius);
        Debug.Log("Hit " + hits.Length + " objects");
        foreach (Collider2D collider in hits)
        {
            Debug.Log("Hit " + collider.gameObject.name);
            Health healthController = collider.gameObject.GetComponent<Health>();
            if (collider.gameObject != this.gameObject )
            {
                if (healthController != null)
                {
                    healthController.AdjustHealth(-15, true);
                    Vector2 attackDir = collider.gameObject.transform.position - this.gameObject.transform.position;
                    collider.gameObject.GetComponent<Rigidbody2D>().AddForce(attackDir * this.attackForce, ForceMode2D.Impulse);
                }
            }
        }
    }

}
