using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunction animatorFunction;
    [SerializeField] int thisIndex;
    //[SerializeField] AudioManager audioManager;

    int buildIndex = 1;

    public float waitTime;

    public GameObject settings;

    // Update is called once per frame
    void Update()
    {
        if (menuButtonController == null)
        {
            menuButtonController = transform.parent.GetComponent<MenuButtonController>();
            menuButtonController.index = 0;
        }
        if(menuButtonController.index == null)
        {
            // menuButtonController = transform.parent.GetComponent<MenuButtonController>();
            // animator = transform.GetComponent<Animator>();
            // animatorFunction = transform.GetComponent<AnimatorFunction>();
            Debug.Log("MenuButtonController index is null in "+ gameObject.name);
        }else if (thisIndex == null){
            Debug.Log("thisIndex is null in "+ gameObject.name);
        }
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if (Input.GetAxis ("Submit") == 1)
            {
                animator.SetBool("pressed", true);
                if (thisIndex == 0)
                {
                    SceneManager.LoadScene(buildIndex);
                }
                else if (thisIndex == 1) {
                    settings.SetActive(true);
                } else {
                    //UnityEditor.EditorApplication.isPlaying = false;
                    Application.Quit();
                }
            } else if (animator.GetBool ("pressed")) {
                animator.SetBool("pressed", false);
                animatorFunction.disableOnce = true;
            }
        } else {
            animator.SetBool("selected", false);
        }
    }

    //IEnumerator ChangeScene()
    //{
    //    //fadeout volume here
    //    audioManager.GetComponent<sounds>;
    //yield return new WaitForSeconds(waitTime);
    //SceneManager.LoadScene(buildIndex);
    //}
}
