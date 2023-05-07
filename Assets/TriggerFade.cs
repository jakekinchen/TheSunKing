using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TriggerFade : MonoBehaviour
{

    public AudioMixerSnapshot fadeOut;

    void Start()
    {
        fadeOut.TransitionTo(4);
    }

    private void Fade()
    {
        fadeOut.TransitionTo(4);
    }
}
