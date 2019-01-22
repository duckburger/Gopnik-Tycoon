using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class NextDeliveryTimer : MonoBehaviour
{
    [SerializeField] UnityEvent onTimerExpired;
    [Space]
    [Tooltip("In seconds")]
    [SerializeField] TextMeshProUGUI timerText;


    float currentTimer = 0;
    WaitForSeconds delay = new WaitForSeconds(1f);

    void Start()
    {
        if (this.timerText == null)
        {
            this.timerText = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
    }

    public void StartTimerForNextDelivery(float timerAmount)
    {
        this.currentTimer = timerAmount;
        StopAllCoroutines();
        StartCoroutine(StartTimer());
        
    }

    IEnumerator StartTimer()
    {
        while (this.currentTimer > 0)
        {
            this.currentTimer--;
            this.timerText.text = $"Time until next food delivery:\n{this.currentTimer} secs";
            yield return delay;
        }
    }
}
