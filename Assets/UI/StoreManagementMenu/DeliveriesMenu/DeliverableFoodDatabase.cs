using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeliverableItemsDatabase", menuName = "Gopnik/Deliverable Items Database")]
public class DeliverableFoodDatabase : ScriptableObject
{
    public List<FoodItemData> deliverableItems = new List<FoodItemData>();
}
