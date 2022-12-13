using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText = null;
    [SerializeField] private TextMeshProUGUI _fundsText = null;
    [SerializeField] private TextMeshProUGUI _coreHealthText = null;
    [SerializeField] private PlayerCore _playerCore = null;


    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (!GameManager.Instance.IsInGame) return;

        _scoreText.text = GameManager.Instance.PlayerState.Score.ToString();
        _fundsText.text = GameManager.Instance.PlayerState.Funds.ToString();
        _coreHealthText.text = _playerCore.Hp.ToString();
    }
}
