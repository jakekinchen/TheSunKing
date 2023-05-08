using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject puzzleCanvas;
    public GameObject xGame;
    public GameObject zGame;
    public Camera playerCamera;
    public Camera puzzleCamera;

    public bool zGameActive = false;
    public bool xGameActive = false;

    //public GameManager gameManager; // Commented out

    public bool gameActive = false;

    private GameObject ship;
    private GameObject simulation;

    void Start()
    {
        
        puzzleCanvas.SetActive(false);
        zGame.SetActive(false);
        xGame.SetActive(false);
        puzzleCamera.enabled = false;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !xGameActive)
        {
            zGameActive = !zGameActive;
            gameActive = !gameActive;
            ToggleGame(gameActive);
            zGame.SetActive(zGameActive);
        }if (Input.GetKeyDown(KeyCode.X) && !zGameActive)
        {
            xGameActive = !xGameActive;
            gameActive = !gameActive;
            ToggleGame(gameActive);
            xGame.SetActive(xGameActive);
        }

    }

    public void ActivateZGame()
    {
        zGameActive = true;
        gameActive = true;
        ToggleGame(gameActive);
        zGame.SetActive(zGameActive);
    }

    public void ActivateWin()
    {
        Debug.Log("Win activated");
        zGameActive = false;
        xGameActive = false;
        gameActive = false;
        ToggleGame(false);
        zGame.SetActive(false);
        xGame.SetActive(false);
    }

    void ToggleGame(bool active)
    {
        puzzleCanvas.SetActive(active);
        puzzleCamera.enabled = active;
        playerCamera.enabled = !active;
        FindThingsAndDeactivate(active);
        simulation.GetComponent<NBodySimulation>().pauseSimulation = active;
        simulation.SetActive(!active);
        ship.SetActive(!active);
        if (active)
        {
            Debug.Log("Game is active");
           Time.timeScale = 0;
        }else
        {
            Debug.Log("Game is inactive");
            Time.timeScale = 1;
        }
        
        /*
        // Commented out the level-related functionality
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
        */
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
