using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBubbleDisplayer : MonoBehaviour
{

    [SerializeField] GameObject bubblePrefab;

    CanvasGroup currentBubbleCG;
    TextMeshProUGUI currentBubbleText;
    GameObject currentBubble;


    public void ShowDialogue(string phrase)
    {
        Debug.Log("Called show dialogue once");
        if (this.currentBubbleCG != null)
        {
            this.currentBubble.SetActive(true);
            StopAllCoroutines();
            LeanTween.alphaCanvas(this.currentBubbleCG, 0, 0.2f)
                .setOnComplete(() => 
                {
                    this.currentBubbleText.text = phrase;
                    LeanTween.alphaCanvas(this.currentBubbleCG, 1, 0.2f);
                    StartCoroutine(Timer());
                } );
            return;
        }
        Vector2 bubblePos = new Vector2(this.transform.position.x, this.transform.position.y + 1);
        this.currentBubble = Instantiate(this.bubblePrefab, bubblePos, Quaternion.identity, this.transform);
        this.currentBubbleCG = this.currentBubble.GetComponent<CanvasGroup>();
        this.currentBubbleText = this.currentBubble.GetComponent<TextMeshProUGUI>();
        this.currentBubbleText.text = phrase;
        LeanTween.alphaCanvas(this.currentBubbleCG, 1, 0.2f);
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2);
        LeanTween.alphaCanvas(this.currentBubbleCG, 0, 0.2f)
               .setOnComplete(() =>
               {
                   this.currentBubble.SetActive(false);
                   Debug.Log("Finished fading out bubble canvas");
               });
    }


}
