using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Button NewGameButton;

    public Button LoadGameButton;

    public Button JoinGameButton;

    public Button QuitButton;

    public string NewGameSceneName;

    public GameObject LoadGameMenu;

    public void NewGame()
    {
        SceneManager.LoadScene(NewGameSceneName);
    }

    public void LoadGame()
    {
        LoadGameMenu.SetActive(true);
    }

    public void JoinGame()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
