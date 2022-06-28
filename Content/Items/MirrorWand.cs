using BuilderEssentials.Common;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class MirrorWand : BuilderEssentialsItem
{
    public override void SetStaticDefaults() {
        Tooltip.SetDefault("Mirrors everything!" +
                           "\nRight Click to make a selection area" +
                           "\nLeft Click to make a mirror axis" +
                           "\n[c/FF0000:Multiplayer usage can be bug prone!]");
            
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
        Item.height = Item.width = 40;
        Item.useTime = Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.value = Item.sellPrice(gold: 2);
        Item.rare = ItemRarityID.Red;
        Item.autoReuse = true;
        Item.noMelee = true;
    }
    
    public override Vector2? HoldoutOffset() => new Vector2(5, -7);
    
    public override void AddRecipes() {
        CreateRecipe()
            .AddRecipeGroup("BuilderEssentials:Magic Mirror")
            .AddIngredient(ItemID.SoulofLight, 25)
            .AddIngredient(ItemID.SoulofNight, 25)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}