using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CollectibleCounter : MonoBehaviour
{
    public int collectedObjectsCount = 0;
    public PlayerController playerController;

    private void Start()
    {
        UpdateCollectibleCount();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DeactivateOnPlayerCollision>())
        {
            collectedObjectsCount++;
            addEnergy();
            UpdateCollectibleCount();
        }
    }

    private void UpdateCollectibleCount()
    {
       collectedObjectsCount += 1;
       if(collectedObjectsCount >= 3)
       {
           TriggerWin();
       }
    }

    private void addEnergy()
    {
        playerController.energy += 50;
    }

    private void TriggerWin()
    {
        Debug.Log("You win!");
        SceneManager.LoadScene("Win");
    }


}
