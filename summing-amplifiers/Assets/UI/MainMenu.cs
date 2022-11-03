using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private UIDocument _doc;
    private Button _mixerButton;
    private Button _gameButton;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _mixerButton = _doc.rootVisualElement.Q<Button>("Game1");
        _gameButton = _doc.rootVisualElement.Q<Button>("Game2");

        _mixerButton.clicked += SinewaveFFT;
        _gameButton.clicked += PlayGame;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("PlayGame");
    }


    public void SinewaveFFT()
    {
        SceneManager.LoadScene("SinewaveFFT");
    }
}
