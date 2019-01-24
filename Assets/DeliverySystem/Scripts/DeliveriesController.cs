using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveriesController : MonoBehaviour
{

    [SerializeField] List<DeliveryTruckSlot> truckSlots = new List<DeliveryTruckSlot>();

    [Space]
    [SerializeField] List<GameObject> truckPrefabs = new List<GameObject>();


    public void DeployDeliveryTruck(object deliveryData)
    {
        List<FoodItemData> foodInDelivery = deliveryData as List<FoodItemData>;
        if (foodInDelivery != null)
        {
            // 1) Find open slot
            // 2) Get a truck spawned and filled 
            // 3) Launch the truck
            DeliveryTruckSlot openSlot = FindOpenSlot();
            if (openSlot == null)
            {
                Debug.Log("Found no open slots in the delivery bay. Should queue this delivery?");
                return; // No open slots
            }
            // When truck finished it will destroy itself so no need to keep track of them here
            GameObject chosenTruck = this.truckPrefabs[Random.Range(0,this.truckPrefabs.Count - 1)];
            GameObject newTruck = Instantiate(chosenTruck, openSlot.transform);
            newTruck.transform.localPosition = new Vector3(-11, 0, 0);
            DeliveryTruck truck = newTruck.GetComponent<DeliveryTruck>();
            truck.currentUnloadArea = openSlot.myUnloadSpot;
            truck.AddLoad(foodInDelivery);
        }

    }

    DeliveryTruckSlot FindOpenSlot()
    {
        for (int i = 0; i < this.truckSlots.Count; i++)
        {
            if (this.truckSlots[i].transform.childCount == 1)
            {
                // Open slot
                return this.truckSlots[i];
            }
        }
        return null;
    }

}
