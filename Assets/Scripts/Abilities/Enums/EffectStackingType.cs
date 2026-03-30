namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Abilities.Enums
{
    public enum EffectStackingType
    {
        Refresh,    // reset duration, keep magnitude
        Stack,      // add magnitude, keep both
        Replace,    // overwrite existing
        Ignore      // do nothing if already present.
    }
}