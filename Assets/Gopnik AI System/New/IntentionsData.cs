using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Gopnik/IntentionsFile")]
public class IntentionsData : ScriptableObject
{
    [SerializeField] CharacterIntentions designatedIntentions;
    [SerializeField] float itemsWanted;
    [SerializeField] [EnumFlag] FoodQuality qualityWanted;

    public FoodQuality QualityWanted
    {
        get
        {
            return this.qualityWanted;
        }
    }
}
