using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnim : MonoBehaviour
{
    [SerializeField] float bobbingAmount = 0.1f;
    [SerializeField] float animTime = 0.24f;
    [SerializeField] LeanTweenType animEase;
    private void Start()
    {
        LeanTween.moveLocalY(this.gameObject, this.transform.localPosition.y - bobbingAmount, animTime).setEase(animEase).setLoopPingPong();
    }
}
