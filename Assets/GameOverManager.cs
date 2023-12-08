using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI timeText; // Assign this in the inspector
    public TextMeshProUGUI coinText; // Assign this in the inspector
    void Start()
    {
        timeText.text = "Time Played: " + GameManager.instance.GetTimePlayed() + " s";
        coinText.text = "Coins Collected: " + GameManager.instance.GetCoinCount();
    }

    public void RestartGame()
    {
        GameManager.instance.ResetGame();
        SceneManager.LoadScene("SampleScene"); // Replace with your actual game scene name
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }
}
