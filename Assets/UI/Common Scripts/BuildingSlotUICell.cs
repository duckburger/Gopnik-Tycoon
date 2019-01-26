﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BuildingSlotUICell : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Image mainIcon;
    [SerializeField] Button purchaseButton;
    [SerializeField] TextMeshProUGUI buildButtonText;

    Building myBuilding;

    public SlotBuildMenu menuController;

    public void Clear()
    {
        if (this.titleText != null)
        {
            this.titleText.text = "";
        }
        if (this.priceText != null)
        {
            this.priceText.text = "";
        }
        if (this.mainIcon != null)
        {
            this.mainIcon.sprite = null;
        }
        if (this.purchaseButton != null)
        {
            this.purchaseButton.onClick.RemoveAllListeners();
        }
    }

    public void Populate(GameObject buildingToDisplay)
    {
        BuildingSlot currentSlot = this.menuController.SelectedSlot.GetComponent<BuildingSlot>();
        Building buildingData = buildingToDisplay.GetComponent<Building>();
        ScriptableFloatVar money = this.GetComponent<ScriptableFloatListener>().VarToTrack;

        if (currentSlot.CurrentBuilding != null && currentSlot.CurrentBuilding.buildingName == buildingData.buildingName)
        {
            // Turn off this slot and set it to current, because this is an already owned building
            // TODO: All the previous buildings should be marked as off as well - if that's how the progression will work
            MarkAsCurrent(buildingToDisplay, currentSlot, buildingData, money);
            return;
        }

        ActivateButton(buildingToDisplay, currentSlot, buildingData, money);
    }

    private void MarkAsCurrent(GameObject buildingToDisplay, BuildingSlot currentSlot, Building buildingData, ScriptableFloatVar money)
    {
        this.myBuilding = buildingToDisplay.GetComponent<Building>();
        if (this.titleText != null)
        {
            this.titleText.text = buildingData.buildingName;
        }
        if (this.priceText != null)
        {
            this.priceText.text = "$" + buildingData.purchasePrice.ToString();
        }
        if (this.mainIcon != null)
        {
            this.mainIcon.sprite = buildingData.mainUIImage;
        }
        if (this.purchaseButton != null)
        {
            if (this.buildButtonText != null)
            {
                this.buildButtonText.text = "owned";
            }
            this.purchaseButton.interactable = false;
        }
    }

    private void ActivateButton(GameObject buildingToDisplay, BuildingSlot currentSlot, Building buildingData, ScriptableFloatVar money)
    {
        this.myBuilding = buildingData;
        if (this.titleText != null)
        {
            this.titleText.text = buildingData.buildingName;
        }
        if (this.priceText != null)
        {
            this.priceText.text = "$" + buildingData.purchasePrice.ToString();
        }
        if (this.mainIcon != null)
        {
            this.mainIcon.sprite = buildingData.mainUIImage;
        }
        if (this.purchaseButton != null)
        {
            this.purchaseButton.onClick.AddListener(() =>
            {
                if (currentSlot.CurrentBuilding == null)
                {
                    // Spawn the prefab of the building into the slot
                    GameObject newBuilding = Instantiate(buildingToDisplay, this.menuController.SelectedSlot.transform.position, Quaternion.identity, this.menuController.SelectedSlot);
                    money.AdjustFloatValue(-this.myBuilding.purchasePrice);
                    currentSlot.CurrentBuilding = newBuilding.GetComponent<Building>();
                    
                    this.menuController.Close();
                }
                else
                {
                    Destroy(currentSlot.CurrentBuilding.gameObject);
                    currentSlot.CurrentBuilding = null;

                    GameObject newBuilding = Instantiate(buildingToDisplay, this.menuController.SelectedSlot.transform.position, Quaternion.identity, this.menuController.SelectedSlot);
                    money.AdjustFloatValue(-this.myBuilding.purchasePrice);
                    currentSlot.CurrentBuilding = newBuilding.GetComponent<Building>();
                    this.menuController.Close();
                }
                
               
            });
        }
        UpdateButtonStatus(money.value);
    }

    public void UpdateButtonStatus(float balance)
    {
        TextMeshProUGUI buttonText = this.purchaseButton.GetComponentInChildren<TextMeshProUGUI>();
        if (this.myBuilding.purchasePrice < balance)
        {
            this.purchaseButton.interactable = true;
            buttonText.text = "Purchase";
        }
        else
        {
            this.purchaseButton.interactable = false;
            buttonText.text = "Not enough money";
        }
    }
}
