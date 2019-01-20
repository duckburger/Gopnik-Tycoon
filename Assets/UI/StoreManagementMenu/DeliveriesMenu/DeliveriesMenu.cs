using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class UnityEventInt : UnityEvent<int> { }

[System.Serializable]
public class DeliveryFoodCart
{
    public void AddItemToCart(FoodItemData newItem)
    {
        this.foodItemsInCart.Add(newItem);
    }

    public void RemoveItemFromCart(FoodItemData itemToRemove)
    {
        if (this.foodItemsInCart.Contains(itemToRemove))
        {
            this.foodItemsInCart.Remove(itemToRemove);
        }
    }

    public int GetItemCount()
    {
        return foodItemsInCart.Count;
    }

    public List<FoodItemData> foodItemsInCart = new List<FoodItemData>();
}

public class DeliveriesMenu : MonoBehaviour
{
    public UnityEventInt onCartUpdated;
    [Space]
    public DeliverableFoodDatabase currentFoodDatabase;

    [SerializeField] CanvasGroup choiceMenuCanvasGroup;
    [SerializeField] GameObject deliveryItemPrefab;
    [SerializeField] RectTransform itemParent;

    [SerializeField] CanvasGroup cartMenuCanvasGroup;
    [SerializeField] RectTransform cartItemParent;

    public DeliveryFoodCart currentFoodDeliveryCart = new DeliveryFoodCart();

    private void OnEnable()
    {
        PopulateAvailableItems();
    }

    public void PopulateAvailableItems()
    {
        if (this.currentFoodDatabase == null || this.itemParent == null || this.deliveryItemPrefab == null)
        {
            return;
        }

        this.currentFoodDeliveryCart = new DeliveryFoodCart();
        ClearList();

        foreach (FoodItemData delFoodItem in currentFoodDatabase.deliverableItems)
        {
            GameObject newFoodItem = Instantiate(this.deliveryItemPrefab, this.itemParent);
            DeliveryItemCell cellController = newFoodItem.GetComponent<DeliveryItemCell>();
            cellController.myDeliveriesMenu = this;
            cellController?.Populate(delFoodItem);
        }
        SwitchToChoicesMenu();
    }


    public void ClearList()
    {
        if (this.itemParent != null && this.itemParent.childCount > 0)
        {
            for (int i = this.itemParent.childCount - 1; i >= 0 ; i--)
            {
                Destroy(this.itemParent.GetChild(i).gameObject);
            }
        }
    }


    #region FoodCartUpdates

    public void AddItemToCart(FoodItemData newItem)
    {
        this.currentFoodDeliveryCart.AddItemToCart(newItem);
        this.onCartUpdated.Invoke(this.currentFoodDeliveryCart.GetItemCount());
    }

    public void RemoveItemFromCar(FoodItemData itemToRemove)
    {
        this.currentFoodDeliveryCart.RemoveItemFromCart(itemToRemove);
        this.onCartUpdated.Invoke(this.currentFoodDeliveryCart.GetItemCount());
    }

    #endregion


    #region Switch to Cart

    public void SwitchToCart()
    {
        if (this.choiceMenuCanvasGroup != null)
        {
            this.choiceMenuCanvasGroup.alpha = 0;
            this.choiceMenuCanvasGroup.interactable = false;
            this.choiceMenuCanvasGroup.blocksRaycasts = false;
        }
        if (this.cartMenuCanvasGroup != null)
        {
            this.cartMenuCanvasGroup.alpha = 1;
            this.cartMenuCanvasGroup.interactable = true;
            this.cartMenuCanvasGroup.blocksRaycasts = true;
        }
    }

    public void SwitchToChoicesMenu()
    {
        if (this.choiceMenuCanvasGroup != null)
        {
            this.choiceMenuCanvasGroup.alpha = 1;
            this.choiceMenuCanvasGroup.interactable = true;
            this.choiceMenuCanvasGroup.blocksRaycasts = true;
        }
        if (this.cartMenuCanvasGroup != null)
        {
            this.cartMenuCanvasGroup.alpha = 0;
            this.cartMenuCanvasGroup.interactable = false;
            this.cartMenuCanvasGroup.blocksRaycasts = false;
        }
    }

    #endregion
}
