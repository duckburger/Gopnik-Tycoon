using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingSlotUICell : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Image mainIcon;
    [SerializeField] Button purchaseButton;

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
        
        Building buildingScript = buildingToDisplay.GetComponent<Building>();
        ScriptableFloatVar money = this.GetComponent<ScriptableFloatListener>().VarToTrack;
        this.myBuilding = buildingScript;
        if (this.titleText != null)
        {
            this.titleText.text = buildingScript.buildingName;
        }
        if (this.priceText != null)
        {
            this.priceText.text = "$" + buildingScript.purchasePrice.ToString();
        }
        if (this.mainIcon != null)
        {
            this.mainIcon.sprite = buildingScript.mainUIImage;
        }
        if (this.purchaseButton != null)
        {
            this.purchaseButton.onClick.AddListener(() =>
            {
                // Spawn the prefab of the building into the slot
                Instantiate(buildingToDisplay, this.menuController.SelectedSlot.transform.position, Quaternion.identity, this.menuController.SelectedSlot);
                money.AddToFloatValue(-this.myBuilding.purchasePrice);
                this.menuController.SelectedSlot.GetComponent<BuildingSlot>().IsOccupied = true;
                this.menuController.Close();
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
