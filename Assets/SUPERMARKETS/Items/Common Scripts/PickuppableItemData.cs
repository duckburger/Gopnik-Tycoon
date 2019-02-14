using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PickuppableItemData : ScriptableObject
{
    [Header("Generic properties")]
    public string name;
    public bool stackable = false;
    [TextArea(3, 10)]
    public string description;
    public float pricePerUnit = 0;
    public bool Stackable => this.stackable;
}
