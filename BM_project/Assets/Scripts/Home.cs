using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("Start");
    }
}
