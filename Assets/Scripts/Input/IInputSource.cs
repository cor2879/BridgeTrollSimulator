namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Input
{
    public interface IInputSource
    {
        float GetHorizontal();
        float GetVertical();
        bool AttackPressed();
        bool ThreatenPressed();
        bool ItchPressed();
        bool JumpPressed();
        bool DiePressed();
    }
}