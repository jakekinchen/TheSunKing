using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManagerVer2 : MonoBehaviour
{
    private static SoundManagerVer2 _instance;

    public AudioMixer theMixer;

    public TMP_Text masterLabel, musicLabel, sfxLabel;

    public Slider masterSlider, musicSlider, sfxSlider;

    public AudioSource audioSource;

    private static bool keepFadingIn;
    private static bool keepFadingOut;

    private void Start()
    {
        float vol = 0f;
        theMixer.GetFloat("MasterVolParam", out vol);
        masterSlider.value = vol;
        theMixer.GetFloat("MusicVolParam", out vol);
        musicSlider.value = vol;
        theMixer.GetFloat("SFXVolParam", out vol);
        sfxSlider.value = vol;

        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
    }

    void Awake()
    {
        //if we don't have an [_instance] set yet
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    public void SetMasterVol()
    {
        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        theMixer.SetFloat("MasterVolParam", masterSlider.value);

        PlayerPrefs.SetFloat("MasterVolParam", masterSlider.value);
    }

    public void SetMusicVol()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        theMixer.SetFloat("MusicVolParam", musicSlider.value);

        PlayerPrefs.SetFloat("MusicVolParam", musicSlider.value);

    }

    public void SetSFXVol()
    {
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        theMixer.SetFloat("SFXVolParam", sfxSlider.value);

        PlayerPrefs.SetFloat("SFXVolParam", sfxSlider.value);
    }

    public void FadeOutAudio(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
