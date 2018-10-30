using System.Collections;
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
        this.titleText.text = slotToShow.name;
        foreach (GameObject building in slotToShow.AvailableBuildings)
        {
            BuildingSlotUICell newCell = Instantiate(this.itemPrefab, this.itemParent).GetComponent<BuildingSlotUICell>();
            newCell.menuController = this;
            newCell.Populate(building);
        }
    }

    public override void Close()
    {
        ExternalPlayerController.Instance.TurnOnAllPlayerSystems();
        this.selectedSlot.GetComponentInChildren<ButtonBadgeDisplayer>().IsOn = false;
        this.selectedSlot = null;
        this.titleText.text = "";
        this.gameObject.SetActive(false);
        for (int i = itemParent.childCount - 1; i >= 0; i--)
        {
            Destroy(this.itemParent.GetChild(i).gameObject);
        }
    }
}
