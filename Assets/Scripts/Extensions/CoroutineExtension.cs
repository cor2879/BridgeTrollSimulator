using UnityEngine;
using System.Collections;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions
{
    public static class CoroutineExtensions
    {
        public static IEnumerator WaitForSecondsRespectingPause(float duration)
        {
            float timer = 0f;

            while (timer < duration)
            {
                if (!GameStateSystem.Instance.IsPaused)
                {
                    timer += Time.deltaTime;
                }

                yield return null;
            }
        }

        public static IEnumerator WaitUntilGameplayActive()
        {
            yield return new WaitUntil(() =>
                !GameStateSystem.Instance.IsPaused);
        }

        private static IEnumerator WaitForKeyUpThenDoAction(System.Action action)
        {
            yield return new WaitUntil(() => !Input.anyKey);
            action?.Invoke();
        }
    }
}