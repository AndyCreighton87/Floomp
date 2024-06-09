using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] private int unitKilledReward = 1;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RewardPlayerCurrency(Player _player, int _amount) {
        _player.ChangeCurrency(_amount);
    }

    public void NotifyUnitKilled(Team _team) {
        Player player = GameManager.Instance.GetOpposingPlayerOfTeam(_team);
        if (player != null) {
            RewardPlayerCurrency(player, unitKilledReward);
        }
    }
}
