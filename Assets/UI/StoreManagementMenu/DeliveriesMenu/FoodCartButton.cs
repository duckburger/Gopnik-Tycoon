using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodCartButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemCountText;

    public void UpdateCountButtonText(int newCount)
    {
        if (this.itemCountText != null)
        {
            this.itemCountText.text = newCount.ToString();
        }
    }
}
