using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class EventWithData : UnityEvent<object> { }

public class ScriptableEventListener : MonoBehaviour
{
    [SerializeField] ScriptableEvent trackedEvent;
    [Space(10)]
    [Header("Responses")]
    [SerializeField] UnityEvent raiseResponse;
    [SerializeField] EventWithData raisedWithData;
    [SerializeField] UnityEvent openResponse;
    [SerializeField] EventWithData openWithDataReponse;
    [SerializeField] UnityEvent closeResponse;
    [SerializeField] EventWithData closeWithDataResponse;

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

    #region Inovking Events

    public void Raise()
    {
        this.raiseResponse.Invoke();
    }

    public void RaiseWithData(object obj)
    {
        this.raisedWithData.Invoke(obj);
    }

    public void Open()
    {
        this.openResponse.Invoke();
    }

    public void Open(object o)
    {
        this.openWithDataReponse.Invoke(o);
    }

    public void Close()
    {
        this.closeResponse.Invoke();
    }

    public void Close(object o)
    {
        this.closeWithDataResponse.Invoke(o);
    }

    #endregion
}
