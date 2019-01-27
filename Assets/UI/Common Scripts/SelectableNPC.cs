using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableNPC : SelectableObject
{
    public bool isSelected;

    public override SelectableType GetSelectableType()
    {
        return SelectableType.NPC;
    }

    //private void OnMouseDown()
    //{
    //    SelectionController.Instance.SelectedObj = this;
    //}
}
