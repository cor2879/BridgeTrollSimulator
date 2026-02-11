/**************************************************
 *  Constants.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Platform;

    /// <summary>
    /// Defines constant values that are used for maintaining the game state
    /// </summary>
    public static class Constants
    {
        public static class Cameras
        {

            public const string MainCamera = "MainCamera";
            public const string MinimapCamera = "MinimapCamera";
            public const string VirtualCamera = "VirtualCamera";
        }

        public const float DistanceTolerance = 0.001f;

        public const float HearingRange = 25.0f;

        /// <summary>
        /// The HeroFiringXDirection Animator parameter
        /// </summary>
        public const string HeroFiringXDirection = "HeroFiringXDirection";

        /// <summary>
        /// The HeroFiringYDirection Animator parameter
        /// </summary>
        public const string HeroFiringYDirection = "HeroFiringYDirection";

        public static class AnimatorParams
        {
            public const string xDirection = "xDirection";
            public const string Speed = "Speed";
        }

        public static class Triggers
        {
            public const string Attack = "Attack";
            public const string Die = "Die";
            public const string Itch = "Itch";
            public const string Jump = "Jump";
            public const string Threaten = "Threaten";
        }

        public const string HorizontalAxis = "Horizontal";

        public const string VerticalAxis = "Vertical";

        public const string Large = "Large";

        public const string Legendary = "Legendary";

        public const string Lots = "Lots";

        public const string Many = "Many";

        public const string Massive = "Massive";

        public const string Medium = "Medium";

        public const string No = "No";

        public const string None = "None";

        public const string One = "One";

        /// <summary>
        /// The pixels per unit
        /// </summary>
        public const float PixelsPerUnit = 64f;

        /// <summary>
        /// The primary scene
        /// </summary>
        public const string PrimaryScene = "Main";

        public const string Small = "Small";

        public const string Some = "Some";

        public const string Tiny = "Tiny";

        /// <summary>
        /// The title screen scene
        /// </summary>
        public const string TitleScreenScene = "TitleScreen";

        /// <summary>
        /// The game version
        /// </summary>
        public static readonly string Version = $"0.0.1-{PlatformManager.Platform}";

        /// <summary>
        /// The XDirection Animator parameter
        /// </summary>
        public const string XDirection = "xDirection";

        /// <summary>
        /// The XFiringDirection Animator parameter
        /// </summary>
        public const string XFiringDirection = "xFiringDirection";

        public const string Yes = "Yes";

        public static readonly string[] YesNoOptions = { No, Yes };

        /// <summary>
        /// The YDirection Animator parameter
        /// </summary>
        public const string YDirection = "yDirection";

        /// <summary>
        /// The YFiringDirection Animator parameter
        /// </summary>
        public const string YFiringDirection = "yFiringDirection";
    }
}