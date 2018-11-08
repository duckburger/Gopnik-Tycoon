using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfItemSlot : MonoBehaviour
{

    [SerializeField] SpriteRenderer mySpriteRenderer;

    FoodQuality myItemQuality = FoodQuality.None;
    public FoodQuality MyItemQuality
    {
        get
        {
            return this.myItemQuality;
        }
    }

    bool isOccupied = false;
    public bool IsOccupied
    {
        get
        {
            return this.isOccupied;
        }
    }

    private void Start()
    {
        this.mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Populate(FoodItem item)
    {
        this.mySpriteRenderer.sprite = item.GetRandomShelfAppearanceSprite();
        this.myItemQuality = item.ContainedQuality;
        this.isOccupied = true;
    }

    public void Clear() { }
}
