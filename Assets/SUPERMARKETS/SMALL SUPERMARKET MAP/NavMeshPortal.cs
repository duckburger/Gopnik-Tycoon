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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<AI_Generic>() != null)
        {
            AI_Generic npc = collider.gameObject.GetComponent<AI_Generic>();
            if (npc.MyTargetNavPortal == this)
            {
                // Switch the NPC's nav mesh to the destination
                npc.GetComponent<PolyNavAgent>().map = this.destinationMesh;
            }
        }
    }
}
