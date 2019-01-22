using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class GlobalModal : MonoBehaviour
{
    public static GlobalModal Instance;

    [SerializeField] CanvasGroup mainCanvasGroup;
    [Space(10)]
    [SerializeField] Button declineButton;
    [SerializeField] TextMeshProUGUI declineButtText;
    [SerializeField] Button acceptButton;
    [SerializeField] TextMeshProUGUI acceptButtText;

    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI body;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void ShowModal(Action acceptAction, string acceptButtText, Action declineAction, string declineButtText, string titleText, string bodyText)
    {
        Reset();
        this.acceptButton.onClick.AddListener(() => acceptAction.Invoke());
        this.acceptButtText.text = acceptButtText;
        this.declineButton.onClick.AddListener(() => declineAction.Invoke());
        this.declineButtText.text = declineButtText;
        this.title.text = titleText;
        this.body.text = bodyText;
        FadeInModal();
    }

    public void ShowModal(Action acceptAction, Action declineAction, ModalWindowData data)
    {
        Reset();
        this.acceptButton.onClick.AddListener(() => acceptAction.Invoke());
        this.acceptButtText.text = data.acceptButtonText;
        this.declineButton.onClick.AddListener(() => declineAction.Invoke());
        this.declineButtText.text = data.declineButtonText;
        this.title.text = data.titleText;
        this.body.text = data.bodyText;
        FadeInModal();
    }

    private void Reset()
    {
        this.declineButton.onClick.RemoveAllListeners();
        this.acceptButton.onClick.RemoveAllListeners();
        this.acceptButton.onClick.AddListener(() => FadeOutModal());
        this.declineButton.onClick.AddListener(() => FadeOutModal());
        this.declineButtText.text = "";
        this.acceptButtText.text = "";
        this.title.text = "";
        this.body.text = "";
    }

    void FadeInModal()
    {
        if (this.mainCanvasGroup != null)
        {
            LeanTween.alphaCanvas(this.mainCanvasGroup, 1, 0.16f).setOnComplete(() => 
            {
                this.mainCanvasGroup.interactable = true;
                this.mainCanvasGroup.blocksRaycasts = true;
            });
        }
    }

    void FadeOutModal()
    {
        if (this.mainCanvasGroup != null)
        {
            LeanTween.alphaCanvas(this.mainCanvasGroup, 0, 0.16f).setOnComplete(() =>
            {
                this.mainCanvasGroup.interactable = false;
                this.mainCanvasGroup.blocksRaycasts = false;
            });
        }
    }

}
