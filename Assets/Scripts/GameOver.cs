using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI playerScoreText;

    public void SetHighScore(int score)
    {
        highScoreText.text = score.ToString();
    }

    public void SetPlayerScore(int score)
    {
        playerScoreText.text = score.ToString();
    }
}
