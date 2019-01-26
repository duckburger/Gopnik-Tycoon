using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] [EnumFlag] FoodQuality preferredQuality;
    [SerializeField] int itemsWanted;
    public int ItemsWanted
    {
        get
        {
            return this.itemsWanted;
        }
    }

    public FoodQuality PreferredFoodQuality
    {
        get
        {
            return this.preferredQuality;
        }
    }

    private void Start()
    {
        this.itemsWanted = 1/*UnityEngine.Random.Range(1, 5)*/;

        // Randomizing the wanted food quality
        int[] values = Enum.GetValues(typeof(FoodQuality)) as int[];
        FoodQuality randomizedQuality = (FoodQuality)values[UnityEngine.Random.Range(0, values.Length)];
        this.preferredQuality = randomizedQuality;
    }

}
