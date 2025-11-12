namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Exception
{
    using System;
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;

    public class BadgeException : Exception
    {
        public BadgeException() { }

        public BadgeException(string message) : base(message) { }

        public BadgeException(string message, Exception innerException) : base(message, innerException) { }

        public BadgeException(Badge badge, string message) : this(message)
        {
            this.Badge = badge;
        }

        public BadgeException(Badge badge, string message, Exception innerException) : this(message, innerException)
        {
            this.Badge = badge;
        }

        public Badge Badge { get; private set; }
    }
}
