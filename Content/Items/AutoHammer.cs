using System;
using System.Collections.Generic;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Configs;
using BuilderEssentials.Common.Enums;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.Items;

[Autoload(true)]
public class AutoHammer : BaseItemToggleableUI
{
    public override ToggleableUiType ToggleableUiType => ToggleableUiType.AutoHammer;
    protected override bool CloneNewInstances => true;
    public override int ItemRange => 10;

    public override bool IsLoadingEnabled(Mod mod) {
        return ModContent.GetInstance<MainConfig>().EnabledItems.AutoHammer;
    }

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

        AutoHammerPanel panel = ToggleableItemsUIState.GetUIPanel<AutoHammerPanel>();
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
            AutoHammerPanel panel = ToggleableItemsUIState.GetUIPanel<AutoHammerPanel>();
            //Can the selected index change between CanUseItem and UseItem at all?
            if (panel.selectedIndex != -1) {
                Point16 coords = new Point16(Player.tileTargetX, Player.tileTargetY);
                ChangeSlope(coords, panel.slopeType, panel.isHalfBlock);

                //Add MirrorPlacement logic
                Tile tile = Framing.GetTileSafely(Player.tileTargetX, Player.tileTargetY);
                MirrorPlacement.MirrorPlacementAction(mirroredCoords => {
                    int[] mirroredSlopes = new[] { 0, 2, 1, 4, 3 };
                    ChangeSlope(mirroredCoords, (SlopeType) mirroredSlopes[(int) tile.Slope], tile.IsHalfBlock);
                });
            }

            Item.hammer = 80;
            canChangeSlope = false;
        }

        return true;
    }

    //Kirtle: Tile.SmoothSlope for the edge case (halfBrick) where it looks glitched?
    public static void ChangeSlope(Point16 coords, SlopeType slopeType, bool isHalfBlock) {
        Tile tile = Framing.GetTileSafely(coords.X, coords.Y);
        if (Main.tileSolid[tile.TileType] && tile.TileType >= 0 && tile.HasTile) {
            //Prevent unnecessary changes to the tile and MP sync
            if (tile.Slope == slopeType && tile.IsHalfBlock == isHalfBlock) return;

            tile.IsHalfBlock = isHalfBlock;
            tile.Slope = isHalfBlock ? SlopeType.Solid : slopeType;

            WorldGen.KillTile(coords.X, coords.Y, true, true);
            WorldGen.SquareTileFrame(coords.X, coords.Y, true);
            SoundEngine.PlaySound(SoundID.Dig);

            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, coords.X, coords.Y, 1);
        }
    }
}