using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CivTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image strengthBar;
    [SerializeField] Image charismaBar;
    [SerializeField] Image cunningBar;
    [SerializeField] Image walletIcon;
    [SerializeField] TextMeshProUGUI walletBalance;

    [Header("Actions")]
    [SerializeField] Button forceActionButton;
    [SerializeField] Button chatActionButton;
    [SerializeField] Button razvodActionButton;

    RectTransform thisRect;
    CanvasGroup mainCanvasGroup;

    private void Start()
    {
        this.thisRect = this.GetComponent<RectTransform>();
        this.mainCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    #region Open/Close

    public void Open()
    {
        LeanTween.alphaCanvas(this.mainCanvasGroup, 1, 0.1f);
    }

    public void Close()
    {
        LeanTween.alphaCanvas(this.mainCanvasGroup, 0, 0.1f);
    }

    #endregion

    #region Populate

    public void Populate(ICharStats stats)
    {

    }

    #endregion
}
