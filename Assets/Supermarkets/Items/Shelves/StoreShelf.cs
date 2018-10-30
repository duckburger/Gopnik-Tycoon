using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

[Serializable]
public enum ShelfCapacity
{

}

public abstract class StoreShelf : Building
{
    
    public TextMeshProUGUI stockAmtText;
    public Slider stockAmtSlider;

    public int perShelfCapacity;
    public int maxStockAmount;
    public int currentFoodStock;
    public List<Transform> shelves;

    protected FoodItem lastStockedItem = null;

    public abstract void InitializeStockAmount(); 
    

    public abstract void Restock(int amt);
    public abstract void DepleteStock();

    public virtual void UpdateStockUI()
    {
        this.stockAmtText.text = this.currentFoodStock + "/" + this.maxStockAmount;
        this.stockAmtSlider.value = this.currentFoodStock;
    }

    public abstract void UpdateShelfAppearance(bool isAdding);
}
