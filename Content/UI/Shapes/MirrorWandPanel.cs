using System;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BuilderEssentials.Content.UI;

public class MirrorWandPanel : BaseShapePanel
{
    private bool horizontalMirror;
    private bool wideMirror;
    private bool validMirrorPlacement;
    public override bool IsHoldingBindingItem()
        => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<MirrorWand>();

    public override bool CanPlaceItems() => false;

    public override bool SelectionHasChanged() => false;

    private bool IsMouseWithinSelection() =>
        cs.IsWithinRange(Player.tileTargetX, cs.RightMouse.Start.X, cs.RightMouse.End.X) &&
        cs.IsWithinRange(Player.tileTargetY, cs.RightMouse.Start.Y, cs.RightMouse.End.Y);
    
    private bool IsMirrorAxisInsideSelection() =>
        cs.LeftMouse.Start != cs.LeftMouse.End &&
        cs.IsWithinRange(cs.LeftMouse.Start.X, cs.RightMouse.Start.X, cs.RightMouse.End.X) &&
        cs.IsWithinRange(cs.LeftMouse.End.X, cs.RightMouse.Start.X, cs.RightMouse.End.X) &&
        cs.IsWithinRange(cs.LeftMouse.Start.Y, cs.RightMouse.Start.Y, cs.RightMouse.End.Y) &&
        cs.IsWithinRange(cs.LeftMouse.End.Y, cs.RightMouse.Start.Y, cs.RightMouse.End.Y);
    
    public override void PlotSelection() {
        //Selected area
        ShapeHelpers.PlotRectangle(cs.RightMouse.Start, cs.RightMouse.End, ShapeHelpers.Blue, 0.90f, false);
        
        //Mirror
        LimitMirrorSize();
        validMirrorPlacement = IsMirrorAxisInsideSelection();
        Color color = validMirrorPlacement ? ShapeHelpers.Yellow : ShapeHelpers.Red;
        ShapeHelpers.PlotRectangle(cs.LeftMouse.Start, cs.LeftMouse.End, color, 0.90f, false);
    }

    private void LimitMirrorSize() {
        Vector2 start = cs.LeftMouse.Start;
        Vector2 end = cs.LeftMouse.End;
        
        int dx = (int) Math.Abs(start.X - end.X);
        int dy = (int) Math.Abs(start.Y - end.Y);
        horizontalMirror = dx > dy;
            
        //Limit x coords
        if (!horizontalMirror && end.X < start.X - 1)
            end.X = start.X - 1;
        else if (!horizontalMirror && end.X > start.X + 1)
            end.X = start.X + 1;
            
        //Limit y coords
        if (horizontalMirror && end.Y < start.Y - 1)
            end.Y = start.Y - 1;
        else if (horizontalMirror && end.Y > start.Y + 1)
            end.Y = start.Y + 1;

        cs.LeftMouse.Start = new Vector2(start.X, start.Y);
        cs.LeftMouse.End = new Vector2(end.X, end.Y);
        
        wideMirror = end.X == start.X - 1 || end.X == start.X + 1 || 
                     end.Y == start.Y - 1 || end.Y == start.Y + 1;
    }

    public override void UpdateRegardlessOfVisibility() {
        bool hasItemInInventory = Main.LocalPlayer.HasItem(ModContent.ItemType<MirrorWand>());
        if ((hasItemInInventory && !IsVisible) || (!hasItemInInventory && IsVisible))
            ShapesUIState.TogglePanelVisibility<MirrorWandPanel>();
    }
}