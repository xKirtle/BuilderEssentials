using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    class MultiCraftingStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to craft all recipes in the game");
        }

        public override void SetDefaults()
        {
            item.width = 102;
            item.height = 40;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.value = 150;
            item.createTile = TileType<Tiles.MultiCraftingStation>();
            item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("PreHardmodeCraftingStation"), 1);
            recipe.AddIngredient(mod.GetItem("HardmodeCraftingStation"), 1);
            recipe.AddIngredient(mod.GetItem("SpecializedCraftingStation"), 1);
            recipe.AddIngredient(mod.GetItem("ThemedFurnitureCraftingStation"), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

}
