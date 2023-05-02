using UnityEngine;
using TMPro;

public class CollectibleCounter : MonoBehaviour
{
    public int collectedObjectsCount = 0;
    public TextMeshProUGUI collectibleCountTextMeshPro;
    public PlayerController playerController;

    private void Start()
    {
        UpdateCollectibleCountText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DeactivateOnPlayerCollision>())
        {
            collectedObjectsCount++;
            addEnergy();
            UpdateCollectibleCountText();
        }
    }

    private void UpdateCollectibleCountText()
    {
        collectibleCountTextMeshPro.text = $"Crystals: {collectedObjectsCount}";
    }

    private void addEnergy()
    {
        playerController.energy += 20;
    }

}
