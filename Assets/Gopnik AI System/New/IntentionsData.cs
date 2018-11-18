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
    [SerializeField] FoodQuality qualityWanted;
}
