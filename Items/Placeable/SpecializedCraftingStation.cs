using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    internal class SpecializedCraftingStation : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Placeable/SpecializedCraftingStation";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Used to craft Specialized items");

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
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.createTile = TileType<Tiles.SpecializedCraftingStation>();
            Item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Keg)
            .AddIngredient(ItemID.BlendOMatic)
            .AddIngredient(ItemID.MeatGrinder)
            .AddRecipeGroup("BuilderEssentials:Campfire")
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}