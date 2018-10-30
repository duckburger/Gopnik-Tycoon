using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControlLayer : MonoBehaviour
{

    [SerializeField] GameObject slotBuildingMenu;
    bool isAMenuOpen = false;

  

    public void CloseAllMenus()
    {
        CloseSlotBuildingMenu();
        // Add other menus later
    }

    public void OpenSlotBuildingMenu(BuildingSlot slotToShow)
    {
        if (this.slotBuildingMenu != null)
        {
            Debug.Log("Opening building slot menu!");

            this.slotBuildingMenu.SetActive(true);
            this.slotBuildingMenu.SendMessage("Populate", slotToShow);
            ExternalPlayerController.Instance.TurnOffAllPlayerSystems();
            this.isAMenuOpen = true;
        }
    }

    public void CloseSlotBuildingMenu()
    {
        if (this.slotBuildingMenu != null)
        {
            Debug.Log("Closing building slot menu!");

            this.slotBuildingMenu.SendMessage("Close", SendMessageOptions.DontRequireReceiver);
            ExternalPlayerController.Instance.TurnOnAllPlayerSystems();
            this.isAMenuOpen = false;
        }
    }

}
