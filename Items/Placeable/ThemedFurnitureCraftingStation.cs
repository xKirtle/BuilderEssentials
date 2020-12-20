using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    class ThemedFurnitureCraftingStation : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Placeable/ThemedFurnitureCraftingStation";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Used to craft Furniture Recipes");

        public override void SetDefaults()
        {
            item.width = 64;
            item.height = 64;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.value = Item.sellPrice(0, 25, 0, 0);;
            item.createTile = TileType<Tiles.ThemedFurnitureCraftingStation>();
            item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BoneWelder);
            recipe.AddIngredient(ItemID.GlassKiln);
            recipe.AddIngredient(ItemID.HoneyDispenser);
            recipe.AddIngredient(ItemID.IceMachine);
            recipe.AddIngredient(ItemID.LivingLoom);
            recipe.AddIngredient(ItemID.SkyMill);
            recipe.AddIngredient(ItemID.Solidifier);
            recipe.AddIngredient(ItemID.FleshCloningVaat);
            recipe.AddIngredient(ItemID.SteampunkBoiler);
            recipe.AddIngredient(ItemID.LihzahrdFurnace);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}