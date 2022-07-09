using System.Collections.Generic;
using BuilderEssentials.Common.Enums;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.UI;

public class ShapesDrawerPanel : BaseShapePanel
{
    public override bool IsHoldingBindingItem() 
        => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<ShapesDrawer>();

    public override bool CanPlaceItems() => SelectedItem.type != ItemID.None;

    public override void PlotSelection() {
        // ShapeHelpers.PlotEllipse(cs.LeftMouse.Start, cs.LeftMouse.End, ShapeHelpers.Blue, visitedPlottedPixels, ShapeSide.All, 0.9f);
    }

    private HashSet<Vector2> oldVisitedCoords;
    public override bool SelectionHasChanged() {
        if (oldVisitedCoords == visitedPlottedPixels) return false;
        
        oldVisitedCoords = visitedPlottedPixels;
        return true;
    }

    private HashSet<Vector2> visitedPlottedPixels = new();
    public override HashSet<Vector2> VisitedPlottedPixels => visitedPlottedPixels;

    public override void UpdateRegardlessOfVisibility() {
        bool hasItemInInventory = Main.LocalPlayer.HasItem(ModContent.ItemType<ShapesDrawer>());
        if ((hasItemInInventory && !IsVisible) || (!hasItemInInventory && IsVisible))
            ShapesUIState.TogglePanelVisibility<ShapesDrawerPanel>();
    }
}