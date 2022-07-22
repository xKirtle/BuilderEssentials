using BuilderEssentials.Common.Configs;
using BuilderEssentials.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items.Placeable;

public class MultiCraftingItem : ModItem
{
    public override string Texture => "BuilderEssentials/Assets/Items/Placeable/" + GetType().Name;

    public override bool IsLoadingEnabled(Mod mod)
        => ModContent.GetInstance<MainConfig>().EnabledTiles.MultiCraftingStation;
    
    public override void SetStaticDefaults() {
        DisplayName.SetDefault("Multi Crafting Station");
        Tooltip.SetDefault("Used to craft all items in the game");
    }

    public override void SetDefaults() {
        Item.width = 64;
        Item.height = 50;
        Item.maxStack = 99;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.consumable = true;
        Item.value = Item.sellPrice(gold: 70);
        Item.createTile = ModContent.TileType<MultiCraftingStation>();
        Item.rare = ItemRarityID.Red;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<PreHMCraftingItem>()
            .AddIngredient<HMCraftingItem>()
            .AddIngredient<SpecCraftingItem>()
            .AddIngredient<TFCraftingItem>()
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}