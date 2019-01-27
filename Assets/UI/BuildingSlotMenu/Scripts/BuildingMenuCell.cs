using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingMenuCell : MonoBehaviour
{
    [SerializeField] Image bgImage;
    [SerializeField] TextMeshProUGUI cellTitle;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI priceText;

    [SerializeField] ScriptableEvent buildCellSelected;

    Building myBuilding = null;
    Color bgImageOrigColor = new Color();

    public void Populate(GameObject buildingToDisplay)
    {
        this.bgImageOrigColor = this.bgImage.color;
        Building buildingData = buildingToDisplay.GetComponent<Building>();
        this.cellTitle.text = buildingData.buildingName;
        this.icon.sprite = buildingData.mainUIImage;
        this.priceText.text = $"$ {buildingData.purchasePrice}";
        this.myBuilding = buildingToDisplay.GetComponent<Building>();
        this.Deselect();
    }

    public void Select()
    {
        this.bgImage.color = new Color(this.bgImage.color.r, this.bgImage.color.g, this.bgImage.color.b, 1f);
        if (this.buildCellSelected != null)
        {
            this.buildCellSelected.RaiseWithData((this));
            this.buildCellSelected.OpenWithData(this.myBuilding);
        }
    }

    public void Deselect()
    {
        if (this.bgImage != null)
        {
            this.bgImage.color = new Color(this.bgImage.color.r, this.bgImage.color.g, this.bgImage.color.b, 0.5f);
        }
    }
}
