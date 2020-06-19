﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    class PreHardmodeCraftingStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to craft Pre Hardmode Recipes");
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
            item.createTile = TileType<Tiles.PreHardmodeCraftingStation>();
            item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("BuilderEssentials:Workbenches");
            recipe.AddRecipeGroup("BuilderEssentials:Furnaces");
            recipe.AddRecipeGroup("BuilderEssentials:Chairs");
            recipe.AddRecipeGroup("BuilderEssentials:Tables");
            recipe.AddRecipeGroup("BuilderEssentials:Anvils");
            recipe.AddRecipeGroup("BuilderEssentials:AlchemyStations");
            recipe.AddRecipeGroup("BuilderEssentials:Sinks");
            recipe.AddIngredient(ItemID.Sawmill);
            recipe.AddIngredient(ItemID.Loom);
            recipe.AddRecipeGroup("BuilderEssentials:CookingPots");
            recipe.AddIngredient(ItemID.TinkerersWorkshop);
            recipe.AddIngredient(ItemID.ImbuingStation);
            recipe.AddIngredient(ItemID.DyeVat);
            recipe.AddIngredient(ItemID.HeavyWorkBench);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}