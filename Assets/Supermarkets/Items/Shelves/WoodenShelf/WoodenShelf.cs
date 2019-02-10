using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenShelf : StoreShelf
{

    private void Start()
    {
        InitializeStockAmount();
        if (this.registerOnStart)
        {
            RegisterInTracker();
        }
    }

    public override void Place()
    {
        RegisterInTracker();
    }

    public void RegisterInTracker()
    {
       BuildingTracker.Instance.AddShelfToTracker(this);
    }

    private void OnDestroy()
    {
        BuildingTracker.Instance.RemoveShelfFromTracker(this);
    }


}
