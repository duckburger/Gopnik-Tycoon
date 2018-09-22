using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableEventListener : MonoBehaviour
{
    [SerializeField] ScriptableEvent trackedEvent;
    [Space(10)]
    [Header("Responses")]
    [SerializeField] UnityEvent raiseResponse;
    [SerializeField] UnityEvent openResponse;
    [SerializeField] UnityEvent closeResponse;

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

    public void Open()
    {
        this.openResponse.Invoke();
    }

    public void Close()
    {
        this.closeResponse.Invoke();
    }

    #endregion
}
