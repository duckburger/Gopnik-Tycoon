using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData CurrentLevel;

    [SerializeField] Transform floor;
    public Transform Floor
    {
        get
        {
            return this.floor;
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
