﻿using UnityEngine;
using UnityEngine.UI;

public class GopUIIcon : MonoBehaviour
{
    public GopnikAI myGopnik;
    [Space(10)]
    [SerializeField] Image mainIcon;
    [SerializeField] Image currStateIcon;
    [SerializeField] ScriptableGopnikData gopStateSprites;

    public void Populate(GopnikAI gopnik)
    {
        this.myGopnik = gopnik;
        this.mainIcon.sprite = this.myGopnik.GetPortrait();
        switch (this.myGopnik.GetCurrentAction())
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
        //this.currStateIcon.sprite = this.gopStateSprites
    }
    
}
