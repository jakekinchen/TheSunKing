using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eyePuzzle : MonoBehaviour
{
    public Image leftEye;
    public Image middleEye;
    public Image rightEye;
    public Sprite openEyeSprite;
    public Sprite closedEyeSprite;
    public Text winText;
    public GameObject winPanel; 
    public GameController gameController;
    private bool won = false;
    

    private bool[] eyeStates;

    void Start()
    {
        eyeStates = new bool[3]; // false means closed, true means open
        CloseEyes();
        winText.enabled = false;
       winPanel.SetActive(false);
    }

    public void ToggleEye(int eyeIndex)
    {
        if (eyeIndex == 0)
        {
            eyeStates[0] = !eyeStates[0];
            eyeStates[2] = !eyeStates[2];
        }
        else if (eyeIndex == 1)
        {
            eyeStates[0] = !eyeStates[0];
            eyeStates[1] = !eyeStates[1];
        }
        else
        {
            eyeStates[2] = !eyeStates[2];
        }

        UpdateAllEyes();
        CheckWinCondition();
    }

    void UpdateAllEyes()
    {
        for (int i = 0; i < eyeStates.Length; i++)
        {
            UpdateEye(i);
        }
    }

    void UpdateEye(int eyeIndex)
    {
        Image eye = eyeIndex == 0 ? leftEye : eyeIndex == 1 ? middleEye : rightEye;
        eye.sprite = eyeStates[eyeIndex] ? openEyeSprite : closedEyeSprite;
    }

    void CloseEyes()
    {
        for (int i = 0; i < eyeStates.Length; i++)
        {
            eyeStates[i] = false;
            UpdateEye(i);
        }
    }

    void CheckWinCondition()
    {
        bool allEyesOpen = true;
        for (int i = 0; i < eyeStates.Length; i++)
        {
            if (!eyeStates[i])
            {
                allEyesOpen = false;
                break;
            }
        }

        //winText.enabled = allEyesOpen;
       //winPanel.SetActive(allEyesOpen);
       // StartCoroutine(WinWait());
       
        if (allEyesOpen && !won)
        {
            gameController.ActivateWin();
            Debug.Log("game controller activated win");
            won = true;
        }
    }

    IEnumerator WinWait()
    {
        yield return new WaitForSeconds(4);
        //winText.enabled = true;
    }
}
