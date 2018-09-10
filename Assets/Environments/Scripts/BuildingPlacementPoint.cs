using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementPoint : MonoBehaviour {

    bool isOccupied;

	public void SetOccupied(bool status)
    {
        isOccupied = status;
    }

    public bool GetOccupancyStatus()
    {
        return isOccupied;
    }
}
