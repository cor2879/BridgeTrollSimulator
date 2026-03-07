using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel
{
    [System.Serializable]
    public class SocialResponseProfile
    {
        [Header("High Resolve")]
        [SerializeField] private SocialResolveBandResponses high;

        [Header("Mid Resolve")]
        [SerializeField] private SocialResolveBandResponses mid;

        [Header("Low Resolve")]
        [SerializeField] private SocialResolveBandResponses low;

        public string GetResponse(
            SocialExchangeOutcome outcome,
            int currentResolve,
            int maxResolve)
        {
            float percent = (float)currentResolve / maxResolve;

            SocialResolveBandResponses band =
                percent > 0.66f ? high :
                percent > 0.33f ? mid :
                                low;

            if (band == null)
                return string.Empty;

            return band.GetLine(outcome);
        }
    }
}