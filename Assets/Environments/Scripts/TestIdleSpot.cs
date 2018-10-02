using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIdleSpot : MonoBehaviour, AI_IdleSpot
{
    public GameObject GetIdlingTarget()
    {
        return this.gameObject;
    }

    public float GetReqIdleProximity()
    {
        return 0.7f
    }
}
