using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Gopnik/FoodItem")]
public class FoodItemData : ScriptableObject
{
    [SerializeField] FoodQuality quality;
    [SerializeField] FoodType foodType;

    [SerializeField] Sprite worldAppearance;
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
