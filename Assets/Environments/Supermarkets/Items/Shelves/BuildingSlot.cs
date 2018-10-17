using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class BuildingSlot : MonoBehaviour
{
    [SerializeField] List<GameObject> availableBuildings = new List<GameObject>();
    public List<GameObject> AvailableBuildings
    {
        get
        {
            return this.availableBuildings;
        }
    }

    bool isOccupied = false;
    public bool IsOccupied
    {
        get
        {
            return this.isOccupied;
        }
        set
        {
            this.isOccupied = value;
        }
    }

    [SerializeField] ScriptableBuildingMenuEvent buildingMenuEvent;

    // This will take over the screen
    public void ShowBuildingMenu()
    {
        if (this.isOccupied)
        {
            return;
        }
        Debug.Log("Showing the building menu on command!");
        if (this.buildingMenuEvent != null)
        {
            this.buildingMenuEvent.Open(this);
        }
    }
}
