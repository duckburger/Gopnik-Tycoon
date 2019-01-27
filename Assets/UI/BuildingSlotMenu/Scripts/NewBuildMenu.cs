using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBuildMenu : MonoBehaviour
{

    [SerializeField] List<BuildingCategory> buildingCategories = new List<BuildingCategory>();
    [Header("PANELS")]
    [SerializeField] RectTransform topPanel;
    [SerializeField] RectTransform bottomPanel;
    [Header("TABS")]
    [SerializeField] Transform tabParent;
    [SerializeField] GameObject tabPrefab;
    [Space]
    [SerializeField] GameObject buildingCellPrefab;
    [SerializeField] GameObject arrowsPrefab;
    GameObject spawnedArrows = null;    

    BuildingSlotRow selectedRow = null;
    BuildingMenuCell selectedCell = null;
    Building selectedBuilding = null;
    GameObject spawnedBuildingSilhouette = null;
    Vector2 topPanelOrigPos;
    Vector2 bottomPanelOrigPos;

    #region Population

    public void Populate(BuildingSlotRow rowOfSlots)
    {
        this.selectedRow = rowOfSlots;
        AnimatePanels();
        Transform firstTab = null;
        for(int i = 0; i < this.buildingCategories.Count; i++)
        {
            BuildMenuTab newTab = Instantiate(this.tabPrefab, this.tabParent).GetComponent<BuildMenuTab>();
            if (i == 0)
            {
                firstTab = newTab.transform;
            }
            newTab.tabBackground.color = this.buildingCategories[i].categoryColor;
            newTab.mainAreaBackground.color = this.buildingCategories[i].categoryColor;
            newTab.tabTitle.text = this.buildingCategories[i].categoryName;
            SpawnCellsForCategory(this.buildingCategories[i], newTab);
        }
        firstTab.SetAsLastSibling();
    }

    void AnimatePanels()
    {
        this.topPanelOrigPos = this.topPanel.anchoredPosition;
        this.bottomPanelOrigPos = this.bottomPanel.anchoredPosition;
        this.topPanel.anchoredPosition = new Vector2(this.topPanel.anchoredPosition.x, this.topPanel.anchoredPosition.y + this.topPanel.rect.height);
        this.bottomPanel.anchoredPosition = new Vector2(this.bottomPanel.anchoredPosition.x, this.bottomPanel.anchoredPosition.y - this.bottomPanel.rect.height);
        LeanTween.moveY(this.topPanel, 0, 0.23f).setEase(LeanTweenType.easeOutExpo).setDelay(0.1f);
        LeanTween.moveY(this.bottomPanel, 0, 0.23f).setEase(LeanTweenType.easeOutExpo).setDelay(0.1f);
    }

    private void SpawnCellsForCategory(BuildingCategory category, BuildMenuTab newTab)
    {
        for (int i = 0; i < category.buildingsInMyCategory.Count; i++)
        {
            BuildingMenuCell cell = Instantiate(this.buildingCellPrefab, newTab.tabItemParent).GetComponent<BuildingMenuCell>();
            cell.Populate(category.buildingsInMyCategory[i].gameObject);
            if (i == 0)
            {
                cell.Select();
            }
            
        }
    }

    #endregion

    #region Close

    public void CloseFromButton()
    {
        MenuControlLayer.Instance.CloseSlotBuildingMenu();
    }

    public void Close()
    {
        this.selectedRow.currentHighlightedSlot.DisplayDefault();
        LeanTween.moveLocalY(this.topPanel.gameObject, this.topPanel.localPosition.y + 100f, 0.13f).setEase(LeanTweenType.easeInExpo).setDelay(0.1f);
        LeanTween.moveLocalY(this.bottomPanel.gameObject, this.bottomPanel.localPosition.y - 200f, 0.14f).setEase(LeanTweenType.easeInExpo).setDelay(0.1f)
            .setOnComplete(() => 
            {
                foreach (Transform transform in this.tabParent.transform)
                {
                    Destroy(transform.gameObject);
                }
                if (this.spawnedBuildingSilhouette != null)
                {
                    Destroy(this.spawnedBuildingSilhouette);
                }
                if (this.spawnedArrows != null)
                {
                    Destroy(this.spawnedArrows);
                }
                this.gameObject.SetActive(false);
            });
    }


    #endregion

    #region Handling Selection

    public void SetCellAsSelected(object newSelectedCell)
    {
        BuildingMenuCell newCell = newSelectedCell as BuildingMenuCell;
        if (this.selectedCell != newCell)
        {
            this.selectedCell?.Deselect();
        }
        this.selectedCell = null;
        this.selectedCell = newCell;
    }

    public void SetBuildingAsSelected(object newSelectedBuilding)
    {
        Building newBuilding = newSelectedBuilding as Building;
        this.selectedBuilding = null;
        this.selectedBuilding = newBuilding;
        if (this.spawnedBuildingSilhouette == null)
        {
            this.spawnedBuildingSilhouette = Instantiate(newBuilding.gameObject, this.selectedRow.currentHighlightedSlot.transform.position, Quaternion.identity);
        }
        else
        {
            Destroy(this.spawnedBuildingSilhouette);
            this.spawnedBuildingSilhouette = null;
            this.spawnedBuildingSilhouette = Instantiate(newBuilding.gameObject, this.selectedRow.currentHighlightedSlot.transform.position, Quaternion.identity);
        }
        this.spawnedBuildingSilhouette.GetComponent<StoreShelf>().registerInTracker = false;
        BuildingArrows arrows;
        Sprite silhouetteSprite = this.spawnedBuildingSilhouette?.GetComponent<SpriteRenderer>().sprite;
        
        if (this.spawnedArrows == null)
        {
            this.spawnedArrows = Instantiate(this.arrowsPrefab, Camera.main.WorldToScreenPoint(this.spawnedBuildingSilhouette.transform.position), Quaternion.identity);
            arrows = this.spawnedArrows.GetComponent<BuildingArrows>();
            arrows.container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, silhouetteSprite.rect.width * 2);
        }
        else
        {
            arrows = this.spawnedArrows.GetComponent<BuildingArrows>();
            this.spawnedArrows.transform.position = Camera.main.WorldToScreenPoint(this.spawnedBuildingSilhouette.transform.position);
            arrows.container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, silhouetteSprite.rect.width * 2);
        }
    }

    #endregion

    #region Build Action

    public void BuildSelectedBuilding()
    {

    }

    #endregion
}
