using BuilderEssentials.UI;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class MirrorWand : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/MirrorWand";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mirrors everything!" +
                               "\nRight Click to make a selection area" +
                               "\nLeft Click to make a mirror axis" +
                               "\n[c/FF0000:This is a work in progress and]" +
                               "\n[c/FF0000:might not work for all multi tiles]");
        }
        public override void SetDefaults()
        {
            item.height = 40;
            item.width = 40;
            item.useTime = 1;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.noMelee = false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(5, -7);

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("BuilderEssentials:Magic Mirror");
            recipe.AddIngredient(ItemID.SoulofLight, 25);
            recipe.AddIngredient(ItemID.SoulofNight, 25);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}