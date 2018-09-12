using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivStats : MonoBehaviour, ICharStats {

    [SerializeField] bool isAgressive;
	public bool IsAgressive
    {
        get
        {
            return isAgressive;
        }
    }
    [SerializeField] float stat_intimidation;
    public float GetStat_Intimidation()
    {
        return stat_intimidation;
    }
}
