using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenShelf : StoreShelf
{

    private void Start()
    {
        InitializeStockAmount();
        if (this.registerInTracker)
        {   
            BuildingTracker.Instance.AddShelfToTracker(this);
        }
    }



}
