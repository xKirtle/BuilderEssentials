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
    public override int[] ItemBoundToDisplay => new[] { ModContent.ItemType<FillWand>() };

    public override bool CanPlaceItems() => doPlacement && SelectedItem.type != ItemID.None;
    
    public override void PlotSelection() {
        int size = FillWand.FillSelectionSize;
        for (int i = 0; i < size; i++)
        for (int j = 0; j < size; j++) {
            ShapeHelpers.PlotPixel(Player.tileTargetX + j, Player.tileTargetY + i - size + 1);
            
            //Move this to update? Might be lagging too much
            if (CanPlaceItems()) {
                QueuePlacement(new Point(Player.tileTargetX + j, Player.tileTargetY + i - size + 1), SelectedItem);
            }
        }
        
        // if (CanPlaceItems())
        //     Console.WriteLine("Queued");
    }

    public override void UpdateRegardlessOfVisibility() {
        int heldItemType = Main.LocalPlayer.HeldItem.type;
        if ((heldItemType == ItemBoundToDisplay[0] && !IsVisible) || 
            (heldItemType != ItemBoundToDisplay[0] && IsVisible)) {
            ShapesUIState.TogglePanelVisibility<FillWandPanel>();
        }
    }
}