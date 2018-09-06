using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour {

    [SerializeField] GameObject selectedBuilding;
    [SerializeField] Transform yard;

    SpriteRenderer buildingSpriteRenderer;
    Camera mainCam;
    bool isPlacingBuilding;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update () {
		
        if (isPlacingBuilding)
        {
            // Use the selected building and "carry" it around with the mouse pos
            if (selectedBuilding != null)
            {
                Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 pos = mouseWorldPos;
                selectedBuilding.transform.position = pos;
                ColorBuildingRed();
            }
        }
	}

    public void PlaceBuilding(GameObject buildingToPlace)
    {
        Debug.Log("Placing a " + buildingToPlace.name + " in the scene");
        isPlacingBuilding = true;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        selectedBuilding = Instantiate(buildingToPlace, mouseWorldPos, Quaternion.identity, yard);
        buildingSpriteRenderer = selectedBuilding.GetComponent<SpriteRenderer>();
        selectedBuilding.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    void ColorBuildingRed()
    {
        Color negativeRed = new Color(1, 0, 0, 0.5f);
        if (buildingSpriteRenderer.color != negativeRed)
        {
            buildingSpriteRenderer.color = negativeRed;
        }
        
    }

    void ColorBuildingGreen()
    {
        Color positiveGreen = new Color(0, 1, 0, 0.5f);
        if (buildingSpriteRenderer.color != positiveGreen)
        {
            buildingSpriteRenderer.color = positiveGreen;
        }

    }
}
