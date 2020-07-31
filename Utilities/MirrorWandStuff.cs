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
            //TODO: ACCOUNT FOR 2 WIDE MIRROR AXIS

            Item item = new Item();
            item.SetDefaults(itemType);
            ItemTypes itemTypes = WhatIsThisItem(itemType);

            bool VerticalMirrorSelection = TransparentSelection.selectedLeftQuarter == 0 || TransparentSelection.selectedLeftQuarter == 1;
            bool HorizontalMirrorSelection = TransparentSelection.selectedLeftQuarter == 2 || TransparentSelection.selectedLeftQuarter == 3;

            if (BuilderEssentials.validMirrorWand)
            {
                float posX = i;
                float posY = j;
                if (VerticalMirrorSelection)
                {
                    float distanceToMirror = MirrorWand.mouseLeftEnd.X - posX;
                    if (IsWithinRange(posY, MirrorWand.mouseLeftStart.Y, MirrorWand.mouseLeftEnd.Y))
                    {
                        float newPos;
                        bool inRange = false;
                        if (distanceToMirror < 0) //Right to the mirror axis
                        {
                            newPos = MirrorWand.mouseLeftEnd.X - Math.Abs(distanceToMirror);
                            if (IsWithinRange(newPos, MirrorWand.start.X, MirrorWand.end.X))
                                inRange = true;
                        }
                        else //Left to the mirror axis
                        {
                            newPos = MirrorWand.mouseLeftEnd.X + Math.Abs(distanceToMirror);
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
                else if (HorizontalMirrorSelection)
                {
                    float distanceToMirror = MirrorWand.mouseLeftEnd.Y - posY;
                    if (IsWithinRange(posX, MirrorWand.mouseLeftStart.X, MirrorWand.mouseLeftEnd.X))
                    {
                        float newPos;
                        bool inRange = false;
                        if (distanceToMirror < 0) //Bottom to the mirror axis
                        {
                            newPos = MirrorWand.mouseLeftEnd.Y - Math.Abs(distanceToMirror);
                            if (IsWithinRange(newPos, MirrorWand.start.Y, MirrorWand.end.Y))
                                inRange = true;
                        }
                        else //Top to the mirror axis
                        {
                            newPos = MirrorWand.mouseLeftEnd.Y + Math.Abs(distanceToMirror);
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
