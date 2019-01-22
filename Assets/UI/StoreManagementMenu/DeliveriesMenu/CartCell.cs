using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CartCell : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemCount;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image itemIcon;

    public void Populate(int count, FoodItemData itemData)
    {
        if (this.itemCount != null)
        {
            this.itemCount.text = "x" + count;
        }
        if (this.title != null)
        {
            this.title.text = itemData.name;
        }
        if (this.itemIcon != null)
        {
            this.itemIcon.sprite = itemData.OnShelfAppearances?[0];
        }
    }
}
