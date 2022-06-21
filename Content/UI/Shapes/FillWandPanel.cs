using System;
using BuilderEssentials.Common;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.UI;

public class FillWandPanel : BaseShapePanel
{
    public override bool IsHoldingBindingItem() =>
        Main.LocalPlayer.HeldItem.type == ModContent.ItemType<FillWand>();

    public override bool CanPlaceItems() => SelectedItem.type != ItemID.None;
    
    public override void PlotSelection() {
        int size = FillWand.FillSelectionSize;
        for (int i = 0; i < size; i++)
        for (int j = 0; j < size; j++) {
            ShapeHelpers.PlotPixel(Player.tileTargetX + j, Player.tileTargetY + i - size + 1);
            QueuePlacement(new Point(Player.tileTargetX + j, Player.tileTargetY + i - size + 1));
        }
    }

    private Vector2 oldTileCoords;
    public override bool SelectionHasChanged() {
        Vector2 newTileCoords = new Vector2(Player.tileTargetX, Player.tileTargetY);
        if (oldTileCoords == newTileCoords) return false;
        
        oldTileCoords = newTileCoords;
        return true;
    }

    public override void UpdateRegardlessOfVisibility() {
        if ((IsHoldingBindingItem() && !IsVisible) || (!IsHoldingBindingItem() && IsVisible))
            ShapesUIState.TogglePanelVisibility<FillWandPanel>();
    }
}