using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDeliveries : MonoBehaviour
{
    public Dictionary<List<FoodItemData>, int> activeDeliveries = new Dictionary<List<FoodItemData>, int>();
    int lastAddedDelivery = 0;

    public void AddActiveDelivery(object cartContents)
    {
        List<FoodItemData> list = cartContents as List<FoodItemData>;
        if (list == null || list.Count <= 0)
        {
            return;
        }
        if (!this.activeDeliveries.ContainsKey(list))
        {
            this.activeDeliveries[list] = this.lastAddedDelivery + 1;
        }
    }

    public void DeployDelivery(int index)
    {
        if (this.activeDeliveries.ContainsValue(index))
        {
            // Activate the delivery

        }
    }
}
