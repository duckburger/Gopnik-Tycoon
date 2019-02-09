﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image bgFill;
    [SerializeField] Image fgFill;
    [Space]
    [SerializeField] float maxVal;
    [SerializeField] float currentVal;
    [Space]
    [SerializeField] bool shake;

    RectTransform myRect;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        this.canvasGroup = this.GetComponent<CanvasGroup>();
        this.myRect = this.transform as RectTransform;
    }

    public void Init(float maxVal)
    {
        this.maxVal = maxVal;
        this.currentVal = this.maxVal;
        this.bgFill.fillAmount = 1;
        this.fgFill.fillAmount = 1;
    }

    public void UpdateBar(float newAmount)
    {        
        this.currentVal = newAmount;
        float normalizedVav = newAmount / this.maxVal;
        this.canvasGroup.alpha = 1;
        if (newAmount <= 0)
        {
            AnimateDestruction();
            return;
        }
        if (this.shake)
        {
            LeanTween.moveX(this.myRect, this.myRect.localPosition.x + 0.002f, 0.1f).setEase(LeanTweenType.easeInOutBounce).setLoopPingPong(1);
        }
        LeanTween.value(this.fgFill.fillAmount, normalizedVav, 0.12f).setOnUpdate((float val) => this.fgFill.fillAmount = val).setEase(LeanTweenType.easeInOutSine);
        LeanTween.value(this.bgFill.fillAmount, normalizedVav, 0.1f).setOnUpdate((float val) => this.bgFill.fillAmount = val)
            .setEase(LeanTweenType.easeInOutSine).setDelay(0.2f)
            .setOnComplete(() => 
            {
                LeanTween.alphaCanvas(this.canvasGroup, 0, 0.1f).setDelay(5f);
            });
    }

    void AnimateDestruction()
    {
        LeanTween.scaleY(this.gameObject, 0.2f, 0.23f).setEase(LeanTweenType.easeInOutBounce);
        LeanTween.alphaCanvas(this.canvasGroup, 0, 0.12f).setDelay(0.2f);
    }

}
