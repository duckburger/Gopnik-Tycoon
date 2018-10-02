using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GopnikPortraits : MonoBehaviour
{
    [SerializeField] Transform portraitParent;
    [Space(10)]
    [SerializeField] NPCController npcController;
    [SerializeField] GameObject gopnikPortraitPrefab;

    List<GameObject> gopIcons = new List<GameObject>();

   public void Populate(ActionType gopIconAction)
    {
        if (this.npcController != null)
        {
            // Add all the portraits to the bar with appropriate action icon, one by one
            foreach (AI_CharController gopnik in this.npcController.GetAllGopniks())
            {
                GopUIIcon newPortrait = Instantiate(this.gopnikPortraitPrefab, this.portraitParent).GetComponent<GopUIIcon>();
                // Assign main icon as well as the current state icon
                newPortrait.Populate(gopnik);
                Button portraitButton = newPortrait.GetComponent<Button>();
                switch (gopIconAction)
                {
                    case ActionType.Force:
                        portraitButton.onClick.AddListener(() => 
                        {
                            Act_ForceMug forceMugAction = gopnik.ActionParent.GetComponent<Act_ForceMug>();
                            forceMugAction.Target = SelectionController.Instance.SelectedObj.gameObject;
                            gopnik.QueueAction(forceMugAction, false);
                            newPortrait.UpdateStateIcon(ActionType.Force);
                        });
                        break;
                    case ActionType.Chat:
                        portraitButton.onClick.AddListener(() => gopnik.QueueAction(null, false));
                        break;
                    case ActionType.Razvod:
                        portraitButton.onClick.AddListener(() => gopnik.QueueAction(null, false));
                        break;
                    default:
                        break;
                }
                this.gopIcons.Add(newPortrait.gameObject);
            }
        }
    }

    public void Clear()
    {
        for (int i = gopIcons.Count - 1; i >= 0; i--)
        {
            Destroy(gopIcons[i]);
        }
        gopIcons.Clear();
    }
}
