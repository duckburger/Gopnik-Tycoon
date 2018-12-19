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

        for (int i = 0; i < this.allNavPortals.Count; i++)
        {
            PolyNav2D destMap1 = this.allNavPortals[i].mesh1;
            PolyNav2D destMap2 = this.allNavPortals[i].mesh2;
            if (destMap1.PointIsValid(pointToCheck) && destMap2 == currentMesh || destMap2.PointIsValid(pointToCheck) && destMap1 == currentMesh)
            {
                return this.allNavPortals[i];
            }
        }

        return null;
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
