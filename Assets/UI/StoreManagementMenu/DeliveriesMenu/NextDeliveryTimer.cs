using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class NextDeliveryTimer : MonoBehaviour
{
    [SerializeField] UnityEvent onTimerExpired;
    [Space]
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
        if (this.currentTimer <= 0)
        {
            StartCoroutine(StartTimer());
        }
    }

    IEnumerator StartTimer()
    {
        while (this.currentTimer > 0)
        {
            this.currentTimer--;
            yield return delay;
        }
    }
}
