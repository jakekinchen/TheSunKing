using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;

    public InputSettings inputSettings;

    public TMP_InputField mouseSensitivity;
    public UnityEngine.UI.Slider mouseSmoothingSlider;


    // Start is called before the first frame update
    void Awake()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        mouseSensitivity.text = inputSettings.mouseSensitivity + "";
        mouseSmoothingSlider.value = inputSettings.mouseSmoothing;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        int sensitivity;
        if (int.TryParse(mouseSensitivity.text, out sensitivity))
        {
            inputSettings.mouseSensitivity = sensitivity;
        }

        inputSettings.mouseSmoothing = mouseSmoothingSlider.value;

        inputSettings.SaveSettings();

        if (inputSettings.lockCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void GotToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}


