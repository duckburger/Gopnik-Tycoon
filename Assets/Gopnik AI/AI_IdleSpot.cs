using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AI_IdleSpot 
{
    GameObject GetIdlingTarget();
    float GetReqIdleProximity();
}
