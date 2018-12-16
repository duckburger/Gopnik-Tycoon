using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawOrderSorter : MonoBehaviour {

    [SerializeField] bool runOnce = true;
    [SerializeField] float baseSortNumber = 5000;
    [SerializeField] float refreshRateInSecs = 0.3f;
    [SerializeField] SortingGroup mySortingGroup;
    Renderer myRenderer;
    Canvas myCanvas;

    int currentSortingLayer = 0;

    // Use this for initialization
    void Start ()
    {
        this.mySortingGroup = GetComponent<SortingGroup>();
        this.myRenderer = GetComponent<Renderer>();
        this.myCanvas = GetComponent<Canvas>();
        Launch();
    }


    public void Launch()
    {
        StartCoroutine(ChangeRenderOrder());
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    IEnumerator ChangeRenderOrder()
    {
        double yPos = GetYPos();
        if (this.mySortingGroup != null)
        {
            if (!this.runOnce)
            {
                while (true)
                {
                    yPos = GetYPos();
                    currentSortingLayer = (int)(baseSortNumber - yPos);
                    this.mySortingGroup.sortingOrder = currentSortingLayer;
                    yield return new WaitForSeconds(refreshRateInSecs);
                }
            }
            else
            {
                yPos = GetYPos();
                currentSortingLayer = (int)(baseSortNumber - yPos);
                this.mySortingGroup.sortingOrder = currentSortingLayer;
                yield break;
            }
        }

        if (this.myRenderer != null) // If you have a renderer
        {
            if (!this.runOnce)
            {
                while (true)
                {
                    yPos = GetYPos();
                    currentSortingLayer = (int)(baseSortNumber - yPos);
                    this.myRenderer.sortingOrder = currentSortingLayer;
                    yield return new WaitForSeconds(refreshRateInSecs);
                }
            }
            else
            {
                yPos = GetYPos();
                currentSortingLayer = (int)(baseSortNumber - yPos);
                this.myRenderer.sortingOrder = currentSortingLayer;
                yield break;
            }

        }
        else if (this.myCanvas != null) // If you have a canvas
        {
            if (!this.runOnce)
            {
                while (true)
                {
                    yPos = GetYPos();
                    this.myCanvas.overrideSorting = true;
                    currentSortingLayer = (int)(baseSortNumber - yPos);
                    this.myCanvas.sortingOrder = currentSortingLayer;
                    yield return new WaitForSeconds(refreshRateInSecs);
                }
            }
            else
            {
                yPos = GetYPos();
                this.myCanvas.overrideSorting = true;
                currentSortingLayer = (int)(baseSortNumber - yPos);
                this.myCanvas.sortingOrder = currentSortingLayer;
                yield break;
            }
        }
    }

    private float GetYPos()
    {
        return (this.transform.position.y * 10);
    }




}
