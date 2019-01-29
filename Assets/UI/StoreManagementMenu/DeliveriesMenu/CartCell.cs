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

    public Button plusButton;
    public Button minusButton;

    int currentItemAmount = 0;

    public void Populate(int count, FoodItemData itemData)
    {
        this.currentItemAmount = count;
        if (this.itemCount != null)
        {
            this.itemCount.text = "x" + this.currentItemAmount;
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

    public void UpdateAmount(int adjustment)
    {
        if (this.currentItemAmount + adjustment <= 0)
        {
            LeanTween.scale(this.gameObject, new Vector3(1.05f, 1.05f, 1.05f), 0.13f).setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>LeanTween.scale(this.gameObject, Vector3.zero, 0.1f).setOnComplete(() => Destroy(this.gameObject)));
            
            return;
        }
        this.currentItemAmount += adjustment;
        if (this.itemCount != null)
        {
            this.itemCount.text = "x" + this.currentItemAmount;
        }
    }
}
