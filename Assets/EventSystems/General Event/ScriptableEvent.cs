using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Gopnik/Scriptable Event")]
public class ScriptableEvent : ScriptableObject
{
    List<ScriptableEventListener> listeners = new List<ScriptableEventListener>();

    #region Raising Events

    public void Raise()
    {
        if (listeners.Count <= 0)
        {
            Debug.LogError("Trying to raise an event - " + this.name + " - without any listeners");
            return;
        }
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Raise();
        }
    }

    public void RaiseWithData(object obj)
    {
        if (listeners.Count <= 0)
        {
            Debug.LogError("Trying to raise an event - " + this.name + " - without any listeners");
            return;
        }
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].RaiseWithData(obj);
        }
    }

    public void Open()
    {
        if (listeners.Count <= 0)
        {
            Debug.LogError("Trying to raise an event - " + this.name + " - without any listeners");
            return;
        }
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Open();
        }
    }

    public void OpenWithData(object o)
    {
        if (listeners.Count <= 0)
        {
            Debug.LogError("Trying to raise an event - " + this.name + " - without any listeners");
            return;
        }
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Open(o);
        }
    }


    public void Close()
    {
        if (listeners.Count <= 0)
        {
            Debug.LogError("Trying to raise an event - " + this.name + " - without any listeners");
            return;
        }
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Close();
        }
    }

    public void CloseWithData(object o)
    {
        if (listeners.Count <= 0)
        {
            //Debug.LogError("Trying to raise an event - " + this.name + " - without any listeners");
            return;
        }
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Close(o);
        }
    }


    #endregion

    #region Registering/DeRegistering Listeners

    public void RegisterListener(ScriptableEventListener newListener)
    {
        if (!listeners.Contains(newListener))
        {
            this.listeners.Add(newListener);
        }
    }

    public void DeRegisterListener(ScriptableEventListener toRemove)
    {
        if (listeners.Contains(toRemove))
        {
            this.listeners.Remove(toRemove);
        }
    }

    #endregion
}
