using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public GameObject player;
    Animator animator;

    private void Start()
    {
        player.GetComponent<Animator>();

    }

    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            Debug.Log("walkingAnimation!");
            animator.SetBool("isWalking", true);
        }
        //else
        //{
        //    Debug.Log("idle!");
        //    animator.SetBool("isWalking", false);
        //}
    }
}
