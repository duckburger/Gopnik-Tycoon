﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class MapRegistrator : MonoBehaviour
{

    PolyNav2D myMap;
    [SerializeField] ScriptableEvent onEnabledScriptableEvent;

    private void OnEnable()
    {
        if (this.myMap == null)
        {
            this.myMap = this.GetComponent<PolyNav2D>();
        }
        StartCoroutine(RegisterWithDelay());
    }

    IEnumerator RegisterWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        if (this.myMap != null && this.onEnabledScriptableEvent != null)
        {
            this.onEnabledScriptableEvent.OpenWithData(this.myMap);
        }
    }

    private void OnDisable()
    {
        if (this.myMap != null && this.onEnabledScriptableEvent != null)
        {
            this.onEnabledScriptableEvent.CloseWithData(this.myMap);
        }
    }
}

   
