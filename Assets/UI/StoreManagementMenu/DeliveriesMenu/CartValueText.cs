using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartValueText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI cartValueText;
    // Start is called before the first frame update
    void Start()
    {
        if (this.cartValueText == null)
        {
            this.cartValueText = this.GetComponent<TextMeshProUGUI>();
        }   
    }

    public void UpdateCartValueText(float newCartCost)
    {
        if (this.cartValueText == null)
        {
            return;
        }
        string newValueText = "Cart Value: $" + Mathf.RoundToInt(newCartCost);
        this.cartValueText.text = newValueText;
    }
    
}
