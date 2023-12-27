using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public GameObject menuPanel;

    public void ToggleMenu()
    {
        
        menuPanel.SetActive(!menuPanel.activeSelf);

        
        Time.timeScale = menuPanel.activeSelf ? 0 : 1;
    }

    public void ResumeGame()
    {
        
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartFromCheckpoint()
    {
      
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
       
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}