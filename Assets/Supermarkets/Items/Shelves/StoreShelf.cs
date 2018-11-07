using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;


public class StoreShelf : Building
{
    
    public TextMeshProUGUI stockAmtText;
    public Slider stockAmtSlider;

    public int perShelfCapacity;
    public int maxStockAmount;
    public int currentFoodStock;
    public List<Transform> shelves;
    public FoodType foodTypeIAccept;

    protected FoodItem lastStockedItem = null;


    public virtual void InitializeStockAmount()
    {
        if (this.shelves == null || this.shelves.Count <= 0)
        {
            return;
        }

        this.maxStockAmount = 0;
        foreach (Transform item in shelves)
        {
            this.maxStockAmount += this.perShelfCapacity;
        }
    }


    public virtual void UpdateStockUI()
    {
        this.stockAmtText.text = this.currentFoodStock + "/" + this.maxStockAmount;
        this.stockAmtSlider.value = this.currentFoodStock;
    }


    public virtual void ShowStockCount()
    {
        this.stockAmtSlider.enabled = true;
        this.stockAmtSlider.gameObject.SetActive(true);
        this.stockAmtText.enabled = true;
    }

    public virtual void HideStockCount()
    {
        this.stockAmtSlider.enabled = false;
        this.stockAmtSlider.gameObject.SetActive(false);
        this.stockAmtText.enabled = false;
    }

    public virtual void ApplyCarriedItem(GameObject character)
    {
        MCharCarry carryController = character.GetComponent<MCharCarry>();
        if (carryController != null && carryController.CurrentItem != null && carryController.CurrentItem.GetType() == typeof(FoodItem))
        {
            ShowStockCount();
            FoodItem carriedFoodItem = carryController.CurrentItem.GetComponent<FoodItem>();
            if (this.foodTypeIAccept == carriedFoodItem.ContainedType)
            {
                int amtToStock = carriedFoodItem.ProvideFoodStock();
                Restock(amtToStock);
                StopAllCoroutines();
                StartCoroutine(StockShowTimer());
            }
            else
            {
                // Do nothinjg because the type doesn't match
            }
           
        }
    }

    public virtual void ApplyPlayerCarriedItem()
    {
        if (!this.listOfNearbyChars.Contains(ExternalPlayerController.Instance.PlayerCarryController.gameObject))
        {
            return;
        }
        if (this.currentFoodStock == this.maxStockAmount)
        {
            Vector2 floatingTextScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            FloatingTextDisplay.SpawnFloatingText(floatingTextScreenPos, "The " + this.gameObject.name + " is already full.");
            return;
        }

        if (ExternalPlayerController.Instance.PlayerCarryController.CurrentItem != null && ExternalPlayerController.Instance.PlayerCarryController.CurrentItem.GetType().BaseType == typeof(FoodItem))
        {
            FoodItem carriedFoodItem = ExternalPlayerController.Instance.PlayerCarryController.CurrentItem.GetComponent<FoodItem>();
            if (carriedFoodItem.ContainedType == this.foodTypeIAccept)
            {
                int amtToStock = carriedFoodItem.ProvideFoodStock();
                if (amtToStock == -1)
                {
                    Debug.Log("The item is empty!");
                    Vector2 floatingTextScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                    FloatingTextDisplay.SpawnFloatingText(floatingTextScreenPos, "The " + ExternalPlayerController.Instance.PlayerCarryController.CurrentItem.name + " is empty!");
                    return;
                }
                this.lastStockedItem = carriedFoodItem;
                Restock(amtToStock);
            }
            else
            {
                // Do nothing because this is not a matching food type
            }
           
        }
    }

    public virtual IEnumerator StockShowTimer()
    {
        yield return new WaitForSeconds(3);
        HideStockCount();
    }

    public virtual void Restock(int restockAmount)
    {
        if (this.currentFoodStock < this.maxStockAmount)
        {
            this.currentFoodStock += restockAmount;

            if (currentFoodStock > this.maxStockAmount)
            {
                this.currentFoodStock = this.maxStockAmount;
            }
            UpdateStockUI();
            UpdateShelfAppearance(true);
            ShowStockCount();
            StopAllCoroutines();
            StartCoroutine(StockShowTimer());
        }
    }

    public virtual void DepleteStock()
    {

    }


    public virtual void UpdateShelfAppearance(bool isAdding)
    {
        if (isAdding)
        {
            for (int i = 0; i < this.shelves.Count; i++)
            {
                Shelf shelfController = shelves[i].GetComponent<Shelf>();
                if (shelfController != null && !shelfController.isOccupied)
                {
                    shelfController.Occupy(this.lastStockedItem.GetRandomShelfAppearanceSprite());
                    return;
                }
            }
        }
        else
        {
            for (int i = this.shelves.Count - 1; i >= 0; i--)
            {
                Shelf shelfController = shelves[i].GetComponent<Shelf>();
                if (shelfController != null && !shelfController.isOccupied)
                {
                    shelfController.Clear();
                    return;
                }
            }
        }

    }
}
