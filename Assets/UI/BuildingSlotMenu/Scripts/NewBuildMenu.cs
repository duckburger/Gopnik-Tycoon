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
    [SerializeField] GameObject sellButtonPrefab;

    GameObject spawnedArrows = null;
    BuildingArrows spawnedArrowsScript = null;
    BuildingSlotRow selectedRow = null;
    BuildingMenuCell selectedCell = null;
    Building selectedBuilding = null;
    GameObject spawnedBuildingSilhouette = null;
    Vector2 topPanelOrigPos;
    Vector2 bottomPanelOrigPos;


    List<GameObject> spawnedSellButtons = new List<GameObject>();
    Camera mainCam = null;

    private void Awake()
    {
        this.mainCam = Camera.main;
    }

    #region Population

    public void Populate(BuildingSlotRow rowOfSlots)
    {
        this.selectedRow = rowOfSlots;
        AnimatePanels();
        Transform firstTab = null;
        for(int i = 0; i < this.selectedRow.buildingCategories.Count; i++)
        {
            BuildMenuTab newTab = Instantiate(this.tabPrefab, this.tabParent).GetComponent<BuildMenuTab>();
            if (i == 0)
            {
                firstTab = newTab.transform;
            }
            newTab.tabBackground.color = this.selectedRow.buildingCategories[i].categoryColor;
            newTab.mainAreaBackground.color = this.selectedRow.buildingCategories[i].categoryColor;
            newTab.tabTitle.text = this.selectedRow.buildingCategories[i].categoryName;
            SpawnCellsForCategory(this.selectedRow.buildingCategories[i], newTab);
        }
        firstTab.SetAsLastSibling();
        SpawnSellButtons();
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

    void SpawnSellButtons()
    {
        if (this.selectedRow == null)
        {
            return;
        }
        for (int i = 0; i < this.selectedRow.AllSlots.Count; i++)
        {
            if (this.selectedRow.AllSlots[i].CurrentBuilding != null)
            {
                GameObject newSellButton = Instantiate(this.sellButtonPrefab, this.selectedRow.AllSlots[i].transform);              
                SellButton buttonScript = newSellButton.GetComponent<SellButton>();
                buttonScript.sellButton.transform.position = this.selectedRow.AllSlots[i].transform.position + Vector3.up;
                GameObject goToDestroy = this.selectedRow.AllSlots[i].CurrentBuilding.gameObject;
                ModularBuildingSlot buildingSlot = this.selectedRow.AllSlots[i];
                this.spawnedSellButtons.Add(newSellButton);
                buttonScript.sellButton.onClick.AddListener(() => 
                {
                    buildingSlot.CurrentBuilding = null;
                    this.spawnedSellButtons.Remove(newSellButton);
                    Destroy(newSellButton);
                    Destroy(goToDestroy);
                    CheckLeftDirectionSlot();
                    CheckRightDirectionSlot();
                });
            }
        }
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
                DestroyAllSellButtons();
                this.gameObject.SetActive(false);
                CameraController.Instance.ReturnCameraToPlayer();

            });
    }

    void DestroyAllSellButtons()
    {
        for (int i = this.spawnedSellButtons.Count - 1; i >= 0; i--)
        {
            Destroy(this.spawnedSellButtons[i]);
        }
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
        StoreShelf spawnedShelf = this.spawnedBuildingSilhouette.GetComponent<StoreShelf>();
        if (spawnedShelf != null)
        {
            spawnedShelf.registerInTracker = false;
        }
        Sprite silhouetteSprite = this.spawnedBuildingSilhouette?.GetComponent<SpriteRenderer>().sprite;

        if (this.spawnedArrows == null)
        {
            this.spawnedArrows = Instantiate(this.arrowsPrefab);
            this.spawnedArrowsScript = this.spawnedArrows.GetComponent<BuildingArrows>();
            this.spawnedArrowsScript.target = this.spawnedBuildingSilhouette.transform;
            this.spawnedArrowsScript.container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, silhouetteSprite.rect.width * 3 + 88);
        }
        else
        {
            this.spawnedArrowsScript = this.spawnedArrows.GetComponent<BuildingArrows>();
            this.spawnedArrowsScript.target = this.spawnedBuildingSilhouette.transform;
            this.spawnedArrowsScript.container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, silhouetteSprite.rect.width * 3 + 88);
        }
        CheckRightDirectionSlot();
        CheckLeftDirectionSlot();
        CameraController.Instance.SetFollowObject(this.spawnedBuildingSilhouette.transform);

    }

    #endregion

    #region Build Action

    public void BuildSelectedBuilding()
    {
        if (this.spawnedBuildingSilhouette != null)
        {
            StoreShelf spawnedShelf = this.spawnedBuildingSilhouette.GetComponent<StoreShelf>();
            if (spawnedShelf != null)
            {
                spawnedShelf.registerInTracker = true;
            }
            this.spawnedBuildingSilhouette.SendMessage("RegisterInTracker", SendMessageOptions.DontRequireReceiver);
            ModularBuildingSlot slotToInstallTo = this.selectedRow.currentHighlightedSlot;
            this.spawnedBuildingSilhouette.transform.parent = slotToInstallTo.transform;
            slotToInstallTo.CurrentBuilding = this.spawnedBuildingSilhouette.GetComponent<Building>();
            this.spawnedBuildingSilhouette = null;
            MenuControlLayer.Instance.CloseSlotBuildingMenu();
        }
    }

    #endregion

    #region Arrow Functions

    public void MoveSilhouetteRight()
    {
        if (this.spawnedBuildingSilhouette == null || this.selectedRow == null)
        {
            return;
        }
        if (this.selectedRow.GetSlotToRightOfSelected() == null)
        {
            return;
        }
        this.spawnedBuildingSilhouette.transform.position = this.selectedRow.GetSlotToRightOfSelected().transform.position;
        if (this.spawnedArrowsScript != null)
        {
            this.spawnedArrowsScript.container.transform.position = this.mainCam.WorldToScreenPoint(this.spawnedBuildingSilhouette.transform.position);
            this.selectedRow.MarkSlotAsHighlighted(this.selectedRow.GetSlotToRightOfSelected());        
        }
        CheckRightDirectionSlot();
    }

    void CheckRightDirectionSlot()
    {
        if (this.selectedRow.GetSlotToRightOfSelected() == null)
        {
            this.spawnedArrowsScript.rightArrow.gameObject.SetActive(false);
        }
        else
        {
            this.spawnedArrowsScript.rightArrow.gameObject.SetActive(true);
        }
        if (this.selectedRow.GetSlotToLeftOfSelected() == null)
        {
            this.spawnedArrowsScript.leftArrow.gameObject.SetActive(false);
        }
        else
        {
            this.spawnedArrowsScript.leftArrow.gameObject.SetActive(true);
        }
    }

    public void MoveSilhouetteLeft()
    {
        if (this.spawnedBuildingSilhouette == null || this.selectedRow == null)
        {
            return;
        }
        if (this.selectedRow.GetSlotToLeftOfSelected() == null)
        {
            return;
        }
        this.spawnedBuildingSilhouette.transform.position = this.selectedRow.GetSlotToLeftOfSelected().transform.position;
        
        if (this.spawnedArrowsScript != null)
        {
            this.spawnedArrowsScript.container.transform.position = this.mainCam.WorldToScreenPoint(this.spawnedBuildingSilhouette.transform.position);
            this.selectedRow.MarkSlotAsHighlighted(this.selectedRow.GetSlotToLeftOfSelected());
            
        }
        CheckLeftDirectionSlot();
    }

    void CheckLeftDirectionSlot()
    {
        if (this.selectedRow.GetSlotToLeftOfSelected() == null)
        {
            this.spawnedArrowsScript.leftArrow.gameObject.SetActive(false);
        }
        else
        {
            this.spawnedArrowsScript.leftArrow.gameObject.SetActive(true);
        }
        if (this.selectedRow.GetSlotToRightOfSelected() == null)
        {
            this.spawnedArrowsScript.rightArrow.gameObject.SetActive(false);
        }
        else
        {
            this.spawnedArrowsScript.rightArrow.gameObject.SetActive(true);
        }
    }

    #endregion
}
