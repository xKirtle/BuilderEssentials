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
        //TODO: ENSURE MULTIPLAYER COMPATIBILITY
        public List<int> paints;
        public override void SetDefaults()
        {
            paints = new List<int>();
            for (int i = 0; i < 27; i++) //Basic && Deep colors
                paints.Add(1073 + i);
            for (int i = 0; i < 3; i++)
                paints.Add(1966 + i);   //Extra Effects

            item.height = 44;
            item.width = 44;
            item.useTime = 10;
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
            BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.inventory[player.selectedItem].IsTheSameAs(item))
                    modPlayer.holdingPaintingTool = true;

                if (PaintWheel.paintWheel != null && !modPlayer.holdingPaintingTool)
                {
                    PaintWheel.paintWheel.Remove();
                    BasePanel.paintingUIOpen = false;
                }

                if (Main.mouseRight && player.talkNPC == -1 && !Main.HoveringOverAnNPC && !player.showItemIcon && !Main.editSign
                        && !Main.editChest && !Main.blockInput && !player.dead && !Main.gamePaused && Main.hasFocus && !player.CCed
                        && (!player.mouseInterface || (BasePanel.paintingUIOpen && BasePanel.paintingPanel.IsMouseHovering))
                        && player.inventory[player.selectedItem].IsTheSameAs(item) && !BasePanel.creativeWheelUIOpen)
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

            int posX = Player.tileTargetX;
            int posY = Player.tileTargetY;
            Tile pointedTile = Main.tile[posX, posY];
            if (foundModdedPaint && !BasePanel.paintingPanel.IsMouseHovering)
            {

                bool anyOperationDone = false;
                //selectedindex + 1 because paint bytes don't start at 0
                switch (modPlayer.paintingToolSelected)
                {
                    case 0:
                        if (pointedTile.color() != (modPlayer.paintingColorSelectedIndex + 1) && modPlayer.paintingColorSelectedIndex != 30)
                        {
                            pointedTile.color((byte)(modPlayer.paintingColorSelectedIndex + 1));
                            anyOperationDone = true;
                        }
                        break;
                    case 1:
                        if (pointedTile.wallColor() != (modPlayer.paintingColorSelectedIndex + 1) && modPlayer.paintingColorSelectedIndex != 30)
                        {
                            pointedTile.wallColor((byte)(modPlayer.paintingColorSelectedIndex + 1));
                            anyOperationDone = true;
                        }
                        break;
                    case 2:
                        if (pointedTile.color() != 0)
                        {
                            pointedTile.color(0);
                            anyOperationDone = true;
                        }
                        if (pointedTile.wallColor() != 0)
                        {
                            pointedTile.wallColor(0);
                            anyOperationDone = true;
                        }
                        break;
                }

                if (anyOperationDone)
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendTileSquare(-1, posX, posY, 1); //syncs painting tiles and walls, not the scraper
                    }
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpectrePaintbrush);
            recipe.AddIngredient(ItemID.SpectrePaintRoller);
            recipe.AddIngredient(ItemID.SpectrePaintScraper);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}