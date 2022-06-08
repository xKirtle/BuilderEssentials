using System;
using System.Collections.Generic;
using BuilderEssentials.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Common;

[Autoload(false)]
public abstract class BaseItemToggleableUI : ModItem
{
    private static UISystem UiSystem = ModContent.GetInstance<UISystem>();
    public virtual UIStateType UiStateType { get; private set; }
    public virtual int ItemRange { get; protected set; } = 8;

    public bool IsUiVisible() => UiSystem.userInterface.CurrentState == UiSystem.uiStates[(int) UiStateType - 1];

    public override void SetDefaults() {
        Item.tileBoost = ItemRange - 18; //So that ItemRange is accurate per tiles
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips) {
        tooltips.Remove(tooltips.Find(x => x.Text.Contains($"{Item.tileBoost} range")));
    }
    
    public override bool? UseItem(Player player) {
        if (player.whoAmI != Main.myPlayer)
            return base.UseItem(player);
        
        if (IsUiVisible())
            UiSystem.ChangeOrToggleUIState(UiStateType);

        return null;
    }

    public override bool AltFunctionUse(Player player) {
        if (player.whoAmI == Main.myPlayer) 
            UiSystem.ChangeOrToggleUIState(UiStateType);

        return false;
    }

    public override void UpdateInventory(Player player) {
        if (player.whoAmI != Main.myPlayer) return;
        
        if (IsUiVisible() && player.HeldItem.type != Item.type)
            UiSystem.ChangeOrToggleUIState(UiStateType);
    }

    public override void HoldItem(Player player) {
        if (player.whoAmI != Main.myPlayer) return;
        
        if (ItemHasRange()) {
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = Type;
        }
    }

    public bool ItemHasRange(float itemRangeInTiles = default) {
        Vector2 screenPosition = Main.screenPosition.ToTileCoordinates().ToVector2();
        Vector2 playerCenterScreen = Main.LocalPlayer.Center.ToTileCoordinates().ToVector2() - screenPosition;
        Vector2 mouseCoords = Main.MouseScreen.ToTileCoordinates().ToVector2();

        itemRangeInTiles = itemRangeInTiles == default ? ItemRange : itemRangeInTiles;
        return Math.Abs(playerCenterScreen.X - mouseCoords.X) <= itemRangeInTiles &&
               Math.Abs(playerCenterScreen.Y - mouseCoords.Y) <= itemRangeInTiles - 2;

        // return (playerCenter - screenPosition).WithinRange(mouseCoords,
        //     itemRangeInTiles == default ? ItemRange : itemRangeInTiles);
    }
}