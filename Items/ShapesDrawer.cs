﻿using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    internal class ShapesDrawer : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/ShapesDrawer";

        private int selectedItemType = -1;
        private bool canPlaceItems;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to draw shapes" +
                               "\nOpen its menu by clicking the arrow on the left of your screen when equipped" +
                               "\nRight Click to make selection" +
                               "\nMiddle Click to select working tile" +
                               "\nLeft Click to place blocks in the selection" +
                               "\n[c/FFCC00:Press LShift to make circles/squares]" +
                               "\n[c/FFCC00:Enables a selection menu on the left of the screen]");
            
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 40;
            Item.width = 40;
            Item.useTime = 1;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(0, 0, 4, 0);
            Item.rare = ItemRarityID.Red;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(2, -9);
        
        private int leftMouseTimer;
        public override void HoldItem(Player player)
        { 
            if (player.whoAmI != Main.myPlayer) return;

            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            if (Main.LocalPlayer.HeldItem == this.Item && !UIUIState.Instance.menuPanel.Visible)
                UIUIState.Instance.arrowPanel.Show();

            if (Main.mouseMiddle && !player.mouseInterface)
                selectedItemType = HelperMethods.PickItem(mp.PointedTile, false, true);

            if (selectedItemType != -1)
            {
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = selectedItemType;
                GameUIState.Instance.rectangleShape.SetItemToPlace(selectedItemType);
                GameUIState.Instance.ellipseShape.SetItemToPlace(selectedItemType);
            }

            canPlaceItems = Main.mouseLeft && !Main.LocalPlayer.mouseInterface && ++leftMouseTimer == 2;
            GameUIState.Instance.rectangleShape.CanPlaceItems = canPlaceItems && selectedItemType != -1;
            GameUIState.Instance.ellipseShape.CanPlaceItems = canPlaceItems && selectedItemType != -1;
            
            if (Main.mouseLeftRelease)
                leftMouseTimer = 0;
        }

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperPickaxe)
                .AddIngredient(ItemID.CopperAxe)
                .AddIngredient(ItemID.CopperHammer)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.TinPickaxe)
                .AddIngredient(ItemID.TinAxe)
                .AddIngredient(ItemID.TinHammer)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}