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

    [Header("Shoplifting")]
    [SerializeField] float amountStolenWanted;


    public FoodQuality PreferredFoodQuality => this.preferredQuality;
    public int ItemsWanted => this.itemsWanted;
    public float Intimidation => intimidation;

    public int BuildingsDamagedWanted => this.buildingsDamagedWanted;
    public int BuildingsDestroyedWanted => this.buildingsDestroyedWanted;

    public float AmountStolenWanted => this.amountStolenWanted;

    private void Start()
    {
        // SHOPPING
        this.itemsWanted = UnityEngine.Random.Range(1, 5);
        // Randomizing the wanted food quality
        int[] values = Enum.GetValues(typeof(FoodQuality)) as int[];
        FoodQuality randomizedQuality = (FoodQuality)values[UnityEngine.Random.Range(0, values.Length)];
        this.preferredQuality = randomizedQuality;
        // CAUSING TROUBLE
        this.buildingsDamagedWanted = UnityEngine.Random.Range(1, 3);
        this.buildingsDestroyedWanted = UnityEngine.Random.Range(1, 3);
        //SHOPLIFTING
        this.amountStolenWanted = UnityEngine.Random.Range(5f, 150f);
    }



}
