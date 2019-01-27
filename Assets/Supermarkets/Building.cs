using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingName;
    public Sprite mainUIImage;
    public int purchasePrice;

    public int slotsRequired = 1;

    public List<GameObject> listOfNearbyChars = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MCharCarry carryController = collision.gameObject.GetComponent<MCharCarry>();
        if (carryController != null)
        {
            if (!listOfNearbyChars.Contains(collision.gameObject))
            {
                this.listOfNearbyChars.Add(collision.gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        MCharCarry carryController = collision.gameObject.GetComponent<MCharCarry>();
        if (carryController != null)
        {
            if (!listOfNearbyChars.Contains(collision.gameObject))
            {
                this.listOfNearbyChars.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MCharCarry carryController = collision.gameObject.GetComponent<MCharCarry>();
        if (carryController != null)
        {
            if (listOfNearbyChars.Contains(collision.gameObject))
            {
                this.listOfNearbyChars.Remove(collision.gameObject);
            }
        }
    }

}
