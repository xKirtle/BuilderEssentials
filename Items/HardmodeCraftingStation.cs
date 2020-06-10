using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class HardmodeCraftingStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hardmode Crafting Stations");
        }

        public override void SetDefaults()
        {
            item.noMelee = true;
            item.width = 24;
            item.height = 17;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("BuilderEssentials:HardmodeAnvils", 1);
            recipe.AddRecipeGroup("BuilderEssentials:Forge", 1);
            recipe.AddRecipeGroup("BuilderEssentials:Bookcase", 1);
            recipe.AddIngredient(ItemID.CrystalBall, 1);
            recipe.AddIngredient(ItemID.Autohammer, 1);
            recipe.AddIngredient(ItemID.LunarCraftingStation, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}