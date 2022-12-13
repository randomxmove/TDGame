using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStatsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText = null;
    [SerializeField] private TextMeshProUGUI enemyText = null;
    [SerializeField] private TextMeshProUGUI levelText = null;

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (!GameManager.Instance.IsInGame) return;

        countdownText.text = GameManager.Instance.CurrentTimer.ToString("F0");
        enemyText.text = GameManager.Instance.RemainingEnemies.ToString();
        levelText.text = GameManager.Instance.Level.ToString();
    }
}
