using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public List<PersonFeatures> dailyFeatures;
    public List<int> dailyQuotas;
    public WinPanel winObj;

    private static int day;

    public static LevelController instance;

    private void Awake() {
        instance = this;
    }

    public static PersonFeatures GetDailyFeatures () {
        return instance.dailyFeatures[day];
    }

    public static int GetDailyQuota() {
        return instance.dailyQuotas[day];
    }

    public void OnDayCompleted() {
        winObj.ShowWinScreen(GameManager.instance.CorrectBaskets >=
            GameManager.instance.quota);
    }

    public void StartNextDay() {
        day++;
        float transitionTime = 2f;
        winObj.TextFadeOutIn("Day " + (day + 1), transitionTime);
        Invoke("ReloadScene", transitionTime + 1);
    }

    private void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
