using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeliveryItemCell : MonoBehaviour
{

    public DeliveriesMenu myDeliveriesMenu;
    [Space]
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI qualityText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI typeText;
    [Space]
    [SerializeField] Image icon;
    [Space]
    [SerializeField] Button addToCartButton;
    [SerializeField] TextMeshProUGUI buttonText;

    FoodItemData myData = null;

    public void Populate(FoodItemData foodData)
    {
        if (foodData == null)
        {
            return;
        }

        AssignTextFields(foodData);

        if (this.icon != null)
        {
            this.icon.sprite = foodData.OnShelfAppearances?[0];
        }

        AssignButtonSettings();
        this.myData = foodData;
    }

    private void AssignButtonSettings()
    {
        if (this.addToCartButton != null)
        {
            // Assign the button function
            this.addToCartButton.onClick.RemoveAllListeners(); 
            this.addToCartButton.onClick.AddListener(() => this.myDeliveriesMenu?.AddItemToCart(this.myData));
            this.addToCartButton.interactable = true;
            
        }

        if (this.buttonText != null)
        {
            this.buttonText.text = "Add X1 to cart";
        }
    }

    private void AssignTextFields(FoodItemData foodData)
    {
        if (this.titleText != null)
        {
            this.titleText.text = foodData.name;
        }

        if (this.costText != null)
        {
            this.costText.text = "Cost: " + foodData.PricePerUnit.ToString();
        }

        if (this.qualityText != null)
        {
            this.qualityText.text =  "Quality: " + foodData.Quality.ToString();
        }

        if (this.descriptionText.text != null)
        {
            this.descriptionText.text = "Description: " + foodData.description;
        }

        if (this.typeText != null)
        {
            this.typeText.text = "Type: " + foodData.FoodType.ToString();
        }
    }
}
