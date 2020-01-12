using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [CanBeNull] 
    public Button startButton, aboutButton, exitButton, homeButton;
    void Start()
    {
        if (CheckButtonOnNull(startButton))
            startButton.onClick.AddListener(StartLevel);
        if (CheckButtonOnNull(aboutButton))
            aboutButton.onClick.AddListener(LoadAboutScene);
        if(CheckButtonOnNull(exitButton))
            exitButton.onClick.AddListener(Exit);
        if (CheckButtonOnNull(homeButton))
            homeButton.onClick.AddListener(LoadStartScene);
    }


    private bool CheckButtonOnNull(Button button)
    {
        return button != null;
    }

    private void StartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void LoadAboutScene()
    {
        SceneManager.LoadScene("AboutScene");
    }

    private void LoadStartScene()
    {
        SceneManager.LoadScene("Start");
    }

    private void Exit()
    {
        Application.Quit();
    }
}
