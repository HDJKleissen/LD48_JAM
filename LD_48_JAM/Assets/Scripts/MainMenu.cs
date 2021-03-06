using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicSystemPlayer.Instance.SetMenuMusic();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        LoadScene("SampleScene");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
