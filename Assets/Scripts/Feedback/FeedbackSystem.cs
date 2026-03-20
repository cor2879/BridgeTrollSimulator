using System.Collections;
using UnityEngine;

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
        }

        private void OnDisable()
        {
            GameEventBus.Unsubscribe<GoldFeedbackEvent>(OnGoldFeedback);
            GameEventBus.Unsubscribe<DamageTakenEvent>(OnDamageTaken);
            GameEventBus.Unsubscribe<DefendEvent>(OnDefended);
            GameEventBus.Unsubscribe<EntityDiedEvent>(OnEntityDied);
            GameEventBus.Unsubscribe<ResolveDamageTakenEvent>(OnResolveDamageTaken);
            GameEventBus.Unsubscribe<ResolveBrokenEvent>(OnResolveBroken);
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
                    evt.Amount,
                    FeedbackColors.Damage,
                    evt.IsCrit));

            GameEventBus.Publish(
                new FlashEvent(
                    this,
                    target,
                    FeedbackColors.Damage,
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
                    evt.Amount,
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

        #endregion

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