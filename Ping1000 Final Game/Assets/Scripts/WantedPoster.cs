using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WantedPoster : MonoBehaviour
{
    private PersonFeatures _wolfFeatures;
    public PersonFeatures WolfFeatures { get {
            return _wolfFeatures;
        } set {
            _wolfFeatures = value;
            if (value == null)
                Destroy(gameObject);
        } }

    [Tooltip("Should have a text object as a child")]
    public GameObject fullWolfPoster;
    Vector3 toPos;
    Vector3 toScale;
    private bool waitingToHidePoster;

    private void Start() {
        WolfFeatures = LevelController.instance.wolfFeatures[LevelController.dayIdx];
        waitingToHidePoster = false;
        toScale = fullWolfPoster.transform.localScale;
        toPos = fullWolfPoster.transform.position;
    }

    private void Update() {
        if (waitingToHidePoster && Input.GetMouseButtonDown(0)) {
            HidePoster();
        }
    }

    public void ShowPoster() {
        fullWolfPoster.SetActive(true);
        fullWolfPoster.transform.position = gameObject.transform.position;
        fullWolfPoster.transform.localScale = Vector3.zero;
        fullWolfPoster.GetComponentInChildren<TextMesh>().text = GetWantedPosterText();

        LeanTween.scale(fullWolfPoster, toScale, .5f).setEaseInOutQuint();
        LeanTween.move(fullWolfPoster, toPos, .5f).setEaseInOutQuint().
            setOnComplete(() => waitingToHidePoster = true);
    }

    public void HidePoster() {
        waitingToHidePoster = false;
        LeanTween.scale(fullWolfPoster, Vector3.zero, .5f).setEaseInCirc();
        LeanTween.move(fullWolfPoster, gameObject.transform.position, .5f).
            setEaseInCirc().setOnComplete(() => fullWolfPoster.SetActive(false));
    }

    private string GetWantedPosterText() {
        string res = "<color=red>WANTED</color>\nBig Bad Wolf Has:\n";

        Debug.Log(_wolfFeatures.ToString());

        foreach (string s in _wolfFeatures.ToString().Split(',')) {
            res += (s + "\n");
        }

        return res;
    }
}
