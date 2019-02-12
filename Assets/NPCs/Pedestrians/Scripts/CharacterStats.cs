using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    [Header("Shopping")]
    [SerializeField] [EnumFlag] FoodQuality preferredQuality;
    [SerializeField] int itemsWanted;
    [SerializeField] float intimidation;

    [Header("Causing Trouble")]
    [SerializeField] int buildingsDamagedWanted;
    [SerializeField] int buildingsDestroyedWanted;
    [SerializeField] int peopleHurtWanted;


    public FoodQuality PreferredFoodQuality => this.preferredQuality;
    public int ItemsWanted => this.itemsWanted;
    public float Intimidation => intimidation;

    public int BuildingsDamagedWanted => this.buildingsDamagedWanted;
    public int BuildingsDestroyedWanted => this.buildingsDestroyedWanted;

    private void Start()
    {
        this.itemsWanted = UnityEngine.Random.Range(1, 5);
        // Randomizing the wanted food quality
        int[] values = Enum.GetValues(typeof(FoodQuality)) as int[];
        FoodQuality randomizedQuality = (FoodQuality)values[UnityEngine.Random.Range(0, values.Length)];
        this.preferredQuality = randomizedQuality;

        this.buildingsDamagedWanted = UnityEngine.Random.Range(1, 3);
        this.buildingsDestroyedWanted = UnityEngine.Random.Range(1, 3);
    }



}
