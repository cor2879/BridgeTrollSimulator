using UnityEngine;
using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Policies;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class DialogSystem : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private EntityController currentInitiator;
        [SerializeField, ReadOnly]
        private EntityController currentTarget;
        [SerializeField, ReadOnly]
        private DialogNode currentNode;

        private static DialogSystem _instance;

        [SerializeField]
        private DialogNode defaultEncounterNode;

        [SerializeField]
        private DialogUIController dialogPanel;

        private Stack<RuntimeDialogNode> runtimeStack = new();

        public static DialogSystem Instance 
        { 
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<DialogSystem>();
                }

                return _instance;
            }
        }

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
            GameEventBus.Subscribe<EntityEncounterEvent>(OnEncounter);
            GameEventBus.Subscribe<DialogStartedEvent>(OnDialogStarted);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<EntityEncounterEvent>(OnEncounter);
            GameEventBus.Unsubscribe<DialogStartedEvent>(OnDialogStarted);
        }

        private void OnEncounter(EntityEncounterEvent evt)
        {
            GameEventBus.Publish(
                new DialogStartedEvent(
                    defaultEncounterNode,
                    evt.Initiator,
                    evt.Target,
                    Time.frameCount));
        }

        private void OnDialogStarted(DialogStartedEvent evt)
        {
            currentInitiator = evt.Initiator;
            currentTarget = evt.Target;
            currentNode = evt.RootNode;

            runtimeStack.Clear();
        }

        public void ShowGeneratedOptions(List<GeneratedOption> options)
        {
            var node = new RuntimeDialogNode
            {
                Text = string.Empty,
                Options = options,
                Initiator = currentInitiator,
                Target = currentTarget
            };

            runtimeStack.Push(node);
            RenderRuntimeNode(node);
        }

        public void GoBack()
        {
            if (runtimeStack.Count <= 1)
            {
                return;
            }

            runtimeStack.Pop();
            RenderRuntimeNode(runtimeStack.Peek());
        }

        private void RenderRuntimeNode(RuntimeDialogNode node)
        {
            dialogPanel.ShowRuntimeNode(
                node.Text,
                node.Options,
                node.Initiator,
                node.Target
            );
        }
    }
}