using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTracker : MonoBehaviour
{ 

    public static BuildingTracker Instance;

    public List<Building> allShelves = new List<Building>();
    public List<BuildingSlot> allBuildingSlots = new List<BuildingSlot>();
    public List<Building> allCashRegisters = new List<Building>();


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



    public void AddBuildingSlotToTracker(BuildingSlot newSlot)
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
        float xAdjustment = Random.Range(3, 3.5f);
        float yADjustment = Random.Range(-3.5f, 3);
        Vector2 pos = Vector2.zero;
        if (this.allShelves.Count <= 0)
        {
            Debug.Log("No shelve to pick from");
            // Choose a random spot inside the store instead
            if (this.allBuildingSlots.Count > 0)
            {
                int randomSlotIndex = Random.Range(0, this.allBuildingSlots.Count - 1);
                pos = new Vector2(this.allBuildingSlots[randomSlotIndex].transform.position.x + xAdjustment, this.allBuildingSlots[randomSlotIndex].transform.position.y + yADjustment);
                RaycastHit2D[] hitList = Physics2D.RaycastAll(pos, Vector3.forward, 1000);
                foreach (RaycastHit2D hit in hitList)
                {
                    if (hit.collider.gameObject.GetComponent<PolyNav.PolyNavObstacle>())
                    {
                        // Recalculate the pos
                        pos = new Vector2(pos.x + xAdjustment, pos.y + yADjustment);
                    }
                }
                //Debug.Log("Picked a spot: " + pos);
                return pos;
            }
            else
            {
                Debug.LogError("No shelves or building slots found on the map!");
                return Vector2.zero;
            }
        }

        int randomShelfIndex = Random.Range(0, this.allShelves.Count - 1);
        pos = new Vector2(this.allShelves[randomShelfIndex].transform.position.x + xAdjustment, this.allShelves[randomShelfIndex].transform.position.y + yADjustment);

        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector3.forward, 1000);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<PolyNav.PolyNavObstacle>())
            {
                // Recalculate the pos
                pos = new Vector2(pos.x + xAdjustment, pos.y + yADjustment);
            }
        }
        return pos;
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
