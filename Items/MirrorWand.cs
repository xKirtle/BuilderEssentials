using BuilderEssentials.UI;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    internal class MirrorWand : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/MirrorWand";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mirrors everything!" +
                               "\nRight Click to make a selection area" +
                               "\nLeft Click to make a mirror axis" +
                               "\n[c/FF0000:This is a work in progress and]" +
                               "\n[c/FF0000:does not support multi tiles yet]");
        }
        public override void SetDefaults()
        {
            Item.height = 40;
            Item.width = 40;
            Item.useTime = 1;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.noMelee = false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(5, -7);

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddRecipeGroup("BuilderEssentials:Magic Mirror")
            .AddIngredient(ItemID.SoulofLight, 25)
            .AddIngredient(ItemID.SoulofNight, 25)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}