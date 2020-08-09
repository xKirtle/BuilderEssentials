using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    class InfiniteWrench : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/InfiniteWrench";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infinite Wrench");
            Tooltip.SetDefault("Allows infinite range and fast placement");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 24;
            item.height = 24;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(ModContent.BuffType<Buffs.InfinitePlacementBuff>(), 10);
            player.blockRange += 55;
            player.wallSpeed += 10;
            player.tileSpeed += 50;
            Player.tileRangeX = 65;
            Player.tileRangeY = 55;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<InfinityUpgrade>());
            modRecipe.AddIngredient(ModContent.ItemType<PlacementWrench>());
            modRecipe.AddTile(TileID.TinkerersWorkbench);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
