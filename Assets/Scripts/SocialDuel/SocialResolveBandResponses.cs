using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel
{
    [System.Serializable]
    public class SocialResolveBandResponses
    {
        [Header("Weak Success")]
        [SerializeField] private string[] weakSuccess;

        [Header("Strong Success")]
        [SerializeField] private string[] strongSuccess;

        [Header("Weak Failure")]
        [SerializeField] private string[] weakFailure;

        [Header("Strong Failure")]
        [SerializeField] private string[] strongFailure;

        [Header("Critical Success")]
        [SerializeField] private string[] criticalSuccess;

        [Header("Critical Failure")]
        [SerializeField] private string[] criticalFailure;

        public string GetLine(SocialExchangeOutcome outcome)
        {
            if (outcome.IsCritical)
            {
                if (outcome.Result == SocialExchangeResult.StrongSuccess)
                    return GetRandom(criticalSuccess);

                if (outcome.Result == SocialExchangeResult.StrongFailure)
                    return GetRandom(criticalFailure);
            }

            return outcome.Result switch
            {
                SocialExchangeResult.StrongSuccess => GetRandom(strongSuccess),
                SocialExchangeResult.WeakSuccess   => GetRandom(weakSuccess),
                SocialExchangeResult.WeakFailure   => GetRandom(weakFailure),
                SocialExchangeResult.StrongFailure => GetRandom(strongFailure),
                _ => string.Empty
            };
        }

        private string GetRandom(string[] lines)
        {
            if (lines == null || lines.Length == 0)
                return string.Empty;

            return lines[Random.Range(0, lines.Length)];
        }
    }
}