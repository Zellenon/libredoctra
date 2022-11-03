using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private UIDocument _doc;
    private Button _randomButton;
    private Button _customButton;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _randomButton = _doc.rootVisualElement.Q<Button>("Game1");
        _customButton = _doc.rootVisualElement.Q<Button>("Game2");

        _randomButton.clicked += RandomGame;
        _customButton.clicked += CustomGame;
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

    public void RandomGame()
    {
        SceneManager.LoadScene("RandomPeriodicScene 1");
    }


    public void CustomGame()
    {
        SceneManager.LoadScene("CustomFuncApproxScene");
    }
}
