using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shelf : MonoBehaviour
{
    public List<ShelfItemSlot> myShelfSlots = new List<ShelfItemSlot>();
    public bool isOccupied = false;
    

    private void Start()
    {
        PopulateShelfSlots();
    }

    private void PopulateShelfSlots()
    {
        myShelfSlots.Clear();
        foreach (Transform child in this.transform)
        {
            SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            this.myShelfSlots.Add(child.GetComponent<ShelfItemSlot>());
        }
    }

    public bool CheckIfContainsFoodQuality(FoodQuality qualityToCheck)
    {
        for (int i = 0; i < this.myShelfSlots.Count; i++)
        {
            if (myShelfSlots[i].MyItem == null)
            {
                continue;
            }
            if (this.myShelfSlots[i].MyItem.Quality == qualityToCheck)
            {
                return true;
            }
        }
        return false;
    }

    public ShelfItemSlot GetSlotWithItemQuality(FoodQuality qualityToCheck)
    {
        for (int i = 0; i < this.myShelfSlots.Count; i++)
        {
            if (this.myShelfSlots[i].MyItem == null)
            {
                continue;
            }
            if (this.myShelfSlots[i].MyItem.Quality == qualityToCheck)
            {
                return this.myShelfSlots[i];
            }
        }
        return null;
    }

    public void Occupy(FoodItemData item)
    {
        foreach (ShelfItemSlot shelfItemSlot in this.myShelfSlots)
        {
            shelfItemSlot.Populate(item);
        }
        this.isOccupied = true;
    }

    public void Clear()
    {
        foreach (ShelfItemSlot shelfSlot in this.myShelfSlots)
        {
            shelfSlot.Clear();
        }
        this.isOccupied = false;
    }
}
