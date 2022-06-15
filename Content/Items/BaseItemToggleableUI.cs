using System;
using System.Collections.Generic;
using System.Linq;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.Items;

[Autoload(false)]
public abstract class BaseItemToggleableUI : ModItem
{
    public override string Texture => "BuilderEssentials/Assets/Items/" + GetType().Name;
    
    private static UISystem UiSystem = ModContent.GetInstance<UISystem>();
    public virtual UIStateType UiStateType { get; private set; }
    public virtual int ItemRange { get; protected set; } = 8;
    public virtual bool CloseUIOnItemSwap { get; protected set; } = true;

    public bool IsUiVisible() => UiStateType != UIStateType.None
        ? UiSystem.uiScaleUI.CurrentState == UiSystem.uiStates[(int) UiStateType - 1] : false;

    public override void SetDefaults() { //TODO: Check if updating tile range in holdItem is a better solution
        Item.tileBoost = ItemRange - 18; //So that ItemRange is accurate per tiles
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips) {
        tooltips.Remove(tooltips.Find(x => x.Text.Contains($"{Item.tileBoost} range")));
    }
    
    public override bool? UseItem(Player player) {
        if (player.whoAmI == Main.myPlayer) {
            if (IsUiVisible())
                UiSystem.ChangeOrToggleUIState(UiStateType);
        }

        return base.UseItem(player);
    }
    
    public override bool AltFunctionUse(Player player) {
        if (player.whoAmI == Main.myPlayer) {
            UiSystem.ChangeOrToggleUIState(UiStateType);
        }

        return false;
    }

    public override void UpdateInventory(Player player) {
        if (player.whoAmI != Main.myPlayer) return;
        
        if (IsUiVisible() && UiStateType.GetInstance()?.BoundItemType.Contains(player.HeldItem.type) == false)
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
    }
}