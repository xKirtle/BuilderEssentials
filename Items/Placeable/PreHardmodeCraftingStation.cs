using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    internal class PreHardmodeCraftingStation : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Placeable/PreHardmodeCraftingStation";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Used to craft Pre Hardmode items");

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 50;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 7, 0, 0);
            Item.createTile = TileType<Tiles.PreHardmodeCraftingStation>();
            Item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("BuilderEssentials:Workbench")
            .AddRecipeGroup("BuilderEssentials:Furnace")
            .AddRecipeGroup("BuilderEssentials:Chair")
            .AddRecipeGroup("BuilderEssentials:Table")
            .AddRecipeGroup("BuilderEssentials:Anvil")
            .AddRecipeGroup("BuilderEssentials:Bottle/Alchemy Item")
            .AddRecipeGroup("BuilderEssentials:Sink")
            .AddIngredient(ItemID.Sawmill)
            .AddIngredient(ItemID.Loom)
            .AddRecipeGroup("BuilderEssentials:Cooking Pot")
            .AddIngredient(ItemID.TinkerersWorkshop)
            .AddIngredient(ItemID.ImbuingStation)
            .AddIngredient(ItemID.DyeVat)
            .AddIngredient(ItemID.HeavyWorkBench)
            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}