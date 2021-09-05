using BuilderEssentials.UI.Elements.ShapesDrawer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Terraria.GameContent.Creative;
using Terraria.ObjectData;

namespace BuilderEssentials.Items
{
    internal class FillWand : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/FillWand";

        internal static int selectedTileItemType = -1;
        internal static int fillSelectionSize = 3;

        private int toolDelay = 10;
        private FillWandSelection panel;

        //TODO: add keybind to replace tiles in range (UIText displaying whether it's on or not)
        
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault
            (
                "Fills Holes" +
                "\nLeft Click to place" +
                "\nRigth Click to remove" +
                "\nMiddle Click to select working tile" +
                "\n[c/FFCC00:Use hotkeys to increase/decrease selection size]" +
                "\n[c/FF0000:Does not support multi tiles]"
            );
            
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.height = 46;
            Item.width = 46;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(0, 1, 40, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-2, -7);
        
        private int mouseLeftTimer = 0;
        private int mouseRightTimer = 0;
        public override void HoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            panel = GameUIState.Instance.fillWandSelection;
            panel.Show();
            panel.uiText.Show();


            //Left Mouse
            if (Main.mouseLeft && selectedTileItemType != -1 && !Main.LocalPlayer.mouseInterface && ++mouseLeftTimer == toolDelay)
            {
                int posX = Player.tileTargetX;
                int posY = Player.tileTargetY - (fillSelectionSize - 1);

                bool canPlaceItems = !Main.LocalPlayer.mouseInterface && selectedTileItemType != -1;
                if (canPlaceItems)
                    HelperMethods.PlaceTilesInArea(posX, posY, posX + fillSelectionSize,
                        posY + fillSelectionSize, selectedTileItemType, true, mp.replaceTiles);
            }
            
            if (Main.mouseLeftRelease || mouseLeftTimer == toolDelay)
                mouseLeftTimer = 0;
            
            //Middle Mouse
            if (Main.mouseMiddle && !player.mouseInterface)
                selectedTileItemType = HelperMethods.PickItem(mp.PointedTile, false, true);

            if (selectedTileItemType != -1)
            {
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = selectedTileItemType;
                GameUIState.Instance.fillWandSelection.SetItemToPlace(selectedTileItemType);
            }

            //Right Mouse
            if (Main.mouseRight && selectedTileItemType != -1 && !Main.LocalPlayer.mouseInterface && ++mouseRightTimer == toolDelay)
            {
                int posX = Player.tileTargetX;
                int posY = Player.tileTargetY - (fillSelectionSize - 1);
                HelperMethods.RemoveTilesInArea(posX, posY, posX + fillSelectionSize,
                    posY + fillSelectionSize, itemToDrop: selectedTileItemType);
            }

            if (Main.mouseRightRelease || mouseRightTimer == toolDelay)
                mouseRightTimer = 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MoltenHamaxe)
                .AddIngredient(ItemID.DirtRod)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}