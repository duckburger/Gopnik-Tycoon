using UnityEngine;
using UnityEngine.UI;

public class GopUIIcon : MonoBehaviour
{
    public AI_CharController myGopnik;
    [Space(10)]
    [SerializeField] Image mainIcon;
    [SerializeField] Image currStateIcon;
    [SerializeField] Button myButton;
    [SerializeField] ScriptableGopnikData gopStateSprites;

   
    private void Start()
    {
        this.myButton = this.GetComponent<Button>();
    }

    public void Populate(AI_CharController gopnik)
    {
        this.myGopnik = gopnik;
        this.mainIcon.sprite = this.myGopnik.GetPortrait();
        //switch (this.myGopnik.GetCurrentAction())
        //{
        //    case GopnikActionType.Idling:
        //        this.currStateIcon.sprite = this.gopStateSprites.idleStateSprite;
               
        //        break;
        //    case GopnikActionType.Force:
        //        this.currStateIcon.sprite = this.gopStateSprites.fightingStateSprite;

        //        break;
        //    case GopnikActionType.Chat:
        //        this.currStateIcon.sprite = this.gopStateSprites.chattingStateSprite;

        //        break;
        //    case GopnikActionType.Razvod:
        //        this.currStateIcon.sprite = this.gopStateSprites.razvodStateSprite;

        //        break;
        //    default:
        //        break;
        //}
        //gopnik.stateChangedEvent.AddListener(UpdateStateIcon);
    }

    void UpdateStateIcon(GopnikActionType newState)
    {
        switch (newState)
        {
            case GopnikActionType.Idling:
                this.currStateIcon.sprite = this.gopStateSprites.idleStateSprite;

                break;
            case GopnikActionType.Force:
                this.currStateIcon.sprite = this.gopStateSprites.fightingStateSprite;

                break;
            case GopnikActionType.Chat:
                this.currStateIcon.sprite = this.gopStateSprites.chattingStateSprite;

                break;
            case GopnikActionType.Razvod:
                this.currStateIcon.sprite = this.gopStateSprites.razvodStateSprite;

                break;
            default:
                break;
        }
    }

  

}
