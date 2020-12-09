using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BuilderEssentials.UI.UIPanels;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Terraria.ObjectData;

namespace BuilderEssentials.Items
{
    public class FillWand : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/FillWand";

        public static int selectedTileItemType = -1;
        public static int fillSelectionSize = 3;
        private bool multiTileSelected = false;
        private bool oneTimeMessage = false;

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
        }

        public override void SetDefaults()
        {
            item.height = 46;
            item.width = 46;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.autoReuse = true;
            item.noMelee = true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-2, -7);

        private int mouseRightTimer = 0;

        public override void HoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;

            if (!UIStateLogic1.fillWandSelection.Visible)
                UIStateLogic1.fillWandSelection.Show();


            BEPlayer mp = player.GetModPlayer<BEPlayer>();
            player.showItemIcon = true;

            //Middle Mouse
            if (Main.mouseMiddle && HelperMethods.IsUIAvailable(notShowingMouseIcon: false))
            {
                TileObjectData data = TileObjectData.GetTileData(mp.PointedTile);
                multiTileSelected = data != null;
                
                if (!multiTileSelected)
                    selectedTileItemType = HelperMethods.PickItem(mp.PointedTile, false);
                else if (!oneTimeMessage)
                {
                    Main.NewText("Fill Wand does not support multi tiles!", Color.Red);
                    oneTimeMessage = true;
                }
            }

            if (selectedTileItemType != -1)
                player.showItemIcon2 = selectedTileItemType;

            //Right Mouse
            if (Main.mouseRight && player.HeldItem == item && selectedTileItemType != -1 &&
                HelperMethods.IsUIAvailable(notShowingMouseIcon: false) && ++mouseRightTimer == 9)
            {
                int posX = Player.tileTargetX;
                int posY = Player.tileTargetY - (fillSelectionSize - 1);
                HelperMethods.RemoveTilesInArea(posX, posY, posX + fillSelectionSize,
                    posY + fillSelectionSize, itemToDrop: selectedTileItemType);
            }

            if (Main.mouseRightRelease || mouseRightTimer == 9)
                mouseRightTimer = 0;
        }
        
        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return false;

            //UI is appended to the bottomLeft coord
            int posX = Player.tileTargetX;
            int posY = Player.tileTargetY - (fillSelectionSize - 1);
            
            //PlaceTilesInArea handles stackReducing itself
            if (selectedTileItemType != -1)
                HelperMethods.PlaceTilesInArea(posX, posY, posX + fillSelectionSize,
                    posY + fillSelectionSize, selectedTileItemType, true);
            
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MoltenHamaxe);
            recipe.AddIngredient(ItemID.DirtRod);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}