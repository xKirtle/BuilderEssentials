using System;
using System.Collections.Generic;
using BuilderEssentials.Common;
using BuilderEssentials.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static BuilderEssentials.Common.DataStructures.CoordSelection;

namespace BuilderEssentials.Content.UI;

public class MirrorWandPanel : BaseShapePanel
{
    public bool wideMirror;
    public bool validMirrorPlacement;
    public bool horizontalMirror;

    private Vector2 selStart => cs.RightMouse.Start;
    private Vector2 selEnd => cs.RightMouse.End;
    private Vector2 mirStart => cs.LeftMouse.Start;
    private Vector2 mirEnd => cs.LeftMouse.End;

    public override bool IsHoldingBindingItem()
        => Main.LocalPlayer.HeldItem.type == ModContent.ItemType<MirrorWand>();

    public override bool CanPlaceItems() => true;

    public override bool SelectionHasChanged() => false;
    
    public override HashSet<Vector2> VisitedPlottedPixels => null;

    public bool IsMouseWithinSelection() =>
        IsWithinRange(Player.tileTargetX, selStart.X, selEnd.X) &&
        IsWithinRange(Player.tileTargetY, selStart.Y, selEnd.Y);
    
    public bool IsMouseAffectedByMirrorAxis() =>
        validMirrorPlacement &&
        (horizontalMirror && IsWithinRange(Player.tileTargetX, mirStart.X, mirEnd.X, true) &&
         (Player.tileTargetY != mirStart.Y && Player.tileTargetY != mirEnd.Y)) ||
        (!horizontalMirror && IsWithinRange(Player.tileTargetY, mirStart.Y, mirEnd.Y, true) &&
         (Player.tileTargetX != mirStart.X && Player.tileTargetX != mirEnd.X));
    
    public bool IsMirrorAxisInsideSelection() =>
        mirStart != mirEnd &&
        IsWithinRange(mirStart.X, selStart.X, selEnd.X) &&
        IsWithinRange(mirEnd.X, selStart.X, selEnd.X) &&
        IsWithinRange(mirStart.Y, selStart.Y, selEnd.Y) &&
        IsWithinRange(mirEnd.Y, selStart.Y, selEnd.Y);

    //TODO: Check against wide Mirror scenarios
    public bool IsMouseLeftOrTopOfSelection() {
        if (!validMirrorPlacement || !IsMouseWithinSelection() || !IsMouseAffectedByMirrorAxis()) return false;
        return !horizontalMirror ? Player.tileTargetX < mirStart.X : Player.tileTargetY < mirStart.Y;
    }

    public override void PlotSelection() {
        //Selected area
        ShapeHelpers.PlotRectangle(selStart, selEnd, ShapeHelpers.Blue * 0.6f, null, 0.90f, false);
        
        //Mirror
        LimitMirrorSize();
        validMirrorPlacement = IsMirrorAxisInsideSelection();
        Color color = validMirrorPlacement ? ShapeHelpers.Yellow : ShapeHelpers.Red;
        ShapeHelpers.PlotRectangle(mirStart, mirEnd, color * 0.6f, null, 0.90f, false);
    }

    private void LimitMirrorSize() {
        Vector2 start = mirStart;
        Vector2 end = mirEnd;

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
        Vector2 initial = tileCoords == default ? new Vector2(Player.tileTargetX, Player.tileTargetY) : tileCoords;
        Vector2 result = initial;

        if (!validMirrorPlacement || !IsMouseWithinSelection() || !IsMouseAffectedByMirrorAxis()) return result;
        
        //Check if coords can be used by current mirror axis
        if (!IsWithinRange(result.X, mirStart.X, mirEnd.X, true) &&
            !IsWithinRange(result.Y, mirStart.Y, mirEnd.Y, true)) return result;

        Tile tile = Framing.GetTileSafely(result);
        //Check if not a wall
        TileObjectData data = tileType != -1 ? TileObjectData.GetTileData(tileType, style, alternate) : null;
        
        bool leftOfTheMirror = result.X < Math.Min(mirStart.X, mirEnd.X);
        bool topOfTheMirror = result.Y < Math.Min(mirStart.Y, mirEnd.Y);
        Point16 tileSize = Point16.Zero;

        Vector2 offset = Vector2.Zero;
        if (data != null) {
            Point16 tileOrigin = data.Origin;
            tileSize = new(data.CoordinateFullWidth / 16, data.CoordinateFullHeight / 16);
            
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
        
        //Check if result placement is within the selection -> for single tiles
        if (!IsWithinRange(result.X, selStart.X, selEnd.X) ||
            !IsWithinRange(result.Y, selStart.Y, selEnd.Y)) return initial;
        
        if (data == null) 
            return result;

        //Check if result placement will overlap with the mirror axis -> for multi tiles
        Point initialTopLeft = MirrorPlacement.GetTopLeftCoordOfTile((int) initial.X, (int) initial.Y, tileData: data);
        Point resultTopLeft = MirrorPlacement.GetTopLeftCoordOfTile((int) result.X, (int) result.Y, isPlaced: false, tileData: data);
        if ((!horizontalMirror && (leftOfTheMirror && (resultTopLeft.X <= (initialTopLeft.X + tileSize.X)) || 
                                   (!leftOfTheMirror && (resultTopLeft.X + tileSize.X) >= initialTopLeft.X))) ||
            (horizontalMirror && (topOfTheMirror && (resultTopLeft.Y <= (initialTopLeft.Y + tileSize.Y)) ||
                                  (!topOfTheMirror && (resultTopLeft.Y + tileSize.Y) >= initialTopLeft.Y))))
            return initial;
        
        return result;
    }

    //Unused
    public void SyncSelection() {
        if (Main.netMode == NetmodeID.MultiplayerClient) {
            Vector2 center = new Vector2((Math.Max(selStart.X, selEnd.X) - Math.Min(selStart.X, selEnd.X) + 1) / 2,
                (Math.Max(selStart.Y, selEnd.Y) - Math.Min(selStart.Y, selEnd.Y) + 1) / 2);

            float syncSizeX = Math.Max(Math.Max(center.X, selStart.X), selEnd.X);
            float syncSizeY = Math.Max(Math.Max(center.Y, selStart.Y), selEnd.Y);
            NetMessage.SendTileSquare(-1, (int) center.X, (int) center.Y, (int) Math.Max(syncSizeX, syncSizeY) + 200);
        }
    }
}