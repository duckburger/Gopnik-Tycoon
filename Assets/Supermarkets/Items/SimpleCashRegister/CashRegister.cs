using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : Building
{

    Queue<GameObject> lineup = new Queue<GameObject>();

    public Vector2 ProvideQueueSpot()
    {
        return Vector2.zero;
    }
    

}
