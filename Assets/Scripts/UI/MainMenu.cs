using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    public void PlayGame()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("testingScene");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        print("Testing");
        SceneManager.LoadScene("testingScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenInstructions()
    {
        PauseMenu.SetActive(true);
    }

    public void CloseInstructinos()
    {
        PauseMenu.SetActive(false);
    }

}