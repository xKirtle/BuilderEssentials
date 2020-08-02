using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    class PlacementWrench : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/PlacementWrench";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows infinite range");
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
            BuilderPlayer modPlayer = player.GetModPlayer<BuilderPlayer>();

            player.blockRange += 55;
            player.wallSpeed += 10;
            player.tileSpeed += 50;
            Player.tileRangeX = 65;
            Player.tileRangeY = 55;
            modPlayer.infiniteRange = true;
        }

        public override void AddRecipes()
        {
            //Not really worried about balancing at this point
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.ArchitectGizmoPack);
            modRecipe.AddIngredient(ItemID.LaserRuler);
            modRecipe.AddIngredient(ItemID.Toolbox);
            modRecipe.AddIngredient(ItemID.Toolbelt);
            modRecipe.AddTile(TileID.AdamantiteForge);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
