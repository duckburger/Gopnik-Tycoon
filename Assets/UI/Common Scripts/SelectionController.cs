using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public static SelectionController Instance;

    [SerializeField] GameObject displayedPanel; // Replace with IPanel
    [SerializeField] GameObject selectedObj;
    [Space(10)]
    [Header("Tooltips")]
    [SerializeField] CivTooltip civillianTooltip;
    public GameObject SelectedObj
    {
        get
        {
            return selectedObj;
        }
        set
        {
            if (value == selectedObj)
            {
                selectedObj = null;
                // Deselect object
            }
            else
            {
                // Assign new selected obj and return
                SelectNewObj(value);
            }

        }
    }

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

    GameObject SelectNewObj(GameObject newObj)
    {
        if (selectedObj)
        {
            // displayedPanel.Close();
            selectedObj = null;
        }
        // Decide what type of object is being selected and whether there is an object already selected
        ICharStats charStats = newObj.GetComponent<ICharStats>();
        if (charStats != null)
        {
            // Open and populate the civ tooltip
            civillianTooltip.Open();
            civillianTooltip.Populate(charStats);
        }
        selectedObj = newObj;
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
