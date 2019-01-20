using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManagementMenu : MonoBehaviour
{
    [SerializeField] Button firstEnabledButton;



    private void Start()
    {
        this.firstEnabledButton?.onClick.Invoke();
        this.firstEnabledButton?.Select();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}

