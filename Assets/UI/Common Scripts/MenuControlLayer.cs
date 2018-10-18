using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControlLayer : MonoBehaviour
{


    [SerializeField] GameObject slotBuildingMenu;


    public void OpenSlotBuildingMenu(BuildingSlot slotToShow)
    {
        if (this.slotBuildingMenu != null)
        {
            this.slotBuildingMenu.SetActive(true);
            this.slotBuildingMenu.SendMessage("Populate", slotToShow);
            ExternalPlayerController.Instance.PlayerWalkController.IsActive = false;
        }
    }

    public void CloseSlotBuildingMenu()
    {
        if (this.slotBuildingMenu != null)
        {
            this.slotBuildingMenu.SendMessage("Close", SendMessageOptions.DontRequireReceiver);
            ExternalPlayerController.Instance.PlayerWalkController.IsActive = true;
        }
    }

}
