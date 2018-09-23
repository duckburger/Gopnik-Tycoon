using UnityEngine.Events;
using UnityEngine;
using System;

[Serializable]
public class GopSpwndEvnt : UnityEvent<GopnikAI>
{
}


public class GopnikSpawnedEventListener : MonoBehaviour
{
    [SerializeField] GopnikSpawnedEvent trackedEvent;
    [Space(10)]
    [Header("Responses")]
    [SerializeField] GopSpwndEvnt raiseResponse;


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

    public void Raise(GopnikAI newGopnik)
    {
        this.raiseResponse.Invoke(newGopnik);
    }

}
