﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BuilderEssentials.Items.Placeable
{
    class MultiCraftingStation : ModItem
    {
        public override string Texture => "BuilderEssentials/Textures/Items/Placeable/MultiCraftingStation";
        public override void SetStaticDefaults() => Tooltip.SetDefault("Used to craft all recipes in the game");

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
            item.value = Item.sellPrice(0, 70, 0, 0);;
            item.createTile = TileType<Tiles.MultiCraftingStation>();
            item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(GetModItem(ItemType<PreHardmodeCraftingStation>()));
            recipe.AddIngredient(GetModItem(ItemType<HardmodeCraftingStation>()));
            recipe.AddIngredient(GetModItem(ItemType<SpecializedCraftingStation>()));
            recipe.AddIngredient(GetModItem(ItemType<ThemedFurnitureCraftingStation>()));
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

}
