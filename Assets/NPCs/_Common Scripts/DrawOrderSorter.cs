﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOrderSorter : MonoBehaviour {

    [SerializeField] bool runOnce = true;
    [SerializeField] float baseSortNumber = 5000;
    [SerializeField] float refreshRateInSecs = 0.3f;
    Renderer myRenderer;
    Canvas myCanvas;

    int currentSortingLayer = 0;

	// Use this for initialization
	void Start () {
        this.myRenderer = GetComponent<Renderer>();
        this.myCanvas = GetComponent<Canvas>();

        StartCoroutine(ChangeRenderOrder());
	}
	
	IEnumerator ChangeRenderOrder()
    {
        if (this.myRenderer != null)
        {
            if (!this.runOnce)
            {
                while (true)
                {
                    currentSortingLayer = (int)(baseSortNumber - this.transform.position.y);
                    this.myRenderer.sortingOrder = currentSortingLayer;
                    yield return new WaitForSeconds(refreshRateInSecs);
                }
            }
            else
            {
                currentSortingLayer = (int)(baseSortNumber - this.transform.position.y);
                this.myRenderer.sortingOrder = currentSortingLayer;
                yield break;
            }
            
        }
        else if (this.myCanvas != null)
        {
            if (!this.runOnce)
            {
                while (true)
                {
                    this.myCanvas.overrideSorting = true;
                    currentSortingLayer = (int)(baseSortNumber - this.transform.position.y);
                    this.myCanvas.sortingOrder = currentSortingLayer;
                    yield return new WaitForSeconds(refreshRateInSecs);
                }
            }
            else
            {
                this.myCanvas.overrideSorting = true;
                currentSortingLayer = (int)(baseSortNumber - this.transform.position.y);
                this.myCanvas.sortingOrder = currentSortingLayer;
                yield break;
            }
            
        }
       
    }
}
