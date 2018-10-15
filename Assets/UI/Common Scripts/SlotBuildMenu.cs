﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlotBuildMenu : Menu
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] Transform itemParent;
    [SerializeField] Button closeButton;

    Transform selectedSlot;
    public Transform SelectedSlot
    {
        get
        {
            return this.selectedSlot;
        }
    }

    [Header("Prefabs")]
    [SerializeField] GameObject itemPrefab;

    public void Populate(BuildingSlot slotToShow)
    {
        // Spawn enough buttons into the parent to show all the available buildings
        this.selectedSlot = slotToShow.transform;
        foreach (GameObject building in slotToShow.AvailableBuildings)
        {
            BuildingSlotUICell newCell = Instantiate(this.itemPrefab, this.itemParent).GetComponent<BuildingSlotUICell>();
            newCell.Populate(building.GetComponent<Building>());
        }
    }

    public override void Close()
    {
        this.titleText.text = "";
        this.gameObject.SetActive(false);
    }
}