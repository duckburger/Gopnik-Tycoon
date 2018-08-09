using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShredder : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D enteredCollider)
    {
        if (enteredCollider.gameObject.layer == 8)
        {
            Destroy(enteredCollider.gameObject);
        }
    }
}
