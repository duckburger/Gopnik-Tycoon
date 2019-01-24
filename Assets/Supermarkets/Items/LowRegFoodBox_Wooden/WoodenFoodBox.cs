using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenFoodBox : FoodContainer
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
            if (this.ContainedItem != null)
            {
                this.foodGraphic.gameObject.SetActive(true);
                this.foodGraphic.sprite = this.ContainedItem.GetRandomShelfAppearanceSprite();
            }
            else
            {
                this.foodGraphic.gameObject.SetActive(false);
            }
            
        }
    }

    public override void Populate(FoodItemData itemToLoad, int amountToLoad)
    {
        base.Populate(itemToLoad, amountToLoad);
        UpdateStockUI();
    }

    public override void Populate(List<FoodItemData> itemsToLoad)
    {
        base.Populate(itemsToLoad);
        UpdateStockUI();
    }

}
