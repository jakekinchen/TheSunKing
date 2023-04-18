using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject puzzle1Canvas;
    public Camera playerCamera;
    public Camera puzzle1Camera;

    public GameManager gameManager;

    public bool gameActive = false;

    private GameObject ship;
    private GameObject simulation;

    void Start()
    {
        puzzle1Canvas.SetActive(false);
        puzzle1Camera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            gameActive = !gameActive;
            ToggleGame(gameActive);
        }
    }

    void ToggleGame(bool active)
    {
        puzzle1Canvas.SetActive(active);
        puzzle1Camera.enabled = active;
        playerCamera.enabled = !active;
        FindThingsAndDeactivate(active);
        simulation.SetActive(!active);
        ship.SetActive(!active);
        if (active)
        {
            Time.timeScale = 0;
        }else
        {
            Time.timeScale = 1;
        }
        
       
        if (active)
        {
            gameManager.LoadLevel(gameManager.currentLevel);
        }
        else
        {
            // Reset the game state and deactivate the current level
            gameManager.currentLevel = 1;
            if (gameManager.currentLevelInstance != null)
            {
                gameManager.currentLevelInstance.SetActive(false);
            }
        }
    }

    public void FindThingsAndDeactivate(bool active){
        if (GameObject.Find("Ship")){
            ship = GameObject.Find("Ship");
            
        }
        if (GameObject.Find("Body Simulation")){
            simulation = GameObject.Find("Body Simulation");
            
        }
        
        
    }
}
