using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] [EnumFlag] FoodQuality preferredQuality;
    [SerializeField] int itemsWanted;
    [SerializeField] float intimidation;

    public FoodQuality PreferredFoodQuality => this.preferredQuality;
    public int ItemsWanted => this.itemsWanted;
    public float Intimidation => intimidation;

    private void Start()
    {
        this.itemsWanted = UnityEngine.Random.Range(1, 5);

        // Randomizing the wanted food quality
        int[] values = Enum.GetValues(typeof(FoodQuality)) as int[];
        FoodQuality randomizedQuality = (FoodQuality)values[UnityEngine.Random.Range(0, values.Length)];
        this.preferredQuality = randomizedQuality;
    }



}
