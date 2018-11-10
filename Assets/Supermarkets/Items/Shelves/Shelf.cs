using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shelf : MonoBehaviour
{
    public List<ShelfItemSlot> myShelfItems = new List<ShelfItemSlot>();
    public bool isOccupied = false;
    

    private void Start()
    {
        foreach (Transform obj in this.transform)
        {
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                this.myShelfItems.Add(obj.gameObject.GetComponent<ShelfItemSlot>());
            }
        }
    }

    public bool CheckIfContainsFoodQuality(FoodQuality qualityToCheck)
    {
        foreach (ShelfItemSlot slot in this.myShelfItems)
        {
            if (slot.MyItemQuality == qualityToCheck)
            {
                return true;
            }
        }
        return false;
    }

    public ShelfItemSlot GetSlotWithItemQuality(FoodQuality qualityToCheck)
    {
        for (int i = 0; i < this.myShelfItems.Count; i++)
        {
            if (this.myShelfItems[i].MyItemQuality == qualityToCheck)
            {
                return this.myShelfItems[i];
            }
        }
        return null;
    }

    public void Occupy(FoodItem item)
    {
        foreach (ShelfItemSlot shelfItemSlot in this.myShelfItems)
        {
            shelfItemSlot.Populate(item);
        }
        this.isOccupied = true;
    }

    public void Clear()
    {
        foreach (ShelfItemSlot shelfSlot in this.myShelfItems)
        {
            shelfSlot.Clear();
        }
        this.isOccupied = false;
    }
}
