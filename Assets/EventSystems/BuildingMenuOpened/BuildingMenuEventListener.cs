using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class BuildingSlotUIEvent : UnityEvent<BuildingSlot> { }

public class BuildingMenuEventListener : MonoBehaviour
{
    [SerializeField] ScriptableBuildingMenuEvent trackedEvent;
    [Space(10)]
    [Header("Responses")]
    [SerializeField] BuildingSlotUIEvent raiseResponse;
    [SerializeField] BuildingSlotUIEvent openResponse;
    [SerializeField] BuildingSlotUIEvent closeResponse;

    #region Registration

    private void OnEnable()
    {
        if (this.trackedEvent != null)
        {
            this.trackedEvent.RegisterListener(this);
        }
        else
        {
            Debug.LogError("No assigned tracked event on the " + this.gameObject.name + "!");
        }
    }

    private void OnDisable()
    {
        if (this.trackedEvent != null)
        {
            this.trackedEvent.DeRegisterListener(this);
        }
        else
        {
            Debug.LogError("No assigned tracked event on the " + this.gameObject.name + "!");
        }
    }

    #endregion

    #region Invoking Events

    public void Raise(BuildingSlot slot)
    {
        this.raiseResponse.Invoke(slot);
    }

    public void Open(BuildingSlot slot)
    {
        this.openResponse.Invoke(slot);
    }

    public void Close(BuildingSlot slot)
    {
        this.closeResponse.Invoke(slot);
    }

    #endregion
}
