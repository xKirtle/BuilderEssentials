using System.Collections.Generic;
using BuilderEssentials.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items
{
    class SuperPaintingTool : ModItem
    {
        public List<int> paints;
        public override void SetDefaults()
        {
            paints = new List<int>();
            for (int i = 0; i < 27; i++) //Basic && Deep colors
                paints.Add(1073 + i);
            for (int i = 0; i < 3; i++)
                paints.Add(1966 + i);   //Extra Effects

            item.height = 20;
            item.width = 18;
            item.useTime = 1;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            item.noUseGraphic = true;
        }

        int mouseRightTimer = 0;
        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (Main.mouseRight && player.talkNPC == -1 && !Main.HoveringOverAnNPC && !player.showItemIcon && !Main.editSign
                        && !Main.editChest && !Main.blockInput && !player.dead && !Main.gamePaused && Main.hasFocus && !player.CCed
                        && (!player.mouseInterface || (BasePanel.paintingUIOpen && BasePanel.paintingPanel.IsMouseHovering))
                        && player.inventory[player.selectedItem].IsTheSameAs(this.item))
                {
                    if (++mouseRightTimer == 2)
                        BasePanel.paintingUIOpen = !BasePanel.paintingUIOpen;
                }

                if (Main.mouseRightRelease)
                    mouseRightTimer = 0;
            }
        }

        public override void HoldItem(Player player)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
            if (modPlayer.paintingColorSelectedIndex != 30)
            {
                player.showItemIcon = true;
                switch (modPlayer.paintingToolSelected)
                {
                    case 0:
                        player.showItemIcon2 = ItemID.SpectrePaintbrush;
                        break;
                    case 1:
                        player.showItemIcon2 = ItemID.SpectrePaintRoller;
                        break;
                    case 2:
                        player.showItemIcon2 = ItemID.SpectrePaintScraper;
                        break;
                }
            }
            else if (modPlayer.paintingColorSelectedIndex == 30 && modPlayer.paintingToolSelected == 2)
            {
                player.showItemIcon = true;
                player.showItemIcon2 = ItemID.SpectrePaintScraper;
            }
        }

        public override bool CanUseItem(Player player)
        {
            BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();
            bool foundModdedPaint = false;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type == mod.ItemType("InfinitePaintBucket"))
                {
                    foundModdedPaint = true;
                    break;
                }
            }

            Tile pointedTile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            if (foundModdedPaint && !BasePanel.paintingPanel.IsMouseHovering)
            {
                //selectedindex + 1 because bytes don't start at 0
                switch (modPlayer.paintingToolSelected)
                {
                    case 0:
                        if (pointedTile.color() != (modPlayer.paintingColorSelectedIndex + 1))
                            pointedTile.color((byte)(modPlayer.paintingColorSelectedIndex + 1));
                        break;
                    case 1:
                        if (pointedTile.wallColor() != (modPlayer.paintingColorSelectedIndex + 1))
                            pointedTile.wallColor((byte)(modPlayer.paintingColorSelectedIndex + 1));
                        break;
                    case 2:
                        if (pointedTile.color() != 0)
                            pointedTile.color(0);
                        if (pointedTile.wallColor() != 0)
                            pointedTile.wallColor(0);
                        break;
                }
            }
            return false;
        }
    }

    public class PaintGlobalItem : GlobalItem
    {
        // public override bool UseItem(Item item, Player player)
        // {

        //     //if item id is a paint tool and there is paint in the inventory, reduce the stack by 1?
        //     //if infinitepaintbucket is on the inventory, allow any color
        //     return true;
        // }
    }
}