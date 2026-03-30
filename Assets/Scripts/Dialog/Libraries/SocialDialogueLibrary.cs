using System.Collections.Generic;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog.Libraries
{
    public static class SocialDialogueLibrary
    {
        private static readonly Dictionary<string, SocialAbilityDialogue> _data = new()
        {
            ["persuade"] = new SocialAbilityDialogue
            {
                WeakSuccess = new[]
                {
                    "You see the wisdom in this.",
                    "This arrangement favors us both.",
                    "Let reason prevail.",
                    "There is balance in what I offer."
                },
                StrongSuccess = new[]
                {
                    "This is settled.",
                    "This is the wiser course.",
                    "You will thank me for this.",
                    "Agreement is reached."
                },
                CriticalSuccess = new[]
                {
                    "It is decided.",
                    "You concede.",
                    "You will pass, and we are done.",
                    "Wisdom triumphs."
                },
                WeakFailure = new []
                {
                    "Consider your position.",
                    "There is another path here.",
                    "Consider the alternative.",
                    "This need not escalate."
                },
                StrongFailure = new[]
                {
                    "You cannot win this exchange.",
                    "This is the only rational outcome!",
                    "Defiance costs you more than you know.",
                    "Choose correctly, or suffer the consequence."
                },
                CriticalFailure = new[]
                {
                    "Surely this benefits us both?",
                    "Let's be sensible about this.",
                    "You see my point, yes?",
                    "There is... logic here... I think?"
                }
            },

            ["intimidate"] = new SocialAbilityDialogue
            {
                WeakSuccess = new[]
                {
                    "There it is.  I see your fear.",
                    "Your courage falters.",
                    "You feel it now.",
                    "Your footing weakens."
                },
                StrongSuccess = new[]
                {
                    "You are already beaten.",
                    "Your will bends.",
                    "The bridge rejects you.",
                    "Leave while you can."
                },
                CriticalSuccess = new[]
                {
                    "You were never my equal.",
                    "Your spirit cracks.",
                    "The toll is the least of your worries.",
                    "Begone!"
                },
                WeakFailure = new[]
                {
                    "You would be wise to reconsider.",
                    "This bridge is not for you.",
                    "You're afraid of me by now, I'm sure!",
                    "If you don't pay the toll I can't eat no rolls!"
                },
                StrongFailure = new[]
                {
                    "Hey!  Stop laughing this isn't funny!",
                    "Trolls collecting tolls is a time honored tradition!",
                    "Why don't you want to pay the toll?",
                    "I have bills to pay!"
                },
                CriticalFailure = new[]
                {
                    "You should be terrified. This is terrifying... right?",
                    "Fear me!\nNo?  Truly?!",
                    "I am DOOM given flesh!  Hey!  Stop laughing!",
                    "You will KNEEL before... uh... something something."
                }
            }
        };

        public static SocialAbilityDialogue Get(string id)
        {
            return _data.TryGetValue(id, out var d) ? d : null;
        }

        public static string GetRandom(string id, SocialExchangeOutcome outcome)
        {
            if (!_data.TryGetValue(id, out var d))
                return string.Empty;

            var lines = outcome.Result switch
            {
                SocialExchangeResult.StrongSuccess => d.StrongSuccess,
                SocialExchangeResult.WeakSuccess => d.WeakSuccess,
                SocialExchangeResult.StrongFailure => d.StrongFailure,
                SocialExchangeResult.WeakFailure => d.WeakFailure,
                _ => null
            };

            if (outcome.IsCritical)
            {
                if (outcome.Result == SocialExchangeResult.StrongSuccess && 
                    !d.CriticalSuccess.IsNullOrEmpty())
                {
                    lines = d.CriticalSuccess;
                }

                if (outcome.Result == SocialExchangeResult.StrongFailure && 
                    !d.CriticalFailure.IsNullOrEmpty())
                {
                    lines = d.CriticalFailure;
                }
            }

            return lines.IsNullOrEmpty() ? string.Empty : lines.GetRandom();
        }
    }
}