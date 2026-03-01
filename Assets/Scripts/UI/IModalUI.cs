using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public interface IModalUI
    {
        void ShowModal(GameObject panel);
        void HideModal(GameObject panel);
    }
}