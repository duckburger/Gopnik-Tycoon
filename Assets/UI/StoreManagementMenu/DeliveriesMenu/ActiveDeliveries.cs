using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDeliveries : MonoBehaviour
{
    public ScriptableEvent deliveryDeployed;
    [Space]
    public Dictionary<int, List<FoodItemData>> activeDeliveries = new Dictionary<int, List<FoodItemData>>();
    int lastAddedDelivery = 0;

    public void AddActiveDelivery(object cartContents)
    {
        List<FoodItemData> list = cartContents as List<FoodItemData>;
        if (list == null || list.Count <= 0)
        {
            return;
        }
        if (!this.activeDeliveries.ContainsValue(list))
        {
            if (this.lastAddedDelivery == 0)
            {
                this.activeDeliveries[this.lastAddedDelivery] = list;
            }
            else
            {
                this.activeDeliveries[this.lastAddedDelivery + 1] = list;
            }
        }
            
    }

    public void DeployDelivery(int index)
    {
        if (this.activeDeliveries.ContainsKey(index) && this.deliveryDeployed != null)
        {
            this.deliveryDeployed.RaiseWithData(this.activeDeliveries[index]);

        }
    }
}
