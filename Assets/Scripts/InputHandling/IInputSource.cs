namespace OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling
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