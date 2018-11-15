﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

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

public class FoodItem 
{
    [Space(10)]
    [Header("UI Elements")]
    public TextMeshProUGUI stockAmtText;

    [Space(10)]
    [SerializeField] protected FoodQuality foodQuality;
    public FoodQuality FoodQuality
    {
        get
        {
            return this.foodQuality;
        }
    }
    [SerializeField] protected FoodType containedType;
    public FoodType ContainedType
    {
        get
        {
            return this.containedType;
        }
    }
    [Range(3, 25)]
    [SerializeField] protected int foodQuantity; // 3 - little, 6 - medium, 12 - large, 25 - huge(?)
    [SerializeField] protected Sprite worldFoodAppearance;

    [SerializeField] protected List<Sprite> onShelfAppearances = new List<Sprite>();
    public List<Sprite> OnShelfAppearances
    {
        get
        {
            return this.onShelfAppearances;
        }
    }

    public Sprite GetRandomShelfAppearanceSprite()
    {
        if (this.onShelfAppearances != null && this.onShelfAppearances.Count > 0)
        {
            int indexOfSprite = UnityEngine.Random.Range(0, this.onShelfAppearances.Count - 1);
            return this.onShelfAppearances[indexOfSprite];
        }
        else
        {
            return null;
        }
    }

   
}
