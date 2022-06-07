using System;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Common;

[Autoload(false)]
public abstract class BaseItemToggleableUI : ModItem
{
    private static UISystem UiSystem = ModContent.GetInstance<UISystem>();
    public virtual UIStateType UiStateType { get; private set; }

    public bool IsUiVisible() => UiSystem.userInterface.CurrentState == UiSystem.uiStates[(int) UiStateType - 1];

    public override bool? UseItem(Player player) {
        if (player.whoAmI != Main.myPlayer)
            return base.UseItem(player);
        
        if (IsUiVisible())
            UiSystem.ChangeOrToggleUIState(UiStateType);
        
        return base.UseItem(player);
    }

    public override bool AltFunctionUse(Player player) {
        if (player.whoAmI == Main.myPlayer) 
            UiSystem.ChangeOrToggleUIState(UiStateType);
        
        return base.AltFunctionUse(player);
    }

    public override void UpdateInventory(Player player) {
        if (player.whoAmI != Main.myPlayer) return;
        
        if (IsUiVisible() && player.HeldItem.type != Item.type)
            UiSystem.ChangeOrToggleUIState(UiStateType);
        
        base.UpdateInventory(player);
    }
}