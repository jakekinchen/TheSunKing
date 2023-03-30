using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPrefs : MonoBehaviour
{
    public AudioMixer theMixer;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolParam"))
        {
            theMixer.SetFloat("MasterVolParam", PlayerPrefs.GetFloat("MasterVolParam"));
        }

        if (PlayerPrefs.HasKey("MusicVolParam"))
        {
            theMixer.SetFloat("MusicVolParam", PlayerPrefs.GetFloat("MusicVolParam"));
        }

        if (PlayerPrefs.HasKey("SFXVolParam"))
        {
            theMixer.SetFloat("SFXVolParam", PlayerPrefs.GetFloat("SFXVolParam"));
        }
    }
}
