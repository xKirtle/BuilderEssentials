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
                int correctionOrigin = 0;
                try
                {
                    Tile tile = Framing.GetTileSafely(i, j);

                    var tileOrigin = TileObjectData.GetTileData(tile).Origin;
                    var tileSize = TileObjectData.GetTileData(tile).CoordinateFullWidth / 16;

                    if (tileSize == 2 && (tileOrigin == new Point16(0, 0) || tileOrigin == new Point16(0, 1)
                        || tileOrigin == new Point16(0, 4) || tileOrigin == new Point16(1, 1)))
                        correctionOrigin = -1;

                    if (tileSize == 4 && (tileOrigin == new Point16(1, 1) || tileOrigin == new Point16(1, 3)))
                        correctionOrigin = -1;

                    if (tileSize == 4 && tileOrigin == new Point16(1, 2))
                        correctionOrigin = -2;
                }
                catch (Exception ex) { Debug.Print(ex.Message); }

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
                        bool inRange = false;
                        if (distanceToMirror < 0) //Right to the mirror axis
                        {
                            newPos = ts.endMirror.X - Math.Abs(distanceToMirror) + correctionOrigin;
                            if (ts.wideMirror && LeftRight) newPos -= 1;
                            if (IsWithinRange(newPos, ts.startSel.X, ts.endSel.X))
                                inRange = true;
                        }
                        else //Left to the mirror axis
                        {
                            newPos = ts.endMirror.X + Math.Abs(distanceToMirror + correctionOrigin);
                            if (ts.wideMirror && LeftRight) newPos -= 1;
                            if (IsWithinRange(newPos, ts.startSel.X, ts.endSel.X))
                                inRange = true;
                        }

                        if (inRange)
                        {
                            switch (itemTypes)
                            {
                                case ItemTypes.Air:
                                    break;
                                case ItemTypes.Tile:
                                    WorldGen.PlaceTile((int)newPos, (int)posY, item.createTile, style: item.placeStyle);
                                    break;
                                case ItemTypes.Wall:
                                    WorldGen.PlaceWall((int)newPos, (int)posY, item.createWall);
                                    break;
                            }

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendTileSquare(-1, (int)newPos, (int)posY, 1);
                        }
                    }
                }
                else if (ts.horizontalSelection)
                {
                    float distanceToMirror = ts.endMirror.Y - posY;
                    if (IsWithinRange(posX, ts.startMirror.X, ts.endMirror.X, true))
                    {
                        float newPos;
                        bool inRange = false;
                        if (distanceToMirror < 0) //Bottom to the mirror axis
                        {
                            newPos = ts.endMirror.Y - Math.Abs(distanceToMirror);
                            if (ts.wideMirror && TopBottom) newPos -= 1;
                            else if (ts.wideMirror && BottomTop) newPos += 1;

                            if (IsWithinRange(newPos, ts.startSel.Y, ts.endSel.Y))
                                inRange = true;
                        }
                        else //Top to the mirror axis
                        {
                            newPos = ts.endMirror.Y + Math.Abs(distanceToMirror);
                            if (ts.wideMirror && TopBottom) newPos -= 1;
                            else if (ts.wideMirror && BottomTop) newPos += 1;

                            if (IsWithinRange(newPos, ts.startSel.Y, ts.endSel.Y))
                                inRange = true;
                        }

                        if (inRange)
                        {
                            switch (itemTypes)
                            {
                                case ItemTypes.Air:
                                    break;
                                case ItemTypes.Tile:
                                    WorldGen.PlaceTile((int)posX, (int)newPos, item.createTile, false, false, -1, item.placeStyle);
                                    break;
                                case ItemTypes.Wall:
                                    WorldGen.PlaceWall((int)posX, (int)newPos, item.createWall);
                                    break;
                            }

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendTileSquare(-1, (int)posX, (int)newPos, 1);
                        }
                    }
                }
            }
        }

    }
}
