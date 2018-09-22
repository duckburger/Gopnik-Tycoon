using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public static SelectionController Instance;

    [SerializeField] ScriptableEvent somethingSelected;
    [SerializeField] GameObject selectionArrow;
    [Space(10)]
    [SerializeField] UIPanel currPanel; 
   
    [SerializeField] SelectableObject selectedObj;
    public SelectableObject SelectedObj
    {
        get
        {
            return selectedObj;
        }
        set
        {
            if (value == selectedObj)
            {
                // Deselect object
                this.selectedObj = null;
                this.selectionArrow.SetActive(false);
                this.currPanel.Clear();
                this.currPanel.Close();
            }
            else
            {
                // Assign new selected obj and return
                SelectNewObj(value);
            }
        }
    }

    [Space(10)]
    [Header("Tooltips")]
    [SerializeField] CivTooltip civillianTooltip;
    // BUildingTooltip currBuildingTooltip;
    // Etc etc tooltip
   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void SelectNewObj(SelectableObject newObj)
    {
        switch (newObj.GetSelectableType())
        {
            case SelectableType.NPC:
                ICharStats charStats = newObj.GetComponent<ICharStats>();
                // Open and populate the civ tooltip
                this.currPanel = civillianTooltip;
                this.currPanel.Open();
                this.currPanel.Populate(charStats);
                break;
            case SelectableType.Building:

                break;
            case SelectableType.Item:

                break;
            default:
                break;
        }
        
        newObj.GetComponent<SelectableNPC>().isSelected = true;
        selectedObj = newObj;
        AssignArrowToNew(selectedObj.transform);
    }

    void AssignArrowToNew(Transform newObj)
    {
        if (!this.selectionArrow.activeSelf)
        {
            this.selectionArrow.SetActive(true);
        }
        this.selectionArrow.transform.parent = newObj;
        float vertOffset = (newObj.GetComponent<SpriteRenderer>().sprite.bounds.size.y * 100 / 64) / 2 + 1; // Getting the vertical size of the sprite
        this.selectionArrow.transform.localPosition = new Vector2(0, vertOffset);

        if (this.somethingSelected)
        {
            this.somethingSelected.Open();
        }
        else
        {
            Debug.LogError("No selection event attached to " + this.name);
        }
    }
   
    public void DeselectAll()
    {
        if (this.selectedObj != null)
        {
            // Clear and then go ahead
            this.selectedObj = null;
            this.selectionArrow.SetActive(false);
            this.currPanel.Clear();
            this.currPanel.Close();
            if (this.somethingSelected)
            {
                this.somethingSelected.Close();
            }
            else
            {
                Debug.LogError("No selection event attached to " + this.name);
            }
        }
    }
}
