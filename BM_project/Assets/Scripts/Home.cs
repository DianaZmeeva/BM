using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    //возвращение к стартовому экрану игры
    void OnMouseDown()
    {
        SceneManager.LoadScene("Start");
    }
}
