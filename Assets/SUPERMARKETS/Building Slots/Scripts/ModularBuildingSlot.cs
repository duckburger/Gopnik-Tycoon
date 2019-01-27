using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularBuildingSlot : MonoBehaviour
{
    [SerializeField] ScriptableBuildingMenuEvent buildingMenuEvent;
    [Space(10)]
    [SerializeField] SpriteRenderer mySpriteRenderer;
    [SerializeField] Building currentBuilding = null;

    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite blockedSprite;

    public Building CurrentBuilding
    {
        get
        {
            return this.currentBuilding;
        }
        set
        {
            this.currentBuilding = value;
        }
    }

    private void Start()
    {
        //BuildingTracker.Instance.AddBuildingSlotToTracker(this);
    }

    // This will take over the screen
    public void ShowBuildingMenu()
    {
        if (this.buildingMenuEvent != null)
        {
            Debug.Log("Showing the building menu on command!");
            //this.buildingMenuEvent.Open(this);
        }
    }

    public void DisplaySelected()
    {
        this.mySpriteRenderer.sprite = this.selectedSprite;
    }

    public void DisplayDefault()
    {
        this.mySpriteRenderer.sprite = this.defaultSprite;
    }

    public void DisplayBlocked()
    {
        this.mySpriteRenderer.sprite = this.blockedSprite;
    }

}
