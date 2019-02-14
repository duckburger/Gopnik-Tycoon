using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[Flags]
public enum FoodQuality
{
    Low = 1,
    Medium = 2,
    High = 4
}

[Serializable]
public enum FoodType
{
    None = 0,
    General,
    Fresh,
    Frozen
}


[Serializable]
[CreateAssetMenu(menuName = "Gopnik/FoodItem")]
public class FoodItemData : PickuppableItemData
{
    [Header("Food item specific")]
    [SerializeField] FoodQuality quality = FoodQuality.Low;
    [SerializeField] FoodType foodType = FoodType.General;

    [SerializeField] List<Sprite> onShelfAppearances = new List<Sprite>();


    public FoodQuality Quality => this.quality;
    public FoodType FoodType => this.foodType;
    public List<Sprite> OnShelfAppearances => this.onShelfAppearances;
    public float PricePerUnit => this.pricePerUnit;

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
