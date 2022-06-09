using System;
using BuilderEssentials.Common;
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
    public override UIStateType UiStateType => UIStateType.MultiWand;
    protected override bool CloneNewInstances => true;
    public override int ItemRange => 10;
    
    public override void SetStaticDefaults() {
        DisplayName.SetDefault("Multi Wand");
        Tooltip.SetDefault("Contains all building wands into one!\n" +
                           "Right Click to open selection menu");
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
    }

    public override Vector2? HoldoutOffset() => new Vector2(2, -9);

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.LivingWoodWand)
            .AddIngredient(ItemID.BoneWand)
            .AddIngredient(ItemID.LeafWand)
            .AddIngredient(ItemID.HiveWall)
            .AddIngredient(ItemID.LivingMahoganyWand)
            .AddIngredient(ItemID.LivingMahoganyLeafWand)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
    
    private readonly int[] wandTypes = {
        ItemID.LivingWoodWand, ItemID.BoneWand, ItemID.LeafWand, 
        ItemID.HiveWand, ItemID.LivingMahoganyWand, ItemID.LivingMahoganyLeafWand
    };
        
    private readonly int[] wandMaterials = {
        ItemID.Wood, ItemID.Bone, ItemID.Wood,
        ItemID.Hive, ItemID.RichMahogany, ItemID.RichMahogany
    };

    private readonly int[] wandPlacedTiles = {
        TileID.LivingWood, TileID.BoneBlock, TileID.LeafBlock,
        TileID.Hive, TileID.LivingMahogany, TileID.LivingMahoganyLeaves
    };

    public override void UpdateInventory(Player player) {
        base.UpdateInventory(player);
        
        var panel = MultiWandState.Instance.menuPanel;
        Item.tileWand = wandMaterials[panel.selectedIndex];
        Item.createTile = wandPlacedTiles[panel.selectedIndex];
    }
}