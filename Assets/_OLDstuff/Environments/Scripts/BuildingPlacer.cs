using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour {

    [SerializeField] GameObject selectedBuilding;
    [SerializeField] float minDistToCursorToPlace = 0.5f;
    [SerializeField] Transform yard;

    List<Transform> buildingPlacementPoints = new List<Transform>();
    SpriteRenderer buildingSpriteRenderer;
    Transform buildingMapParent;
    Camera mainCam;
    bool isPlacingBuilding;
    bool canPlaceNow;
    Transform mouseoverPlacementPoint;

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
                Vector2 twoDPos = mouseWorldPos;
                
                foreach (Transform placementPoint in buildingPlacementPoints)
                {
                    if (Vector2.Distance(placementPoint.position, twoDPos) < minDistToCursorToPlace)
                    {
                        // Stick the item to the point and color it
                        selectedBuilding.transform.position = placementPoint.position;
                        selectedBuilding.transform.localScale = new Vector3(1f, 1f, 1f);
                        ColorBuildingGreen();
                        canPlaceNow = true;
                        mouseoverPlacementPoint = placementPoint;
                        ClickToPlace();
                        return;
                    }
                }
                selectedBuilding.transform.position = twoDPos;
                selectedBuilding.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                ColorBuildingRed();
                canPlaceNow = false;
                mouseoverPlacementPoint = null;
                ClickToPlace();
            }
        }
	}

    #region Handling Clicks

    private void ClickToPlace()
    {
        if (Input.GetMouseButtonDown(0) && canPlaceNow)
        {
            // Place building and turn off the silhouettes on all items
            if (mouseoverPlacementPoint != null)
            {
                this.selectedBuilding.transform.parent = mouseoverPlacementPoint;
                selectedBuilding.GetComponent<IBuildingSetup>().InitialSetup();
                ResetToOccupied();
                return;
            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            // Cancel placement
            ResetToEmpty();
        }

        if (Input.GetMouseButtonDown(0) && !canPlaceNow)
        {
            // Shake to say "no"
            LeanTween.moveLocalX(selectedBuilding, selectedBuilding.transform.localPosition.x + 0.3f, 0.02f).setEase(LeanTweenType.easeInOutBounce)
                .setOnComplete(() => LeanTween.moveLocalX(selectedBuilding, selectedBuilding.transform.localPosition.x - 0.6f, 0.02f).setEase(LeanTweenType.easeInOutBounce)
                .setOnComplete(() => LeanTween.moveLocalX(selectedBuilding, selectedBuilding.transform.localPosition.x + 0.3f, 0.02f).setEase(LeanTweenType.easeInOutBounce)));
        }
        
    }

    #endregion

    #region Reset Methods

    private void ResetToEmpty()
    {
        // Cancel the placement
        isPlacingBuilding = false;
        canPlaceNow = false;
        mouseoverPlacementPoint = null;
        if (selectedBuilding != null)
        {
            Destroy(selectedBuilding);
        }
        selectedBuilding = null;
        if (buildingSpriteRenderer != null)
        {
            buildingSpriteRenderer.color = Color.white;
            buildingSpriteRenderer = null;
        }
        for (int i = buildingPlacementPoints.Count - 1; i >= 0; i--)
        {
            buildingPlacementPoints[i].GetComponent<SpriteRenderer>().enabled = false;
            buildingPlacementPoints.Remove(buildingPlacementPoints[i]);
        }
    }

    private void ResetToOccupied()
    {
        // Cancel the placement
        isPlacingBuilding = false;
        canPlaceNow = false;
        mouseoverPlacementPoint = null;
        this.selectedBuilding = null;
        buildingSpriteRenderer = null;
        buildingMapParent = null;
        for (int i = buildingPlacementPoints.Count - 1; i >= 0; i--)
        {
            buildingPlacementPoints[i].GetComponent<SpriteRenderer>().enabled = false;
            buildingPlacementPoints.Remove(buildingPlacementPoints[i]);
        }
    }

    #endregion

    #region Placing the building

    public void PrepareToPlaceBuilding(GameObject buildingToPlace)
    {
        string mapParentTag = buildingToPlace.GetComponent<BuildingData>().GetMapParentTag();
        CollectPlacementPoints(mapParentTag);

        if (buildingPlacementPoints.Count <= 0)
        {
            Debug.LogError("Couldn't find any placement points on the map, not proceeding with the placement");
            return;
        }
        if (this.selectedBuilding != null)
        {
            Debug.LogError("Already got a building in hand, not spawning another one");
            return;
        }

        Debug.Log("Placing a " + buildingToPlace.name + " in the scene");
        isPlacingBuilding = true;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        this.selectedBuilding = Instantiate(buildingToPlace, mouseWorldPos, Quaternion.identity, yard);
        buildingSpriteRenderer = selectedBuilding.GetComponent<SpriteRenderer>();
        this.selectedBuilding.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);      
    }

    #endregion

    private void CollectPlacementPoints(string mapParentTag)
    {
        if (string.IsNullOrEmpty(mapParentTag))
        {
            Debug.LogError("No map parent tag specified for this building");
            return;
        }
        buildingMapParent = GameObject.FindGameObjectWithTag(mapParentTag).transform;
        if (buildingMapParent != null)
        {
            // Find all the possible placement points on the map
            foreach (Transform child in buildingMapParent)
            {
                if (!child.GetComponent<BuildingPlacementPoint>().GetOccupancyStatus())
                {
                    buildingPlacementPoints.Add(child);
                    child.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
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
