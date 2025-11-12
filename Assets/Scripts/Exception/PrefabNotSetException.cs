namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Exceptions
{
    using System;

    public class PrefabNotSetException : Exception
    {
        public PrefabNotSetException() { }

        public PrefabNotSetException(string message) : base(message) { }

        public PrefabNotSetException(string message, Exception innerException) { }
    }
}
