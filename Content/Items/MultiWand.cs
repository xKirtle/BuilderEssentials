﻿using System;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Configs;
using BuilderEssentials.Common.Enums;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class MultiWand : BaseItemToggleableUI
{
    public override ToggleableUiType ToggleableUiType => ToggleableUiType.MultiWand;
    protected override bool CloneNewInstances => true;
    public override int ItemRange => 10;

    public override bool IsLoadingEnabled(Mod mod) => ModContent.GetInstance<ServerConfig>().EnabledItems.MultiWand;

    public override void SetStaticDefaults() {
        // TOOLTIP
        // DisplayName.SetDefault("Multi Wand");
        // Tooltip.SetDefault("Contains all building wands into one!\n" +
        //     "Right Click to open selection menu");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
        base.SetDefaults();

        Item.width = Item.height = 44;
        Item.useTime = Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.value = Item.sellPrice(silver: 2);
        Item.rare = ItemRarityID.Red;
        Item.noMelee = true;
        Item.autoReuse = true;
        Item.tileWand = WandMaterials[0];

        //I'm not sure why, but this is required for the tileWand to be the correct value when I access it..?
        //Updating it in UpdateInventory doesn't seem to do it. Maybe call order is at stake?
        if (Main.netMode != NetmodeID.Server && !Main.dedServ) {
            MultiWandPanel panel = ToggleableItemsUIState.GetUIPanel<MultiWandPanel>();
            Item.tileWand = WandMaterials[panel?.selectedIndex ?? 0];
            Item.createTile = WandPlacedTiles[panel?.selectedIndex ?? 0];
        }
    }

    public override Vector2? HoldoutOffset() => new Vector2(2, -9);

    public override void AddRecipes() => CreateRecipe()
        .AddIngredient(ItemID.LivingWoodWand)
        .AddIngredient(ItemID.BoneWand)
        .AddIngredient(ItemID.LeafWand)
        .AddIngredient(ItemID.HiveWall)
        .AddIngredient(ItemID.LivingMahoganyWand)
        .AddIngredient(ItemID.LivingMahoganyLeafWand)
        .AddTile(TileID.TinkerersWorkbench)
        .Register();

    public static readonly int[] WandTypes = {
        ItemID.LivingWoodWand, ItemID.BoneWand, ItemID.LeafWand, ItemID.HiveWand, ItemID.LivingMahoganyWand, ItemID.LivingMahoganyLeafWand
    };

    public static readonly int[] WandMaterials = {
        ItemID.Wood, ItemID.Bone, ItemID.Wood, ItemID.Hive, ItemID.RichMahogany, ItemID.RichMahogany
    };

    public static readonly int[] WandPlacedTiles = {
        TileID.LivingWood, TileID.BoneBlock, TileID.LeafBlock, TileID.Hive, TileID.LivingMahogany, TileID.LivingMahoganyLeaves
    };

    public override bool? UseItem(Player player) {
        if (player.whoAmI == Main.myPlayer && IsPanelVisible())
            TogglePanel();

        return base.UseItem(player);
    }

    public override void UpdateInventory(Player player) {
        if (player.whoAmI != Main.myPlayer)
            return;

        MultiWandPanel panel = ToggleableItemsUIState.GetUIPanel<MultiWandPanel>();
        Item.tileWand = WandMaterials[panel.selectedIndex];
        Item.createTile = WandPlacedTiles[panel.selectedIndex];
    }
}