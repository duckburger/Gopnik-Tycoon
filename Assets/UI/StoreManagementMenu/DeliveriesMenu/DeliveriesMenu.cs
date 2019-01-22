using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

[Serializable]
public class UnityEventInt : UnityEvent<int> { }

[Serializable]
public class UnityEventFloat : UnityEvent<float> { }

[Serializable]
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

    public float GetItemCost()
    {
        float costOfCartItems = 0;
        foreach (FoodItemData foodItem in this.foodItemsInCart)
        {
            costOfCartItems += foodItem.PricePerUnit;
        }
        return costOfCartItems;
    }

    public List<FoodItemData> foodItemsInCart = new List<FoodItemData>();
}

public class DeliveriesMenu : MonoBehaviour
{

    public static DeliveriesMenu Instance;
    [Space]
    public UnityEventInt onCartCountUpdated;
    public UnityEventFloat onCartValueUpdated;
    public ScriptableEvent onCartSubmitted;
    public ModalWindowData onCartSubmittedModal;
    [Space]
    public DeliverableFoodDatabase currentFoodDatabase;

    [SerializeField] CanvasGroup choiceMenuCanvasGroup;
    [SerializeField] GameObject deliveryItemPrefab;
    [SerializeField] RectTransform itemParent;

    [SerializeField] CanvasGroup cartMenuCanvasGroup;
    [SerializeField] GameObject cartItemPrefab;
    [SerializeField] RectTransform cartItemParent;

    public DeliveryFoodCart currentFoodDeliveryCart = new DeliveryFoodCart();

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

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
        ClearChoicesList();

        foreach (FoodItemData delFoodItem in currentFoodDatabase.deliverableItems)
        {
            GameObject newFoodItem = Instantiate(this.deliveryItemPrefab, this.itemParent);
            DeliveryItemCell cellController = newFoodItem.GetComponent<DeliveryItemCell>();
            cellController.myDeliveriesMenu = this;
            cellController?.Populate(delFoodItem);
        }
        SwitchToChoicesMenu();
    }

    public void PopulateCurrentItemsInCart()
    {
        if (this.currentFoodDatabase == null || this.cartItemParent == null || this.cartItemPrefab == null)
        {
            return;
        }

        ClearCartList();
        List<FoodItemData> alreadyCheckedItemGroups = new List<FoodItemData>();

        foreach (FoodItemData item in this.currentFoodDeliveryCart.foodItemsInCart)
        {
            if (alreadyCheckedItemGroups.Contains(item))
            {
                continue;
            }
            List<FoodItemData> matchingItems = new List<FoodItemData>();
            matchingItems = this.currentFoodDeliveryCart.foodItemsInCart.FindAll(x => x.name == item.name);
            int countOfSameItem = matchingItems.Count;
            GameObject newCartItem = Instantiate(this.cartItemPrefab, this.cartItemParent);
            CartCell newCell = newCartItem?.GetComponent<CartCell>();
            alreadyCheckedItemGroups.Add(item);
            newCell.Populate(countOfSameItem, item);
        }

    }


    public void ClearChoicesList()
    {
        if (this.itemParent != null && this.itemParent.childCount > 0)
        {
            for (int i = this.itemParent.childCount - 1; i >= 0 ; i--)
            {
                Destroy(this.itemParent.GetChild(i).gameObject);
            }
        }
    }

    public void ClearCartList()
    {
        if (this.cartItemParent != null && this.cartItemParent.childCount > 0)
        {
            for (int i = this.cartItemParent.childCount - 1; i >= 0; i--)
            {
                Destroy(this.cartItemParent.GetChild(i).gameObject);
            }
        }
    }


    #region FoodCartUpdates

    public void AddItemToCart(FoodItemData newItem)
    {
        this.currentFoodDeliveryCart.AddItemToCart(newItem);
        this.onCartCountUpdated.Invoke(this.currentFoodDeliveryCart.GetItemCount());
        this.onCartValueUpdated.Invoke(this.currentFoodDeliveryCart.GetItemCost());
    }

    public void RemoveItemFromCar(FoodItemData itemToRemove)
    {
        this.currentFoodDeliveryCart.RemoveItemFromCart(itemToRemove);
        this.onCartCountUpdated.Invoke(this.currentFoodDeliveryCart.GetItemCount());
        this.onCartValueUpdated.Invoke(this.currentFoodDeliveryCart.GetItemCost());
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
        PopulateCurrentItemsInCart();
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

    #region Confirm Delivery

    public void SubmitCurrentCart()
    {
        if (this.currentFoodDeliveryCart.GetItemCount() <= 0)
        {
            return;
        }
        if (this.onCartSubmitted != null && this.onCartSubmittedModal != null)
        {
            Action newAction = () =>
            {
                this.onCartSubmitted.RaiseWithData(currentFoodDeliveryCart.foodItemsInCart);
                ClearCartList();
                ClearChoicesList();
                MenuControlLayer.Instance.CloseStoreManagementMenu();
            };
            this.onCartSubmittedModal.bodyText = $"Would you like to confirm this order for {currentFoodDeliveryCart.GetItemCount()} items at a value of ${currentFoodDeliveryCart.GetItemCost()}?";
            GlobalModal.Instance.ShowModal(newAction, null, this.onCartSubmittedModal);
        }
        
    }

    #endregion
}
