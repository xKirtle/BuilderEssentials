using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    class SpecializedCraftingStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to craft Specialized Recipes");
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
            item.createTile = TileType<Tiles.SpecializedCraftingStation>();
            item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Keg);
            recipe.AddIngredient(ItemID.BlendOMatic);
            recipe.AddIngredient(ItemID.MeatGrinder);
            recipe.AddRecipeGroup("BuilderEssentials:Campfires");

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}