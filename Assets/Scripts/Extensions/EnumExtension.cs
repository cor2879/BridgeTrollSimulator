namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            return (value.GetType().
                GetField(value.ToString()).
                GetCustomAttributes(typeof(DescriptionAttribute), false).
                FirstOrDefault() as DescriptionAttribute).Description;
        }
    }
}
