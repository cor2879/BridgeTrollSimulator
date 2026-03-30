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

        [SerializeField]
        private AudioClip soundEffect;

        private Type _cachedType;

        public EffectStackingType StackingType => stackingType;
        public AudioClip SoundEffect => soundEffect;
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
                return (StatusEffect)Activator.CreateInstance(_cachedType, magnitude, duration);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to create StatusEffect: {effectTypeName} | {ex}");
                return null;
            }

        }
    }
}