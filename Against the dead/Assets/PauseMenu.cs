using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Cursor.lockState = CursorLockMode.None;
            if (GameIsPaused)
            {
                Back();
            }
            else
            {
                Pause();
            }
        }
    }
    
    public void Back()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadDidacticiel()
    {
        SceneManager.LoadScene("Menu/Didacticiel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
