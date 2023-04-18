using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Shape playerShape;

    public void RotateButtonClicked()
    {
        playerShape.Rotate();
    }

    public void FlipButtonClicked()
    {
        playerShape.Flip();
    }
}
