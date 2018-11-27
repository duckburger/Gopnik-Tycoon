using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Gopnik/FoodItem")]
public class FoodItemData : PickuppableItemData
{
    [Header("Food item specific")]
    [SerializeField] FoodQuality quality = FoodQuality.Low;
    [SerializeField] FoodType foodType = FoodType.Regular;
    [SerializeField] float pricePerUnit = 0;

  
    [SerializeField] List<Sprite> onShelfAppearances = new List<Sprite>();


    public FoodQuality Quality
    {
        get
        {
            return this.quality;
        }
    }
    public FoodType FoodType
    {
        get
        {
            return this.foodType;
        }
    }
    public List<Sprite> OnShelfAppearances
    {
        get
        {
            return this.onShelfAppearances;
        }
    }

    public float PricePerUnit
    {
        get
        {
            return this.pricePerUnit;
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
