using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSlotRow : MonoBehaviour
{
    [SerializeField] ScriptableEvent newBuildMenuOpenedEvent;
    [Space(10)]
    [SerializeField] List<BuildingCategory> buildingCategories = new List<BuildingCategory>();
    [SerializeField] List<ModularBuildingSlot> allSlots = new List<ModularBuildingSlot>();
    [Space(10)]
    GameObject spawnedArrows = null;

    public ModularBuildingSlot currentHighlightedSlot = null;

    #region StartUp

    // Start is called before the first frame update
    void Start()
    {
        GatherSlots();
    }

    void GatherSlots()
    {
        if (this.transform.childCount > 0)
        {
            foreach (Transform transform in this.transform)
            {
                ModularBuildingSlot foundSlot = transform.GetComponent<ModularBuildingSlot>();
                if (foundSlot != null)
                {
                    this.allSlots.Add(foundSlot);
                }
            }
        }
    }

    #endregion


    public void MarkSlotAsHighlighted(ModularBuildingSlot newSlot)
    {
        if (this.allSlots.Contains(newSlot))
        {
            this.currentHighlightedSlot?.DisplayDefault();
            this.currentHighlightedSlot = newSlot;
            this.currentHighlightedSlot.DisplaySelected();
        }
    }

    public void ShowBuildingMode()
    {
        ModularBuildingSlot closestSlotToPlayer = FindSlotClosestToPlayer();
        closestSlotToPlayer.DisplaySelected();
        this.currentHighlightedSlot = closestSlotToPlayer;
        if (this.newBuildMenuOpenedEvent != null)
        {
            this.newBuildMenuOpenedEvent.RaiseWithData(this);
        }
        
    }

    ModularBuildingSlot FindSlotClosestToPlayer()
    {
        ModularBuildingSlot closestSlot = null;
        float shortestDistance = Mathf.Infinity;
        for (int i = 0; i < this.allSlots.Count; i++)
        {
            float dist = Vector2.Distance(ExternalPlayerController.Instance.PlayerTransform.position, this.allSlots[i].transform.position);
            if (dist < shortestDistance && this.allSlots[i].CurrentBuilding == null)
            {
                closestSlot = this.allSlots[i];
                shortestDistance = dist;
            }
        }
        return closestSlot;
    }

    #region Providing Slots to Menu

    public ModularBuildingSlot GetSlotToRightOfSelected()
    {
        if (this.currentHighlightedSlot == null)
        {
            Debug.Log("Trying to request a slot to the right with no current slot selected");
            return null;
        }
        int selectedIndex = this.allSlots.IndexOf(currentHighlightedSlot);
        if (selectedIndex + 1 > this.allSlots.Count - 1)
        {
            Debug.Log("No more slots to the right, returning null");
            return null;
        }
        return ReturnFreeRightSlot(selectedIndex, 1);
    }

    ModularBuildingSlot ReturnFreeRightSlot(int selectedIndex, int i)
    {
        if (this.allSlots[selectedIndex + i].CurrentBuilding == null)
        {
            return this.allSlots[selectedIndex + i];
        }
        else if (selectedIndex + i + 1 < this.allSlots.Count)
        {
            return ReturnFreeRightSlot(selectedIndex, i + 1);
        }
        else
        {
            return null;
        }
    }

    public ModularBuildingSlot GetSlotToLeftOfSelected()
    {
        if (this.currentHighlightedSlot == null)
        {
            Debug.Log("Trying to request a slot to the left with no current slot selected");
            return null;
        }
        int selectedIndex = this.allSlots.IndexOf(currentHighlightedSlot);
        if (selectedIndex - 1 < 0)
        {
            Debug.Log("No more slots to the left, returning null");
            return null;
        }
        return ReturnFreeLeftSlot(selectedIndex, 1);
    }

    ModularBuildingSlot ReturnFreeLeftSlot(int selectedIndex, int i)
    {
        if (this.allSlots[selectedIndex - i].CurrentBuilding == null)
        {
            return this.allSlots[selectedIndex - i];
        }
        else if (selectedIndex - (i + 1) >= 0)
        {
            return ReturnFreeLeftSlot(selectedIndex, i + 1);
        }
        else
        {
            return null;
        }
    }

    #endregion
}
