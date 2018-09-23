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
    [SerializeField] float stat_strength;
    public float GetStat_Strength()
    {
        return stat_strength;
    }
    [SerializeField] float stat_charisma;
    public float GetStat_Charisma()
    {
        return stat_charisma;
    }
    [SerializeField] float stat_cunning;
    public float GetStat_Cunning()
    {
        return stat_cunning;
    }
    public float GetWalletBalance()
    {
        return this.GetComponent<Wallet>().CurrentBalance();
    }
    [SerializeField] Sprite myPortrait;
    public Sprite GetPortrait()
    {
        return myPortrait;
    }
}
