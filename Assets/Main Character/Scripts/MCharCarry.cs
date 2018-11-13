using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCharCarry : MonoBehaviour
{
    [SerializeField] Transform carryTransform;
    [SerializeField] Pickuppable currentItem;

    bool isPlayer = false;
    public Pickuppable CurrentItem
    {
        get
        {
            return this.currentItem;
        }
    }

    List<Pickuppable> nearItems = new List<Pickuppable>();


    private void Start()
    {
        if (this.gameObject.tag == "Player")
        {
            this.isPlayer = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public void AddToNearItem(Pickuppable newItem)
    {
        if (!this.nearItems.Contains(newItem))
        {
            this.nearItems.Add(newItem);
        }
    }

    public void RemoveNearItem(Pickuppable itemToRemove)
    {
        if (this.nearItems.Contains(itemToRemove))
        {
            this.nearItems.Remove(itemToRemove);
        }
    }


    Pickuppable FindNearestItem()
    {
        float nearestDist = Mathf.Infinity;
        int index = 0;
        for (int i = 0; i < nearItems.Count; i++)
        {
            float distTo = Vector2.Distance(this.transform.position, nearItems[i].transform.position);
            if (distTo < nearestDist)
            {
                nearestDist = distTo;
                index = i;
            }
        }
        return nearItems[index];
    }

    private void Update()
    {
        if (this.isPlayer && Input.GetKeyDown(KeyCode.E))
        {
            PickUpNearestItem(); 
        }
        if (this.isPlayer && ExternalPlayerController.Instance.PlayerCarryController.currentItem != null && Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }

    }


    public void PickUpNearestItem()
    {
        if (this.currentItem != null)
        {
            DropItem();
        }
        if (this.nearItems.Count <= 0)
        {
            Debug.Log("No pickuppable items near the player");
            return;
        }
        Debug.Log("Picking up an item");
        Pickuppable itemToPickup = FindNearestItem();
        

        itemToPickup.transform.parent = this.carryTransform;
        this.currentItem = itemToPickup;
        this.currentItem.transform.localPosition = Vector2.zero;
    }

    public void PickUpSpecificItem(Pickuppable itemToPickUp)
    {
        if (itemToPickUp == null)
        {
            Debug.Log("No pickuppable item specified");
            return;
        }
        if (this.currentItem != null)
        {
            DropItem();
        }
           
        itemToPickUp.transform.parent = this.carryTransform;
        this.currentItem = itemToPickUp;
        this.currentItem.transform.localPosition = Vector2.zero;
    }
    

    public void DropItem()
    {
        if (this.currentItem != null)
        {
            this.currentItem.transform.parent = LevelData.CurrentLevel.Floor;
            this.currentItem.transform.position = this.transform.position;
            this.currentItem = null;
        }
    }

}
