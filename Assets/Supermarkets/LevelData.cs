using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData CurrentLevel;

    [SerializeField] Transform floor;
    [SerializeField] Transform entranceExitPoint;

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
    }
}
