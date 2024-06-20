using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{

    public TMP_Text distanceText, coinsText, speedDebugText, highScore;

    // Update is called once per frame
    void Update()
    {
        long distance = (long) GameState.Instance.getDistance();
        long coins = GameState.Instance.getCoins();

        distanceText.SetText(distance.ToString());
        coinsText.SetText(coins.ToString());
        speedDebugText.SetText("+" + GameState.Instance.getAdditionalSpeed() + "%");

        long highScore = (long) PlayerPrefs.GetFloat(GameState.HighscoreKey);
        if (distance > highScore)
        {
            highScore = distance;
        }
        
        this.highScore.SetText("Highest Score:\n" + highScore);
    }
}
