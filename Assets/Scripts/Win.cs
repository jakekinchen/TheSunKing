using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    private bool humanReached = false;
    private bool ghostReached = false;

    // Update is called once per frame
    void Update()
    {
        if(humanReached && ghostReached)
        {
            LevelLoader.instance.LoadNextLevel();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Human")
            humanReached = true;
        if (other.gameObject.tag == "Ghost")
            ghostReached = true;
    }

}
