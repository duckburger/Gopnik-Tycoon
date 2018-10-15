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

    public void Populate(Building buildingToDisplay)
    {
        if (this.titleText != null)
        {
            this.titleText.text = buildingToDisplay.buildingName;
        }
        if (this.priceText != null)
        {
            this.priceText.text = "$" + buildingToDisplay.purchasePrice.ToString();
        }
        if (this.mainIcon != null)
        {
            this.mainIcon.sprite = buildingToDisplay.mainUIImage;
        }
        if (this.purchaseButton != null)
        {
            this.purchaseButton.onClick.AddListener(() =>
            {
                // Spawn the prefab of the building into the slot
                Instantiate(buildingToDisplay.gameObject, this.menuController.SelectedSlot.transform.position, Quaternion.identity, this.menuController.SelectedSlot);
            });
        }
    }
}
