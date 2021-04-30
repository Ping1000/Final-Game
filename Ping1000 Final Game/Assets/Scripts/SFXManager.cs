using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum volumeType {
    veryQuiet,
    quiet,
    half,
    loud,
    veryLoud
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public static float veryQuietVolume = 0.1f;
    public static float quietVolume = 0.2f;
    public static float halfVolume = 0.4f;
    public static float loudVolume = 0.6f;
    public static float veryLoudVolume = 0.8f;

    public AudioSource miscSource1;
    public AudioSource miscSource2;

 

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }


    public void PlayMainMenuSound(string path)
    {
        PlayNewSound(path, volumeType.loud);
    }

    /// <summary>
    /// Plays a specific song at the specified volume level.
    /// </summary>
    /// <param name="path">The path to the audio clip, starting in Resources</param>
    /// <param name="volumeMode">The volume mode/level to play at</param>
    public static void PlayNewSound(string path, volumeType volumeMode, float pitch = 1f,
        Transform parent = null)
    {
        if (path == null || path == "")
            return;

        GameObject sfxPlayer = Instantiate(Resources.Load("Audio/SFX Player") as GameObject, parent);
        AudioSource playerSrc = sfxPlayer.GetComponent<AudioSource>();
        playerSrc.clip = Resources.Load(path) as AudioClip;

        switch (volumeMode)
        {
            case volumeType.veryQuiet:
                playerSrc.volume = veryQuietVolume;
                break;
            case volumeType.quiet:
                playerSrc.volume = quietVolume;
                break;
            case volumeType.half:
                playerSrc.volume = halfVolume;
                break;
            case volumeType.loud:
                playerSrc.volume = loudVolume;
                break;
            case volumeType.veryLoud:
                playerSrc.volume = veryLoudVolume;
                break;
        }

        playerSrc.pitch = pitch;
        playerSrc.Play();
        Destroy(playerSrc.gameObject, playerSrc.clip.length);
    }

    IEnumerator FadeIn(AudioSource src, float fadeTime = 2f, float maxVolume = 1f)
    {
        if (!src.isPlaying)
            src.Play();
        for (float progress = 0; progress < fadeTime; progress += Time.deltaTime)
        {
            src.volume = Mathf.Lerp(0, maxVolume, progress / fadeTime);
            yield return null;
        }
    }

    IEnumerator FadeOut(AudioSource src, float fadeTime = 2f, float maxVolume = 1f)
    {
        for (float progress = 0; progress < fadeTime; progress += Time.deltaTime)
        {
            src.volume = Mathf.Lerp(maxVolume, 0, progress / fadeTime);
            yield return null;
        }
    }
}
