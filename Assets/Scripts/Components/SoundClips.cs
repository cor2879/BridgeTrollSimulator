/**************************************************
 *  SoundClips.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components
{
    using System.Linq;

    /// <summary>
    /// Static class that stores the name and location of all of the game's sound clips.
    /// These constants should be used when accessing any Sound Clip in the AudioManager class.
    /// </summary>
    public static class SoundClips
    {
        #region Sound Effects and Short Clips

        public const string AmbientCave = "Audio/Sound Effects/Cave_01";

        public const string AngryMonsterRoar = "Audio/Sound Effects/AngryMonsterRoar";

        public const string ArrowHit = "Audio/Sound Effects/ArrowHit";

        public const string ArrowShoot = "Audio/Sound Effects/ArrowShoot";

        public const string BadgeEarned = "Audio/Music/BadgeEarned";

        public static readonly string[] BatFlapping =
        {
            Flapping1
        };

        public const string BearTrapGet = "Audio/Sound Effects/BearTrapGet";

        public const string BearTrapSet = "Audio/Sound Effects/BearTrapSet";

        public const string BearTrapTrigger = "Audio/Sound Effects/BearTrapTrigger";

        public const string Bell = "Audio/Sound Effects/Bell";

        public const string BigVictory = "Audio/Sound Effects/Big Victory";

        public const string Blip = "Audio/Sound Effects/Blip";

        public const string BowString1 = "Audio/Sound Effects/Bow string drawing fast 1";

        public const string BowString2 = "Audio/Sound Effects/Bow string drawing fast 2";

        public const string BowString3 = "Audio/Sound Effects/Bow string drawing fast 3";

        public const string BowString4 = "Audio/Sound Effects/Bow string drawing fast 4";

        public const string BowString5 = "Audio/Sound Effects/Bow string drawing fast 5";

        public static readonly Carousel<string> BowStrings = new Carousel<string>(new[]
        {
            BowString1,
            BowString2,
            BowString3,
            BowString4,
            BowString5
        });

        public const string Click1 = "Audio/Sound Effects/Click1";

        public const string Click2 = "Audio/Sound Effects/Click2";

        public const string Crash1 = "Audio/Sound Effects/Crash 1";

        public const string Crash2 = "Audio/Sound Effects/Crash 2";

        public const string Defeat = "Audio/Sound Effects/Defeat";

        public const string Falling = "Audio/Sound Effects/Falling2";

        public const string Flapping1 = "Audio/Sound Effects/Bird Flapping Wings_LOUDER";

        public const string Flapping2 = "Audio/Sound Effects/Bird Flapping Wings";

        public const string Flapping3 = "Audio/Sound Effects/Bird Flapping Wings_1";

        public const string Flapping4 = "Audio/Sound Effects/Bird Flapping Wings_4";

        public const string Footstep = "Audio/Sound Effects/walking";

        public const string GlassClink2 = "Audio/Sound Effects/GlassClink2";

        public static Carousel<string> Grunting = new Carousel<string>(new[]
        {
            LargeGrunt,
            MediumGrunt,
            ZombieGrunt1,
            ZombieGrunt2,
            ZombieGrunt3,
            ZombieGrunt4
        });

        public const string HugeBreathing1 = "Audio/Sound Effects/Huge monster Breathing 1";

        public const string HugeBreathing2 = "Audio/Sound Effects/Huge monster Breathing 2";

        public static readonly string[] HugeBreathing =
        {
            HugeBreathing1,
            HugeBreathing2,
            HugeBreathing2
        };

        public const string HugeFootstep1 = "Audio/Sound Effects/Huge Footsteps 12";

        public const string HugeFootstep2 = "Audio/Sound Effects/Huge Footsteps 13";

        public static readonly string[] HugeMonsterFootsteps =
        {
            HugeFootstep1,
            HugeFootstep2
        };

        public static readonly string[] IdleFlapping =
{
            Flapping2,
            Flapping3,
            Flapping4
        };

        public static readonly string[] IdleWind =
{
            Wind1,
            Wind2,
            Wind3,
            Wind4
        };

        public const string Ignite1 = "Audio/Sound Effects/Ignite1";

        public const string Ignite2 = "Audio/Sound Effects/Ignite2";

        public const string LargeAnimalFootstep1 = "Audio/Sound Effects/Large animal footsteps _5";

        public const string LargeAnimalFootstep2 = "Audio/Sound Effects/Large animal footsteps _6";

        public const string LargeBreathing1 = "Audio/Sound Effects/large monster Breathing 1";

        public const string LargeBreathing2 = "Audio/Sound Effects/large monster Breathing 2";

        public static readonly string[] LargeBreathing =
        {
            LargeBreathing1,
            LargeBreathing2
        };

        public const string LargeFootstep1 = "Audio/Sound Effects/Large monster footsteps 4";

        public const string LargeFootstep2 = "Audio/Sound Effects/Large monster footsteps 5";

        public static readonly string[] LargeMonsterFootsteps =
        {
            LargeFootstep1,
            LargeFootstep2
        };

        public const string LargeGrunt = "Audio/Sound Effects/large monster Grunt (Gets hit) 3";

        public const string LionRoar = "Audio/Sound Effects/LionRoar";

        public const string Lose = "Audio/Sound Effects/Lose";

        public const string LowRoar = "Audio/Sound Effects/LowRoar";

        public const string MediumBreathing1 = "Audio/Sound Effects/Medium monster Breathing 1";

        public const string MediumBreathing2 = "Audio/Sound Effects/Medium monster Breathing 2";

        public static readonly string[] MediumBreathing =
        {
            MediumBreathing2,
            MediumBreathing2,
            MediumBreathing1
        };

        public const string MediumFootstep1 = "Audio/Sound Effects/Medium animal footsteps _4";

        public const string MediumFootstep2 = "Audio/Sound Effects/Medium animal footsteps _5";

        public static readonly string[] MediumMonsterFootsteps =
        {
            MediumFootstep1,
            MediumFootstep2
        };

        public const string MediumGrunt = "Audio/Sound Effects/Medium monster Grunt (Gets hit) 5";

        public const string MetalClick = "Audio/Sound Effects/MetalClick";

        public const string MonsterRoar = "Audio/Sound Effects/MonsterRoar";

        public const string MonsterDeath = "Audio/Sound Effects/MonsterDeath";

        public const string OpenBottle = "Audio/Sound Effects/OpenBottle";

        public const string PlayerFootstep1 = "Audio/Sound Effects/Concrete footsteps 1";

        public const string PlayerFootstep2 = "Audio/Sound Effects/Concrete footsteps 2";

        public const string PlayerFootstep3 = "Audio/Sound Effects/Concrete footsteps 3";

        public const string PlayerFootstep4 = "Audio/Sound Effects/Concrete footsteps 4";

        public static readonly string[] PlayerFootsteps =
        {
            PlayerFootstep1,
            PlayerFootstep2,
            PlayerFootstep3,
            PlayerFootstep4
        };

        public const string Recovery = "Audio/Sound Effects/Recovery3";

        public const string RetroScream = "Audio/Sound Effects/retroScream";

        public const string Roar = "Audio/Sound Effects/Roar";

        public const string RopeNet = "Audio/Sound Effects/RopeNet";

        public const string SmallExplosion = "Audio/Sound Effects/SmallExplosion";

        public const string SmallVictory = "Audio/Sound Effects/Small Victory";

        public const string SuccessDing = "Audio/Sound Effects/Success2";

        public const string Up = "Audio/Sound Effects/Up";

        public const string Victory = "Audio/Music/Victory";

        public const string Wilhelm = "Audio/Sound Effects/Wilhelm";

        public const string Wind1 = "Audio/Sound Effects/Wind 1";

        public const string Wind2 = "Audio/Sound Effects/Wind 2";

        public const string Wind3 = "Audio/Sound Effects/Wind 3";

        public const string Wind4 = "Audio/Sound Effects/Wind 4";

        public const string WindStorm = "Audio/Sound Effects/Wind Storm";

        public const string ZombieGrunt1 = "Audio/Sound Effects/Zombie Hit_01";

        public const string ZombieGrunt2 = "Audio/Sound Effects/Zombie Hit_03";

        public const string ZombieGrunt3 = "Audio/Sound Effects/Zombie Hit_04";

        public const string ZombieGrunt4 = "Audio/Sound Effects/Zombie Hit_05";

        #endregion

        #region Songs

        public const string AnotherGalaxy = "Audio/Music/Another Galaxy";

        public const string CaveBattle = "Audio/Music/Cave Battle";

        public const string CurseOfTheWolf = "Audio/Music/Curse Of The Wolf";

        public const string DarkDream = "Audio/Music/Dark Dream";

        public const string EerieScene = "Audio/Music/Eerie Scene";

        public const string FuturisticTech = "Audio/Music/Futuristic Tech";

        public const string HeroInTheDarkness = "Audio/Music/Hero in the Darkness";

        public const string HuntTheMuglump = "Audio/Music/HuntTheMuglump";

        public const string MixAndMash = "Audio/Music/Mix And Mash";

        public const string MuglumpDungeon = "Audio/Music/MuglumpDungeon";

        public const string Ominous = "Audio/Music/Ominous";

        public const string RPGMain1 = "Audio/Music/RPG Main 1";

        public const string TheArcade = "Audio/Music/The Arcade";

        public const string TheEmbraceFinalFight = "Audio/Music/The Embrace (Final Fight)";

        public const string TitleScreenBgm = "Audio/Music/TitleScreen_Bgm";

        public const string UIHover = "Audio/Sound Effects/UIHover";

        public const string WelcomeToTheClub = "Audio/Music/Welcome To The Club";

        public static string CurrentBGM { get; set; } = SoundClips.WelcomeToTheClub;

        public static readonly string[] Playlist =
        {
            AnotherGalaxy,
            CaveBattle,
            CurseOfTheWolf,
            EerieScene,
            FuturisticTech,
            HeroInTheDarkness,
            HuntTheMuglump,
            MixAndMash,
            RPGMain1,
            TheArcade,
            TheEmbraceFinalFight,
            TitleScreenBgm,
            WelcomeToTheClub
        };

        public static readonly string[] PlaylistFriendlyNames =
        {
            AnotherGalaxy.Split('/').Last(),
            CaveBattle.Split('/').Last(),
            CurseOfTheWolf.Split('/').Last(),
            EerieScene.Split('/').Last(),
            FuturisticTech.Split('/').Last(),
            HeroInTheDarkness.Split('/').Last(),
            HuntTheMuglump.Split('/').Last(),
            MixAndMash.Split('/').Last(),
            RPGMain1.Split('/').Last(),
            TheArcade.Split('/').Last(),
            TheEmbraceFinalFight.Split('/').Last(),
            TitleScreenBgm.Split('/').Last(),
            WelcomeToTheClub.Split('/').Last()
        };

        #endregion
    }
}