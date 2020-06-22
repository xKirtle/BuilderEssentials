using Terraria;

namespace BuilderEssentials.Utilities
{
    public class UIUtilities
    {
        public static bool IsUIAvailable()
        {
            //Add specific UI mouse hoverings etc..
            var player = Main.LocalPlayer;
            return Main.hasFocus &&
                !Main.playerInventory &&
                !Main.drawingPlayerChat &&
                !Main.editSign &&
                !Main.editChest &&
                !Main.blockInput &&
                !Main.gamePaused &&
                !player.mouseInterface &&
                !Main.mapFullscreen &&
                !Main.HoveringOverAnNPC &&
                !player.showItemIcon &&
                player.talkNPC == -1 &&
                (player.itemTime == 0 && player.itemAnimation == 0) &&
                !player.dead &&
                !player.CCed;
        }
    }
}