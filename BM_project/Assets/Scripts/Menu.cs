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
    
    //открытие cцены Игры
    void StartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }

    //открытие cцены ОБ Игре
    void About()
    {
        SceneManager.LoadScene("AboutScene");
    }

    //выход из игры
    void Ex()
    {
        Application.Quit();
    }
}
