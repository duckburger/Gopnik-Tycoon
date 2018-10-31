using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenGroceryShelf : StoreShelf
{
    // Only works if shelves are dragged in through the inspector
    public override void InitializeStockAmount()
    {
        if (this.shelves == null || this.shelves.Count <= 0)
        {
            return;
        }

        foreach (Transform item in shelves)
        {
            this.maxStockAmount += this.perShelfCapacity;
        }
    }

    public void ShowStockCount()
    {
        this.stockAmtSlider.enabled = true;
        this.stockAmtSlider.gameObject.SetActive(true);
        this.stockAmtText.enabled = true;
    }

    public void HideStockCount()
    {
        this.stockAmtSlider.enabled = false;
        this.stockAmtSlider.gameObject.SetActive(false);
        this.stockAmtText.enabled = false;
    }

    public void ApplyCarriedItem(GameObject character)
    {
        MCharCarry carryController = character.GetComponent<MCharCarry>();
        if (carryController != null && carryController.CurrentItem != null && carryController.CurrentItem.GetType() == typeof(FoodItem))
        {
            ShowStockCount();
            FoodItem carriedFoodItem = carryController.CurrentItem.GetComponent<FoodItem>();
            int amtToStock = carriedFoodItem.ProvideFoodStock();
            Restock(amtToStock);
            StopAllCoroutines();
            StartCoroutine(StockShowTimer());
        }
    }

    public void ApplyPlayerCarriedItem()
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
    }

    IEnumerator StockShowTimer()
    {
        yield return new WaitForSeconds(3);
        HideStockCount();
    }

    public override void Restock(int restockAmount)
    {
        if (this.currentFoodStock < this.maxStockAmount)
        {
            this.currentFoodStock += restockAmount;

            if (currentFoodStock > this.maxStockAmount)
            {
                this.currentFoodStock = this.maxStockAmount;
            }
            base.UpdateStockUI();
            UpdateShelfAppearance(true);
            ShowStockCount();
            StopAllCoroutines();
            StartCoroutine(StockShowTimer());
        }
    }

    public override void DepleteStock()
    {

    }


    public override void UpdateShelfAppearance(bool isAdding)
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
