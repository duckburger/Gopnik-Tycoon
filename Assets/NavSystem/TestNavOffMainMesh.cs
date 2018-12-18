using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNavOffMainMesh : MonoBehaviour
{
    [SerializeField] Transform testDestination;
    [SerializeField] AI_Generic affectedNPC;

    public void MakeNpcGoToTarget()
    {
        if (this.testDestination != null && this.affectedNPC != null)
        {
            this.affectedNPC.Target = this.testDestination.position;
        }
    }
}
