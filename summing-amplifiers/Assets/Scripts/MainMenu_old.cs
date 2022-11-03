using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_old : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("PlayGame");
    }


    public void SinewaveFFT()
    {
        SceneManager.LoadScene("SinewaveFFT");
    }


    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }



}
