using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ButtonBadgeDisplayer : MonoBehaviour
{
    [SerializeField] GameObject buttonBadge;
    [SerializeField] TextMeshPro buttonText;
    [SerializeField] KeyCode actionKey;

    [SerializeField] UnityEvent actionResponder;
    [SerializeField] bool toggleAction;

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

    #region OnTriggerEvents

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            DisplayButtonBadge();
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
}
