using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    public GameObject gameOverOverlay;
    public TMP_Text gameOverDistanceText, gameOverCoinsText;

    public float additionalSpeedFactor = 0.005F;

    private bool running = true;
    private long coins = 0;
    private double distance = 0;

    private void Start()
    {
        Instance = this;
        Cursor.visible = false;
    }

    public bool isRunning()
    {
        return running;
    }

    public void die()
    {
        Cursor.visible = true;
        running = false;
        gameOverOverlay.SetActive(true);
        gameOverDistanceText.SetText("Distance: " + (long) getDistance());
        gameOverCoinsText.SetText("Gems: " + getCoins());
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
        float distance = (float) GameState.Instance.getDistance();

        if (distance > 0)
        {
            return Mathf.Pow(distance, additionalSpeedFactor);
        }

        return 1f; //no changes
    }
}
