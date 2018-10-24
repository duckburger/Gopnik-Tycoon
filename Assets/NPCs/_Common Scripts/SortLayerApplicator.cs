using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum SortingLayerNames
{
    Default,
    UILayer,
}

public class SortLayerApplicator : MonoBehaviour
{
    [SerializeField] SortingLayerNames layerName;
    [SerializeField] bool includeAllChildren;
    Renderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        this.myRenderer = this.GetComponent<Renderer>();
        ApplyLayer();
    }

    void ApplyLayer()
    {
        if (myRenderer != null)
        {
            this.myRenderer.sortingLayerName = layerName.ToString();
        }
        if (includeAllChildren)
        {
            ApplyToChildren(this.transform);
        }

    }


    void ApplyToChildren(Transform parentToCheck)
    {
        foreach (Transform child in parentToCheck)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.sortingLayerName = layerName.ToString();
            }
            if (child.childCount > 0)
            {
                ApplyToChildren(child);
            }
        }

    }
   
}
