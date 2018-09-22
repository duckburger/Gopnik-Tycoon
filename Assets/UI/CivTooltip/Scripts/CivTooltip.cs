using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CivTooltip : UIPanel
{

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

    [Header("Actions")]
    [SerializeField] Button forceActionButton;
    [SerializeField] Button chatActionButton;
    [SerializeField] Button razvodActionButton;

    RectTransform thisRect;
    CanvasGroup mainCanvasGroup;

    bool isOpen = false;
    bool isPopulated = false;

    private void Start()
    {
        this.thisRect = this.GetComponent<RectTransform>();
        this.mainCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    #region Open/Close

    public override void Open()
    {
        if (!isOpen)
        {
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
            LeanTween.alphaCanvas(this.mainCanvasGroup, 0, 0.1f).setOnComplete(() =>
            {
                this.mainCanvasGroup.interactable = false;
                this.mainCanvasGroup.blocksRaycasts = false;
            });
            this.isOpen = false;
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
