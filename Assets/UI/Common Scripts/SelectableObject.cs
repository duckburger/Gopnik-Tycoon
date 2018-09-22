using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectableType
{
    NPC,
    Building,
    Item
}

public class SelectableObject : MonoBehaviour
{
    public virtual SelectableType GetSelectableType()
    {
        return SelectableType.NPC; // This is a default value
    }
}
