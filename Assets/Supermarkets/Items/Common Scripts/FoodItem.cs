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
            Debug.Log("On shelf appearance sprite list is empty on " + this.gameObject.name);
            return null;
        }
    }

    public int ProvideFoodStock()
    {
        int amtToProvide = 3;
        if (this.foodQuantity - amtToProvide < 0)
        {
            return -1;
        }
        this.foodQuantity -= amtToProvide;
        return amtToProvide;

    }
}
