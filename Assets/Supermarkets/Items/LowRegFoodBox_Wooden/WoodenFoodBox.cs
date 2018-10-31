using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenFoodBox : FoodItem
{

    [SerializeField] SpriteRenderer foodGraphic;

    public Transform ReturnMyTransform()
    {
        return this.transform;
    }

    private void Start()
    {
        UpdateStockUI();
    }

    protected override void UpdateStockUI()
    {
        base.UpdateStockUI();
        if (this.foodQuantity <= 0 && this.foodGraphic != null)
        {
            this.foodGraphic.enabled = false;
        }
        else
        {
            this.foodGraphic.enabled = true;
            this.foodGraphic.sprite = this.worldFoodAppearance;
        }
    }


}
