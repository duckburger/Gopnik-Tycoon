using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CivTooltip : UIPanel
{
    [Space(10)]
    [Header("Anim settings")]
    [SerializeField] float animSpeed;
    [SerializeField] LeanTweenType easeIn;
    [SerializeField] LeanTweenType easeOut;
    [Space(10)]
    [Header("Global Refs")]
    [SerializeField] ScriptableFloatVar maxStatAmt;
    [Space(10)]
    [Header("Object Refs")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image strengthBar;
    [SerializeField] TextMeshProUGUI strTextAmt;
    [SerializeField] Image charismaBar;
    [SerializeField] TextMeshProUGUI charTextAmt;
    [SerializeField] Image cunningBar;
    [SerializeField] TextMeshProUGUI cunTextAmt;
    [SerializeField] Image walletIcon;
    [SerializeField] TextMeshProUGUI walletBalance;
    [SerializeField] RectTransform gopPortraitsPanel;

    [Header("Actions")]
    [SerializeField] Transform actionsParent;
    [Space(10)]
    [SerializeField] Button forceActionButton;
    [SerializeField] Button chatActionButton;
    [SerializeField] Button razvodActionButton;

    List<Button> actionButtons = new List<Button>();

    RectTransform thisRect;
    CanvasGroup mainCanvasGroup;

    bool isOpen = false;
    bool gopPanelIsOpen = false;
    bool isPopulated = false;

    private void Start()
    {
        this.thisRect = this.GetComponent<RectTransform>();
        this.mainCanvasGroup = this.GetComponent<CanvasGroup>();
        if (this.actionButtons.Count <= 0)
        {
            foreach (Transform button in actionsParent)
            {
                this.actionButtons.Add(button.GetComponent<Button>());
            }
        }
        AssignButtonFunctions();
    }

    void AssignButtonFunctions()
    {
        foreach(Button button in this.actionButtons)
        {
            button.onClick.AddListener(() => 
            {
                ToggleGopPanel(button);
            });
        }
    }

    void TurnOffOtherButtons(Button dontTouch)
    {
        foreach (Button button in this.actionButtons)
        {
            if (button != dontTouch)
            {
                button.interactable = false;
            }
        }
    }

    void TurnOnAllButtons()
    {
        foreach (Button button in this.actionButtons)
        {
            button.interactable = true;
        }
    }

    #region Open/Close

    public override void Open()
    {
        if (!isOpen)
        {
            // Populate window as well
            LeanTween.alphaCanvas(this.mainCanvasGroup, 1, 0.1f).setOnComplete(() =>
            {
                this.mainCanvasGroup.interactable = true;
                this.mainCanvasGroup.blocksRaycasts = true;
            });
            this.isOpen = true;
        }       
    }

    public override void Close()
    {
        if (isOpen)
        {
            // Clear window
            LeanTween.alphaCanvas(this.mainCanvasGroup, 0, 0.1f).setOnComplete(() =>
            {
                this.mainCanvasGroup.interactable = false;
                this.mainCanvasGroup.blocksRaycasts = false;
            });
            this.isOpen = false;
        }
    }

    void ToggleGopPanel(Button buttonActivated = null)
    {
        if (!this.gopPanelIsOpen)
        {
            OpenGopPanel();
            if (buttonActivated != null)
            {
                TurnOffOtherButtons(buttonActivated);
            }
        }
        else
        {
            CloseGopPanel();
            TurnOnAllButtons();
        }
    }

    void OpenGopPanel()
    {
        if (!this.gopPanelIsOpen)
        {
            CanvasGroup cg = this.gopPortraitsPanel.GetComponent<CanvasGroup>();
            LeanTween.alphaCanvas(cg, 1, animSpeed * 1.25f);
            LeanTween.moveY(gopPortraitsPanel, -(this.gopPortraitsPanel.rect.height / 2), animSpeed).setEase(easeIn).setOnComplete(() => 
            {
               
                cg.interactable = true;
                cg.blocksRaycasts = true;
                this.gopPortraitsPanel.GetComponent<GopnikPortraits>().Populate();
            });
            this.gopPanelIsOpen = true;
        }
    }

    void CloseGopPanel()
    {
        if (this.gopPanelIsOpen)
        {
            CanvasGroup cg = this.gopPortraitsPanel.GetComponent<CanvasGroup>();
            LeanTween.alphaCanvas(cg, 0, animSpeed * 1.25f);
            LeanTween.moveY(gopPortraitsPanel, this.gopPortraitsPanel.rect.height / 2, animSpeed).setEase(easeOut).setOnComplete(() =>
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;
                this.gopPortraitsPanel.GetComponent<GopnikPortraits>().Clear();
            });
            this.gopPanelIsOpen = false;
        }
        
    }

    #endregion

    #region Populate

    public override void Populate(ICharStats stats)
    {
        if (!isPopulated)
        {
            ApplyNewValues(stats);
            isPopulated = true;
        }
        else
        {
            Clear();
            ApplyNewValues(stats);
        }
    }

    private void ApplyNewValues(ICharStats stats)
    {
        // Fill out the tooltip with the character's 
        this.title.text = stats.GetCharName();
        // Strength
        this.strTextAmt.text = stats.GetStat_Strength().ToString();
        float strFillAmt = stats.GetStat_Strength() / maxStatAmt.value;
        strengthBar.fillAmount = strFillAmt;
        // Charisma
        this.charTextAmt.text = stats.GetStat_Charisma().ToString();
        float charFillAmt = stats.GetStat_Charisma() / maxStatAmt.value;
        charismaBar.fillAmount = charFillAmt;
        // Cunning
        this.cunTextAmt.text = stats.GetStat_Cunning().ToString();
        float cunFillAmt = stats.GetStat_Cunning() / maxStatAmt.value;
        this.cunningBar.fillAmount = cunFillAmt;
        // Cash
        this.walletBalance.text = stats.GetWalletBalance().ToString("$0");
    }

    #endregion


    public void Clear()
    {
        // Clear all data
        // Fill out the tooltip with the character's 
        this.title.text = "";
        // Strength
        this.strTextAmt.text = "";
        strengthBar.fillAmount = maxStatAmt.value;
        // Charisma
        this.charTextAmt.text = "";
        charismaBar.fillAmount = maxStatAmt.value;
        // Cunning
        this.cunTextAmt.text = "";
        cunningBar.fillAmount = maxStatAmt.value;

    }
}
