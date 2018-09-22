using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCatcher : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ScriptableEvent globalDeselect;

    public void OnPointerClick(PointerEventData eventData)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        SelectableObject selObj = null;
        if (hit.collider != null)
        {
            selObj = hit.collider.GetComponent<SelectableObject>();
        }
        if (globalDeselect != null && selObj == null)
        {
            globalDeselect.Raise();
        }
    }
}
