using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Filter", 0f);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Filter", 1f);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Filter", 0f);
        SceneManager.LoadScene("MainMenu");
    }
}
