using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BuilderEssentials.UI.UIStates;
using BuilderEssentials.Utilities;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace BuilderEssentials.Items
{
    public class InfinitePaintBucket : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/InfinitePaintBucket";
        public override void SetStaticDefaults() => 
            Tooltip.SetDefault("Allows infinite painting!" +
               "\n[c/FFCC00:Must be the first paint bucket in your inventory!]" +
               "\n[c/FFCC00:Copies the color of the next paint bucket in your inventory]");

        public override void SetDefaults()
        {
            Item.height = 20;
            Item.width = 18;
            Item.useTime = 1;
            Item.useAnimation = 10;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override bool ConsumeItem(Player player) => false;

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return;

            base.UpdateInventory(player);
            player.GetModPlayer<BEPlayer>().infinitePaintBucketEquipped = true;
            Item.paint = HelperMethods.FirstPaintBucketInInventory();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.DeepRedPaint, 999)
            .AddIngredient(ItemID.DeepGreenPaint, 999)
            .AddIngredient(ItemID.DeepBluePaint, 999)
            .AddIngredient(ItemID.NegativePaint, 999)
            .AddTile(TileID.DyeVat)
            .Register();
        }
    }
}