using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivStats : MonoBehaviour, ICharStats {

    [SerializeField] string name;
    public string GetCharName()
    {
        return name;
    }
    [SerializeField] bool isAgressive;
	public bool IsAgressive
    {
        get
        {
            return isAgressive;
        }
    }
    [SerializeField] int stat_strength;
    public int GetStat_Strength()
    {
        return stat_strength;
    }
    [SerializeField] int stat_charisma;
    public int GetStat_Charisma()
    {
        return stat_charisma;
    }
    [SerializeField] int stat_cunning;
    public int GetStat_Cunning()
    {
        return stat_cunning;
    }
    public float GetWalletBalance()
    {
        return this.GetComponent<Wallet>().CurrentBalance;
    }
    [SerializeField] Sprite myPortrait;
    public Sprite GetPortrait()
    {
        return myPortrait;
    }
}
