using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button startButton, aboutButton, exitButton;
    void Start()
    {
        startButton.onClick.AddListener(StartLevel);
        aboutButton.onClick.AddListener(About);
        exitButton.onClick.AddListener(Ex);
    }

    void StartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void About()
    {
        SceneManager.LoadScene("AboutScene");
    }

    void Ex()
    {
        Application.Quit();
    }
}
