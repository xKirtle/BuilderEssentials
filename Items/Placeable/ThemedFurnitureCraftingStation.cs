using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    class ThemedFurnitureCraftingStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to craft Furniture Recipes");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.value = 150;
            item.createTile = TileType<Tiles.ThemedFurnitureCraftingStation>();
            item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BoneWelder, 1);
            recipe.AddIngredient(ItemID.GlassKiln, 1);
            recipe.AddIngredient(ItemID.HoneyDispenser, 1);
            recipe.AddIngredient(ItemID.IceMachine, 1);
            recipe.AddIngredient(ItemID.LivingLoom, 1);
            recipe.AddIngredient(ItemID.SkyMill, 1);
            recipe.AddIngredient(ItemID.Solidifier, 1);
            recipe.AddIngredient(ItemID.FleshCloningVaat, 1);
            recipe.AddIngredient(ItemID.SteampunkBoiler, 1);
            recipe.AddIngredient(ItemID.LihzahrdFurnace, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}