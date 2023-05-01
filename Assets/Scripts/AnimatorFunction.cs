using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunction : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    public bool disableOnce;


    void PlaySound(AudioClip whichSound)
    {
        if (!disableOnce)
        {
            menuButtonController.audioSource.PlayOneShot(whichSound);
        } else {
            disableOnce = false;
        }
    }
}
