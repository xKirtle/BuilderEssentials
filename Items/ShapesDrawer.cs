using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    public class ShapesDrawer : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/ShapesDrawer";

        public static int selectedItemType = -1;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to draw shapes" +
                               "\nRight Click to make selection" +
                               "\nMiddle Click to select working tile" +
                               "\nLeft Click to place blocks in the selection" +
                               "\n[c/FFCC00:Press LShift to make circles/squares]" +
                               "\n[c/FFCC00:Enables a selection menu on the left of the screen]");
        }

        public override void SetDefaults()
        {
            item.height = 40;
            item.width = 40;
            item.useTime = 1;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 0, 4, 0);
            item.rare = ItemRarityID.Red;
            item.useTurn = true;
            item.autoReuse = true;
            item.noMelee = true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(2, -9);

        public static bool LMBDown;
        public override void HoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;
            
            //Had to create this static variable since I couldn't use player.mouseInterface in the draw method
            LMBDown = Main.mouseLeft && !player.mouseInterface;
            BEPlayer mp = player.GetModPlayer<BEPlayer>();

            //Middle Mouse
            if (Main.mouseMiddle && !player.mouseInterface)
                selectedItemType = HelperMethods.PickItem(mp.PointedTile, false);

            if (selectedItemType != -1)
            {
                player.showItemIcon = true;
                player.showItemIcon2 = selectedItemType;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CopperPickaxe);
            recipe.AddIngredient(ItemID.CopperAxe);
            recipe.AddIngredient(ItemID.CopperHammer);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TinPickaxe);
            recipe.AddIngredient(ItemID.TinAxe);
            recipe.AddIngredient(ItemID.TinHammer);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}