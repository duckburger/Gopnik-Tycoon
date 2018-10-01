using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GopActionButton : MonoBehaviour
{
    [SerializeField] ActionType myActionType;
    public ActionType MyActionType
    {
        get
        {
            return myActionType;
        }
    }

    
}
