#pragma warning disable CS0649
/**************************************************
 *  GameManager.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours
{
    using System;
    using System.Collections;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UI;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours.GameplayManagement;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Platform;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;
    using System.Security.AccessControl;

    /// <summary>
    /// This is a Singleton class and is the central "hub" for managing all state in the game.
    /// Almost any behaviour can be managed or accessed by starting in this class.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(PrefabLibrary))]
    public class GameManager : MonoBehaviour, IGameManager
    {
        /// <summary>
        /// The lock input
        /// </summary>
        private bool lockInput = false;

        /// <summary>
        /// The camera manager
        /// </summary>
        public CameraManager cameraManager;

        [SerializeField, ReadOnly]
        private ISceneManager sceneManager;

        /// <summary>
        /// The music manager
        /// </summary>
        [SerializeField]
        private AudioManager musicManager;

        [SerializeField, ReadOnly]
        private PrefabLibrary prefabLibrary;

        /// <summary>
        /// The sound effect manager
        /// </summary>
        [SerializeField]
        private AudioManager soundEffectManager;

        /// <summary>
        /// The UI canvas
        /// </summary>
        [SerializeField]
        private Canvas uiCanvas;

        /// <summary>
        /// The pause action
        /// </summary>
        [SerializeField]
        private bool pauseAction;

        /// <summary>
        /// The badge earned panel behaviour
        /// </summary>
        [SerializeField]
        private BadgeEarnedPanelBehaviour badgeEarnedPanelBehaviour;

        /// <summary>
        /// The sprite2 d material
        /// </summary>
        [SerializeField]
        private Material sprite2DMaterial;

        /// <summary>
        /// The camera target prefab
        /// </summary>
        public GameObject cameraTargetPrefab;

        public static GameManager Instance { get; private set; }

        public Guid SessionId { get; } = Guid.NewGuid();

        public bool IsGameOver { get; set; }

        public CameraManager CameraManager
        {
            get => this.cameraManager;
        }

        /// <summary>
        /// Gets the music manager.
        /// </summary>
        /// <value>
        /// The music manager.
        /// </value>
        public AudioManager MusicManager
        {
            get => this.musicManager;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [pause action].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [pause action]; otherwise, <c>false</c>.
        /// </value>
        public bool PauseAction
        {
            get => this.pauseAction;
            set => this.pauseAction = value;
        }

        public PrefabLibrary PrefabLibrary
        {
            get
            {
                if (this.prefabLibrary is null)
                {
                    this.prefabLibrary = this.GetComponent<PrefabLibrary>();
                }

                return this.prefabLibrary;
            }
        }

        /// <summary>
        /// Gets the sound effect manager.
        /// </summary>
        /// <value>
        /// The sound effect manager.
        /// </value>
        public AudioManager SoundEffectManager
        {
            get => this.soundEffectManager;
        }

        /// <summary>
        /// Gets or sets the sprite2 d material.
        /// </summary>
        /// <value>
        /// The sprite2 d material.
        /// </value>
        public Material Sprite2DMaterial
        {
            get => this.sprite2DMaterial;
        }

        public ISceneManager SceneManager 
        {
            get
            {
                if (this.sceneManager is null)
                {
                    this.sceneManager = this.GetComponent<ISceneManager>();
                }

                return this.sceneManager;
            }
        }

        /// <summary>
        /// Executes when this instance is awakened.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Executes during the Start event of the GameObject life cycle.
        /// </summary>
        private void Start()
        {
            SoundClips.CurrentBGM = SoundClips.Playlist[UnityEngine.Random.Range(0, SoundClips.Playlist.Length)];
            this.SoundEffectManager.Volume = Settings.SoundEffectVolume;
            this.MusicManager.Volume = Settings.MusicVolume;
            this.MusicManager.SetBackgroundMusic(SoundClips.CurrentBGM);
            this.SoundEffectManager.SetBackgroundMusic(SoundClips.AmbientCave);

            this.IsGameOver = false;

#if DEBUG
#if UNITY_STEAM
            Debug.Log($"Steam App ID: {SteamManager.AppId}");
            Debug.Log($"Steam User Id: {SteamManager.SteamUserId}");
#endif
#endif
        }

        /// <summary>
        /// Executes when each frame is updated by the Unity Engine.
        /// </summary>
        private void Update()
        {
            if (!this.PauseAction && !this.lockInput)
            {
                this.lockInput = true;

                StartCoroutine(nameof(this.WaitForPredicateToBeFalseThenDoAction),
                    new WaitAction(
                        InputExtension.IsOpenMinimapPressed,
                        () =>
                        {
                            this.lockInput = false;
                        }));
            }

            InputExtension.HideMouseIfGamepadIsPresent();
        }

        /// <summary>
        /// Clears the main window text.
        /// </summary>
        public static void ClearMainWindowText()
        {
            GameplayMenuManagerBehaviour.ClearMainWindowText();
        }
        
        public void ShowBadgeEarned(Badge badge, float duration, Action onShowBadgeComplete)
        {
            this.badgeEarnedPanelBehaviour.Show(badge, duration, onShowBadgeComplete);
        }

        public MessageBoxBehaviour MessageBoxBehaviour { get; set; }

        public MinimapBehaviour Minimap { get; set; }

        public SettingsPanelBehaviourBase SettingsPanel { get; set; }

        public MenuPanelBehaviour MenuPanel { get; set; }
        
        public void GameOver(GameOverCondition gameOverCondition)
        {
            // TODO
        }

        /// <summary>
        /// Waits for predicate to be false then does the action.
        /// </summary>
        /// <param name="waitAction">The wait action.</param>
        /// <returns></returns>
        public IEnumerator WaitForPredicateToBeFalseThenDoAction(WaitAction waitAction)
        {
            while (waitAction.Predicate())
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            waitAction.DoAction.Invoke();
        }
    }
}
