using BuilderEssentials.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items.Placeable;

public class HMCraftingItem : ModItem
{
    public override string Texture => "BuilderEssentials/Assets/Items/Placeable/" + GetType().Name;

    public override void SetStaticDefaults() {
        DisplayName.SetDefault("Hardmode Crafting Station");
        Tooltip.SetDefault("Used to craft Hardmode items");
    }

    public override void SetDefaults() {
        Item.width = 64;
        Item.height = 64;
        Item.maxStack = 99;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.consumable = true;
        Item.value = Item.sellPrice(gold: 25);
        Item.createTile = ModContent.TileType<HMCraftingStation>();
        Item.rare = ItemRarityID.Red;
    }

    public override void AddRecipes() {
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