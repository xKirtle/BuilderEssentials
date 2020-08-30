using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    class InfinityUpgrade : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/InfinityUpgrade";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Allows infinite placement");

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
        }

        public override void AddRecipes()
        {
            //Not really worried about balancing at this point
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.LunarBar, 40);
            modRecipe.AddIngredient(ItemID.FragmentSolar, 20);
            modRecipe.AddIngredient(ItemID.FragmentVortex, 20);
            modRecipe.AddIngredient(ItemID.FragmentNebula, 20);
            modRecipe.AddIngredient(ItemID.FragmentStardust, 20);
            modRecipe.AddTile(TileID.LunarCraftingStation);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
