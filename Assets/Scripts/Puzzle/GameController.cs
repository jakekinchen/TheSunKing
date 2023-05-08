using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject puzzleCanvas;
   // public GameObject xGame;
    public GameObject zGame;
    public Camera playerCamera;
    public Camera puzzleCamera;
    public GameObject player;

    //public bool zGameActive = false;
    //public bool xGameActive = false;

    //public GameManager gameManager; // Commented out

    public bool gameActive = false;

    private GameObject ship;
    private GameObject simulation;

    void Start()
    {
        //zGame.SetActive(false);
        puzzleCamera.enabled = false;
        puzzleCanvas.SetActive(false);
        
        //xGame.SetActive(false);
        
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //zGameActive = !zGameActive;
            gameActive = !gameActive;
            ToggleGame(gameActive);
            //zGame.SetActive(zGameActive);
        }if (Input.GetKeyDown(KeyCode.X))
        {
           // xGameActive = !xGameActive;
            gameActive = !gameActive;
            ToggleGame(gameActive);
           // xGame.SetActive(xGameActive);
        }
    }

    public void ActivateZGame()
    {
        //zGameActive = !zGameActive;
        gameActive = !gameActive;
        ToggleGame(gameActive);
       // zGame.SetActive(gameActive);
        Debug.Log("Z Game Activated from GameController");
    }

    public void ActivateWin()
    {
        //zGameActive = !zGameActive;
        gameActive = !gameActive;
        ToggleGame(false);
        //zGame.SetActive(gameActive);
        Debug.Log("Win Activated from GameController");
        player.GetComponent<PlayerController>().ActivateSunCrystal();
    }

    void ToggleGame(bool active)
    {
        puzzleCanvas.SetActive(active);
        //zGame.SetActive(active);

        puzzleCamera.enabled = active;
        playerCamera.enabled = !active;
        FindThingsAndDeactivate(active);
        simulation.GetComponent<NBodySimulation>().pauseSimulation = active;
        player.SetActive(!active);
        simulation.SetActive(!active);
        ship.SetActive(!active);
        if (active)
        {
            Debug.Log("Time set to 0");
           Time.timeScale = 0;
        }else
        {
            Debug.Log("Time set to 1");
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
