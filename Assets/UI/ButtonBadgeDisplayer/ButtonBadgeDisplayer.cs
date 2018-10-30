using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class ButtonBadgeDisplayer : MonoBehaviour
{
    [SerializeField] GameObject buttonBadge;
    [SerializeField] TextMeshPro buttonText;
    [SerializeField] KeyCode actionKey;

    [SerializeField] UnityEvent actionResponder;
    [SerializeField] bool toggleAction;

    [SerializeField] List<MonoBehaviour> triggerItemList = new List<MonoBehaviour>();
    
    bool IsTriggeredByItemType(Type type)
    {
        foreach (MonoBehaviour item in triggerItemList)
        {
            if (item.GetType() == type)
            {
                return true;
            }
        }
        return false;

    }

    bool isOn = false;
    public bool IsOn
    {
        get
        {
            return this.isOn;
        }
        set
        {
            this.isOn = value;
        }
    }
    bool isActive = false;


    #region OnTriggerEvents

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.triggerItemList != null && this.triggerItemList.Count > 0 )
        {
            if (ExternalPlayerController.Instance.PlayerCarryController.CurrentItem == null)
            {
                return;
            }
            Type itemType = ExternalPlayerController.Instance.PlayerCarryController.CurrentItem.GetType();
            bool hasItem = IsTriggeredByItemType(itemType);
            bool playerNear = collision.gameObject.tag.Equals("Player");
            Debug.Log("InTriggerList: " + hasItem + " PlayerNear: " + playerNear);
            if (hasItem && playerNear)
            {
                DisplayButtonBadge();
                return;
            }
            return;
        }
        
  
        if (collision.gameObject.tag.Equals("Player"))
        {
            DisplayButtonBadge();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (this.triggerItemList == null || this.triggerItemList.Count <= 0 || ExternalPlayerController.Instance.PlayerCarryController.CurrentItem == null)
        {
            return;
        }
        if (collision.gameObject.tag.Equals("Player"))
        {
            Type itemType = ExternalPlayerController.Instance.PlayerCarryController.CurrentItem.GetType();
            bool hasItem = IsTriggeredByItemType(itemType);
            if (hasItem)
            {
                DisplayButtonBadge();
                return;
            }
            else
            {
                HideButtonBadge();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            HideButtonBadge();
        }
    }

    #endregion


    public void DisplayButtonBadge()
    {
        if (this.buttonBadge != null && !this.buttonBadge.activeSelf)
        {
            this.buttonText.text = this.actionKey.ToString();
            this.buttonBadge.SetActive(true);
            this.isActive = true;
        }
    }

    public void HideButtonBadge()
    {
        if (this.buttonBadge != null && this.buttonBadge.activeSelf)
        {
            this.buttonBadge.SetActive(false);
            this.isActive = false;
        }
    }

    private void Update()
    {
        if (this.isActive && Input.GetKeyDown(this.actionKey))
        {
            if (this.toggleAction && !this.isOn)
            {
                this.actionResponder.Invoke();
                this.isOn = true;
                return;
            }
            else if (!this.toggleAction)
            {
                this.actionResponder.Invoke();
            }
        }
    }

   
}
