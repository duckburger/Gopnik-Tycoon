using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class NavmeshPortalManager : MonoBehaviour
{
    public static NavmeshPortalManager Instance;

    
    [SerializeField] List<NavMeshPortal> allNavPortals = new List<NavMeshPortal>();
    public List<NavMeshPortal> AllNavPortals
    {
        get
        {
            return this.allNavPortals;
        }
    }
    public PolyNav2D mainNavMap;
    [SerializeField] List<PolyNav2D> allNavMapsInScene = new List<PolyNav2D>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void AddNavMapToList(object o)
    {
        PolyNav2D newMap = o as PolyNav2D;
        if (newMap != null && !this.allNavMapsInScene.Contains(newMap))
        {
            this.allNavMapsInScene.Add(newMap);
        }
    }

    public void RemoveNavMapFromList(object o)
    {
        PolyNav2D mapToDelete = o as PolyNav2D;
        if (mapToDelete != null && this.allNavMapsInScene.Contains(mapToDelete))
        {
            this.allNavMapsInScene.Remove(mapToDelete);
        }
    }

    public void AddNavPortalToList(object o)
    {
        NavMeshPortal newPortal = o as NavMeshPortal;
        if (newPortal != null && !this.allNavPortals.Contains(newPortal))
        {
            this.allNavPortals.Add(newPortal);
        }
    }
    

    public void RemoveNavPortalFromList(object o)
    {
        NavMeshPortal newPortal = o as NavMeshPortal;
        if (newPortal != null && this.allNavPortals.Contains(newPortal))
        {
            this.allNavPortals.Remove(newPortal);
        }
    }


    public NavMeshPortal FindNextNavPortal(Vector2 pointToCheck, PolyNav2D currentCharMap)
    {
        // No nav portals to provide
        if (this.allNavPortals.Count <= 0)
        {
            return null;
        }
        
        // Finding a nav portal where current and destination maps connect directly together
        NavMeshPortal discoveredDirectPortal = GetPortalIfAdjacentMap(pointToCheck, currentCharMap);
        if (discoveredDirectPortal != null)
        {
            return discoveredDirectPortal;
        }
        else
        {          
           return FindDestinationInClosestMap(pointToCheck, currentCharMap); // Do a deeper search
        }
    }


    NavMeshPortal GetPortalIfAdjacentMap(Vector2 point, PolyNav2D currentMesh)
    {
        NavMeshPortal foundDirectPortal = null;
        for (int i = 0; i < this.allNavPortals.Count; i++)
        {
            PolyNav2D connectedMap1 = this.allNavPortals[i].map1;
            PolyNav2D connectedMap2 = this.allNavPortals[i].map2;
            if (this.allNavPortals[i].map1 == null || this.allNavPortals[i].map2 == null)
            {
                Debug.LogError("The " + this.allNavPortals[i].gameObject.name + " has one or more empty maps!");
            }
            // Checking whether the 2 meshes (current and connected) are directly tied within this portal
            if (connectedMap1.PointIsValid(point) && connectedMap2 == currentMesh || connectedMap2.PointIsValid(point) && connectedMap1 == currentMesh)
            {
               foundDirectPortal = this.allNavPortals[i];
            }
        }
        return foundDirectPortal;
    }

    NavMeshPortal FindDestinationInClosestMap(Vector2 pointToCheck, PolyNav2D currentCharMap)
    {
        PolyNav2D mapWithTarget = FindMapContainingPoint(pointToCheck);
        List<NavMeshPortal> allTargetConnectingPortals = FindConnectingPortals(mapWithTarget);
        List<NavMeshPortal> allCurrMapPortals = FindConnectingPortals(currentCharMap);
        NavMeshPortal portalToNextMap = FindPortalToMutualMap(allTargetConnectingPortals, allCurrMapPortals);
        return portalToNextMap;
    }

    PolyNav2D FindMapContainingPoint(Vector2 point)
    {
        PolyNav2D matchingMap = null;
        for (int i = 0; i < this.allNavMapsInScene.Count; i++)
        {
            if (this.allNavMapsInScene[i].PointIsValid(point))
            {
                matchingMap = this.allNavMapsInScene[i];
            }
        }
        return matchingMap;
    }


    List<NavMeshPortal> FindConnectingPortals(PolyNav2D map)
    {
        List<NavMeshPortal> portals = new List<NavMeshPortal>();
        for (int i = 0; i < this.allNavPortals.Count; i++)
        {
            if (this.allNavPortals[i].map1.gameObject == map.gameObject || this.allNavPortals[i].map2.gameObject == map.gameObject)
            {
                portals.Add(this.allNavPortals[i]);
            }
        }
        return portals;
    }

    NavMeshPortal FindPortalToMutualMap(List<NavMeshPortal> targetPortalList, List<NavMeshPortal> charPortalList)
    {
        List<NavMeshPortal> eligiblePortalsOnCharMap = new List<NavMeshPortal>();
        foreach (NavMeshPortal targetPortal in targetPortalList)
        {
            foreach (NavMeshPortal charPortal in charPortalList)
            {
                if (targetPortal.map1.gameObject == charPortal.map1.gameObject || targetPortal.map1.gameObject == charPortal.map2.gameObject
                    || targetPortal.map2.gameObject == charPortal.map1.gameObject || targetPortal.map2.gameObject == charPortal.map2.gameObject)
                {
                    eligiblePortalsOnCharMap.Add(charPortal);
                }
            }
        }

        if (eligiblePortalsOnCharMap.Count > 0)
        {
            return eligiblePortalsOnCharMap[0];
        }
        return null;
    }

    public Vector2 GetMainMeshPos()
    {
        if (this.mainNavMap == null)
        {
            return Vector2.zero;
        }

        return this.mainNavMap.transform.position;
    }
}
