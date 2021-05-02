using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinPanel : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI winText;
    private Image winPanel;

    [Tooltip("A list of objects to deactivate when showing win screen.")]
    public List<GameObject> objsToDeactivate;

    // Start is called before the first frame update
    void Start() {
        winPanel = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    public void ShowWinScreen(bool didWin, string msg = "") {
        if (msg == "") {
            winText.text = didWin ? "You met your daily quota!" :
                "You did not meet your daily quota.";
        } else {
            winText.text = msg;
        }
        gameObject.SetActive(true);
        winPanel.color = new Color(0, 0, 0, 0);
        if (objsToDeactivate != null) {
            foreach (GameObject obj in objsToDeactivate)
                obj.SetActive(false);
        }
        foreach (Basket basket in FindObjectsOfType<Basket>()) {
            basket.gameObject.SetActive(false);
        }
        LeanTween.alpha(winPanel.rectTransform, 1, 3).setOnComplete(() => {
            if (didWin)
                LevelController.instance.StartNextDay();
            else
                LevelController.instance.RestartDay();
        });
    }

    public void ShowWolfScene() {
        ShowWinScreen(false,
            "You gave the wolf a basket. They went on to terrorize the village.");
    }

    /// <summary>
    /// Fades out the win screen text, and fades in with newText. The total
    /// animation time (out + in) is transitionTime (seconds)
    /// </summary>
    /// <param name="newText"></param>
    /// <param name="transitionTime"></param>
    public void TextFadeOutIn(string newText, float transitionTime = 2f) {
        LeanTween.alphaText(winText.rectTransform, 0, transitionTime / 2).
            setOnComplete(() => {
                winText.text = newText;
                LeanTween.alphaText(winText.rectTransform, 1, transitionTime / 2);
            });
    }
}
