using System;
using BuilderEssentials.Common;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Content.UI;

public class MirrorWandPanel : BaseShapePanel
{
    public bool wideMirror;
    public bool validMirrorPlacement;
    public bool horizontalMirror;
    
    public override bool IsHoldingBindingItem()
        => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<MirrorWand>();

    public override bool CanPlaceItems() => true;

    public override bool SelectionHasChanged() => false;

    public bool IsMouseWithinSelection() =>
        cs.IsWithinRange(Player.tileTargetX, cs.RightMouse.Start.X, cs.RightMouse.End.X) &&
        cs.IsWithinRange(Player.tileTargetY, cs.RightMouse.Start.Y, cs.RightMouse.End.Y);
    
    public bool IsMirrorAxisInsideSelection() =>
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
    
    public Vector2 GetMirroredTileTargetCoordinate() {
        Vector2 result = new Vector2(Player.tileTargetX, Player.tileTargetY);

        Vector2 selStart = cs.RightMouse.Start;
        Vector2 selEnd = cs.RightMouse.End;
        Vector2 mirStart = cs.LeftMouse.Start;
        Vector2 mirEnd = cs.LeftMouse.End;
        
        if (!validMirrorPlacement || !IsMouseWithinSelection()) return result;
        
        if (!cs.IsWithinRange(result.X, selStart.X, selEnd.X, true) &&
            !cs.IsWithinRange(result.Y, selStart.Y, selEnd.Y, true)) return result;
        
        Tile tile = Framing.GetTileSafely(result);
        TileObjectData data = TileObjectData.GetTileData(tile);
        
        Vector2 offset = Vector2.Zero;
        if (data != null) {
            //TODO: figure out multi tile origin offset fix math    
        }
        
        if (!horizontalMirror) {
            float minMirrorX = Math.Min(mirStart.X, mirEnd.X);
            bool leftOfTheMirror = result.X < minMirrorX;
            float distanceToMirror = Math.Min(Math.Abs(result.X - mirStart.X), Math.Abs(result.X - mirEnd.X));
            
            result.X += (int) ((distanceToMirror * 2 + (wideMirror ? 1 : 0) + offset.X) * (leftOfTheMirror ? 1 : -1));
        }
        else {
            float minMirrorY = Math.Min(mirStart.Y, mirEnd.Y);
            bool topOfTheMirror = result.Y < minMirrorY;
            float distanceToMirror = Math.Min(Math.Abs(result.Y - mirStart.Y), Math.Abs(result.Y - mirEnd.Y));
            
            result.Y += (int) ((distanceToMirror * 2 + (wideMirror ? 1 : 0) + offset.Y) * (topOfTheMirror ? 1 : -1));
        }
        
        return result;
    }
}