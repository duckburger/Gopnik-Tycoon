using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class BuildingTracker : MonoBehaviour
{ 

    public static BuildingTracker Instance;

    public List<Building> allShelves = new List<Building>();
    public List<BuildingSlotRow> allBuildingSlots = new List<BuildingSlotRow>();
    public List<Building> allCashRegisters = new List<Building>();
    public LayerMask mapRaycastMask;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void AddBuildingSlotToTracker(BuildingSlotRow newSlot)
    {
        if (!this.allBuildingSlots.Contains(newSlot))
        {
            this.allBuildingSlots.Add(newSlot);
        }
    }

    public void AddShelfToTracker(Building newShelf)
    {
       if (!this.allShelves.Contains(newShelf))
        {
            this.allShelves.Add(newShelf);
        }
    }

    public Vector2 GetRandomNearShelfLocation()
    {
        float xAdjustment = Random.Range(-2.3f, 2.3f);
        float yAdjustment = Random.Range(-2f, -1f);


        Vector2 pos = Vector2.zero;
        float choice = Random.Range(-1f, 1f);
        if (choice > 0 || this.allShelves.Count <= 0) // Choose to walk to a shelf
        {
            Debug.Log("No shelf to pick from, picking from slots instead");
            // Choose a random spot inside the store instead
            if (this.allBuildingSlots.Count > 0)
            {
                int randomSlotIndex = Random.Range(0, this.allBuildingSlots.Count - 1);
                Vector2 slotPos = this.allBuildingSlots[randomSlotIndex].transform.position;
                pos = new Vector2(slotPos.x + xAdjustment, slotPos.y + yAdjustment);
                RaycastHit2D[] hitList = Physics2D.RaycastAll(pos, Vector3.forward, 1000);
                foreach (RaycastHit2D hit in hitList)
                {
                    if (hit.collider.gameObject.GetComponent<PolyNavObstacle>() || !CheckIfPosInMainLvlMesh(hit.point))
                    {
                        // Recalculate the pos
                        Debug.Log("<color=red>Hit an obstacle</color> while raycsting at the chosen random waypoint. Choosing another one");
                        pos = new Vector2(pos.x + xAdjustment, pos.y + yAdjustment);
                        if (!CheckIfPosInMainLvlMesh(pos))
                        {
                            pos = AdjustWaypointUntilValid(pos);
                        }
                    }
                }
                if (!CheckIfPosInMainLvlMesh(pos))
                {
                    pos = AdjustWaypointUntilValid(pos);
                }
                return pos;
            }
            else
            {
                Debug.LogError("No shelves or building slots found on the map!");
                return Vector2.zero;
            }
        }
        else if (choice <= 0 && this.allShelves.Count > 0 || this.allShelves.Count > 0)
        {
            int randomShelfIndex = Random.Range(0, this.allShelves.Count - 1);
            pos = new Vector2(this.allShelves[randomShelfIndex].transform.position.x + xAdjustment, this.allShelves[randomShelfIndex].transform.position.y + yAdjustment);

            RaycastHit2D hitVar = Physics2D.Raycast(pos, Vector3.forward, 1000, mapRaycastMask.value);
            if (hitVar && hitVar.collider.gameObject.GetComponent<PolyNav.PolyNavObstacle>())
            {
                // Recalculate the pos
                pos = new Vector2(pos.x + xAdjustment, pos.y + yAdjustment);
            }
        }
        return pos;
    }

    bool CheckIfPosInMainLvlMesh(Vector2 pointToCheck)
    {
        PolyNav2D mainNavMesh = NavmeshPortalManager.Instance.mainNavMap;
        if (mainNavMesh != null)
        {
            return mainNavMesh.PointIsValid(pointToCheck);
        }
        return false;
    }

    Vector2 AdjustWaypointUntilValid(Vector2 pos)
    {
        float xAdjustment = Random.Range(-1.5f, 1.5f);
        float yAdjustment = Random.Range(-1.5f, 1.5f);
        Vector2 adjustedPos = new Vector2(pos.x + xAdjustment, pos.y + yAdjustment);
        if (NavmeshPortalManager.Instance.mainNavMap != null && NavmeshPortalManager.Instance.mainNavMap.PointIsValid(adjustedPos))
        {
            return adjustedPos;
        }
        else
        {
            float x = NavmeshPortalManager.Instance.mainNavMap.gameObject.transform.position.x;
            float y = NavmeshPortalManager.Instance.mainNavMap.gameObject.transform.position.y;
            return new Vector2(x + xAdjustment, y + yAdjustment);
        }
    }

    public StoreShelf FindShelfByFoodQuality(FoodQuality qualityToCheck)
    {
        if (this.allShelves == null || this.allShelves.Count <= 0)
        {
            return null;
        }

        List<Building> eligibleBuildings = new List<Building>();
        foreach (Building building in this.allShelves)
        {
            StoreShelf shelfController = building.gameObject.GetComponent<StoreShelf>();
            if (shelfController != null && shelfController.CheckIfContainsFoodQuality(qualityToCheck))
            {
                return shelfController;
            }
        }
        return null;
    }


    public void AddCashRegisterToTracker(Building newCashRegister)
    {
        if (!this.allCashRegisters.Contains(newCashRegister))
        {
            this.allCashRegisters.Add(newCashRegister);
        }
    }

    public CashRegisterSlot GetCashRegisterWithShortestLine() // TODO: Add a fluke possibility where this will provide just a random cash register
    {
        if (this.allCashRegisters.Count <= 0)
        {
            Debug.Log("No registered cash registers, so returning null from the building tracker");
            return null;
        }

        int shortestLine = 100;
        CashRegisterSlot registerWithShortestLine = null;
        for (int i = 0; i < this.allCashRegisters.Count; i++)
        {
            CashRegisterSlot cashSlot = this.allCashRegisters[i].GetComponentInParent<CashRegisterSlot>();
            if (cashSlot != null && cashSlot.CurrentPeopleInQueue < shortestLine)
            {
                registerWithShortestLine = cashSlot;    
            }
        }
        
        if (registerWithShortestLine != null)
        {
            return registerWithShortestLine;
        }

        Debug.LogError("Didn't find a cash register, returning null");
        return null;
    }

    public bool CheckIfThereAreCashRegisters()
    {
        if (this.allCashRegisters != null && this.allCashRegisters.Count > 0)
        {
            return true;
        }
        return false;
    }
}
