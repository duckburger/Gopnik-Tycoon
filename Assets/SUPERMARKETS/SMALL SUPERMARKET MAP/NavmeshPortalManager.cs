using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
