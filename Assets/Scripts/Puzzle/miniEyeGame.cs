using UnityEngine;
using UnityEngine.UI;

public class miniEyeGame : MonoBehaviour
{
    public Button leftButton;
    public Button middleButton;
    public Button rightButton;
    public Image leftEye;
    public Image middleEye;
    public Image rightEye;
    public Sprite openEyeSprite; 
    public Sprite closedEyeSprite;


    void Start()
    {
        leftButton.onClick.AddListener(() => CloseEyes(true, false, true));
        middleButton.onClick.AddListener(() => CloseEyes(true, true, false));
        rightButton.onClick.AddListener(() => CloseEyes(false, false, true));
    }

    void CloseEyes(bool closeLeftEye, bool closeMiddleEye, bool closeRightEye)
    {
        setEyeState(leftEye, closeLeftEye);
        setEyeState(middleEye, closeMiddleEye); 
        setEyeState(rightEye, closeRightEye);
    }

    void setEyeState(Image eye, bool closed){
        eye.sprite = closed ? closedEyeSprite : openEyeSprite;
    }
}