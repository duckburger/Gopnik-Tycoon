using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeliveryTruck : MonoBehaviour
{
    public Transform currentUnloadArea;

    [SerializeField] List<FoodItemData> deliveryLoad = new List<FoodItemData>();
    [Space]
    [SerializeField] GameObject foodBoxPrefab;
    
    public void AddLoad(List<FoodItemData> newLoad)
    {
        this.deliveryLoad.Clear();
        this.deliveryLoad = newLoad;
        // New load added, start moving towards 0
        LeanTween.moveLocalX(this.gameObject, 0, 3f).setEase(LeanTweenType.easeInOutQuart).setOnComplete(() => GenerateItems());
    }

    void GenerateItems()
    {
        if (this.deliveryLoad == null || this.deliveryLoad.Count <= 0)
        {
            return;
        }
        if (this.foodBoxPrefab == null)
        {
            Debug.LogError($"No foodbox prefab connected to the truck - {this.gameObject.name}");
            return;
        }
        List<FoodItemData> uniquesList = this.deliveryLoad.Distinct().ToList();
        for (int i = 0; i < uniquesList.Count; i++)
        {
            List<FoodItemData> listOfThisFoodType = new List<FoodItemData>();
            listOfThisFoodType = uniquesList.FindAll(x => uniquesList[i].name == x.name);
            // TODO: Make a way to deploy different types of boxes
            WoodenFoodBox newBox = Instantiate(this.foodBoxPrefab, currentUnloadArea).GetComponent<WoodenFoodBox>();
            Vector2 initialPos = new Vector2(newBox.transform.localPosition.x + Random.Range(-0.35f, 0.35f), newBox.transform.localPosition.y + Random.Range(-0.35f, 0.35f));
            newBox.transform.localPosition = initialPos;
            newBox.Populate(listOfThisFoodType);
        }
    }
    
}
