using UnityEngine;
using System;
using System.Linq;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Enums;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects
{
    [System.Serializable]
    public class EffectDefinition
    {
        [SerializeField, EffectDropdown]
        private string effectTypeName;

        [SerializeField]
        private EffectTarget target = EffectTarget.Target;

        [SerializeField]
        private EffectStackingType stackingType = EffectStackingType.Refresh;

        [SerializeField]
        private int duration;

        [SerializeField]
        private int magnitude;

        [Header("Status Texts")]
        [SerializeField]
        private string appliedText;
        [SerializeField]
        private string tickText;
        [SerializeField]
        private string expiredText;

        [Header("Feedback")]
        [SerializeField]
        private AudioClip soundEffect;
        [SerializeField]
        private Color feedbackColor = Color.white;
        [SerializeField] 
        private bool flashTarget = true;
        [SerializeField] 
        private float flashDuration = 0.15f;
        [SerializeField] 
        private bool flashOnTick = true;
        [SerializeField]
        private bool playSoundOnTick = true;

        private Type _cachedType;

        public string EffectTypeName => effectTypeName;
        public EffectStackingType StackingType => stackingType;
        public int Magnitude => magnitude;
        public int Duration => duration;
        public AudioClip SoundEffect => soundEffect;
        public Color FeedbackColor => feedbackColor;
        public bool FlashTarget => flashTarget;
        public float FlashDuration => flashDuration;
        public bool FlashOnTick => flashOnTick;
        public bool PlaySoundOnTick => playSoundOnTick;
        public string AppliedText => appliedText;
        public string TickText => tickText;
        public string ExpiredText => expiredText;

        public EffectTarget Target => target;

        public StatusEffect Create()
        {
            if (string.IsNullOrEmpty(effectTypeName))
            {
                return null;
            }

            if (_cachedType == null)
            {
                _cachedType = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.FullName == effectTypeName);
            }

            try
            {
                return (StatusEffect)Activator.CreateInstance(_cachedType, magnitude, duration, this);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to create StatusEffect: {effectTypeName} | {ex}");
                return null;
            }

        }

        public override string ToString()
        {
            return $"{nameof(EffectDefinition)}::EffectTypeName:{EffectTypeName}::StackingType:{StackingType}" +
                $"::SoundEffect:{SoundEffect}::FeedbackColor:{FeedbackColor}::FlashTarget:{FlashTarget}::FlashDuration:{FlashDuration}" +
                $"::FlashOnTick:{FlashOnTick}::PlaySoundOnTick:{PlaySoundOnTick}";
        }
    }
}