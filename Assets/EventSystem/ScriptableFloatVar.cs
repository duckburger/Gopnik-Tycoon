using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ScriptableFloatVar : ScriptableObject 
{
    public float defaultValue;
    public float value;

    public void Reset()
    {
        this.value = this.defaultValue;
    }
}
