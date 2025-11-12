/**************************************************
 *  IGameManager.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces
{
    using OldSchoolGames.BridgeTrollSimulator.Scripts.MonoBehaviours;

    public interface IGameManager
    {
        AudioManager MusicManager { get; }

        AudioManager SoundEffectManager { get; }

        ISceneManager SceneManager { get; }

        PrefabLibrary PrefabLibrary { get; }
    }
}