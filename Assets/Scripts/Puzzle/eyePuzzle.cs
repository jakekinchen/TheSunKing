using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eyePuzzle : MonoBehaviour
{
    // public Button leftButton;
    // public Button middleButton;
    // public Button rightButton;
    public Image leftEye;
    public Image middleEye;
    public Image rightEye;
    public Sprite openEyeSprite;
    public Sprite closedEyeSprite;

    void Start()
    {
        CloseEyes();
        // leftButton.onClick.AddListener(() => OpenEyes(true, false, true));
        // middleButton.onClick.AddListener(() => OpenEyes(true, true, false));
        // rightButton.onClick.AddListener(() => OpenEyes(false, false, true));
    }

     public void OpenEye(int eyeIndex)
    {
        switch (eyeIndex)
        {
            case 0:
                leftEye.sprite = openEyeSprite;
                rightEye.sprite = openEyeSprite;
                break;
            case 1:
                leftEye.sprite = openEyeSprite;
                middleEye.sprite = openEyeSprite;
                break;
            case 2:
                rightEye.sprite = openEyeSprite;
                break;
        }
    }

    // void SetEyeState(Image eye, bool open)
    // {
    //     eye.sprite = open ? closedEyeSprite : openEyeSprite;
    // }

   public void CloseEyes()
    {
        leftEye.sprite = closedEyeSprite;
    middleEye.sprite = closedEyeSprite;
    rightEye.sprite = closedEyeSprite;
    }
}