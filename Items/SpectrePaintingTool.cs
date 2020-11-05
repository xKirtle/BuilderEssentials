using BuilderEssentials.Utilities;
using System.Collections.Generic;
using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIStates;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items
{
    class SpectrePaintingTool : ModItem
    {
        private Point toolRange;
        private bool canPaint;

        public override string Texture => "BuilderEssentials/Textures/Items/SpectrePaintingTool";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Able to paint and remove paint from tiles and walls!" +
                               "\nRight Click to open selection menu");
        }

        public override void SetDefaults()
        {
            //TODO: Add item.tileBoost to the toolRange
            item.height = 44;
            item.width = 44;
            item.useTime = 1;
            item.useAnimation = 1;
            item.useTurn = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.autoReuse = true;
            toolRange = new Point(8, 8);
        }

        public override Vector2? HoldoutOffset() => new Vector2(5, -8);

        public override void HoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            PaintWheel panel = ItemsUIState.paintWheel;
            if (Main.netMode != NetmodeID.Server && mp.ValidCursorPos)
            {
                canPaint = HelperMethods.ToolHasRange(toolRange) && (panel.colorIndex != -1 || panel.toolIndex == 2) &&
                           HelperMethods.IsUIAvailable(playerNotWieldingItem: false);
                player.showItemIcon = canPaint && !panel.IsMouseHovering;
                
                switch (panel.toolIndex)
                {
                    case 0: //Paint tiles
                        if (mp.PointedTile.type >= 0 && mp.PointedTile.active())
                            player.showItemIcon2 = ItemID.SpectrePaintbrush;
                        break;
                    case 1: //Paint walls
                        if (mp.PointedTile.type >= 0 && mp.PointedTile.wall > 0)
                            player.showItemIcon2 = ItemID.SpectrePaintRoller;
                        break;
                    case 2: //Scrap paint
                        if (mp.PointedTile.color() != 0 || mp.PointedTile.wallColor() != 0)
                            player.showItemIcon2 = ItemID.SpectrePaintScraper;
                        break;
                }
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return false;
            if ((!canPaint && ItemsUIState.paintWheel.toolIndex != 2) || !HelperMethods.ToolHasRange(toolRange)) return false;
            
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            PaintWheel panel = ItemsUIState.paintWheel;
            byte selectedColor = (byte) (panel.colorIndex + 1);
            bool infPaintBucket = panel.infPaintBucket;
            
            switch (panel.toolIndex)
            {
                case 0:
                    HelperMethods.PaintTileOrWall(selectedColor, 0, infPaintBucket);
                    break;
                case 1:
                    HelperMethods.PaintTileOrWall(selectedColor, 1, infPaintBucket);
                    break;
                case 2:
                    HelperMethods.ScrapPaint();
                    break;
            }
            
            return true;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) return;
            
            base.Update(ref gravity, ref maxFallSpeed);
            //Check if UI is Visible while item is dropped and close it if so.
            if (ItemsUIState.paintWheel.Visible)
                ItemsUIState.paintWheel.Hide();
        }

        private int mouseRightTimer = 0;

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            
            base.UpdateInventory(player);
            //Check if UI is Visible while item is not the held one and close it if so.
            if (player.HeldItem.IsNotTheSameAs(item) && ItemsUIState.paintWheel.Visible)
                ItemsUIState.paintWheel.Hide();

            if (Main.mouseRight && player.HeldItem.IsTheSameAs(item) && HelperMethods.IsUIAvailable() &&
                ++mouseRightTimer == 2)
            {
                ItemsUIState.paintWheel.Toggle();
            }

            if (Main.mouseRightRelease)
                mouseRightTimer = 0;
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