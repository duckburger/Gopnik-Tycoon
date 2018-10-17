using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControlLayer : MonoBehaviour
{

    MCharWalk mainCharWalkController;

    private void Awake()
    {
        mainCharWalkController = GameObject.FindGameObjectWithTag("Player").GetComponent<MCharWalk>();
    }

    [SerializeField] GameObject slotBuildingMenu;

    public void TurnOffPLayerMovement()
    {       
        mainCharWalkController.IsActive = false;
    }

    public void TurnOnPlayerMovement()
    {
        mainCharWalkController.IsActive = true;
    }



    public void OpenSlotBuildingMenu(BuildingSlot slotToShow)
    {
        if (this.slotBuildingMenu != null)
        {
            this.slotBuildingMenu.SetActive(true);
            this.slotBuildingMenu.SendMessage("Populate", slotToShow);
            TurnOffPLayerMovement();
        }
    }

    public void CloseSlotBuildingMenu()
    {
        if (this.slotBuildingMenu != null)
        {
            this.slotBuildingMenu.SendMessage("Close", SendMessageOptions.DontRequireReceiver);
            TurnOnPlayerMovement();
        }
    }

}
