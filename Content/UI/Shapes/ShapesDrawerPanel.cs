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
        var menuPanel = ToggleableItemsUIState.GetUIPanel<ShapesDrawerMenuPanel>();
        Shapes shape = menuPanel.SelectedShape;
        ShapeSide drawSide = menuPanel.SelectedShapeSide;
        bool isFilled = menuPanel.IsFilled;
        Color color = isFilled ? ShapeHelpers.Yellow : ShapeHelpers.Blue;
        
        if (shape == Shapes.Rectangle)
            ShapeHelpers.PlotRectangle(cs.LeftMouse.Start, cs.LeftMouse.End, color, visitedPlottedPixels, 0.9f, isFilled);
        else if (shape == Shapes.Ellipse)
            ShapeHelpers.PlotEllipse(cs.LeftMouse.Start, cs.LeftMouse.End, color, visitedPlottedPixels, drawSide, 0.9f, isFilled);
    }

    private HashSet<Vector2> oldVisitedCoords;
    public override bool SelectionHasChanged() {
        if (oldVisitedCoords == visitedPlottedPixels) return false;
        
        //TODO: Troublesome?
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