/**************************************************
 *  PlayerPrefsProperty.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{

    public abstract class PlayerPrefsProperty<TValue>
    {
        protected PlayerPrefsProperty(string key)
        {
            this.Key = key;
        }

        protected PlayerPrefsProperty(string key, TValue defaultValue)
        {
            this.Key = key;
            this.DefaultValue = defaultValue;
        }

        protected abstract PlayerPrefsDataType PlayerPrefsDataType { get; }

        protected TValue DefaultValue { get; set; }

        public string Key { get; private set; }

        protected TValue Value { get; set; }

        public virtual void Set(TValue value)
        {
            this.Value = value;
            this.Save();
        }

        public virtual TValue Get()
        {
            return this.Value;
        }

        public virtual void Save()
        {
            if (!PlayerPrefsManager.IsKeyRegistered(this.Key, this.PlayerPrefsDataType))
            {
                PlayerPrefsManager.RegisterKey(this.Key, this.PlayerPrefsDataType);
            }
        }
    }

    public class PlayerPrefsIntProperty
        : PlayerPrefsProperty<int>
    {
        public PlayerPrefsIntProperty(string key)
            : base(key)
        {
            PlayerPrefsManager.RegisterKey(Key, this.PlayerPrefsDataType);
            this.Value = PlayerPrefsManager.GetInt(Key);
        }

        public PlayerPrefsIntProperty(string key, int defaultValue)
            : base(key, defaultValue)
        {
            PlayerPrefsManager.RegisterKey(Key, this.PlayerPrefsDataType);
            this.Value = PlayerPrefsManager.GetInt(Key, defaultValue);
        }

        protected override PlayerPrefsDataType PlayerPrefsDataType { get => PlayerPrefsDataType.Int; }

        public override void Save()
        {
            base.Save();
            PlayerPrefsManager.SetProperty(this.Key, this.Value);
        }
    }

    public class PlayerPrefsBoolProperty
        : PlayerPrefsProperty<bool>
    {
        public PlayerPrefsBoolProperty(string key)
            : base(key)
        {
            PlayerPrefsManager.RegisterKey(key, this.PlayerPrefsDataType);
            this.Value = PlayerPrefsManager.GetBool(key);
        }

        public PlayerPrefsBoolProperty(string key, bool defaultValue)
            : base(key)
        {
            PlayerPrefsManager.RegisterKey(key, this.PlayerPrefsDataType);
            this.Value = PlayerPrefsManager.GetBool(key, defaultValue);
        }

        protected override PlayerPrefsDataType PlayerPrefsDataType { get => PlayerPrefsDataType.Bool; }

        public override void Save()
        {
            base.Save();
            PlayerPrefsManager.SetProperty(this.Key, this.Value);
        }
    }

    public class PlayerPrefsStringProperty
        : PlayerPrefsProperty<string>
    {
        public PlayerPrefsStringProperty(string key)
            : base(key)
        {
            PlayerPrefsManager.RegisterKey(key, this.PlayerPrefsDataType);
            this.Value = PlayerPrefsManager.GetString(key);
        }

        public PlayerPrefsStringProperty(string key, string defaultValue)
            : base(key, defaultValue)
        {
            PlayerPrefsManager.RegisterKey(key, this.PlayerPrefsDataType);
            var value = PlayerPrefsManager.GetString(key, defaultValue);

            if (value == null && defaultValue != null)
            {
                PlayerPrefsManager.SetProperty(key, defaultValue);
                value = defaultValue;
            }

            this.Value = value;
        }

        protected override PlayerPrefsDataType PlayerPrefsDataType { get => PlayerPrefsDataType.String; }

        public override void Save()
        {
            base.Save();
            PlayerPrefsManager.SetProperty(this.Key, this.Value);
        }
    }

    public class PlayerPrefsObjectProperty<TValue>
        : PlayerPrefsProperty<TValue>
    {
        public PlayerPrefsObjectProperty(string key, TValue defaultValue)
            : base(key)
        {
            PlayerPrefsManager.RegisterKey(key, this.PlayerPrefsDataType);
            var value = PlayerPrefsManager.GetData(key, defaultValue);

            if (value == null && defaultValue != null)
            {
                PlayerPrefsManager.SetDataProperty(key, defaultValue);
                value = defaultValue;
            }

            this.Value = value;
        }

        protected override PlayerPrefsDataType PlayerPrefsDataType { get => PlayerPrefsDataType.Json; }

        public override void Save()
        {
            base.Save();
            PlayerPrefsManager.SetDataProperty(this.Key, this.Value);
        }
    }
}
