using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuControlLayer : MonoBehaviour
{

    public static MenuControlLayer Instance;

    [SerializeField] GameObject slotBuildingMenu;
    [SerializeField] GameObject storeManagementMenu;
    bool isAMenuOpen = false;
    public bool IsAMenuOpen
    {
        get
        {
            return this.isAMenuOpen;
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void CloseAllMenus()
    {
        CloseSlotBuildingMenu();
        // Add other menus later
    }

    public void OpenSlotBuildingMenu(object slotToShow)
    {
        if (isAMenuOpen)
        {
            return;
        }
        BuildingSlotRow slotRowSent = slotToShow as BuildingSlotRow;
        if (this.slotBuildingMenu != null)
        {
            Debug.Log("Opening building slot menu!");

            this.slotBuildingMenu.SetActive(true);
            this.slotBuildingMenu.SendMessage("Populate", slotRowSent);
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
            StartCoroutine(DelayedCall(() => { ExternalPlayerController.Instance.TurnOnAllPlayerSystems(); this.isAMenuOpen = false; }, 0.4f));
        }
    }

    public void OpenStoreManagementMenu()
    {
        if (isAMenuOpen)
        {
            return;
        }
        Debug.Log("Opening the store management menu!");
        this.storeManagementMenu?.SetActive(true);
        ExternalPlayerController.Instance.TurnOffAllPlayerSystems();
        this.isAMenuOpen = true;
    }

    public void CloseStoreManagementMenu()
    {
        Debug.Log("Closing store management menu!");
        ExternalPlayerController.Instance.TurnOnAllPlayerSystems();
        this.storeManagementMenu?.SendMessage("Close", SendMessageOptions.DontRequireReceiver);
        this.isAMenuOpen = false;
    }

    IEnumerator DelayedCall(Action call, float delay)
    {
        yield return new WaitForSeconds(delay);
        call.Invoke();
    }
}
