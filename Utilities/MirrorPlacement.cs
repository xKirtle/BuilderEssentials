using System;
using BuilderEssentials.UI.UIPanels.ShapesDrawing;
using BuilderEssentials.UI.UIStates;
using Terraria;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace BuilderEssentials.Utilities
{
    public static partial class HelperMethods
    {
        public static void MirrorPlacement(int i, int j, int itemType)
        {
            MirrorWandSelection ms = UIStateLogic1.mirrorWandSelection;
            CoordsSelection cs = ms.cs;

            //Mirror coords
            Vector2 start = cs.LMBStart;
            Vector2 end = cs.LMBEnd;

            //Check if coords are within selection
            if (!ms.validMirrorPlacement || !HelperMethods.IsWithinRange(i, cs.RMBStart.X, cs.RMBEnd.X) ||
                !HelperMethods.IsWithinRange(j, cs.RMBStart.Y, cs.RMBEnd.Y)) return;

            //Check if coords intersect the mirror axis
            if (!HelperMethods.IsWithinRange(i, start.X, end.X, true) &&
                !HelperMethods.IsWithinRange(j, start.Y, end.Y, true)) return;


            //Compensate for vanilla's weird tile origin
            int correctionOrigin = 0;
            Point16 tileOrigin;
            int tileSize;
            
            if (WhatIsThisItem(itemType) == ItemTypes.Tile)
            {
                Tile tile = Framing.GetTileSafely(i, j);
                TileObjectData data = TileObjectData.GetTileData(tile);

                if (data != null)
                {
                    tileOrigin = TileObjectData.GetTileData(tile).Origin;
                    tileSize = TileObjectData.GetTileData(tile).CoordinateFullWidth / 16;

                    if (tileSize == 2 && (tileOrigin == new Point16(0, 0) || tileOrigin == new Point16(0, 1) ||
                        tileOrigin == new Point16(0, 2)) || tileOrigin == new Point16(0, 4) || tileOrigin == new Point16(1, 2))
                        correctionOrigin -= 1;

                    if (tileSize == 2 && tileOrigin == new Point16(1, 1))
                        correctionOrigin += 1;

                    if (tileSize == 4 && (tileOrigin == new Point16(1, 1) || tileOrigin == new Point16(1, 3)))
                        correctionOrigin -= 1;

                    if (tileSize == 4 && tileOrigin == new Point16(1, 2))
                        correctionOrigin -= 2;
                }
            }
            
            //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
            bool LeftRight = ms.selectedQuarter == 1 || ms.selectedQuarter == 3;
            bool TopBottom = ms.selectedQuarter == 2 || ms.selectedQuarter == 3;
            bool BottomTop = ms.selectedQuarter == 0 || ms.selectedQuarter == 1;

            if (!ms.horizontalMirror)
            {
                float distanceToMirror = start.X - i > end.X - i ? start.X - i : end.X - i;

                float newPosX;
                newPosX = end.X + correctionOrigin;
                newPosX += distanceToMirror < 0 ? -Math.Abs(distanceToMirror) : Math.Abs(distanceToMirror);
                if (ms.wideMirror && LeftRight) newPosX -= 1;
                if (HelperMethods.IsWithinRange(newPosX, cs.RMBStart.X, cs.RMBEnd.X))
                    HelperMethods.PlaceTile((int) newPosX, j, itemType, true);
            }
            else
            {
                float distanceToMirror = start.Y - j > end.Y - j ? start.Y - j : end.Y - j;

                float newPosY = end.Y; /*no correction needed*/
                newPosY += distanceToMirror < 0 ? -Math.Abs(distanceToMirror) : Math.Abs(distanceToMirror);
                if (ms.wideMirror && TopBottom) newPosY -= 1;
                else if (ms.wideMirror && BottomTop) newPosY += 1;
                if (HelperMethods.IsWithinRange(newPosY, cs.RMBStart.Y, cs.RMBEnd.Y))
                    HelperMethods.PlaceTile(i, (int) newPosY, itemType, true);
            }
        }
    }
}