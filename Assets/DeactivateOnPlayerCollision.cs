using System.Collections;
using UnityEngine;

public class DeactivateOnPlayerCollision : MonoBehaviour
{
    [SerializeField] public ParticleSystem crystalParticleSystem;
    [SerializeField] public ParticleSystem crystalParticleSystem2;
    [SerializeField] private bool useAnimation = true;
    [SerializeField] private float popAnimationDuration = 1f;
    [SerializeField] private bool isPuzzleCrystal = false;
    [SerializeField] GameController gameController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (useAnimation)
            {
                StartCoroutine(PlayPopAnimationAndDeactivate());
            }
            else
            {
                StartCoroutine(stopLoopingAndDeactivate());
                //gameObject.SetActive(false);
            }
            if (isPuzzleCrystal)
            {
                gameObject.SetActive(false);
                gameController.ActivateZGame();
                
            }
        }
    }

    private IEnumerator stopLoopingAndDeactivate()
    {
        // Stop the particle system
       crystalParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Set looping to false
        var main = crystalParticleSystem.main;
        main.loop = false;
        main.duration = .5f;
        crystalParticleSystem.Play();
        // Wait for the duration and deactivate the game object
        yield return new WaitForSeconds(main.duration);
        gameObject.SetActive(false);
    }

    private IEnumerator PlayPopAnimationAndDeactivate()
    {
         // Stop the particle system
        crystalParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Set looping to false
        var main = crystalParticleSystem.main;
        main.loop = false;

        // Set the duration to popAnimationDuration
        main.duration = popAnimationDuration;

        // Change the particle system to have a single burst
        var emission = crystalParticleSystem.emission;
        emission.rateOverTime = 0;
        emission.burstCount = 1; // Add this line to set the burst count to 1 before setting the burst
        emission.SetBurst(0, new ParticleSystem.Burst(0f, 100));

        // //Change size over lifetime
        // var sizeOverLifetime = crystalParticleSystem.sizeOverLifetime;
        // sizeOverLifetime.enabled = true;
        // sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(0.1f, new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0)));

        //Color over lifetime
        var colorOverLifetime = crystalParticleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(
            new Gradient
            {
                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(0, 1) },
                colorKeys = new GradientColorKey[] { new GradientColorKey(main.startColor.color, 0), new GradientColorKey(Color.white, 1) }
            }
        );

        //Add rotation
        var rotationOverLifetime = crystalParticleSystem.rotationOverLifetime;
        rotationOverLifetime.enabled = true;
        rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(180, -180);

        //Add a force over lifetime
        var forceOverLifetime = crystalParticleSystem.forceOverLifetime;
        forceOverLifetime.enabled = true;
        forceOverLifetime.y = new ParticleSystem.MinMaxCurve(0.1f, 1f);

        // Play the particle system
        if (crystalParticleSystem2 != null){
        crystalParticleSystem2.Play();
        }

        crystalParticleSystem.Play();

        

        // Wait for the duration and deactivate the game object
        yield return new WaitForSeconds(popAnimationDuration);
        //set parent to null
        crystalParticleSystem.transform.parent = null;
        gameObject.SetActive(false);
    }
}
