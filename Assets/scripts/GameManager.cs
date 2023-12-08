using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI coinCountText; // Assign this in the inspector
    private int coinCount = 0; 
    private float timePlayed = 0f;

    void Update()
    {
        timePlayed += Time.deltaTime;
    }

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the GameManager across scenes
        }
        else if (instance != this)
        {
            Destroy(gameObject); // There can only be one instance of this class
        }
    }

    void Start()
    {
        // Initialize coin count UI
        UpdateCoinCountUI();
    }

    public void AddCoins(int amount)
    {
        // Increase coin count and update UI
        coinCount += amount;
        UpdateCoinCountUI();
    }

    private void UpdateCoinCountUI()
    {
        // Set the text of the coin count UI element to the current coin count
        if (coinCountText != null)
        {
            coinCountText.text = coinCount.ToString();
        }
        else
        {
            Debug.LogWarning("CoinCountText not set in the GameManager.");
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScene"); // Replace with your actual game over scene name
    }

    public float GetTimePlayed()
    {
        return timePlayed;
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void ResetGame()
    {
        timePlayed = 0f;
        coinCount = 0;
        UpdateCoinCountUI();
        // Reset other necessary stats or states
    }

    
}