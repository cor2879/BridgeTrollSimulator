#pragma warning disable CS0649
/**************************************************
 *  MusicSelectorBehaviour.cs
 *  
 *  copyright (c) 2023 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.UI
{
    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;

    public class MusicSelectorBehaviour : OptionSelectorBehaviour
    {
        public override void Initialize()
        {
            this.LocalizeText = false;
            this.Options = SoundClips.PlaylistFriendlyNames;
        }
    }
}
