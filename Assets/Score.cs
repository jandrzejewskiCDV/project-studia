using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public TMP_Text distanceText, coinsText, speedDebugText;

    // Update is called once per frame
    void Update()
    {
        long distance = (long) GameState.Instance.getDistance();
        long coins = GameState.Instance.getCoins();

        distanceText.SetText(distance.ToString());
        coinsText.SetText(coins.ToString());
        speedDebugText.SetText("+" + GameState.Instance.getAdditionalSpeed().ToString() + "%");
    }
}
