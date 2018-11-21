using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PickuppableItemData : ScriptableObject
{
    [Header("Generic properties")]
    [SerializeField] string name;
    [SerializeField] bool stackable = false;


    public bool Stackable
    {
        get
        {
            return this.stackable;
        }
    }
}
