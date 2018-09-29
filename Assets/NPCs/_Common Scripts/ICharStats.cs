using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharStats
{
    string GetCharName();
    int GetStat_Strength();
    int GetStat_Charisma();
    int GetStat_Cunning();
    float GetWalletBalance();
    Texture2D GetPortrait();
}
