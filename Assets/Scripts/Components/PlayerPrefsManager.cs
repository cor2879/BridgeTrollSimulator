/**************************************************
 *  PlayerPrefsProperty.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using UnityEngine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Platform;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
    using System.Text.Json.Serialization;

    public static class PlayerPrefsManager
    {
        private const string PlayerPrefsVersion = "1.0.0.0";
        private const string VersionKey = "PlayerPrefsVersion";
        private const string MasterKey = "KeyRegistry";

        private static Dictionary<string, PlayerPrefsDataType> keys = Initialize();

        private static Dictionary<string, PlayerPrefsDataType> Keys { get => keys; }

        public static void RegisterKey(string keyName, PlayerPrefsDataType dataType)
        {
            if (!Keys.ContainsKey(keyName))
            {
                Keys.Add(keyName, dataType);
            }

            SaveMasterKeyList();
        }

        private static void SaveMasterKeyList()
        {
            PlayerPrefs.SetString(MasterKey, JsonSerializer.Serialize(Keys));
            PlayerPrefsManager.SavePlayerPrefs();
        }

        private static Dictionary<string, PlayerPrefsDataType> Initialize()
        {
            if (PlayerPrefs.HasKey(VersionKey))
            {
                var playerPrefsVersion = PlayerPrefs.GetString(VersionKey);
                Debug.Log($"Current instance PlayerPrefs Version: {playerPrefsVersion ?? (null)}");

                if (!string.Equals(playerPrefsVersion, PlayerPrefsManager.PlayerPrefsVersion))
                {
                    Debug.Log("Installed PlayerPrefs Version is not up to date.");
                }
            }
            else
            {
                Debug.Log("Legacy PlayerPrefs Version detected.  Updating to current PlayerPrefs version.");
                PlayerPrefs.SetString(VersionKey, PlayerPrefsManager.PlayerPrefsVersion);
            }

            if (PlayerPrefs.HasKey(MasterKey))
            {
                var masterKeyList = PlayerPrefs.GetString(MasterKey);
                Debug.Log($"PlayerPrefs Master Key List: {masterKeyList}");

                return JsonSerializer.Deserialize<Dictionary<string, PlayerPrefsDataType>>(masterKeyList);
            }

            return new Dictionary<string, PlayerPrefsDataType>();
        }

        public static void ImportPlayerPrefs()
        {
            var savedPreferences =
                JsonSerializer.Deserialize<Dictionary<string, Pair<PlayerPrefsDataType, object>>>(PlatformManager.Instance.GetSaveFileContent());

            ImportPlayerPreferences(savedPreferences);
        }

        public static bool IsKeyRegistered(string keyName, PlayerPrefsDataType dataType)
        {
            return Keys.ContainsKey(keyName) && Keys[keyName] == dataType;
        }

        public static IEnumerable<Pair<string, PlayerPrefsDataType>> GetKeys()
        {
            foreach (var kvp in Keys)
            {
                yield return new Pair<string, PlayerPrefsDataType>() { First = kvp.Key, Second = kvp.Value };
            }
        }

        /// <summary>
        /// Clears out all stored PlayerPrefs data.
        /// </summary>
        public static void ClearAll()
        {
            foreach (var key in PlayerPrefsManager.GetKeys().Select(pair => pair.First).ToArray())
            {
                PlayerPrefsManager.DeleteKey(key);
            }
        }

        private static void ImportPlayerPreferences(Dictionary<string, Pair<PlayerPrefsDataType, object>> playerPreferences)
        {
            foreach (var kvp in playerPreferences)
            {
                if (!IsKeyRegistered(kvp.Key, kvp.Value.First))
                {
                    RegisterKey(kvp.Key, kvp.Value.First);
                }

                switch (kvp.Value.Second)
                {
                    case PlayerPrefsDataType.Int:
                        SetProperty(kvp.Key, (int)kvp.Value.Second);
                        break;
                    case PlayerPrefsDataType.Float:
                        SetProperty(kvp.Key, (float)kvp.Value.Second);
                        break;
                    case PlayerPrefsDataType.String:
                        SetProperty(kvp.Key, (string)kvp.Value.Second);
                        break;
                    case PlayerPrefsDataType.Bool:
                        SetProperty(kvp.Key, (bool)kvp.Value.Second);
                        break;
                    case PlayerPrefsDataType.DateTime:
                        SetProperty(kvp.Key, (DateTime)kvp.Value.Second);
                        break;
                    case PlayerPrefsDataType.DateTimeOffset:
                        SetProperty(kvp.Key, (DateTimeOffset)kvp.Value.Second);
                        break;
                    case PlayerPrefsDataType.Json:
                        SetJsonPropertyRaw(kvp.Key, (string)kvp.Value.Second);
                        break;
                }
            }
        }

        public static Dictionary<string, Pair<PlayerPrefsDataType, object>> GetAllPlayerPrefs()
        {
            var dictionary = new Dictionary<string, Pair<PlayerPrefsDataType, object>>();

            foreach (var kvp in Keys)
            {
                var pair = new Pair<PlayerPrefsDataType, object>() { First = kvp.Value };

                switch (kvp.Value)
                {
                    case PlayerPrefsDataType.Int:
                        pair.Second = GetInt(kvp.Key);
                        break;
                    case PlayerPrefsDataType.Float:
                        pair.Second = GetFloat(kvp.Key);
                        break;
                    case PlayerPrefsDataType.String:
                        pair.Second = GetString(kvp.Key);
                        break;
                    case PlayerPrefsDataType.Bool:
                        pair.Second = GetBool(kvp.Key);
                        break;
                    case PlayerPrefsDataType.DateTime:
                        pair.Second = GetDateTime(kvp.Key);
                        break;
                    case PlayerPrefsDataType.DateTimeOffset:
                        pair.Second = GetDateTimeOffset(kvp.Key);
                        break;
                    case PlayerPrefsDataType.Json:
                        pair.Second = GetJsonString(kvp.Key);
                        break;
                }

                dictionary.Add(kvp.Key, pair);
            }

            return dictionary;
        }

        public static void SetProperty(string keyName, int value)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Int)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as an int data type.");
            }

            PlayerPrefs.SetInt(keyName, value);
            PlayerPrefsManager.SavePlayerPrefs();
        }

        public static void SetProperty(string keyName, float value)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Float)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a float data type.");
            }

            PlayerPrefs.SetFloat(keyName, value);
            PlayerPrefsManager.SavePlayerPrefs();
        }

        public static void SetProperty(string keyName, string value)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.String)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a string data type.");
            }

            PlayerPrefs.SetString(keyName, value);
            PlayerPrefsManager.SavePlayerPrefs();
        }

        private static void SetJsonPropertyRaw(string keyName, string value)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Json)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a JSON data type.");
            }

            PlayerPrefs.SetString(keyName, value);
            PlayerPrefsManager.SavePlayerPrefs();
        }

        public static void SetProperty(string keyName, bool value)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Bool)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a bool data type.");
            }

            PlayerPrefs.SetInt(keyName, value ? 1 : 0);
            PlayerPrefsManager.SavePlayerPrefs();
        }

        public static void SetProperty(string keyName, DateTime value)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.DateTime)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a DateTime data type.");
            }

            PlayerPrefs.SetString(keyName, value.ToString());
            PlayerPrefsManager.SavePlayerPrefs();
        }

        public static void SetProperty(string keyName, DateTimeOffset value)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.DateTimeOffset)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a DateTimeOffset data type.");
            }

            PlayerPrefs.SetString(keyName, value.ToString());
            PlayerPrefsManager.SavePlayerPrefs();
        }

        public static void SetDataProperty<TData>(string keyName, TData data)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Json)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a JSON data type.");
            }

            PlayerPrefs.SetString(keyName, JsonSerializer.Serialize(data));
            PlayerPrefsManager.SavePlayerPrefs();
        }

        public static int GetInt(string keyName)
        {
            return GetInt(keyName, 0);
        }

        public static int GetInt(string keyName, int defaultValue)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Int)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as an int data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return defaultValue;
            }

            return PlayerPrefs.GetInt(keyName);
        }

        public static float GetFloat(string keyName)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Float)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a float data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return 0.0f;
            }

            return PlayerPrefs.GetFloat(keyName);
        }

        public static string GetString(string keyName)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.String)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a string data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return null;
            }

            return PlayerPrefs.GetString(keyName);
        }

        public static string GetString(string keyName, string defaultValue)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.String)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a string data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return defaultValue;
            }

            return PlayerPrefs.GetString(keyName);
        }

        public static string GetJsonString(string keyName)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Json)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a JSON data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return null;
            }

            return PlayerPrefs.GetString(keyName);
        }

        public static bool GetBool(string keyName)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Bool)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a bool data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return default(bool);
            }

            return PlayerPrefs.GetInt(keyName) != 0;
        }

        public static bool GetBool(string keyName, bool defaultValue)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Bool)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a bool data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return defaultValue;
            }

            return PlayerPrefs.GetInt(keyName) != 0;
        }

        public static DateTime GetDateTime(string keyName)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.DateTime)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a DateTime data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return default(DateTime);
            }

            if (DateTime.TryParse(PlayerPrefs.GetString(keyName), out var date))
            {
                return date;
            }

            return default(DateTime);
        }

        public static DateTimeOffset GetDateTimeOffset(string keyName)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.DateTimeOffset)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a DateTimeOffset data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return default(DateTimeOffset);
            }

            if (DateTimeOffset.TryParse(PlayerPrefs.GetString(keyName), out var date))
            {
                return date;
            }

            return default(DateTimeOffset);
        }

        public static TData GetData<TData>(string keyName)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Json)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a JSON data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return default(TData);
            }

            return JsonSerializer.Deserialize<TData>(PlayerPrefs.GetString(keyName));
        }

        public static TData GetData<TData>(string keyName, TData defaultData)
        {
            if (!Keys.ContainsKey(keyName))
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered.");
            }

            if (Keys[keyName] != PlayerPrefsDataType.Json)
            {
                throw new PlayerPrefsException($"The key {keyName} is not registered as a JSON data type.");
            }

            if (!PlayerPrefs.HasKey(keyName))
            {
                return defaultData;
            }

            return JsonSerializer.Deserialize<TData>(PlayerPrefs.GetString(keyName));
        }

        public static void DeleteKey(string keyName)
        {
            if (PlayerPrefs.HasKey(keyName))
            {
                PlayerPrefs.DeleteKey(keyName);
            }

            if (Keys.ContainsKey(keyName))
            {
                Keys.Remove(keyName);
            }

            SaveMasterKeyList();
        }

        public static void SavePlayerPrefs()
        {
            PlayerPrefs.Save();
            // PlatformManager.Instance?.SaveFile(JsonSerialize.Serialize(GetAllPlayerPrefs()));
        }
    }
}
