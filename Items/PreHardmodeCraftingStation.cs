using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class PreHardmodeCraftingStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("PreHardmode Crafting Stations");
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
            recipe.AddRecipeGroup("BuilderEssentials:Workbenches", 1);
            recipe.AddRecipeGroup("BuilderEssentials:Furnaces", 1);
            recipe.AddRecipeGroup("BuilderEssentials:Chairs", 1);
            recipe.AddRecipeGroup("BuilderEssentials:Tables", 1);
            recipe.AddRecipeGroup("BuilderEssentials:Anvils", 1);
            recipe.AddRecipeGroup("BuilderEssentials:AlchemyStations", 1);
            recipe.AddRecipeGroup("BuilderEssentials:Sinks", 1);
            recipe.AddIngredient(ItemID.Sawmill, 1);
            recipe.AddIngredient(ItemID.Loom, 1);
            recipe.AddRecipeGroup("BuilderEssentials:CookingPots", 1);
            recipe.AddIngredient(ItemID.TinkerersWorkshop, 1);
            recipe.AddIngredient(ItemID.ImbuingStation, 1);
            recipe.AddIngredient(ItemID.DyeVat, 1);
            recipe.AddIngredient(ItemID.HeavyWorkBench, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}