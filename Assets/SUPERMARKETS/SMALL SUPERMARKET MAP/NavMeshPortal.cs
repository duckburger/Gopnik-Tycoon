using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class NavMeshPortal : MonoBehaviour
{
    [SerializeField] ScriptableEvent onRegistered;
    public PolyNav2D map1;
    public PolyNav2D map2;

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
                // Let the npc know that they reached their target portal and let them advance to their actual target
                if (npc.NavAgent.map == this.map1)
                {
                    npc.NavAgent.map = this.map2;
                }
                else if (npc.NavAgent.map == this.map2)
                {
                    npc.NavAgent.map = this.map1;
                }
                npc.AdvanceToTargetAfterReachingPortal();
            }
        }
    }

}


