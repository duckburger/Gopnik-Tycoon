using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Building Category", menuName = "Gopnik/Building Category")]
public class BuildingCategory : ScriptableObject
{
    public string categoryName;
    public List<GameObject> buildingsInMyCategory = new List<GameObject>();
    public Color categoryColor;
}
