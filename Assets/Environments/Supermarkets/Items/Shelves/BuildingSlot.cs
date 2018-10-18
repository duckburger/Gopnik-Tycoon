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

    Building currentBuilding;
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

    [SerializeField] ScriptableBuildingMenuEvent buildingMenuEvent;

    // This will take over the screen
    public void ShowBuildingMenu()
    {
        Debug.Log("Showing the building menu on command!");
        if (this.buildingMenuEvent != null)
        {
            this.buildingMenuEvent.Open(this);
        }
    }
}
