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
        //PopulateShelfSlots();
    }

    private void PopulateShelfSlots()
    {
        myShelfItems.Clear();
        foreach (Transform child in this.transform)
        {
            SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            this.myShelfItems.Add(child.GetComponent<ShelfItemSlot>());
        }
    }

    public bool CheckIfContainsFoodQuality(FoodQuality qualityToCheck)
    {
        for (int i = 0; i < this.myShelfItems.Count; i++)
        {
            if (this.myShelfItems[i].MyItemQuality == qualityToCheck)
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

    public void Occupy(FoodItemData item)
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
