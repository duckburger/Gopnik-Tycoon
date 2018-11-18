using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PickuppableItemData : ScriptableObject
{
    [Header("Generic properties")]
    [SerializeField] string name;
    [SerializeField] Sprite worldAppearance;
    [SerializeField] bool stackable = false;

    public Sprite WorldAppearance
    {
        get
        {
            return this.worldAppearance;
        }
    }

    public bool Stackable
    {
        get
        {
            return this.stackable;
        }
    }
}
