using System;
using System.Collections.Generic;
using BuilderEssentials.Common;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.UI;

public class FillWandPanel : BaseShapePanel
{
    public override bool IsHoldingBindingItem() => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<FillWand>();

    public override bool CanPlaceItems() => SelectedItem.type != ItemID.None;

    public override HashSet<Vector2> VisitedPlottedPixels => null;

    public override void PlotSelection() {
        int size = FillWand.FillSelectionSize;
        for (int x = 0; x < size; x++)
        for (int y = 0; y < size; y++) {
            ShapeHelpers.PlotPixel(Player.tileTargetX + y, Player.tileTargetY + x - size + 1, ShapeHelpers.Blue, this, 0.9f);
        }
    }

    private Vector2 oldTileCoords;
    public override bool SelectionHasChanged() {
        Vector2 newTileCoords = new(Player.tileTargetX, Player.tileTargetY);
        if (oldTileCoords == newTileCoords)
            return false;

        oldTileCoords = newTileCoords;
        return true;
    }


    public override void UpdateRegardlessOfVisibility() {
        if (IsHoldingBindingItem() && !IsVisible || !IsHoldingBindingItem() && IsVisible)
            ShapesUIState.TogglePanelVisibility<FillWandPanel>();
    }
}