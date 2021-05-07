using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource src1;
    public float src1FadeTime;
    public AudioSource src2;
    public float src2FadeTime;

    public AudioSource src3;
    public float src3FadeTime;
    public AudioSource src4;
    public float src4FadeTime;

    public static MusicManager instance;
    public static float maxVolume = 0.2f;

    public static bool hasPlayerSwappped = false;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        instance = this;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator FadeIn(AudioSource src, float fadeTime) {
        src.volume = 0;
        if (!src.isPlaying)
            src.Play();
        for (float progress = 0; progress < fadeTime; progress += Time.deltaTime) {
            src.volume = Mathf.Lerp(0, maxVolume, progress / fadeTime);
            yield return null;
        }
    }

    IEnumerator FadeOut(AudioSource src, float fadeTime, bool stopOnComplete = false) {
        for (float progress = 0; progress < fadeTime; progress += Time.deltaTime) {
            src.volume = Mathf.Lerp(maxVolume, 0, progress / fadeTime);
            yield return null;
        }
        if (stopOnComplete)
            src.Stop();
    }

    public void PlayEndRush()
    {
        if (src4.isPlaying) { return; }
        Debug.Log("End Rush Music Playing!");
        src4FadeTime = 0.5f;
        //src1FadeTime = 0.5f;
        //StartCoroutine(FadeIn(src4, src2FadeTime));
        //StartCoroutine(FadeOut(src1, src1FadeTime, true));
        src4.Play();
    }

    public void StopEndRush()
    {
        src4FadeTime = 0.5f;
        src4.Stop();
        //StartCoroutine(FadeOut(src4, src2FadeTime));
    }

    void OnSceneChanged(Scene current, Scene next) {
        //Debug.Log("Scene changed");
        switch (next.name) {
            case "Beta Scene":
                //Debug.Log("Trying BG music");
                AudioClip clip = Resources.Load("Audio/Music/BGM1") as AudioClip;
                AudioClip clip2 = Resources.Load("Audio/SFX/Ambient_Crowd") as AudioClip;
                AudioClip clip3 = Resources.Load("Audio/SFX/Ambient_Wind") as AudioClip;

                if (src1.clip != clip && src2.clip != clip)
                {
                    //Debug.Log("Attempting playing BG music");
                    src1.Stop();
                    src2.Stop();
                    src3.Stop();
                    src1.clip = clip;
                    src1.volume = maxVolume;
                    src2.clip = clip2;
                    src3.clip = clip3;
                    src4.clip = Resources.Load("Audio/Music/End Rush") as AudioClip;
                    //src4.Play();
                    src1.Play();
                    src2.Play();
                    src3.Play();
                }
                break;
            case "Downward Box Tutorial":
                /*AudioClip clip = Resources.Load("Audio/Music/Ambient") as AudioClip;
                if (src1.clip != clip && src2.clip != clip) {
                    src1.Stop();
                    src1.clip = clip;
                    src1.volume = maxVolume;
                    src1.Play();
                }*/
                break;
            // first half of intermediate levels case
            // second half of intermediate levels case
            case "Boss Development":
                /*src1.Stop();
                src2.Stop();
                SFXManager.PlayNewSound("Audio/Music/Boss_Intro", volumeType.quiet, 1,
                    instance.transform);
                Invoke("BossMusicBegin", 6.25f);
                src1.clip = Resources.Load("Audio/Music/Boss_Phase_1") as AudioClip;
                src2.clip = Resources.Load("Audio/Music/Boss_Phase_2") as AudioClip;*/
                break;
            case "Main Menu":
                Transform sfxChild = transform.Find("SFX Player(Clone)");
                if (sfxChild != null)
                    Destroy(sfxChild.gameObject);
                break;
        }
    }

   /* void BossMusicBegin() {
        src1.volume = maxVolume;
        src1.Play();
    }

    public void EnragedMusicBegin() {
        src2FadeTime = 0.5f;
        src1FadeTime = 0.5f;
        StartCoroutine(FadeIn(src2, src2FadeTime));
        StartCoroutine(FadeOut(src1, src1FadeTime, true));
    }

    public void FinalMusicBegin() {
        src1.clip = Resources.Load("Audio/Music/Boss_Phase_3") as AudioClip;
        StartCoroutine(FadeOut(src2, src2FadeTime, true));
        StartCoroutine(FadeIn(src1, src1FadeTime));
    }*/
}
