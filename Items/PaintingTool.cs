﻿using System;
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
using Terraria.GameContent.Creative;

namespace BuilderEssentials.Items
{
    internal class PaintTool : ModItem
    {
        private Vector2 toolRange;
        private bool canPaint;
        private PaintWheel panel;

        public override string Texture => "BuilderEssentials/Textures/Items/PaintTool";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Able to paint and remove paint from tiles and walls!" +
                               "\nRight Click to open selection menu");
            
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 44;
            Item.width = 44;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            toolRange = new Vector2(9, 8);
        }

        public override Vector2? HoldoutOffset() => new Vector2(5, -8);

        private int mouseRightTimer = 0;
        public override void HoldItem(Player player)
        {
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            if (player.whoAmI != Main.myPlayer || Main.netMode == NetmodeID.Server || !mp.ValidCursorPos) return;

            PaintWheel panel = UIUIState.Instance.paintWheel;
            
            if (Main.mouseRight && player.HeldItem == Item &&
                (HelperMethods.IsUIAvailable(notShowingMouseIcon: false) || panel.IsMouseHovering) && ++mouseRightTimer == 2)
                panel.Toggle();

            if (Main.mouseRightRelease)
                mouseRightTimer = 0;
            
            //canPlaceItems = HelperMethods.ToolHasRange(toolRange) && !Main.LocalPlayer.mouseInterface;
            canPaint = HelperMethods.ToolHasRange(toolRange) && !Main.LocalPlayer.mouseInterface &&
                       (panel.colorIndex != -1 || panel.toolIndex == 2);
            player.cursorItemIconEnabled = canPaint;

            switch (panel.toolIndex)
            {
                case 0: //Paint tiles
                    if (mp.PointedTile.type >= 0 && mp.PointedTile.IsActive)
                        player.cursorItemIconID = ItemID.Paintbrush;
                    break;
                case 1: //Paint walls
                    if (mp.PointedTile.type >= 0 && mp.PointedTile.wall > 0)
                        player.cursorItemIconID = ItemID.PaintRoller;
                    break;
                case 2: //Scrap paint
                    if (mp.PointedTile.Color != 0 || mp.PointedTile.WallColor != 0)
                        player.cursorItemIconID = ItemID.PaintScraper;
                    break;
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return false;
            panel = UIUIState.Instance.paintWheel;
            if ((!canPaint && panel.toolIndex != 2) ||
                !HelperMethods.ToolHasRange(toolRange)) return false;

            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            byte selectedColor = (byte) (panel.colorIndex + 1);
            bool infPaintBucket = mp.infinitePaintBucketEquipped;

            switch (panel.toolIndex)
            {
                case 0:
                    HelperMethods.PaintTileOrWall(selectedColor, panel.toolIndex, mp.PointedCoord, infPaintBucket);
                    break;
                case 1:
                    HelperMethods.PaintTileOrWall(selectedColor, panel.toolIndex, mp.PointedCoord, infPaintBucket);
                    break;
                case 2:
                    HelperMethods.ScrapPaint(mp.PointedCoord);
                    break;
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpectrePaintbrush)
                .AddIngredient(ItemID.SpectrePaintRoller)
                .AddIngredient(ItemID.SpectrePaintScraper)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}