using UnityEngine;
using TMPro;

public class DefenderHud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _wavesText;
    [SerializeField] private TextMeshProUGUI _playerHeathText;

    public void UpdatePlayerHeath(float currentHp, float maxHp)
    {
        _playerHeathText.text = $"{(int)(currentHp / maxHp * 100)}%";
    }

    public void UpdateScenarioWaves(int currentWave, int wavesCount)
    {
        _wavesText.text = $"wave {currentWave}/{wavesCount}";
    }
}
