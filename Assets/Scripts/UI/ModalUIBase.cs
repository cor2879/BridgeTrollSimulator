using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public abstract class ModalUIBase : MonoBehaviour, IModalUI
    {
        public abstract string SourceName { get; }
        public abstract GameSystemType SystemType { get; }

        public abstract bool IsBlockingUI { get; }

        protected virtual void ShowModal(GameObject panel)
        {
            ModalUISystem.Instance.OpenModal(this);
            panel.SetActive(true);
        }

        protected virtual void HideModal(GameObject panel)
        {
            ModalUISystem.Instance.CloseModal(this);
            panel.SetActive(false);
        }
    }
}