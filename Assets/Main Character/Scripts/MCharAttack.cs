using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCharAttack : MonoBehaviour
{
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
            HandleAttack();
        }
    }

    void HandleAttack()
    {
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
    }

}
