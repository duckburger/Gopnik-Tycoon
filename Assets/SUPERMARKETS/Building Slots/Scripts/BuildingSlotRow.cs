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
            if (dist < shortestDistance)
            {
                closestSlot = this.allSlots[i];
                shortestDistance = dist;
            }
        }
        return closestSlot;
    }
}
