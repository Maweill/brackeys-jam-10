using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu : MonoBehaviour
{
    private void ExitGame()
    {
        Application.Quit();
        Debug.Log("Игра закрылась");
    }
}
