using BuilderEssentials.Items;
using Microsoft.Xna.Framework;
using System;
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
            //Vanilla, what the hell?
            //    //size 2, origin (0, 0), x needs to decrease 1
            //    //size 2, origin (0, 1), x needs to decrease 1
            //    //size 2, origin (0, 4), x needs to decrease 1
            //    //size 2, origin (1, 1), x needs to increase 1
            //    //size 4, origin (1, 1), x needs to decrease 1
            //    //size 4, origin (1, 2), x needs to decrease 2
            //    //size 4, origin (1, 3), x needs to decrease 1

            Item item = new Item();
            item.SetDefaults(itemType);
            ItemTypes itemTypes = WhatIsThisItem(itemType);

            if (BuilderEssentials.validMirrorWand)
            {
                int correctionOrigin = 0;
                try
                {
                    //Throwing ingame error for non multi tile items
                    Tile tile = Framing.GetTileSafely(i, j);
                    //Tile tile = Main.tile[i, j];

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
                catch (Exception) { }


                float posX = i;
                float posY = j;
                if (MirrorWand.VerticalLine)
                {
                    float distanceToMirror = MirrorWand.mirrorEnd.X - posX;
                    if (MirrorWand.mirrorStart.X - posX > MirrorWand.mirrorEnd.X - posX)
                        distanceToMirror = MirrorWand.mirrorStart.X - posX;
                        
                    if (IsWithinRange(posY, MirrorWand.mirrorStart.Y, MirrorWand.mirrorEnd.Y))
                    {
                        float newPos;
                        bool inRange = false;
                        if (distanceToMirror < 0) //Right to the mirror axis
                        {
                            newPos = MirrorWand.mirrorEnd.X - Math.Abs(distanceToMirror) + correctionOrigin;
                            if (MirrorWand.WideMirrorAxis && MirrorWand.LeftRight) newPos -= 1;
                            if (IsWithinRange(newPos, MirrorWand.selectionStart.X, MirrorWand.selectionEnd.X))
                                inRange = true;
                        }
                        else //Left to the mirror axis
                        {
                            newPos = MirrorWand.mirrorEnd.X + Math.Abs(distanceToMirror + correctionOrigin);
                            if (MirrorWand.WideMirrorAxis && MirrorWand.LeftRight) newPos -= 1;
                            if (IsWithinRange(newPos, MirrorWand.selectionStart.X, MirrorWand.selectionEnd.X))
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
                else if (MirrorWand.HorizontalLine)
                {
                    float distanceToMirror = MirrorWand.mirrorEnd.Y - posY;
                    if (IsWithinRange(posX, MirrorWand.mirrorStart.X, MirrorWand.mirrorEnd.X))
                    {
                        float newPos;
                        bool inRange = false;
                        if (distanceToMirror < 0) //Bottom to the mirror axis
                        {
                            newPos = MirrorWand.mirrorEnd.Y - Math.Abs(distanceToMirror);
                            if (MirrorWand.WideMirrorAxis && MirrorWand.TopBottom) newPos -= 1;
                            else if (MirrorWand.WideMirrorAxis && MirrorWand.BottomTop) newPos += 1;

                            if (IsWithinRange(newPos, MirrorWand.selectionStart.Y, MirrorWand.selectionEnd.Y))
                                inRange = true;
                        }
                        else //Top to the mirror axis
                        {
                            newPos = MirrorWand.mirrorEnd.Y + Math.Abs(distanceToMirror);
                            if (MirrorWand.WideMirrorAxis && MirrorWand.TopBottom) newPos -= 1;
                            else if (MirrorWand.WideMirrorAxis && MirrorWand.BottomTop) newPos += 1;

                            if (IsWithinRange(newPos, MirrorWand.selectionStart.Y, MirrorWand.selectionEnd.Y))
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
