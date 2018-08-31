using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    public Button NewGameButton;

    public Button LoadGameButton;

    public Button JoinGameButton;

    public Button QuitButton;

    public string NewGameSceneName;

    public GameObject LoadGameMenu;

    public void NewGame()
    {
        SceneManager.LoadScene("Waiting");
    }

    public void LoadGame() {
        Debug.Log("load");
    }

    public void JoinGame() {
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
