using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CollectibleCounter : MonoBehaviour
{
    public int collectedObjectsCount = 0;
    public PlayerController playerController;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DeactivateOnPlayerCollision>())
        {
            //collectedObjectsCount++;
            //addEnergy();
            //UpdateCollectibleCounter();
        }
    }

    public void UpdateCollectibleCounter()
    {
       collectedObjectsCount += 1;
       playerController.energy += 50;
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
