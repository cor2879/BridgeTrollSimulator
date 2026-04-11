using System.Collections;
using UnityEngine;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.StatusEffects;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Effects;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback
{
    public class FeedbackSystem : MonoBehaviour, IEventSource
    {
        [SerializeField]
        private ShakeBehaviour cameraShake;

        #region IEventSource

        public string SourceName => nameof(FeedbackSystem);
        public GameSystemType SystemType => GameSystemType.System;

        #endregion

        #region Initialization

        private void OnEnable()
        {
            GameEventBus.Subscribe<GoldFeedbackEvent>(OnGoldFeedback);
            GameEventBus.Subscribe<DamageTakenEvent>(OnDamageTaken);
            GameEventBus.Subscribe<DefendEvent>(OnDefended);
            GameEventBus.Subscribe<EntityDiedEvent>(OnEntityDied);
            GameEventBus.Subscribe<ResolveDamageTakenEvent>(OnResolveDamageTaken);
            GameEventBus.Subscribe<ResolveBrokenEvent>(OnResolveBroken);
            GameEventBus.Subscribe<StatusEffectAppliedEvent>(OnStatusEffectApplied);
            GameEventBus.Subscribe<StatusEffectTickEvent>(OnStatusEffectTick);
            GameEventBus.Subscribe<HealthRestoredEvent>(OnHealthRestored);
            GameEventBus.Subscribe<ResolveRestoredEvent>(OnResolveRestored);
            GameEventBus.Subscribe<StaminaDamageTakenEvent>(OnStaminaDamageTaken);
            GameEventBus.Subscribe<StaminaRestoredEvent>(OnStaminaRestoredEvent);
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<GoldFeedbackEvent>(OnGoldFeedback);
            GameEventBus.Unsubscribe<DamageTakenEvent>(OnDamageTaken);
            GameEventBus.Unsubscribe<DefendEvent>(OnDefended);
            GameEventBus.Unsubscribe<EntityDiedEvent>(OnEntityDied);
            GameEventBus.Unsubscribe<ResolveDamageTakenEvent>(OnResolveDamageTaken);
            GameEventBus.Unsubscribe<ResolveBrokenEvent>(OnResolveBroken);
            GameEventBus.Unsubscribe<StatusEffectAppliedEvent>(OnStatusEffectApplied);
            GameEventBus.Unsubscribe<StatusEffectTickEvent>(OnStatusEffectTick);
            GameEventBus.Unsubscribe<HealthRestoredEvent>(OnHealthRestored);
            GameEventBus.Unsubscribe<ResolveRestoredEvent>(OnResolveRestored);
            GameEventBus.Unsubscribe<StaminaDamageTakenEvent>(OnStaminaDamageTaken);
            GameEventBus.Unsubscribe<StaminaRestoredEvent>(OnStaminaRestoredEvent);
        }

        #endregion

        #region Event Handlers

        private void OnGoldFeedback(GoldFeedbackEvent evt)
        {
            SpawnGoldPopup(evt);
        }

        private void OnDamageTaken(DamageTakenEvent evt)
        {
            var isCrit = evt.IsCrit;
            var duration = isCrit ? 0.6f : 0.375f;
            var magnitude = isCrit ? 0.4f : 0.2f;
            var target = evt.Sender as IReceiver;

            StartCoroutine(DoHitStop(duration * 0.5f));

            if (cameraShake != null)
            {
                Debug.Log("camera shake");
                cameraShake.StartShake(duration, magnitude, 3f);
            }

            GameEventBus.Publish(
                new FloatingTextEvent(
                    this,
                    target,
                    $"-{evt.Amount}",
                    FeedbackColors.HealthDamage,
                    evt.IsCrit));

            GameEventBus.Publish(
                new FlashEvent(
                    this,
                    target,
                    FeedbackColors.HealthDamage,
                    0.15f));

            GameEventBus.Publish(
                new SoundEffectEvent(
                    this,
                    evt.IsCrit ? 
                        AudioSystem.Library.crit : 
                        AudioSystem.Library.attack,
                    Time.frameCount));
        }

        private void OnDefended(DefendEvent evt)
        {
            GameEventBus.Publish(
                new SoundEffectEvent(
                    this,
                    AudioSystem.Library.defend,
                    Time.frameCount));
        }

        private void OnEntityDied(EntityDiedEvent evt)
        {
            StartCoroutine(DoHitStop(0.2f));

            cameraShake.StartShake(0.2f, 0.5f);
        }

        private void OnResolveDamageTaken(ResolveDamageTakenEvent evt)
        {
            var isCrit = evt.IsCrit;
            var duration = isCrit ? 0.4f : 0.25f;
            var magnitude = isCrit ? 0.25f : 0.1f;
            var target = evt.Sender as IReceiver;

            StartCoroutine(DoHitStop(duration * 0.5f));

            cameraShake?.StartShake(duration, magnitude, 2f);

            GameEventBus.Publish(
                new FloatingTextEvent(
                    this,
                    target,
                    $"-{evt.Amount}",
                    FeedbackColors.Resolve,
                    evt.IsCrit));

            GameEventBus.Publish(
                new FlashEvent(
                    this,
                    target,
                    FeedbackColors.Resolve, 
                    0.15f));

            if (target.IsPlayerControlled)
            {
                GameEventBus.Publish(
                    new SoundEffectEvent(
                        this,
                        isCrit ? 
                            AudioSystem.Library.playerResolveDamageCrit :
                            AudioSystem.Library.playerResolveDamage,
                        Time.frameCount));
            }
            else
            {
                GameEventBus.Publish(
                    new SoundEffectEvent(
                        this,
                        isCrit ? 
                            AudioSystem.Library.resolveDamageCrit :
                            AudioSystem.Library.resolveDamage,
                        Time.frameCount));
            }
        }

        private void OnResolveBroken(ResolveBrokenEvent evt)
        {
            StartCoroutine(DoHitStop(0.25f));

            cameraShake?.StartShake(0.4f, 0.4f);
            var entity = evt.Sender as IReceiver;

            GameEventBus.Publish(
                new FlashEvent(
                    this,
                    entity,
                    Color.magenta,
                    0.25f));

            GameEventBus.Publish(
                new SoundEffectEvent(
                    this,
                    entity.IsPlayerControlled ? 
                        AudioSystem.Library.playerBreakResolve :
                        AudioSystem.Library.breakResolve,
                    Time.frameCount));
        }

        private void OnStatusEffectApplied(StatusEffectAppliedEvent evt)
        {
            var effects = evt.Effect;

            GameEventBus.Publish(
                new SoundEffectEvent(
                    this,
                    effects.SoundEffect,
                    Time.frameCount));

            GameEventBus.Publish(
                new FlashEvent(
                    this,
                    evt.Target,
                    effects.FeedbackColor,
                    effects.FlashDuration));
        }

        public void OnStatusEffectTick(StatusEffectTickEvent evt)
        {
            var effects = evt.Effect;

            if (effects.PlaySoundOnTick && effects.SoundEffect != null)
            {
                GameEventBus.Publish(
                    new SoundEffectEvent(
                        this,
                        effects.SoundEffect,
                        Time.frameCount));
            }

            if (effects.FlashOnTick && effects.FlashTarget)
            {
                GameEventBus.Publish(
                    new FlashEvent(
                        this,
                        evt.Target,
                        effects.FeedbackColor,
                        effects.FlashDuration));
            }
        }

        private void OnHealthRestored(HealthRestoredEvent evt)
        {
            var target = evt.Sender as IReceiver;

            GameEventBus.Publish(
                new FloatingTextEvent(
                    this,
                    target,
                    $"+{evt.Amount}",
                    FeedbackColors.HealthRestore,
                    evt.IsCrit));        
        }

        private void OnResolveRestored(ResolveRestoredEvent evt)
        {
            var target = evt.Sender as IReceiver;

            GameEventBus.Publish(
                new FloatingTextEvent(
                    this,
                    target,
                    $"+{evt.Amount}",
                    FeedbackColors.Resolve,
                    evt.IsCrit));
        }

        private void OnStaminaDamageTaken(StaminaDamageTakenEvent evt)
        {
            var target = evt.Sender as IReceiver;

            GameEventBus.Publish(
                new FloatingTextEvent(
                    this,
                    target,
                    $"-{evt.Amount}",
                    FeedbackColors.Stamina,
                    evt.IsCrit));
        }

        private void OnStaminaRestoredEvent(StaminaRestoredEvent evt)
        {
            var target = evt.Sender as IReceiver;

            GameEventBus.Publish(
                new FloatingTextEvent(
                    this,
                    target,
                    $"+{evt.Amount}",
                    FeedbackColors.Stamina,
                    evt.IsCrit));
        }

        #endregion

        private void HandleEffects(EffectDefinition effects, IReceiver target)
        {
            if (effects == null)
            {
                return;
            }

            if (effects.PlaySoundOnTick && effects.SoundEffect != null)
            {
                GameEventBus.Publish(
                    new SoundEffectEvent(
                        this,
                        effects.SoundEffect,
                        Time.frameCount));
            }

            if (effects.FlashOnTick && effects.FlashTarget)
            {
                GameEventBus.Publish(
                    new FlashEvent(
                        this,
                        target,
                        effects.FeedbackColor,
                        effects.FlashDuration));
            }
        }

        private IEnumerator DoHitStop(float duration)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
        }

        private void SpawnGoldPopup(GoldFeedbackEvent evt)
        {
            var popupUI = ((EntityController)evt.Sender).GoldPopupUI;

            if (popupUI == null)
            {
                return;
            }

            popupUI.Play(evt.PreviousAmount, evt.NewAmount, evt.Delta);
        }
    }
}