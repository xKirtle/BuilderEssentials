using System;
using System.Collections.Generic;
using System.Linq;
using BuilderEssentials.Common;
using BuilderEssentials.Common.Enums;
using BuilderEssentials.Common.Systems;
using BuilderEssentials.Content.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuilderEssentials.Content.Items;

[Autoload(false)]
public abstract class BaseItemToggleableUI : BuilderEssentialsItem
{
    private static ToggleableItemsUISystem UiSystem = ModContent.GetInstance<ToggleableItemsUISystem>();
    public virtual ToggleableUiType ToggleableUiType { get; private set; }

    public override void SetDefaults() //TODO: Check if updating tile range in holdItem is a better solution
        => Item.tileBoost = ItemRange - 18;
    //So that ItemRange is accurate per tiles
    public override void ModifyTooltips(List<TooltipLine> tooltips)
        => tooltips.Remove(tooltips.Find(x => x.Text.Contains($"{Item.tileBoost} range")));

    public override bool AltFunctionUse(Player player) {
        if (player.whoAmI == Main.myPlayer && Main.netMode != NetmodeID.Server)
            TogglePanel();

        return false;
    }

    public bool IsPanelVisible() => ToggleableItemsUIState.GetUIPanel((int) ToggleableUiType).IsVisible;

    public void TogglePanel() => ToggleableItemsUIState.TogglePanelVisibility((int) ToggleableUiType);
}