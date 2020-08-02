using BuilderEssentials.Items;
using BuilderEssentials.UI;
using System;
using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Utilities
{
    public static partial class Tools
    {
        public static void MirrorPlacement(int i, int j, int itemType)
        {
            Item item = new Item();
            item.SetDefaults(itemType);
            ItemTypes itemTypes = WhatIsThisItem(itemType);

            if (BuilderEssentials.validMirrorWand)
            {
                float posX = i;
                float posY = j;
                if (MirrorWand.VerticalLine)
                {
                    float distanceToMirror = MirrorWand.mouseLeftEnd.X - posX;
                    if (MirrorWand.mouseLeftStart.X - posX > MirrorWand.mouseLeftEnd.X - posX)
                        distanceToMirror = MirrorWand.mouseLeftStart.X - posX;
                        
                    if (IsWithinRange(posY, MirrorWand.mouseLeftStart.Y, MirrorWand.mouseLeftEnd.Y))
                    {
                        float newPos;
                        bool inRange = false;
                        if (distanceToMirror < 0) //Right to the mirror axis
                        {
                            newPos = MirrorWand.mouseLeftEnd.X - Math.Abs(distanceToMirror);
                            if (MirrorWand.WideMirrorAxis && MirrorWand.LeftRight) newPos -= 1;
                            if (IsWithinRange(newPos, MirrorWand.start.X, MirrorWand.end.X))
                                inRange = true;
                        }
                        else //Left to the mirror axis
                        {
                            newPos = MirrorWand.mouseLeftEnd.X + Math.Abs(distanceToMirror);
                            if (MirrorWand.WideMirrorAxis && MirrorWand.LeftRight) newPos -= 1;
                            if (IsWithinRange(newPos, MirrorWand.start.X, MirrorWand.end.X))
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
                    float distanceToMirror = MirrorWand.mouseLeftEnd.Y - posY;
                    if (IsWithinRange(posX, MirrorWand.mouseLeftStart.X, MirrorWand.mouseLeftEnd.X))
                    {
                        float newPos;
                        bool inRange = false;
                        if (distanceToMirror < 0) //Bottom to the mirror axis
                        {
                            newPos = MirrorWand.mouseLeftEnd.Y - Math.Abs(distanceToMirror);
                            if (MirrorWand.WideMirrorAxis && MirrorWand.TopBottom) newPos -= 1;
                            else if (MirrorWand.WideMirrorAxis && MirrorWand.BottomTop) newPos += 1;

                            if (IsWithinRange(newPos, MirrorWand.start.Y, MirrorWand.end.Y))
                                inRange = true;
                        }
                        else //Top to the mirror axis
                        {
                            newPos = MirrorWand.mouseLeftEnd.Y + Math.Abs(distanceToMirror);
                            if (MirrorWand.WideMirrorAxis && MirrorWand.TopBottom) newPos -= 1;
                            else if (MirrorWand.WideMirrorAxis && MirrorWand.BottomTop) newPos += 1;

                            if (IsWithinRange(newPos, MirrorWand.start.Y, MirrorWand.end.Y))
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
