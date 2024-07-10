using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGameButton()
    {
        SceneManager.LoadSceneAsync("EntranceHall");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
