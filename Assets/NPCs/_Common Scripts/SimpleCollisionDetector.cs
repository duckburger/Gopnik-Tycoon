using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class OnFightCollisionDetected : UnityEvent<string> { }



public class SimpleCollisionDetector : MonoBehaviour
{
    [SerializeField] OnFightCollisionDetected onCollisionDetected;

    private void OnTriggerStay2D(Collider2D collision)
    {
        onCollisionDetected.Invoke(collision.gameObject.tag);
      
    }

}
