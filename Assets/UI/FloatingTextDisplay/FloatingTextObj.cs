using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextObj : MonoBehaviour
{

    CanvasGroup myCanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        //Start flying upwards
        myCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    IEnumerator Animate()
    {
        LeanTween.moveLocalY(this.gameObject, this.transform.position.y + 50f, 1f).setEase(LeanTweenType.easeOutCirc);
        yield return new WaitForSeconds(0.2f);
        LeanTween.alphaCanvas(myCanvasGroup, 0, 0.5f).setOnComplete(() => Destroy(this.gameObject)); 
    }

}
