using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shelf : MonoBehaviour
{
    public List<ShelfItemSlot> myShelfSlots = new List<ShelfItemSlot>();
    public bool isFullyOccupied = false;

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
            if (this.myShelfSlots[i].MyItem.Quality.HasFlag(qualityToCheck))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckIfContainsAnItem()
    {
        for (int i = 0; i < this.myShelfSlots.Count; i++)
        {
            if (myShelfSlots[i].MyItem == null)
            {
                continue;
            }
            if (this.myShelfSlots[i].MyItem != null)
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

    public ShelfItemSlot GetSlotWithAnItem()
    {
        for (int i = 0; i < this.myShelfSlots.Count; i++)
        {
            if (this.myShelfSlots[i].MyItem != null)
            {
                return this.myShelfSlots[i];
            }
        }
        return null;
    }

    public int Occupy(FoodItemData item, int slotsToOccupy)
    {
        int occupiedSlots = 0;
        foreach (ShelfItemSlot shelfItemSlot in this.myShelfSlots)
        {
            if (!shelfItemSlot.IsOccupied)
            {
                shelfItemSlot.Populate(item);
                occupiedSlots++;
                if (occupiedSlots == slotsToOccupy)
                {
                    DoSlotCheck();
                    return slotsToOccupy;
                }
            }
        }
        return occupiedSlots;
    }

    public void DoSlotCheck()
    {
        bool allSlotsFull = true;
        for (int i = 0; i < this.myShelfSlots.Count; i++)
        {
            if (!this.myShelfSlots[i].IsOccupied)
            {
                allSlotsFull = false;
            }
        }
        if (allSlotsFull)
        {
            this.isFullyOccupied = true;
        }
        else
        {
            this.isFullyOccupied = false;
        }
    }

    public void Clear()
    {
        foreach (ShelfItemSlot shelfSlot in this.myShelfSlots)
        {
            shelfSlot.Clear();
        }
        this.isFullyOccupied = false;
    }
}
