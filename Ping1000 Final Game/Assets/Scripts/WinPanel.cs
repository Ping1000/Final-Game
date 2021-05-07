using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinPanel : MonoBehaviour {
    [SerializeField]
    private TextMesh winText;
    [SerializeField]
    private SpriteRenderer winSprite;

    [Tooltip("A list of objects to deactivate when showing win screen.")]
    public List<GameObject> objsToDeactivate;

    // Start is called before the first frame update
    void Start() {
        // gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    private void SetSpriteAlpha(float newAlpha) {
        Color c = winSprite.color;
        winSprite.color = new Color(c.r, c.g, c.b, newAlpha);
    }

    private void SetTextAlpha(float newAlpha) {
        Color c = winText.color;
        winText.color = new Color(c.r, c.g, c.b, newAlpha);
    }

    public void ShowWinScreen(bool didWin, string msg = "") {
        if (msg == "") {
            winText.text = didWin ? "You met your daily quota!" :
                "You did not meet your daily quota.";
        } else {
            winText.text = msg;
        }
        gameObject.SetActive(true);
        winSprite.color = new Color(1, 1, 1, 0);
        if (objsToDeactivate != null) {
            foreach (GameObject obj in objsToDeactivate)
                obj.SetActive(false);
        }
        foreach (Basket basket in FindObjectsOfType<Basket>()) {
            basket.gameObject.SetActive(false);
        }
        LeanTween.value(winText.gameObject, SetTextAlpha, 0, 1, 3);
        LeanTween.value(winSprite.gameObject, SetSpriteAlpha, 0, 1, 3).setOnComplete(() => {
            if (didWin)
                LevelController.instance.StartNextDay();
            else
                LevelController.instance.RestartDay();
        });
    }

    public void ShowWolfScene() {
        ShowWinScreen(false,
            "You gave the wolf a basket.\nThey went on to terrorize the village.");
    }

    /// <summary>
    /// Fades out the win screen text, and fades in with newText. The total
    /// animation time (out + in) is transitionTime (seconds)
    /// </summary>
    /// <param name="newText"></param>
    /// <param name="transitionTime"></param>
    public void TextFadeOutIn(string newText, float transitionTime = 2f) {
        LeanTween.value(winText.gameObject, SetTextAlpha, 1, 0, transitionTime / 2).
            setOnComplete(() => {
                winText.text = newText;
                LeanTween.value(winText.gameObject, SetTextAlpha, 0, 1, transitionTime / 2);
            });
    }
}
