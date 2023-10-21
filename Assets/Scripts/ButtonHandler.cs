using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void StartGame()
    {
        SetGameStateForPlaying();
        SceneManager.LoadScene("Savannah");
    }

    public void LoadArctic()
    {
        SetGameStateForPlaying();
        SceneManager.LoadScene("Arctic");
    }

    public void LoadMenu()
    {
        SetGameStateForMenu();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadForest()
    {
        SetGameStateForMenu();
        SceneManager.LoadScene("Forest");
    }

    private void SetGameStateForPlaying()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetGameStateForMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
