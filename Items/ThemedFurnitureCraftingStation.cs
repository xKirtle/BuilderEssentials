using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class ThemedFurnitureCraftingStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Themed Furniture Crafting Stations");
        }

        public override void SetDefaults()
        {
            item.noMelee = true;
            item.width = 24;
            item.height = 17;
            item.value = Item.sellPrice(0, 10, 0, 0);
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