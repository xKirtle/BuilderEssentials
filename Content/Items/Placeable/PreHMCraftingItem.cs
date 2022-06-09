﻿using BuilderEssentials.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items.Placeable;

public class PreHMCraftingItem : ModItem
{
    public override string Texture => "BuilderEssentials/Assets/Items/Placeable/" + GetType().Name;

    public override void SetStaticDefaults() {
        DisplayName.SetDefault("Pre Hardmode Crafting Station");
        Tooltip.SetDefault("Used to craft Pre Hardmode items");
    }

    public override void SetDefaults() {
        Item.width = 64;
        Item.height = 50;
        Item.maxStack = 99;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.consumable = true;
        Item.value = Item.sellPrice(gold: 7);
        Item.createTile = ModContent.TileType<PreHMCraftingStation>();
        Item.rare = ItemRarityID.Red;
    }

    public override void AddRecipes() {
        
    }
}