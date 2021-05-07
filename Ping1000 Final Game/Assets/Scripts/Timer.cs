using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public int daySeconds = 70;
    /// <summary>
    /// Real-time seconds for each "game second"
    /// </summary>
    [Range(0, float.PositiveInfinity)]
    public float secondLength = 1f;
    private float countTrigger;
    public TextMeshProUGUI timerText;
    /// <summary>
    /// Text to prepend before the timer.
    /// </summary>
    public string prefixText;

    private bool isCountingDown = false;

    public static Timer instance;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        countTrigger = 0f;
        SetTimerUI();
        // can make this not happen immediately
        StartCountdown();
    }

    public static void StartCountdown() {
        instance.isCountingDown = true;
    }

    public static void StopCountdown() {
        instance.isCountingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCountingDown) {
            countTrigger += Time.deltaTime;
            if (countTrigger >= secondLength) {
                daySeconds--;
                SetTimerUI();
                countTrigger = 0f;
                if (daySeconds <= 30) // TODO CHANGE BACK TO 30
                {
                    MusicManager.instance.PlayEndRush();
                }
                else
                {
                    MusicManager.instance.StopEndRush();
                }
                if (daySeconds <= 0) {
                    StopCountdown();
                    if (OnCountdownComplete != null) {
                        timerText.text = "Last customer!";
                        OnCountdownComplete();
                    }
                }
            }
        } else {
            countTrigger = 0f;
        }
    }

    public static event Action OnCountdownComplete;

    public void SetTimerUI() {
        int minutes = daySeconds / 60;
        int seconds = daySeconds % 60;
        string secondString = seconds >= 10 ? seconds.ToString() : "0" + seconds;

        timerText.text = prefixText + minutes + ":" + secondString;
    }
}
