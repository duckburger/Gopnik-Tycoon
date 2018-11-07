using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTracker : MonoBehaviour
{ 

    public static BuildingTracker Instance;

    public List<StoreShelf> allShelves = new List<StoreShelf>();
    public List<BuildingSlot> allBuildingSlots = new List<BuildingSlot>();


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

    public void AddBuildingToTracker(StoreShelf newBuilding)
    {
       if (!this.allShelves.Contains(newBuilding))
        {
            this.allShelves.Add(newBuilding);
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
                RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector3.forward, 1000);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.GetComponent<PolyNav.PolyNavObstacle>())
                    {
                        // Recalculate the pos
                        pos = new Vector2(pos.x + xAdjustment, pos.y + yADjustment);
                    }
                }
                Debug.Log("Picked a spot: " + pos);
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
        return pos;
    }

}
