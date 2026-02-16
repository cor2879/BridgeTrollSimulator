using UnityEngine;
using UnityEngine.UI;
using TMPro;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;

public class CombatUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TMP_Text playerStatsText;
    [SerializeField]
    private TMP_Text enemyStatsText;
    [SerializeField]
    private Button attackButton;

    private CombatSystem combatSystem;

    public void Initialize(CombatSystem system)
    {
        combatSystem = system;
        HookButtons();
    }

    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    public void UpdateStats(EntityController player, EntityController enemy)
    {
        playerStatsText.text = $"HP: {player.CurrentHealth}";
        enemyStatsText.text = $"HP: {enemy.CurrentHealth}";
    }

    private void HookButtons()
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() =>
        {
            combatSystem.PlayerAttack();
        });
    }
}