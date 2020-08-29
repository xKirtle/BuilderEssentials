using BuilderEssentials.UI;
using System;
using System.Diagnostics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;

namespace BuilderEssentials.Utilities
{
    public static partial class Tools
    {
        public static void MirrorPlacement(int i, int j, int itemType)
        {
            TransparentSelectionUI ts = TransparentSelectionUI.Instance;

            Item item = new Item();
            item.SetDefaults(itemType);
            ItemTypes itemTypes = WhatIsThisItem(itemType);

            if (ts.validMirrorPlacement)
            {
                //compensate for multi tiles
                int correctionOrigin = 0;
                //if (itemTypes == ItemTypes.Tile)
                //{
                //    //GetTileData throwing random null refs
                //    Tile tile = Framing.GetTileSafely(i, j);
                //    var tileOrigin = TileObjectData.GetTileData(tile).Origin;
                //    var tileSize = TileObjectData.GetTileData(tile).CoordinateFullWidth / 16;


                //    if (tileSize == 2 && (tileOrigin == new Point16(0, 0) || tileOrigin == new Point16(0, 1)
                //        || tileOrigin == new Point16(0, 4) || tileOrigin == new Point16(1, 1)))
                //        correctionOrigin = -1;

                //    if (tileSize == 4 && (tileOrigin == new Point16(1, 1) || tileOrigin == new Point16(1, 3)))
                //        correctionOrigin = -1;

                //    if (tileSize == 4 && tileOrigin == new Point16(1, 2))
                //        correctionOrigin = -2;
                //}

                float posX = i;
                float posY = j;
                //0:TopLeft; 1:TopRight; 2:BottomLeft; 3:BottomRight;
                bool LeftRight = ts.selectedQuarter == 1 || ts.selectedQuarter == 3;
                bool TopBottom = ts.selectedQuarter == 2 || ts.selectedQuarter == 3;
                bool BottomTop = ts.selectedQuarter == 0 || ts.selectedQuarter == 1;

                if (!ts.horizontalSelection)
                {
                    float distanceToMirror = ts.endMirror.X - posX;
                    if (ts.startMirror.X - posX > ts.endMirror.X - posX)
                        distanceToMirror = ts.startMirror.X - posX;

                    if (IsWithinRange(posY, ts.startMirror.Y, ts.endMirror.Y, true))
                    {
                        float newPos;
                        if (distanceToMirror < 0) //Right to the mirror axis
                        {
                            newPos = ts.endMirror.X - Math.Abs(distanceToMirror) + correctionOrigin;
                            if (ts.wideMirror && LeftRight) newPos -= 1;
                            if (IsWithinRange(newPos, ts.startSel.X, ts.endSel.X))
                                PlaceTile((int)newPos, (int)posY, item.type);
                        }
                        else //Left to the mirror axis
                        {
                            newPos = ts.endMirror.X + Math.Abs(distanceToMirror + correctionOrigin);
                            if (ts.wideMirror && LeftRight) newPos -= 1;
                            if (IsWithinRange(newPos, ts.startSel.X, ts.endSel.X))
                                PlaceTile((int)newPos, (int)posY, item.type);
                        }
                    }
                }
                else if (ts.horizontalSelection)
                {
                    float distanceToMirror = ts.endMirror.Y - posY;
                    if (IsWithinRange(posX, ts.startMirror.X, ts.endMirror.X, true))
                    {
                        float newPos;
                        if (distanceToMirror < 0) //Bottom to the mirror axis
                        {
                            newPos = ts.endMirror.Y - Math.Abs(distanceToMirror);
                            if (ts.wideMirror && TopBottom) newPos -= 1;
                            else if (ts.wideMirror && BottomTop) newPos += 1;

                            if (IsWithinRange(newPos, ts.startSel.Y, ts.endSel.Y))
                                PlaceTile((int)posX, (int)newPos, item.type);
                        }
                        else //Top to the mirror axis
                        {
                            newPos = ts.endMirror.Y + Math.Abs(distanceToMirror);
                            if (ts.wideMirror && TopBottom) newPos -= 1;
                            else if (ts.wideMirror && BottomTop) newPos += 1;

                            if (IsWithinRange(newPos, ts.startSel.Y, ts.endSel.Y))
                                PlaceTile((int)posX, (int)newPos, item.type);
                        }
                    }
                }
            }
        }

    }
}
