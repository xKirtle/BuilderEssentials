using System;
using System.Collections.Generic;
using BuilderEssentials.Common;
using BuilderEssentials.Content.UI;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class AutoHammer : BaseItemToggleableUI
{
    public override UIStateType UiStateType => UIStateType.AutoHammer;
    protected override bool CloneNewInstances => true;
    public override int ItemRange => 10;

    public override void SetStaticDefaults() {
        DisplayName.SetDefault("Auto Hammer");
        Tooltip.SetDefault("Better than a regular hammer!\n" +
                           "Right Click to open selection menu");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
        base.SetDefaults();
        
        Item.width = Item.height = 44;
        Item.useTime = Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.value = Item.sellPrice(silver: 80);
        Item.rare = ItemRarityID.Red;
        Item.damage = 26;
        Item.DamageType = DamageClass.Melee;
        Item.hammer = 80;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.Pwnhammer)
            .AddRecipeGroup("BuilderEssentials:Wood", 200)
            .Register();
    }

    //Note: Hacky workaround so that the hammer swings even when we're changing the slope ourselves.
    //Setting Item.hammer to 0 disables vanilla hammer's logic and thus, the UseItem swing doesn't mess with our slope change.
    //Can this be done properly?
    private bool canChangeSlope;
    public override bool CanUseItem(Player player) {
        if (player.whoAmI != Main.myPlayer || !ItemHasRange()) return true;

        var panel = ToggleableItemsUIState.GetUIPanel<AutoHammerPanel>();
        if (panel.selectedIndex != -1) {
            Item.hammer = 0;
            canChangeSlope = true;
        }

        return true;
    }
    
    public override bool? UseItem(Player player) {
        if (player.whoAmI == Main.myPlayer && IsPanelVisible())
            TogglePanel();
        
        if (canChangeSlope) {
            var panel = ToggleableItemsUIState.GetUIPanel<AutoHammerPanel>();
            //Can the selected index change between CanUseItem and UseItem at all?
            if (panel.selectedIndex != -1)
                ChangeSlope(panel.slopeType, panel.isHalfBlock);

            Item.hammer = 80;
            canChangeSlope = false;
        }
        
        return true;
    }

    public static void ChangeSlope(SlopeType slopeType, bool isHalfBlock) {
        Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
        if (Main.tileSolid[tile.TileType] && tile.TileType >= 0 && tile.HasTile) {
            //Prevent unnecessary changes to the tile and MP sync
            if ((tile.Slope == slopeType || tile.IsHalfBlock != isHalfBlock) && 
                tile.IsHalfBlock == isHalfBlock) return;
            
            tile.IsHalfBlock = isHalfBlock;
            tile.Slope = slopeType;
            
            WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, fail: true, effectOnly: true);
            WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
            SoundEngine.PlaySound(SoundID.Dig);

            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
        }
    }
}