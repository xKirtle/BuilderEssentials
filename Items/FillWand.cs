using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    public class FillWand : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/FillWand";

        //Move selTileItemType to UI?
        public static int selectedTileItemType = -1;
        public static int fillSelectionSize = 3;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault
            (
                "Fills Holes" +
                "\nLeft Click to place" +
                "\nRigth Click to remove" +
                "\nMiddle Click to select working tiles" +
                "\n[c/FFCC00:Use hotkeys to increase/decrease selection size]"
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

        public override void HoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            BEPlayer mp = player.GetModPlayer<BEPlayer>();

            player.showItemIcon = true;

            //Middle Mouse
            if (Main.mouseMiddle && HelperMethods.IsUIAvailable(notShowingMouseIcon: false))
                selectedTileItemType = HelperMethods.PickItem(mp.PointedTile, false);

            if (selectedTileItemType != -1)
                player.showItemIcon2 = selectedTileItemType;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 0) //LMB
            {
                //Place Tiles
            }

            return true;
        }

        private int mouseRightTimer = 0;

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            
            if (Main.mouseRight && player.HeldItem.IsTheSameAs(item) &&
                HelperMethods.IsUIAvailable(notShowingMouseIcon: false) && ++mouseRightTimer == 2)
            {
                //Break Tiles
            }

            if (Main.mouseRightRelease)
                mouseRightTimer = 0;
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