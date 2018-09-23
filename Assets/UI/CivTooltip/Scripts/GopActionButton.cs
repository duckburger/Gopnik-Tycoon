using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GopActionButton : MonoBehaviour
{
    [SerializeField] GopnikActionType myActionType;
    public GopnikActionType MyActionType
    {
        get
        {
            return myActionType;
        }
    }

    
}
