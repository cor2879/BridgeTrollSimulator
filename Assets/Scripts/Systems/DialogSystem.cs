using UnityEngine;
using System.Collections.Generic;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Policies;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Scenarios;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Systems
{
    public class DialogSystem : MonoBehaviour, IEventSource
    {
        #region Singleton

        private static DialogSystem _instance;

        public static DialogSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<DialogSystem>();

                return _instance;
            }
        }

        #endregion

        #region State

        [SerializeField, ReadOnly]
        private EntityController currentInitiator;

        [SerializeField, ReadOnly]
        private EntityController currentTarget;

        [SerializeField]
        private DialogNode defaultEncounterNode;

        [SerializeField]
        private DialogUIController dialogPanel;

        private readonly Stack<RuntimeDialogNode> runtimeStack = new();

        #endregion

        #region IEventSource

        public string SourceName => nameof(DialogSystem);
        public GameSystemType SystemType => GameSystemType.System;

        #endregion

        public DialogUIController DialogPanel => dialogPanel;
        public DialogNode DefaultEncounterNode => defaultEncounterNode;

        #region Initialization

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
            GameEventBus.Subscribe<DialogStartedEvent>(OnDialogStarted);
            // GameEventBus.Subscribe<TollDemandedEvent>(OnTollDemanded);
            // GameEventBus.Subscribe<RefusePassageEvent>(OnRefusedPassage);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<DialogStartedEvent>(OnDialogStarted);
            // GameEventBus.Unsubscribe<TollDemandedEvent>(OnTollDemanded);
            // GameEventBus.Unsubscribe<RefusePassageEvent>(OnRefusedPassage);
        }

        #endregion

        #region Event Handlers

        private void OnDialogStarted(DialogStartedEvent evt)
        {
            currentInitiator = evt.Initiator as EntityController;
            currentTarget = evt.Target as EntityController;

            runtimeStack.Clear();
        }

        /*
        private void OnTollDemanded(TollDemandedEvent evt)
        {
            var player = evt.Initiator;
            var npc = evt.Target;

            var scenario = DemandTollScenario.Instance;

            var reaction = ReactionResolver.Resolve(
                scenario,
                npc,
                player,
                evt);
            
            reaction.Execute(npc, player, evt);
        }

        private void OnRefusedPassage(RefusePassageEvent evt)
        {
            var player = evt.Initiator;
            var npc = evt.Target;

            var scenario = RefusePassageScenario.Instance;
            var reaction = ReactionResolver.Resolve(
                scenario,
                npc,
                player,
                evt);

            reaction.Execute(npc, player, evt);
        }
        */

        #endregion

        #region Escalation

        private List<GeneratedOption> BuildEscalationOptions(
            EntityController initiator,
            EntityController target)
        {
            var options = new List<GeneratedOption>
            {
                new GeneratedOption
                {
                    Label = "Attack",
                    Execute = (i, t) =>
                    {
                        GameEventBus.Publish(
                            new CombatStartedEvent(
                                initiator,
                                target,
                                Time.frameCount));
                    }
                },
                new GeneratedOption
                {
                    Label = "Threaten / Persuade",
                    Execute = (i, t) =>
                    {
                        GameEventBus.Publish(
                            new SocialDuelStartedEvent(
                                initiator,
                                target,
                                Time.frameCount));
                    }
                },
                new GeneratedOption
                {
                    Label = "Refuse Passage",
                    Execute = (i, t) =>
                    {
                        GameEventBus.Publish(
                            new RefusePassageEvent(
                                initiator,
                                target,
                                Time.frameCount));
                    }
                }
            };

            return options;
        }

        #endregion

        #region Runtime Navigation

        public void ShowGeneratedOptions(
            List<GeneratedOption> options,
            EntityController initiator = null,
            EntityController target = null,
            string text = "")
        {
            if (initiator == null)
                initiator = currentInitiator;

            if (target == null)
                target = currentTarget;

            var node = new RuntimeDialogNode
            {
                Text = text,
                Options = options,
                Initiator = initiator,
                Target = target
            };

            runtimeStack.Push(node);
            RenderRuntimeNode(node);
        }

        public void AdvanceStaticNode(DialogNode nextNode)
        {
            if (nextNode == null)
            {
                GameEventBus.Publish(
                    new DialogEndedEvent(currentInitiator, currentTarget, Time.frameCount));
                return;
            }

            dialogPanel.ShowNode(nextNode);
        }

        public void GoBack()
        {
            if (runtimeStack.Count <= 1)
                return;

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

        #endregion
    }
}