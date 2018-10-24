using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenFoodBox : FoodItem
{

    [SerializeField] Transform foodSpawnSpot;

    public Transform ReturnMyTransform()
    {
        return this.transform;
    }

    
}
