using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioSource _src;

    public Slider musicSlider;
    public Slider sfxSlider;

    private static float sfxMultiplier = 1f;
    private static float musicMultiplier = 1f;

    private static float defaultVeryQuietVolume = .1f;
    private static float defaultQuietVolume = .2f;
    private static float defaultHalfVolume = .4f;
    private static float defaultLoudVolume = .6f;
    private static float defaultVeryLoudVolume = .8f;
    private static float defaultMusicVolume = 0.2f;

    // Start is called before the first frame update
    void Start() {
        _src.volume = defaultMusicVolume;
        musicSlider.value = musicMultiplier;
        sfxSlider.value = sfxMultiplier;
    }

    // Update is called once per frame
    void Update() {

    }

    public void TweenSubmenuOpen(RectTransform menu) {
        menu.gameObject.SetActive(true);
        menu.localScale = Vector3.zero;
        LeanTween.scale(menu, Vector3.one, 0.5f).setEaseOutBack();
    }

    public void TweenSubmenuClose(RectTransform menu) {
        LeanTween.scale(menu, Vector3.zero, 0.5f).setEaseInBack().
            setOnComplete(() => {
                menu.localScale = Vector3.one;
                menu.gameObject.SetActive(false);
            });
    }

    public void ScaleSFXVolume(float value) {
        SFXManager.veryQuietVolume = Mathf.Clamp(defaultVeryQuietVolume * value, 0, 1);
        SFXManager.quietVolume = Mathf.Clamp(defaultQuietVolume * value, 0, 1);
        SFXManager.halfVolume = Mathf.Clamp(defaultHalfVolume * value, 0, 1);
        SFXManager.loudVolume = Mathf.Clamp(defaultLoudVolume * value, 0, 1);
        SFXManager.veryLoudVolume = Mathf.Clamp(defaultVeryLoudVolume * value, 0, 1);
        sfxMultiplier = value;
    }

    public void ScaleMusicVolume(float value) {
        MusicManager.maxVolume = Mathf.Clamp(defaultMusicVolume * value, 0, 1);
        _src.volume = Mathf.Clamp(defaultMusicVolume * value, 0, 1);
        musicMultiplier = value;
    }

    public void Play(string level)
    {
        SceneManager.LoadScene(level);
    }
}
