using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    internal class HardmodeCraftingStation : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Placeable/HardmodeCraftingStation";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Used to craft Hardmode items");

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 25, 0, 0);;
            Item.createTile = TileType<Tiles.HardmodeCraftingStation>();
            Item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("BuilderEssentials:Hardmode Anvil")
                .AddRecipeGroup("BuilderEssentials:Forge")
                .AddRecipeGroup("BuilderEssentials:Bookcase")
                .AddIngredient(ItemID.CrystalBall)
                .AddIngredient(ItemID.Autohammer)
                .AddIngredient(ItemID.LunarCraftingStation)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}