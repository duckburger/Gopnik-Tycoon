using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class NavMeshPortal : MonoBehaviour
{
    [SerializeField] ScriptableEvent onRegistered;
    public PolyNav2D destinationMesh;

    private void Start()
    {
        if (this.onRegistered != null)
        {
            this.onRegistered.OpenWithData(this);
        }
    }


    private void OnDisable()
    {
        this.onRegistered.CloseWithData(this);
    }
}
