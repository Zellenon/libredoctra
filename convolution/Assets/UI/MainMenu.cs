using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private UIDocument _doc;
    private Button _explorerButton;
    private Button _continuousButton;
    private Button _gameButton;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _explorerButton = _doc.rootVisualElement.Q<Button>("Explorer");
        _continuousButton = _doc.rootVisualElement.Q<Button>("Continuous");
        _gameButton = _doc.rootVisualElement.Q<Button>("Game");

        _explorerButton.clicked += Explorer;
        _continuousButton.clicked += ContinuousExplorer;
        _gameButton.clicked += Game;
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

    public void Explorer()
    {
        SceneManager.LoadScene("ConvolutionExplorer");
    }


    public void ContinuousExplorer()
    {
        SceneManager.LoadScene("ConvolutionExplorer");
    }

    public void Game()
    {
        SceneManager.LoadScene("ConvolutionDrawing");
    }
}
