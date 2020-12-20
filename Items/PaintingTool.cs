using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using BuilderEssentials.Utilities;
using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIStates;

namespace BuilderEssentials.Items
{
    public class PaintingTool : ModItem
    {
        private Point toolRange;
        private bool canPaint;

        public override string Texture => "Terraria/Item_" + ItemID.Paintbrush;
        
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Able to paint and remove paint from tiles and walls!" +
                               "\nRight Click to open selection menu");
        }
        
        public override void SetDefaults()
        {
            item.height = 26;
            item.width = 26;
            item.useTime = 1;
            item.useAnimation = 1;
            item.useTurn = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 0, 80, 0);
            item.rare = ItemRarityID.Red;
            item.autoReuse = true;
            toolRange = new Point(5, 5);
        }
        
        public override Vector2? HoldoutOffset() => new Vector2(5, -8);
        
        public override void HoldItem(Player player)
        {
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            if (player.whoAmI != Main.myPlayer || Main.netMode == NetmodeID.Server || !mp.ValidCursorPos) return;

            PaintWheel panel = UIStateLogic1.paintWheel;
            canPaint = HelperMethods.ToolHasRange(toolRange) && (panel.colorIndex != -1 || panel.toolIndex == 2) &&
                       HelperMethods.IsUIAvailable(playerNotWieldingItem: false);
            player.showItemIcon = canPaint && !panel.IsMouseHovering;

            switch (panel.toolIndex)
            {
                case 0: //Paint tiles
                    if (mp.PointedTile.type >= 0 && mp.PointedTile.active())
                        player.showItemIcon2 = ItemID.Paintbrush;
                    break;
                case 1: //Paint walls
                    if (mp.PointedTile.type >= 0 && mp.PointedTile.wall > 0)
                        player.showItemIcon2 = ItemID.PaintRoller;
                    break;
                case 2: //Scrap paint
                    if (mp.PointedTile.color() != 0 || mp.PointedTile.wallColor() != 0)
                        player.showItemIcon2 = ItemID.PaintScraper;
                    break;
            }
        }
        
        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return false;
            if ((!canPaint && UIStateLogic1.paintWheel.toolIndex != 2) ||
                !HelperMethods.ToolHasRange(toolRange)) return false;

            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            PaintWheel panel = UIStateLogic1.paintWheel;
            byte selectedColor = (byte) (panel.colorIndex + 1);
            bool infPaintBucket = mp.infinitePaintBucketEquipped;

            switch (panel.toolIndex)
            {
                case 0:
                    HelperMethods.PaintTileOrWall(selectedColor, panel.toolIndex, infPaintBucket);
                    break;
                case 1:
                    HelperMethods.PaintTileOrWall(selectedColor, panel.toolIndex, infPaintBucket);
                    break;
                case 2:
                    HelperMethods.ScrapPaint();
                    break;
            }

            return true;
        }
        
        private int mouseRightTimer = 0;

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            
            if (Main.mouseRight && player.HeldItem == item &&
                HelperMethods.IsUIAvailable() && ++mouseRightTimer == 2)
                UIStateLogic1.paintWheel.Toggle();

            if (Main.mouseRightRelease)
                mouseRightTimer = 0;
        }
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Paintbrush);
            recipe.AddIngredient(ItemID.PaintRoller);
            recipe.AddIngredient(ItemID.PaintScraper);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}