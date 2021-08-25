using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    internal class ShapesDrawer : ModItem
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
        
        public override void HoldItem(Player player)
        { 
            if (player.whoAmI != Main.myPlayer) return;

            BEPlayer mp = player.GetModPlayer<BEPlayer>();

            //Middle Mouse
            if (Main.mouseMiddle && !player.mouseInterface)
                selectedItemType = HelperMethods.PickItem(mp.PointedTile, false, true);

            if (selectedItemType != -1)
            {
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = selectedItemType;
            }
        }

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;

            var uiState = UIUIState.Instance;
            if (Main.LocalPlayer.HeldItem != this.Item)
            {
                uiState.arrowPanel.Hide();
                uiState.menuPanel.Hide();
            }
            else if (Main.LocalPlayer.HeldItem == this.Item && !uiState.menuPanel.Visible)
                uiState.arrowPanel.Show();
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