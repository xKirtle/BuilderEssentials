using BuilderEssentials.Common;
using BuilderEssentials.Common.Configs;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class InfinitePaintBucket : BuilderEssentialsItem
{
    public override bool IsLoadingEnabled(Mod mod) => ModContent.GetInstance<ServerConfig>().EnabledItems.InfinitePaintBucket;

    public override void SetStaticDefaults() {
        // TOOLTIP
        // Tooltip.SetDefault("Allows infinite painting while in the inventory!");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
        Item.height = 32;
        Item.width = 36;
        Item.useTime = 1;
        Item.useAnimation = 10;
        Item.value = Item.sellPrice(gold: 3);
        Item.rare = ItemRarityID.Red;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = false;
        Item.noMelee = true;
        Item.noUseGraphic = true;
    }

    public override bool ConsumeItem(Player player) => false;

    public override void UpdateInventory(Player player) {
        if (player.whoAmI != Main.myPlayer)
            return;

        player.GetModPlayer<BEPlayer>().InfinitePaint = true;
    }

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.DeepRedPaint, 999)
        .AddIngredient(ItemID.DeepGreenPaint, 999)
        .AddIngredient(ItemID.DeepBluePaint, 999)
        .AddIngredient(ItemID.NegativePaint, 999)
        .AddTile(TileID.DyeVat)
        .Register();
}