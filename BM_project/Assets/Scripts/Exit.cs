using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    //выход из игры
    void OnMouseDown()
    {
        Application.Quit();
    }
}
