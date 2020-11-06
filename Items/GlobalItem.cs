using BuilderEssentials.UI.UIStates;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items
{
    public class GlobalItem : Terraria.ModLoader.GlobalItem
    {
        public override void UpdateInventory(Item item, Player player)
        {
            if (player.whoAmI == Main.myPlayer && 
                player.HeldItem.type != ItemType<MultiWand>() && ItemsUIState.multiWandWheel.Visible)
                ItemsUIState.multiWandWheel.Hide();
            
            if ( player.whoAmI == Main.myPlayer && 
                 player.HeldItem.type != ItemType<AutoHammer>() && ItemsUIState.autoHammerWheel.Visible)
                ItemsUIState.autoHammerWheel.Hide();
            
            if (player.whoAmI == Main.myPlayer && 
                player.HeldItem.type != ItemType<SpectrePaintingTool>() && ItemsUIState.paintWheel.Visible)
                ItemsUIState.paintWheel.Hide();
        }
    }
}