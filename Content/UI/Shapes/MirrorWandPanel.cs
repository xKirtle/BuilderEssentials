using System;
using BuilderEssentials.Common;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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

    public Vector2 GetMirroredTileTargetCoordinate(Vector2 tileCoords = default, int tileType = 0, int style = 0, int alternate = 0) {
        Vector2 initial =  tileCoords == default ? new Vector2(Player.tileTargetX, Player.tileTargetY) : tileCoords;
        Vector2 result = initial;

        Vector2 selStart = cs.RightMouse.Start;
        Vector2 selEnd = cs.RightMouse.End;
        Vector2 mirStart = cs.LeftMouse.Start;
        Vector2 mirEnd = cs.LeftMouse.End;
        
        if (!validMirrorPlacement || !IsMouseWithinSelection()) return result;
        
        //Check if coords can be used by current mirror axis
        if (!cs.IsWithinRange(result.X, mirStart.X, mirEnd.X, true) &&
            !cs.IsWithinRange(result.Y, mirStart.Y, mirEnd.Y, true)) return result;

        Tile tile = Framing.GetTileSafely(result);
        //Check if not a wall
        TileObjectData data = tileType != -1 ? TileObjectData.GetTileData(tileType, style, alternate) : null;
        
        bool leftOfTheMirror = result.X < Math.Min(mirStart.X, mirEnd.X);
        bool topOfTheMirror = result.Y < Math.Min(mirStart.Y, mirEnd.Y);
        
        Vector2 offset = Vector2.Zero;
        if (data != null) {
            Point16 tileOrigin = data.Origin;
            Point16 tileSize = new(data.CoordinateFullWidth / 16, data.CoordinateFullHeight / 16);
            
            if (tileSize.X % 2 == 0) {
                int middleBiasRight = (int) (tileSize.X / 2f + 0.5f);
                offset.X = (tileOrigin.X >= middleBiasRight ? -1 : 1) * (leftOfTheMirror ? -1 : 1);
            }

            if (tileSize.Y % 2 == 0) {
                int middleBiasBottom = (int) (tileSize.Y / 2f + 0.5f);
                offset.Y = (tileOrigin.Y >= middleBiasBottom ? -1 : 1) * (topOfTheMirror ? -1 : 1);
            }
        }
        
        if (!horizontalMirror) {
            float distanceToMirror = Math.Min(Math.Abs(result.X - mirStart.X), Math.Abs(result.X - mirEnd.X));
            result.X += (int) ((distanceToMirror * 2 + (wideMirror ? 1 : 0) + offset.X) * (leftOfTheMirror ? 1 : -1));
        }
        else {
            float distanceToMirror = Math.Min(Math.Abs(result.Y - mirStart.Y), Math.Abs(result.Y - mirEnd.Y));
            result.Y += (int) ((distanceToMirror * 2 + (wideMirror ? 1 : 0) + offset.Y) * (topOfTheMirror ? 1 : -1));
        }
        
        if (cs.IsWithinRange(result.X, selStart.X, selEnd.X) &&
            cs.IsWithinRange(result.Y, selStart.Y, selEnd.Y)) return initial;
        
        return result;
    }
}