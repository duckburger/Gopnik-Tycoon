using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCharAttack : MonoBehaviour
{
    [Header("Player Related")]
    [SerializeField] ScriptableEvent onPlayerAttackCooldownUpdated;
    [Space(10)]
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

    AttackData lastAppliedAttack;
    WaitForSeconds attackCooldown;

    public bool IsAttacking => this.isAttacking;
    public bool CanAttackAgain => this.canAttackAgain;
    public float EffectiveDistance => this.effectiveDistance;

    List<Collider2D> itemsHitOnThisSwing = new List<Collider2D>();

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
            this.itemsHitOnThisSwing.Clear();
            Attack();
            this.canAttackAgain = false;
        }
    }

    public void AttackExternal()
    {
        if (!this.isAttacking && this.canAttackAgain) // Only respond to controls if this is the player
        {
            this.itemsHitOnThisSwing.Clear();
            Attack();
            this.canAttackAgain = false;
        }
    }

    IEnumerator StartAttackCooldown()
    {
        float timer = this.lastAppliedAttack.cooldown;
        while (timer >= 0)
        {
            this.onPlayerAttackCooldownUpdated?.RaiseWithData(Mathf.InverseLerp(this.lastAppliedAttack.cooldown, 0, timer));
            timer -= Time.deltaTime;            
            yield return null;
        }
        this.onPlayerAttackCooldownUpdated?.RaiseWithData(1f);
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
        this.lastAppliedAttack = attackToApply;
        this.isAttacking = true;
        this.myRB.velocity = this.myRB.velocity / 2;
    }

    void AttackTrigger()
    {
        float animDuration = this.myAnimator.GetCurrentAnimatorStateInfo(0).length;
        this.isAttacking = false;
        StartCoroutine(StartAttackCooldown());
        Collider2D[] hits = new Collider2D[5];
        Physics2D.OverlapCircleNonAlloc(this.meleePoint.transform.position, this.attRadius, hits);
        Debug.Log("Hit " + hits.Length + " objects");
        foreach (Collider2D collider in hits)
        {
            if (collider != null && collider.gameObject != null)
            {
                CheckForCharacterHits(collider);
                CheckForBuildingHits(collider);
            }

        }
    }

    private void CheckForCharacterHits(Collider2D collider)
    {
        Health healthController = collider.gameObject.GetComponent<Health>();
        if (healthController == null)
        {
            return;
        }
            if (collider.gameObject != this.gameObject && !this.itemsHitOnThisSwing.Contains(collider))
        {
           
            this.itemsHitOnThisSwing.Add(collider);
            healthController.AdjustHealth(-15, true); // TODO: Deplete actual amount of health
            Vector2 attackDir = collider.gameObject.transform.position - this.gameObject.transform.position;
            collider.gameObject.GetComponent<Rigidbody2D>().AddForce(attackDir * this.attackForce, ForceMode2D.Impulse);
            AnimTrigger animTrigger = collider.gameObject.GetComponent<AnimTrigger>();
            Debug.Log("<color=magenta>Triggering</color> the hit flash");
            animTrigger?.GetHit(attackDir, this.gameObject);            
        }
    }


    private void CheckForBuildingHits(Collider2D collider)
    {
        BuildingHealth healthController = collider.gameObject.GetComponent<BuildingHealth>();
        if (healthController == null)
        {
            return;
        }
        if (collider.gameObject != this.gameObject && !this.itemsHitOnThisSwing.Contains(collider))
        {
            this.itemsHitOnThisSwing.Add(collider);
            healthController.GotHit(15);
        }
    }
}
