using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class LevelData : MonoBehaviour
{
    public static LevelData CurrentLevel;

    [SerializeField] Transform floor;
    [SerializeField] Transform entranceExitPoint;
    [SerializeField] PolyNav2D navMapController;

    public Transform Floor
    {
        get
        {
            return this.floor;
        }
    }
    public Transform EntranceExitPoint
    {
        get
        {
            return this.entranceExitPoint;
        }
    }
    public PolyNav2D mainMapController => this.navMapController;


    private void Awake()
    {
        if (CurrentLevel == null)
        {
            CurrentLevel = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        this.navMapController = this.GetComponent<PolyNav2D>();
    }
}
