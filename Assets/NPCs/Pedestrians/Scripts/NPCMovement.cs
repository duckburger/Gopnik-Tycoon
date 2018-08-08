using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour {

    [SerializeField] float movementSpeed;

    [SerializeField] Transform target;

    bool isFreeToMove = true;
    Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (target != null)
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        if (Vector2.Distance(this.transform.position, this.target.position) > 0.5f)
        {
            Vector2 dirToTarget = this.target.transform.position - this.transform.position;
            myRigidbody.AddForce(dirToTarget * movementSpeed * Time.deltaTime);
        }
        else
        {
            myRigidbody.velocity = Vector2.zero;
        }
    }

    public void StopMovement()
    {
        isFreeToMove = false;
        myRigidbody.velocity = Vector2.zero;
    }
}
