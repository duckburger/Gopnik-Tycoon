using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Gopnik/Gopnik Spawned Event")]
public class GopnikSpawnedEvent : ScriptableObject
{
    List<GopnikSpawnedEventListener> listeners = new List<GopnikSpawnedEventListener>();

    #region Registering/DeRegistering Listeners

    public void RegisterListener(GopnikSpawnedEventListener newListener)
    {
        if (!listeners.Contains(newListener))
        {
            this.listeners.Add(newListener);
        }
    }

    public void DeRegisterListener(GopnikSpawnedEventListener toRemove)
    {
        if (listeners.Contains(toRemove))
        {
            this.listeners.Remove(toRemove);
        }
    }

    #endregion

    public void Raise(AI_CharController newGopnik)
    {
        if (listeners.Count <= 0)
        {
            Debug.LogError("Trying to raise an event - " + this.name + " - without any listeners");
            return;
        }
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Raise(newGopnik);
        }
    }
}
