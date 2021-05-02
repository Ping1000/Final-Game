using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Tooltip("Non-NONE features will be randomized in the baskets. Each" +
        "PersonFeatures object corresponds to the day's features.")]
    public List<PersonFeatures> dailyFeatures;
    [Tooltip("Non-NONE features will be assigned to the wolf for each day.")]
    public List<PersonFeatures> wolfFeatures;
    [Tooltip("Quota to meet for aech day.")]
    public List<int> dailyQuotas;
    public WinPanel winObj;

    public static int dayIdx { get; private set; }

    public static LevelController instance;

    private void Awake() {
        instance = this;
    }

    public static PersonFeatures GetDailyFeatures () {
        return instance.dailyFeatures[dayIdx];
    }

    public static int GetDailyQuota() {
        return instance.dailyQuotas[dayIdx];
    }

    public static PersonFeatures GetDailyWolfFeatures() {
        return instance.wolfFeatures[dayIdx];
    }

    public void OnDayCompleted() {
        winObj.ShowWinScreen(GameManager.instance.CorrectBaskets >=
            GameManager.instance.quota);
    }

    public void StartNextDay() {
        dayIdx++;
        float transitionTime = 2f;
        winObj.TextFadeOutIn("Day " + (dayIdx + 1), transitionTime);
        if (dayIdx >= dailyFeatures.Count)
            Invoke("GoToWinScene", transitionTime + 1);
        else
            Invoke("ReloadScene", transitionTime + 1);
    }

    public void RestartDay() {
        float transitionTime = 2f;
        winObj.TextFadeOutIn("Let's try again.", transitionTime);
        Invoke("ReloadScene", transitionTime + 1);
    }

    private void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToWinScene() {
        SceneManager.LoadScene("Win Scene");
    }
}
