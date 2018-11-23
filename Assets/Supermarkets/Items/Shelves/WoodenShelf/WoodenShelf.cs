using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenShelf : StoreShelf
{

    private void Start()
    {
        InitializeStockAmount();
        BuildingTracker.Instance.AddShelfToTracker(this);
    }



}
