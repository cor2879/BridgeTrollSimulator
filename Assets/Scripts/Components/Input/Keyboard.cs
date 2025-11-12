namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Components.Input
{
    public static class Keyboard
    {
        public static readonly string[] KeyboardActions = new string[]
        {
            "Move North",
            "Move West",
            "Move South",
            "Move East",
            string.Empty,
            "Fire North",
            "Fire West",
            "Fire South",
            "Fire East",
            string.Empty,
            "Cycle Inventory Forward",
            "Cycle Inventory Back",
            string.Empty,
            "Open/Close Minimap",
            "Center Minimap on Player",
            "Zoom Minimap In",
            "Zoom Minimap Out",
            string.Empty,
            "Refresh room messages",
            "Pause/Open Menu "
        };

        public static readonly string[] KeyboardCommandKeys = new string[]
        {
            "W",
            "A",
            "S",
            "D",
            string.Empty,
            "NumPad 8",
            "NumPad 4",
            "NumPad 2",
            "NumPad 6",
            string.Empty,
            ".",
            ",",
            string.Empty,
            "M",
            "Space",
            "=",
            "-",
            string.Empty,
            "Space",
            "ESC"
        };
    }
}