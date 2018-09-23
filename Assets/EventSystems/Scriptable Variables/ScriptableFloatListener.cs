using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class onFloatUpdatedEvent: UnityEvent<float> {}

[Serializable]
public class ScriptableFloatListener : MonoBehaviour {

    [SerializeField] ScriptableFloatVar varToTrack;
    public onFloatUpdatedEvent onFloatUpdated;

    private void OnEnable()
    {
        if (varToTrack != null)
        {
            varToTrack.RegisterANewListerner(this);
        }
    }

    private void OnDisable()
    {
        if (varToTrack != null)
        {
            varToTrack.DeRegisterAnExistingListerner(this);
        }
    }

    public void Raise(float newValue)
    {
        onFloatUpdated.Invoke(newValue);
    }
}
