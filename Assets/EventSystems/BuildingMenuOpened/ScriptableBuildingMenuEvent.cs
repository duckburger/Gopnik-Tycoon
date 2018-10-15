using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Gopnik/Scriptable Building Menu Event")]
public class ScriptableBuildingMenuEvent : ScriptableObject
{
    List<BuildingMenuEventListener> listeners = new List<BuildingMenuEventListener>();

    #region Raising Events

    public void Raise(BuildingSlot slot)
    {
        if (listeners.Count <= 0)
        {
            Debug.LogError("Trying to raise an event - " + this.name + " - without any listeners");
            return;
        }
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Raise(slot);
        }
    }

    public void Open(BuildingSlot slot)
    {
        if (listeners.Count <= 0)
        {
            Debug.LogError("Trying to raise an event - " + this.name + " - without any listeners");
            return;
        }
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Open(slot);
        }
    }

    public void Close(BuildingSlot slot)
    {
        if (listeners.Count <= 0)
        {
            Debug.LogError("Trying to raise an event - " + this.name + " - without any listeners");
            return;
        }
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Close(slot);
        }
    }

    #endregion

    #region Registering/DeRegistering Listeners

    public void RegisterListener(BuildingMenuEventListener newListener)
    {
        if (!listeners.Contains(newListener))
        {
            this.listeners.Add(newListener);
        }
    }

    public void DeRegisterListener(BuildingMenuEventListener toRemove)
    {
        if (listeners.Contains(toRemove))
        {
            this.listeners.Remove(toRemove);
        }
    }

    #endregion
}
