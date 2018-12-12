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
    }
    [SerializeField] protected int foodQuantity; // 3 - little, 6 - medium, 12 - large, 25 - huge(?)

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

}
