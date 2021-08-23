using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    internal class MultiCraftingStation : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Placeable/MultiCraftingStation";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Used to craft all items in the game");

        public override void SetDefaults()
        {
            Item.width = 102;
            Item.height = 40;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 70, 0, 0);;
            Item.createTile = TileType<Tiles.MultiCraftingStation>();
            Item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(GetModItem(ItemType<PreHardmodeCraftingStation>()))
            .AddIngredient(GetModItem(ItemType<HardmodeCraftingStation>()))
            .AddIngredient(GetModItem(ItemType<SpecializedCraftingStation>()))
            .AddIngredient(GetModItem(ItemType<ThemedFurnitureCraftingStation>()))
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }

}
