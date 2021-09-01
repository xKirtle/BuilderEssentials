using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items.Accessories
{
    public class ImprovedRuler : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Accessories/ImprovedRuler";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Only works when equipped and player's empty handed." +
                               "\nHold Left Click to draw a line." +
                               "\nHold Right Click to curve the line.");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.vanity = false;
            Item.width = 42;
            Item.height = 42;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Red;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.whoAmI != Main.myPlayer) return;
            player.GetModPlayer<BEPlayer>().improvedRulerEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Ruler)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        }
    }
}