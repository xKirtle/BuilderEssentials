using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class InfinitePaintBucket : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Necessary to use the Super Painting Tool");
        }
        public override void SetDefaults()
        {
            item.height = 20;
            item.width = 18;
            item.useTime = 1;
            item.useAnimation = 10;
            //item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.noMelee = true;
            item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DeepRedPaint, 999);
            recipe.AddIngredient(ItemID.DeepGreenPaint, 999);
            recipe.AddIngredient(ItemID.DeepBluePaint, 999);
            recipe.AddIngredient(ItemID.NegativePaint, 999);
            recipe.AddTile(TileID.DyeVat);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
