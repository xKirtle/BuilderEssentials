using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    class PreHardmodeCraftingStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to craft Pre Hardmode Recipes");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.value = 150;
            item.createTile = TileType<Tiles.PreHardmodeCraftingStation>();
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