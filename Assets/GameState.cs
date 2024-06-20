using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }
    public const String HighscoreKey = "ENDLESS_RUNNER_HighScore";

    public GameObject gameOverOverlay;
    public TMP_Text gameOverDistanceText, gameOverCoinsText, gameOverHighestScoreText;
    public AudioSource backgroundMusic;
    
    public float additionalSpeedFactor = 0.005F;

    private bool running = true;
    private long coins;
    private double distance;

    private void Start()
    {
        Instance = this;
        Cursor.visible = false;
        backgroundMusic.PlayDelayed(2);
    }

    public bool isRunning()
    {
        return running;
    }

    public void die()
    {
        var distance = (long) getDistance();
        
        Cursor.visible = true;
        running = false;
        gameOverOverlay.SetActive(true);
        gameOverDistanceText.SetText("Distance: " + distance);
        gameOverCoinsText.SetText("Gems: " + getCoins());

        var highScore = (long) PlayerPrefs.GetFloat(HighscoreKey);

        if (highScore < distance)
        {
            PlayerPrefs.SetFloat(HighscoreKey, distance);
        }
        
        gameOverHighestScoreText.SetText((highScore < distance ? "New " : "") + "Highest Score: " + (highScore < distance ? distance : highScore));
    }

    public void restart()
    {
        SceneManager.LoadScene(1);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public long getCoins()
    {
        return coins;
    }

    public void addCoin()
    {
        if (running)
            this.coins++;
    }

    public double getDistance()
    {
        return this.distance;
    }

    public void setDistance(double distance)
    {
        if (running)
        {
            this.distance = distance;
        }
    }

    public void addDistance(double distance)
    {
        setDistance(getDistance() + distance);
    }

    public float getAdditionalSpeed()
    {
        float distance = (float) getDistance();

        return distance > 0 ? Mathf.Pow(distance, additionalSpeedFactor) : 1f; //no changes
    }
}
