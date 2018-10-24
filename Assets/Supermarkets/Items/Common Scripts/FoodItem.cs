using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum FoodQuality
{
    None = 0,
    Low,
    Medium,
    High
}

[Serializable]
public enum FoodType
{
    None = 0,
    Regular, 
    Fresh
}

public class FoodItem : Pickuppable
{
    [SerializeField] protected FoodQuality containedQuality;
    [SerializeField] protected FoodType containedType;
    [Range(3, 25)]
    [SerializeField] protected int foodQuantity; // 3 - little, 6 - medium, 12 - large, 25 - huge(?)

}
