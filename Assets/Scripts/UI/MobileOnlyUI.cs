using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Platform;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    public class MobileOnlyUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject target;

        [SerializeField]
        private bool showInEditor = true;

        private void Start()
        {
#if UNITY_EDITOR
            target.SetActive(showInEditor);
#else
            target.SetActive(PlatformManager.UsesMobileTouchControls);
#endif
        }
    }
}