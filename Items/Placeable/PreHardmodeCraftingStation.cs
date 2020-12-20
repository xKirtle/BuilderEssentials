using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    class PreHardmodeCraftingStation : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Placeable/PreHardmodeCraftingStation";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Used to craft Pre Hardmode Recipes");

        public override void SetDefaults()
        {
            item.width = 64;
            item.height = 50;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.value = Item.sellPrice(0, 7, 0, 0);
            item.createTile = TileType<Tiles.PreHardmodeCraftingStation>();
            item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("BuilderEssentials:Workbench");
            recipe.AddRecipeGroup("BuilderEssentials:Furnace");
            recipe.AddRecipeGroup("BuilderEssentials:Chair");
            recipe.AddRecipeGroup("BuilderEssentials:Table");
            recipe.AddRecipeGroup("BuilderEssentials:Anvil");
            recipe.AddRecipeGroup("BuilderEssentials:Bottle/Alchemy Item");
            recipe.AddRecipeGroup("BuilderEssentials:Sink");
            recipe.AddIngredient(ItemID.Sawmill);
            recipe.AddIngredient(ItemID.Loom);
            recipe.AddRecipeGroup("BuilderEssentials:Cooking Pot");
            recipe.AddIngredient(ItemID.TinkerersWorkshop);
            recipe.AddIngredient(ItemID.ImbuingStation);
            recipe.AddIngredient(ItemID.DyeVat);
            recipe.AddIngredient(ItemID.HeavyWorkBench);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}