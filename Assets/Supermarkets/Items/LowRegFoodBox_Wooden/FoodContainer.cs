using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodContainer : Pickuppable
{
    [Space(10)]
    [Header("UI Elements")]
    public TextMeshProUGUI stockAmtText;

    [SerializeField] FoodItemData containedItem;
    public FoodItemData ContainedItem
    {
        get
        {
            return this.containedItem;
        }
        set
        {
            this.containedItem = value;
        }
    }
    public int foodQuantity; // 3 - little, 6 - medium, 12 - large, 25 - huge(?)

    public int ProvideFoodStock()
    {
        int amtToProvide = 3;
        if (this.foodQuantity - amtToProvide < 0)
        {
            amtToProvide = 1;
        }
        if (this.foodQuantity - amtToProvide < 0)
        {
            return -1;
        }
        this.foodQuantity -= amtToProvide;
        UpdateStockUI();
        return amtToProvide;

    }

    protected virtual void UpdateStockUI()
    {
        if (this.stockAmtText != null)
        {
            this.stockAmtText.text = this.foodQuantity.ToString();
        }
    }

    public virtual void Populate(FoodItemData itemToLoad, int amountToLoad)
    {
        if (itemToLoad == null)
        {
            return;
        }
        this.containedItem = itemToLoad;
        this.foodQuantity = amountToLoad;
        UpdateStockUI();
    }

    public virtual void Populate(List<FoodItemData> itemsToLoad)
    {
        // Make sure the list passed in is of the same type
        if (itemsToLoad == null || itemsToLoad.Count <= 0)
        {
            return;
        }
        this.containedItem = itemsToLoad[0];
        this.foodQuantity = itemsToLoad.Count;
        UpdateStockUI();
    }

}
