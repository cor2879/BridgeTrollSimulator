using UnityEngine;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement
{
    public class GameStateSystem : MonoBehaviour, IEventSource
    {
        private static GameStateSystem _instance;

        public static GameStateSystem Instance 
        { 
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameStateSystem>();
                }

                return _instance;
            }
        }

        [SerializeField]
        private GameState currentState = GameState.World;
        [SerializeField, ReadOnly]
        private GameState previousState;

        public GameState CurrentState => currentState;
        public GameState PreviousState => previousState;

        public string SourceName => nameof(GameStateSystem);
        public GameSystemType SystemType => GameSystemType.System;
        public bool IsPaused { get; private set; }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void OnEnable()
        {
            GameEventBus.Subscribe<PauseRequestEvent>(OnPauseRequested);
            GameEventBus.Subscribe<ResumeRequestEvent>(OnResumeRequested);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<PauseRequestEvent>(OnPauseRequested);
            GameEventBus.Unsubscribe<ResumeRequestEvent>(OnResumeRequested);
        }

        private void OnPauseRequested(PauseRequestEvent evt)
        {
            Pause();
        }

        private void OnResumeRequested(ResumeRequestEvent evt)
        {
            Resume();
        }

        public void SetState(GameState newState)
        {
            if (newState == currentState)
            {
                return;
            }

            previousState = currentState;
            currentState = newState;

            GameEventBus.Publish(
                new GameStateChangedEvent(
                    this,
                    previousState,
                    currentState,
                    Time.frameCount));
        }

        public void Pause()
        {
            this.IsPaused = true;
        }

        public void Resume()
        {
            this.IsPaused = false;
        }

        public bool Is(GameState state) => currentState == state;
    }
}