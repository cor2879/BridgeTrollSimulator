#pragma warning disable CS0649
/**************************************************
 *  PlatformManager.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/
namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Platform
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using Debug = UnityEngine.Debug;

#if UNITY_STEAMWORKS
    using Steamworks;
#endif

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;
    using System.Diagnostics;
#if UNITY_EDITOR
    using UnityEditor.Localization.Plugins.XLIFF.V20;
#endif

    public class PlatformManager : MonoBehaviour
    {
#if UNITY_STEAMWORKS
        public static SteamManager SteamManager
        {
            get => SteamManager.Instance;
        }

        private Dictionary<string, Leaderboard> leaderboards;
#endif
        public const SupportedPlatform Platform =

#if UNITY_STANDALONE_OSX
                                        SupportedPlatform.OSX;
#elif UNITY_IOS
                                        SupportedPlatform.iOS;
#elif UNITY_WEBGL
                                        SupportedPlatform.WebGL;
#elif !DISABLESTEAMWORKS
                                        SupportedPlatform.Steam;
#else
                                        SupportedPlatform.Windows;
#endif

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static PlatformManager Instance { get; private set; }

        public static string UserName
        {
            get
            {
                string username = null;
#if UNITY_STEAM
                username = SteamManager.GetUsername();
#endif

#if UNITY_IOS
                username = GameCenterManager.Instance.GetUsername();
                Debug.Log($"Username: {username}");
#endif
                return username;
            }
        }

        /// <summary>
        /// Executes when this instance is awakened.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }

#if UNITY_STEAM
            this.leaderboards = new Dictionary<string, Leaderboard>()
            {
                {LeaderboardNames.TopScores,  new Leaderboard() { Name = LeaderboardNames.TopScores } },
                {LeaderboardNames.RoomsExplored, new Leaderboard() { Name = LeaderboardNames.RoomsExplored } }
            };

            foreach (var leaderboard in this.leaderboards.Values)
            {
                leaderboard.Init();
            }
#endif
#if UNITY_IOS
            KTGameCenter.SharedCenter().Authenticate();
#endif

        }

        void OnEnable()
        {
#if UNITY_IOS
            StartCoroutine(RegisterForGameCenter());
#endif
        }

#if UNITY_IOS
        IEnumerator RegisterForGameCenter()
        {
            yield return new WaitForSeconds(0.5f);
            KTGameCenter.SharedCenter().GCUserAuthenticated += GCAuthentication;
            // KTGameCenter.SharedCenter().GCScoreSubmitted += ScoreSubmitted;
            // KTGameCenter.SharedCenter().GCAchievementSubmitted += AchievementSubmitted;
            // KTGameCenter.SharedCenter().GCAchievementsReset += AchivementsReset;
            // KTGameCenter.SharedCenter().GCMyScoreFetched += MyScoreFetched;
        }

        void GCAuthentication(string status)
        {
            Debug.Log("delegate call back status= " + status);
            StartCoroutine(CheckAttributes());
        }

        IEnumerator CheckAttributes()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log(" alias= " + KTGameCenter.SharedCenter().PlayerAlias + " name= " +
                   KTGameCenter.SharedCenter().PlayerName + " id= " + KTGameCenter.SharedCenter().PlayerId);
        }
#endif
        public void EarnBadge(Badge badge)
        {
#if UNITY_STEAM
            StartCoroutine(nameof(this.EarnBadgeRoutine), badge);
#endif

#if UNITY_IOS
            KTGameCenter.SharedCenter().SubmitAchievement(100, badge.Name, true);
#endif
        }

        public void UpdateLeaderboardScore(string leaderboardName, int score)
        {
#if UNITY_STEAM
            this.leaderboards[leaderboardName].UploadLeaderboardScore(score);
#endif
        }

#if UNITY_STEAM
        private IEnumerator EarnBadgeRoutine(Badge badge)
        {
            try
            {
                var earned = SteamUserStats.SetAchievement(badge.Name);
                Debug.Log($"Badge {badge.Name} earned: {earned}");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            yield return null;
        }
#endif

#if UNITY_IOS
        private void AchievementSubmitted(string achId, string error)
        {
            
        }
#endif

        public void SaveFile(string text)
        {
#if UNITY_STEAM
            SteamManager.SaveFileOffline(text);
#endif
        }

        public string GetSaveFileContent()
        {
#if UNITY_STEAM
            return SteamManager.GetSaveFileContent();
#else
            return string.Empty;
#endif
        }
    }
}
