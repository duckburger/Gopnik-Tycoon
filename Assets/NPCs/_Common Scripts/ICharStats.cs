using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharStats
{
    string GetCharName();
    float GetStat_Strength();
    float GetStat_Charisma();
    float GetStat_Cunning();
    float GetWalletBalance();
    Sprite GetPortrait();
}
