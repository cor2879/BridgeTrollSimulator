namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions
{
    using System;

    public class PlayerPrefsException : Exception
    {
        public PlayerPrefsException() { }

        public PlayerPrefsException(string message) : base(message) { }

        public PlayerPrefsException(string message, Exception innerException) { }
    }
}
