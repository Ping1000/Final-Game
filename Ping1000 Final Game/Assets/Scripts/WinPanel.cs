using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinPanel : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI winText;

    [Tooltip("A list of objects to deactivate when showing win screen.")]
    public List<GameObject> objsToDeactivate;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ShowWinScreen(bool didWin) {
        winText.text = didWin ? "You met your daily quota!" :
            "You did not meet your daily quota.";
        gameObject.SetActive(true);
        if (objsToDeactivate != null) {
            foreach (GameObject obj in objsToDeactivate)
                obj.SetActive(false);
        }
        foreach (Basket basket in FindObjectsOfType<Basket>()) {
            basket.gameObject.SetActive(false);
        }
    }
}
