using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickuppable : MonoBehaviour
{


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<MCharCarry>().AddToNearItem(this);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<MCharCarry>().RemoveNearItem(this);
        }
    }

}
