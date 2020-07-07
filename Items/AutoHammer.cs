using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class AutoHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Better than a regular hammer!");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Pwnhammer);
        }

        int mouseRightTimer = 0;
        public override void HoldItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (AutoHammerWheel.AutoHammerWheelPanel != null && !player.inventory[player.selectedItem].IsTheSameAs(item))
                {
                    AutoHammerWheel.AutoHammerWheelPanel.Remove();
                    AutoHammerWheel.AutoHammerUIOpen = false;
                }

                if (Main.mouseRight && Tools.IsUIAvailable()
                        && (!player.mouseInterface || (AutoHammerWheel.AutoHammerUIOpen && AutoHammerWheel.AutoHammerWheelPanel.IsMouseHovering))
                        && player.inventory[player.selectedItem].IsTheSameAs(item))
                {
                    if (++mouseRightTimer == 2)
                        AutoHammerWheel.AutoHammerUIOpen = !AutoHammerWheel.AutoHammerUIOpen;
                }

                if (Main.mouseRightRelease)
                    mouseRightTimer = 0;
            }
        }
    }
}
