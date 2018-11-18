using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

[Serializable]
[Flags]
public enum FoodQuality
{
    Low = 1,
    Medium = 2,
    High = 4
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(FoodQuality))]
public class FoodQualityDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        property.intValue = (int)(FoodQuality)EditorGUI.EnumFlagsField(position, label, (FoodQuality)property.intValue);
        EditorGUI.EndProperty();
    }
}
#endif

[Serializable]
public enum FoodType
{
    None = 0,
    Regular, 
    Fresh
}

public class FoodItem 
{
    [Space(10)]
    [Header("UI Elements")]
    public TextMeshProUGUI stockAmtText;

    [Space(10)]
    [SerializeField] protected FoodQuality foodQuality;
    public FoodQuality FoodQuality
    {
        get
        {
            return this.foodQuality;
        }
    }
    [SerializeField] protected FoodType containedType;
    public FoodType ContainedType
    {
        get
        {
            return this.containedType;
        }
    }
    [Range(3, 25)]
    [SerializeField] protected int foodQuantity; // 3 - little, 6 - medium, 12 - large, 25 - huge(?)
    [SerializeField] protected Sprite worldFoodAppearance;

    [SerializeField] protected List<Sprite> onShelfAppearances = new List<Sprite>();
    public List<Sprite> OnShelfAppearances
    {
        get
        {
            return this.onShelfAppearances;
        }
    }

    public Sprite GetRandomShelfAppearanceSprite()
    {
        if (this.onShelfAppearances != null && this.onShelfAppearances.Count > 0)
        {
            int indexOfSprite = UnityEngine.Random.Range(0, this.onShelfAppearances.Count - 1);
            return this.onShelfAppearances[indexOfSprite];
        }
        else
        {
            return null;
        }
    }

   
}
