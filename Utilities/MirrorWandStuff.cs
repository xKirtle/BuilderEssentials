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
                    float distanceToMirror = MirrorWand.mirrorEnd.X - posX;
                    if (MirrorWand.mirrorStart.X - posX > MirrorWand.mirrorEnd.X - posX)
                        distanceToMirror = MirrorWand.mirrorStart.X - posX;
                        
                    if (IsWithinRange(posY, MirrorWand.mirrorStart.Y, MirrorWand.mirrorEnd.Y))
                    {
                        float newPos;
                        bool inRange = false;
                        if (distanceToMirror < 0) //Right to the mirror axis
                        {
                            newPos = MirrorWand.mirrorEnd.X - Math.Abs(distanceToMirror);
                            if (MirrorWand.WideMirrorAxis && MirrorWand.LeftRight) newPos -= 1;
                            if (IsWithinRange(newPos, MirrorWand.selectionStart.X, MirrorWand.selectionEnd.X))
                                inRange = true;
                        }
                        else //Left to the mirror axis
                        {
                            newPos = MirrorWand.mirrorEnd.X + Math.Abs(distanceToMirror);
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
