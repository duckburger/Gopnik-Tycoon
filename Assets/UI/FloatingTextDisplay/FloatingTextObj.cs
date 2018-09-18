using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextObj : MonoBehaviour
{
    public TextMeshProUGUI text;
    CanvasGroup myCanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        //Start flying upwards
        myCanvasGroup = this.GetComponent<CanvasGroup>();
        text = this.GetComponent<TextMeshProUGUI>();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        LeanTween.moveLocalY(this.gameObject, this.transform.position.y + 5f, 5.5f).setEase(LeanTweenType.easeOutQuart);
        yield return new WaitForSeconds(0.2f);
        LeanTween.alphaCanvas(myCanvasGroup, 0, 0.5f).setOnComplete(() => Destroy(this.gameObject)); 
    }

}
