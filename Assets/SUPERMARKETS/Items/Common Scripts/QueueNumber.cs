using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueNumber : MonoBehaviour
{
    [SerializeField] int currentNumberInQueue;
    public int CurrentNumberInQueue
    {
        get
        {
            return this.currentNumberInQueue;
        }
        set
        {
            this.currentNumberInQueue = value;
        }
    }

    int lastNumberInQueue;
    public int LastNumberInQueue
    {
        get
        {
            return this.lastNumberInQueue;
        }
        set
        {
            this.lastNumberInQueue = value;
        }
    }
}
