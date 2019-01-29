using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenShelf : StoreShelf
{

    private void Start()
    {
        InitializeStockAmount();
        RegisterInTracker();
    }

    public void RegisterInTracker()
    {
        if (this.registerInTracker)
        {
            BuildingTracker.Instance.AddShelfToTracker(this);
        }
    }

    private void OnDestroy()
    {
        BuildingTracker.Instance.RemoveShelfFromTracker(this);
    }


}
