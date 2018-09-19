using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableNPC : MonoBehaviour
{
    bool isSelected;

    private void OnMouseDown()
    {
        SelectionController.Instance.SelectedObj = this.gameObject;
    }
}
