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


    public NavMeshPortal FindNavPortalWithDestinationForPoint(Vector2 pointToCheck, PolyNav2D currentMesh)
    {
        if (this.allNavPortals.Count <= 0)
        {
            return null;
        }
       
        NavMeshPortal foundDirectPortal = CheckIfCanReachDirectly(pointToCheck, currentMesh);
        if (foundDirectPortal != null)
        {
            return foundDirectPortal;
        }
        else
        {
           // Do search through the chain
            return DoSearchThroughChain(pointToCheck);
        }
    }


    NavMeshPortal CheckIfCanReachDirectly(Vector2 point, PolyNav2D currentMesh)
    {
        NavMeshPortal foundDirectPortal = null;
        for (int i = 0; i < this.allNavPortals.Count; i++)
        {
            PolyNav2D destMap1 = this.allNavPortals[i].mesh1;
            PolyNav2D destMap2 = this.allNavPortals[i].mesh2;
            if (destMap1.PointIsValid(point) && destMap2 == currentMesh || destMap2.PointIsValid(point) && destMap1 == currentMesh)
            {
               foundDirectPortal = this.allNavPortals[i];
            }
        }
        return foundDirectPortal;
    }

    NavMeshPortal DoSearchThroughChain(Vector2 pointToCheck)
    {
        NavMeshPortal finalPortal = null;
        PolyNav2D meshToCheckNext = null;
        for (int i = 0; i < this.allNavPortals.Count; i++)
        {
            PolyNav2D destMap1 = this.allNavPortals[i].mesh1;
            PolyNav2D destMap2 = this.allNavPortals[i].mesh2;
            if (destMap1.PointIsValid(pointToCheck))
            {
                // Found the destination portal, now follow the thread
                finalPortal = this.allNavPortals[i];
                meshToCheckNext = destMap2;
                break;
            }
            else if (destMap2.PointIsValid(pointToCheck))
            {
                finalPortal = this.allNavPortals[i];
                meshToCheckNext = destMap1;
                break;
            }
        }

        // TODO: Add contingency for when the point couldn't be placed in any of the meshes!
        
        NavMeshPortal portalToNextMapInLine = CheckIfCanReachDirectly(pointToCheck, meshToCheckNext);
        
        
        if (portalToNextMapInLine != null)
        {
            return portalToNextMapInLine;
        }
        else
        {
            pointToCheck = meshToCheckNext.gameObject.transform.position;
            finalPortal = CheckIfCanReachDirectly(pointToCheck, meshToCheckNext);
            if (finalPortal != null)
            {
                return finalPortal;
            }
            else
            {
                return DoSearchThroughChain(pointToCheck);
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
