using BuilderEssentials.UI;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class AutoHammer : ModItem
    {
        int toolRange;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Better than a regular hammer!");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Pwnhammer);
            item.tileBoost += 2;
            toolRange = 8;
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

                BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();
                if (modPlayer.infiniteRange || Tools.ToolHasRange(toolRange) && AutoHammerWheel.selectedIndex != -1)
                {
                    player.showItemIcon2 = ItemID.WoodenHammer;
                }
            }
        }

        int oldPosX;
        int oldPosY;
        Tile previousClickedTile;
        public override bool CanUseItem(Player player)
        {
            BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();

            if (AutoHammerWheel.IsAutoHammerUIVisible)
                return false;

            if (AutoHammerWheel.selectedIndex == -1)
                return true;

            if (modPlayer.infiniteRange || Tools.ToolHasRange(toolRange))
            {
                if (AutoHammerWheel.selectedIndex != -1)
                {
                    Tools.ChangeSlope(ref oldPosX, ref oldPosY, ref previousClickedTile, AutoHammerWheel.selectedIndex);
                    return false;
                }
            }

            return true;
        }
    }
}
