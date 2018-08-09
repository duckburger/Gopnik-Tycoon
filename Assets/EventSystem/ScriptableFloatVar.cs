using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu (menuName = "Gopnik/ScriptableFloatVar")]
public class ScriptableFloatVar : ScriptableObject 
{
    public float defaultValue;
    public float value;
    public List<ScriptableFloatListener> myListeners = new List<ScriptableFloatListener>();

    #region Adding/Remove Listeners

    public void RegisterANewListerner(ScriptableFloatListener newListener)
    {
        if (!myListeners.Contains(newListener))
        {
            myListeners.Add(newListener);
        }
    }

    public void DeRegisterAnExistingListerner(ScriptableFloatListener listenerToRemove)
    {
        if (myListeners.Contains(listenerToRemove))
        {
            myListeners.Remove(listenerToRemove);
        }
    }

    #endregion

    #region Modifying Value
    public void AddToFloatValue(float adjustment)
    {
        value += adjustment;
        SendMessageToAllListeners();
    }
   
    public void Reset()
    {
        this.value = this.defaultValue;
    }

    #endregion

    #region Sending Messages to Listeners
    void SendMessageToAllListeners()
    {
        for (int i = myListeners.Count - 1; i >= 0; i--)
        {
            myListeners[i].Raise(value);
        }
    }

    #endregion

}
