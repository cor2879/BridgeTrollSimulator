#if UNITY_IOS
namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Platform
{
    using UnityEngine;
    using UnityEngine.SocialPlatforms.GameCenter;

    using System.Collections;

    public class GameCenterManager : MonoBehaviour
    {
        public static GameCenterManager Instance { get; private set; }

        public string GetUsername()
        {
            return KTGameCenter.SharedCenter().PlayerAlias;
        }

        public void Authenticate()
        {
            KTGameCenter.SharedCenter().Authenticate();
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
        }

        void OnEnable()
        {
            StartCoroutine(RegisterForGameCenter());
        }

        void OnDisable()
        {
            KTGameCenter.SharedCenter().GCUserAuthenticated -= GCAuthentication;
            KTGameCenter.SharedCenter().GCScoreSubmitted -= ScoreSubmitted;
            KTGameCenter.SharedCenter().GCAchievementSubmitted -= AchievementSubmitted;
            KTGameCenter.SharedCenter().GCAchievementsReset -= AchivementsReset;
            KTGameCenter.SharedCenter().GCMyScoreFetched -= MyScoreFetched;
        }

        IEnumerator RegisterForGameCenter()
        {
            yield return new WaitForSeconds(0.5f);
            KTGameCenter.SharedCenter().GCUserAuthenticated += GCAuthentication;
            KTGameCenter.SharedCenter().GCScoreSubmitted += ScoreSubmitted;
            KTGameCenter.SharedCenter().GCAchievementSubmitted += AchievementSubmitted;
            KTGameCenter.SharedCenter().GCAchievementsReset += AchivementsReset;
            KTGameCenter.SharedCenter().GCMyScoreFetched += MyScoreFetched;
        }

        void Start()
        {
            KTGameCenter.SharedCenter().Authenticate();
        }

        void GCAuthentication(string status)
        { 
            Debug.Log("delegate call back status= " + status);
            StartCoroutine(CheckAttributes());
        }

        void ScoreSubmitted(string leaderboardId, string error)
        {
            print("score submitted with id " + leaderboardId + " and error= " + error);
        }

        void AchievementSubmitted(string achId, string error)
        {
            print("achievement submitted with id " + achId + " and error= " + error);
        }

        void AchivementsReset(string error)
        {
            print("Achievment reset with error= " + error);
        }


        void MyScoreFetched(string leaderboardId, int score, string error)
        {
            print("My score for leaderboardId= " + leaderboardId + " is " + score + " with error= " + error);
        }

        IEnumerator CheckAttributes()
        {
            yield return new WaitForSeconds(1.0f);
            print(" alias= " + KTGameCenter.SharedCenter().PlayerAlias + " name= " +
                   KTGameCenter.SharedCenter().PlayerName + " id= " + KTGameCenter.SharedCenter().PlayerId);
        }
    }
}
#endif