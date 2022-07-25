using BuilderEssentials.Common.Configs;
using BuilderEssentials.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items.Placeable;

public class TFCraftingItem : ModItem
{
    public override string Texture => "BuilderEssentials/Assets/Items/Placeable/" + GetType().Name;

    public override bool IsLoadingEnabled(Mod mod) {
        return ModContent.GetInstance<MainConfig>().EnabledTiles.TFCraftingStation;
    }

    public override void SetStaticDefaults() {
        DisplayName.SetDefault("Themed Furniture Crafting Station");
        Tooltip.SetDefault("Used to craft Themed Furniture items");
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
        Item.createTile = ModContent.TileType<TFCraftingStation>();
        Item.rare = ItemRarityID.Red;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.BoneWelder)
            .AddIngredient(ItemID.GlassKiln)
            .AddIngredient(ItemID.HoneyDispenser)
            .AddIngredient(ItemID.IceMachine)
            .AddIngredient(ItemID.LivingLoom)
            .AddIngredient(ItemID.SkyMill)
            .AddIngredient(ItemID.Solidifier)
            .AddIngredient(ItemID.LesionStation)
            .AddIngredient(ItemID.FleshCloningVaat)
            .AddIngredient(ItemID.SteampunkBoiler)
            .AddIngredient(ItemID.LihzahrdFurnace)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}