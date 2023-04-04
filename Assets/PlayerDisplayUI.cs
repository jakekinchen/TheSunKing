using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDisplayUI : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI upVelocityText;
    public TextMeshProUGUI downVelocityText;
    public TextMeshProUGUI isFlyingText;


    void Update()
    {
        if (playerController != null && energyText != null)
        {//also list player's upwards and downwards velocity
            energyText.text = "Energy: " + playerController.energy.ToString("F2");
        }
        if (playerController != null && upVelocityText != null)
        {
            upVelocityText.text = "Up Velocity: " + playerController.upVelocity.ToString("F2");
        }
        if (playerController != null && downVelocityText != null)
        {
            downVelocityText.text = "Down Velocity: " + playerController.downVelocity.ToString("F2");
        }
        if (playerController != null && isFlyingText != null)
        {
            isFlyingText.text = "Is Flying: " + playerController.isFlying.ToString();
        }
    }
}
