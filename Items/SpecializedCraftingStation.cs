using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class SpecializedCraftingStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Specialized Crafting Stations");
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
            recipe.AddIngredient(ItemID.Keg, 1);
            recipe.AddIngredient(ItemID.BlendOMatic, 1);
            recipe.AddIngredient(ItemID.MeatGrinder, 1);
            recipe.AddRecipeGroup("BuilderEssentials:Campfires", 1);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}