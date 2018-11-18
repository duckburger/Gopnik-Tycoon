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
    public List<Transform> shelves = new List<Transform>();
    public FoodType foodTypeIAccept;

    protected FoodItemData lastStockedItem = null;


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

    public bool CheckIfContainsFoodQuality(FoodQuality qualityToCheck)
    {
        for (int i = 0; i < this.shelves.Count; i++)
        {
            Shelf shelfController = this.shelves[i].GetComponent<Shelf>();
            if (shelfController != null)
            {
                if (shelfController.CheckIfContainsFoodQuality(qualityToCheck))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public virtual void ApplyCarriedItem(GameObject character)
    {
        MCharCarry carryController = character.GetComponent<MCharCarry>();
        if (carryController != null && carryController.CurrentItem != null && carryController.CurrentItem.GetType() == typeof(FoodItem))
        {
            ShowStockCount();
            FoodContainer carriedFoodItem = carryController.CurrentItem.GetComponent<FoodContainer>();
            if (this.foodTypeIAccept == carriedFoodItem.ContainedItem.FoodType)
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

    public void TakeFoodOut(GameObject character, FoodQuality preferredQuality)
    {
        MCharCarry carryController = character.GetComponent<MCharCarry>();
        AI_Generic charAIController = character.GetComponent<AI_Generic>();
        if (carryController != null && charAIController != null)
        {
            ShelfItemSlot slotWithItem = FindItemOnShelf(preferredQuality);
            carryController.PickUpFoodItem(slotWithItem.EmptyAndTakeItem());
            this.currentFoodStock--;
            UpdateStockUI();
            ShowStockCount();
        }
    }

    public ShelfItemSlot FindItemOnShelf(FoodQuality qualityToLookFor)
    {
        for (int i = 0; i < this.shelves.Count; i++)
        {        
            Shelf shelfController = shelves[i].GetComponent<Shelf>();
            if (shelfController.CheckIfContainsFoodQuality(qualityToLookFor))
            {
                ShelfItemSlot slotWithMyItem = shelfController.GetSlotWithItemQuality(qualityToLookFor);
                // Take item out and put it in the character's hands
                return slotWithMyItem;
            }
        }
        return null;
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

        if (ExternalPlayerController.Instance.PlayerCarryController.CurrentItem != null && ExternalPlayerController.Instance.PlayerCarryController.CurrentItem.GetType().BaseType == typeof(FoodContainer))
        {
            FoodContainer carriedFoodContainer = ExternalPlayerController.Instance.PlayerCarryController.CurrentItem.GetComponent<FoodContainer>();
            if (carriedFoodContainer.ContainedItem.FoodType == this.foodTypeIAccept)
            {
                int amtToStock = carriedFoodContainer.ProvideFoodStock();
                if (amtToStock == -1)
                {
                    Debug.Log("The item is empty!");
                    Vector2 floatingTextScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                    FloatingTextDisplay.SpawnFloatingText(floatingTextScreenPos, "The " + ExternalPlayerController.Instance.PlayerCarryController.CurrentItem.name + " is empty!");
                    return;
                }
                this.lastStockedItem = carriedFoodContainer.ContainedItem;
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
                    shelfController.Occupy(this.lastStockedItem);
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
