using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TimerController : MonoBehaviour
{
    [SerializeField] private float updateTime = 1f;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private int dayEndHour = 17;
    [SerializeField] private int dayEndMinute = 0;

    private int hours;
    private int minutes;

    public delegate void TimerHit();

    public event TimerHit OnDayEnd;

    private void Start()
    {
        StartCounter(09, 00);
    }

    bool isCounting = false;
    Coroutine C_Counting;

    private void StartCounter(int startingHour, int startingMinute)
    {
        if (isCounting) return;

        isCounting = true;

        if (C_Counting != null) return;

        C_Counting = StartCoroutine(IncrementTime(startingHour, startingMinute));
    }

    private void EndCounting()
    {
        if (!isCounting) return;

        isCounting = false;

        if (C_Counting == null) return;

        StopCoroutine(C_Counting);
    }

    private IEnumerator IncrementTime(int startingHour, int startingMinute)
    {
        hours = startingHour;
        minutes = startingMinute;

        while (isCounting)
        {
            minutes++;

            if (minutes >= 60)
            {
                hours++;

                if(hours >= 24)
                {
                    hours = 0;
                }

                minutes = 0;
            }

            timerText.text = string.Format("{0:00}:{1:00}", hours, minutes);

            if(hours == dayEndHour && minutes == dayEndMinute)
            {
                OnDayEnd?.Invoke();
                break;
            }

            yield return new WaitForSeconds(updateTime);
        }
    }
}
