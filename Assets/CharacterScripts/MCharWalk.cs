using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCharWalk : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    Animator myAnimator;
    Rigidbody2D myRB;
    float inputX;
    float inputY;
    MCharAttack attackController;

    bool isActive = true;
    public bool IsActive
    {
      get
        {
            return this.isActive;
        }

      set
        {
            this.isActive = value;
        }
    }


    private void Start()
    {
        this.myAnimator = this.GetComponent<Animator>();
        this.myRB = this.GetComponent<Rigidbody2D>();
        this.attackController = this.GetComponent<MCharAttack>();
    }

    private void Update()
    {
        if (!this.attackController.IsAttacking && isActive)
        {
            HandleMovement();
        }
        
    }

    void HandleMovement()
    {
        this.inputX = Input.GetAxisRaw("Horizontal");
        this.inputY = Input.GetAxisRaw("Vertical");

        Vector2 newDirection = new Vector2(this.inputX, this.inputY);
        Vector2 calculatedDir = newDirection * this.movementSpeed;

        if (this.inputX > 0.1f || this.inputX < -0.1f || this.inputY > 0.1f || this.inputY < -0.1f)
        {
            this.myAnimator.Play("Walk");
            this.myAnimator.SetFloat("xInput", this.inputX);
            this.myAnimator.SetFloat("yInput", this.inputY);
        }
        else
        {
            this.myAnimator.SetTrigger("Idle");
        }


        this.myRB.velocity = calculatedDir;
    }
        

    public void Paralyze(float paralysisTime)
    {
        if (this.isActive == true)
        {
            this.myRB.velocity = Vector2.zero;
            this.myAnimator.SetTrigger("Idle");
            this.isActive = false;
            StartCoroutine(StartParalysisTimer(paralysisTime));
        }
    }


    IEnumerator StartParalysisTimer(float time)
    {
        yield return new WaitForSeconds(time);
        this.isActive = true;
    }
}
