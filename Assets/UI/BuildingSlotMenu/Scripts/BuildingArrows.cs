using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingArrows : MonoBehaviour
{
    public RectTransform container;
    public Button leftArrow;
    public Button rightArrow;

    public Transform target;
    Camera mainCam = null;

    private void Awake()
    {
        this.mainCam = Camera.main;
    }

    private void Update()
    {
        if (this.target != null && this.mainCam != null)
        {
            this.container.transform.position = this.mainCam.WorldToScreenPoint(this.target.transform.position);
        }
    }
}
