using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{

    [SerializeField]
    private AudioClip walkingSound;
    [SerializeField]
    private AudioClip flyingSound;
    [SerializeField]
    private AudioClip swimmingSound;
    [SerializeField]
    private AudioClip enterWater;
    [SerializeField]
    private AudioClip capeFlapping;

    [Range(0f, 1f)] public float walkingVolume = 1f;
    [Range(0f, 1f)] public float flyingVolume = 1f;
    [Range(0f, 1f)] public float swimmingVolume = 1f;
    [Range(0f, 1f)] public float capeVolume = 1f;
    [Range(0f, 1f)] public float enterWaterVolume = 1f;

    private AudioSource audioSource;
    private Coroutine loopCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public void PlaySound(string soundName)
    {
        AudioClip soundToPlay = null;
        float volume = 1f;

        switch (soundName)
        {
            case "walking":
                soundToPlay = walkingSound;
                volume = walkingVolume;
                break;
            case "flying":
                soundToPlay = flyingSound;
                volume = flyingVolume;
                break;
            case "swimming":
                soundToPlay = swimmingSound;
                volume = swimmingVolume;
                break;
            case "onWaterEnter":
                soundToPlay = enterWater;
                volume = enterWaterVolume;
                break;
            case "cape":
                soundToPlay = capeFlapping;
                volume = capeVolume;
                break;
            default:
                Debug.LogWarning("Sound: " + soundName + " not found!");
                break;
        }

        if (soundToPlay != null)
        {
            audioSource.PlayOneShot(soundToPlay, volume * audioSource.volume);
        }
    }

    public bool IsPlaying(string sound)
    {
        return audioSource.isPlaying;
    }

    public void StopPlaying()
    {
        audioSource.Stop();
    }

    //public void PlaySound(string action)
    //{
    //    if (loopCoroutine != null)
    //    {
    //        StopCoroutine(loopCoroutine);
    //        loopCoroutine = null;
    //    }

    //    switch (action)
    //    {
    //        case "walking":
    //            loopCoroutine = StartCoroutine(PlayLoopingSound(walkingSound));
    //            break;
    //        case "flying":
    //            audioSource.PlayOneShot(flyingSound);
    //            break;
    //        case "OnWaterEnter":
    //            audioSource.PlayOneShot(enterWater);
    //            break;
    //        case "swimming":
    //            loopCoroutine = StartCoroutine(PlayLoopingSound(swimmingSound));
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //IEnumerator PlayLoopingSound(AudioClip clip)
    //{
    //    while (true)
    //    {
    //        audioSource.PlayOneShot(clip);
    //        yield return new WaitForSeconds(clip.length);
    //    }
    //}

    //public void StopSound()
    //{
    //    if (loopCoroutine != null)
    //    {
    //        StopCoroutine(loopCoroutine);
    //        loopCoroutine = null;
    //    }
    //    audioSource.Stop();
    //}

    //public bool IsPlayingAnySound()
    //{
    //    return audioSource.isPlaying;
    //}



}
