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
    public PolyNav2D mainNavMesh;


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


    public NavMeshPortal FindNavPortalWithDestinationForPoint(Vector2 pointToCheck, PolyNav2D currentCharMap)
    {
        // No nav portals to provide
        if (this.allNavPortals.Count <= 0)
        {
            return null;
        }
       
        NavMeshPortal discoveredDirectPortal = GetPortalIfAdjacentMap(pointToCheck, currentCharMap);
        if (discoveredDirectPortal != null)
        {
            return discoveredDirectPortal;
        }
        else
        {
           // Do search through the chain
            return FindDestinationInClosestMesh(pointToCheck, currentCharMap);
        }
    }


    NavMeshPortal GetPortalIfAdjacentMap(Vector2 point, PolyNav2D currentMesh)
    {
        NavMeshPortal foundDirectPortal = null;
        for (int i = 0; i < this.allNavPortals.Count; i++)
        {
            PolyNav2D connectedMap1 = this.allNavPortals[i].mesh1;
            PolyNav2D connectedMap2 = this.allNavPortals[i].mesh2;
            // Checking whether the 2 meshes (current and connected) are directly tied with this portal
            if (connectedMap1.PointIsValid(point) && connectedMap2 == currentMesh || connectedMap2.PointIsValid(point) && connectedMap1 == currentMesh)
            {
               foundDirectPortal = this.allNavPortals[i];
            }
        }
        return foundDirectPortal;
    }

    NavMeshPortal FindDestinationInClosestMesh(Vector2 pointToCheck, PolyNav2D currentCharNav)
    {
        NavMeshPortal finalPortal = null;
        PolyNav2D meshToCheckNext = null;

        for (int i = 0; i < this.allNavPortals.Count; i++)
        {
            PolyNav2D connectedMap1 = this.allNavPortals[i].mesh1;
            PolyNav2D connectedMap2 = this.allNavPortals[i].mesh2;
            if (connectedMap1.PointIsValid(pointToCheck))
            {
                // Found the destination portal, now find a portal that leads to the other navmesh that didn't match
                finalPortal = this.allNavPortals[i];
                meshToCheckNext = connectedMap2;
                break;
            }
            else if (connectedMap2.PointIsValid(pointToCheck))
            {
                finalPortal = this.allNavPortals[i];
                meshToCheckNext = connectedMap1;
                break;
            }
        }

        if (finalPortal == null)
        {
            // Couldn't find a matching mesh (something is wrong)
            return null;
        }

        NavMeshPortal portalToNextMapInLine = GetPortalIfAdjacentMap(meshToCheckNext.transform.position, currentCharNav);
        
        if (portalToNextMapInLine != null)
        {
            return portalToNextMapInLine;
        }
        else
        {
            pointToCheck = meshToCheckNext.gameObject.transform.position;
            finalPortal = GetPortalIfAdjacentMap(pointToCheck, meshToCheckNext);
            if (finalPortal != null)
            {
                return finalPortal;
            }
            else
            {
                return FindDestinationInClosestMesh(pointToCheck, currentCharNav);
            }
        }
    }

    public Vector2 GetMainMeshPos()
    {
        if (this.mainNavMesh == null)
        {
            return Vector2.zero;
        }

        return this.mainNavMesh.transform.position;
    }
}
