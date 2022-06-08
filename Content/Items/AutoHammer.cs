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

    public override bool CanUseItem(Player player) {
        if (player.whoAmI != Main.myPlayer) return false;
        
        var panel = AutoHammerState.Instance.menuPanel;
        if (ItemHasRange() && panel.selectedIndex != -1) {
            ChangeSlope(panel.slopeType, panel.isHalfBlock);
            return false;
        }

        return true;
    }

    internal static void ChangeSlope(SlopeType slopeType, bool isHalfBlock) {
        Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
        if (Main.tileSolid[tile.TileType] && tile.TileType >= 0 && tile.HasTile) {
            //Prevent unnecessary changes to the tile and MP sync
            if ((tile.Slope == slopeType || tile.IsHalfBlock != isHalfBlock) 
                && tile.IsHalfBlock == isHalfBlock) return;
            
            tile.IsHalfBlock = isHalfBlock;
            tile.Slope = slopeType;

            SoundEngine.PlaySound(SoundID.Dig);
            WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, false);

            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
        }
    }
}