using System;
using System.Collections.Generic;

using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Policies;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Systems;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog
{
    [CreateAssetMenu(menuName = "BridgeTroll/Dialog Node")]
    public class DialogNode : ScriptableObject, IDialogRenderable
    {
        public DialogSpeakerRole SpeakerRole;

        [TextArea(2, 5)]
        public string Text;

        public List<DialogChoice> Choices = new List<DialogChoice>();

        List<GeneratedOption> IDialogRenderable.Options
        {
            get
            {
                if (Choices == null)
                    return null;

                var generated = new List<GeneratedOption>();

                foreach (var choice in Choices)
                {
                    generated.Add(new GeneratedOption
                    {
                        Label = choice.ChoiceText,
                        Execute = (initiator, target) =>
                        {
                            // 2️⃣ Then advance static dialog
                            DialogSystem.Instance.AdvanceStaticNode(choice.NextNode);

                            // 1️⃣ Execute actions FIRST
                            foreach (var action in choice.Actions)
                            {
                                action.Execute(initiator, target);
                            }
                        }
                    });
                }

                return generated;
            }
        }

        string IDialogRenderable.Text => this.Text;
    }
}