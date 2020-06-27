using BuilderEssentials.Items;
using BuilderEssentials.UI;
using System;
using Terraria;
using Terraria.ID;

namespace BuilderEssentials.Utilities
{
    public static class UIUtilities
    {
        public static bool IsUIAvailable()
        {
            //Add specific UI mouse hoverings etc..
            var player = Main.LocalPlayer;
            return Main.hasFocus &&
                !Main.playerInventory &&
                !Main.drawingPlayerChat &&
                !Main.editSign &&
                !Main.editChest &&
                !Main.blockInput &&
                !Main.gamePaused &&
                //!player.mouseInterface && //Causing me issues until I add specific UI's to here
                !Main.mapFullscreen &&
                !Main.HoveringOverAnNPC &&
                !player.showItemIcon &&
                player.talkNPC == -1 &&
                (player.itemTime == 0 && player.itemAnimation == 0) &&
                !player.dead &&
                !player.CCed;
        }

        //2x2 tiles seem to be placed wrong in an odd numbered mirror, might need to do an offet "hotfix"
        //War Table seems to crash if attempted to use here?
        public static void MirrorWandPlacement(int i, int j, Item item, int wallType)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            bool IsWithinRange(float number, float value1, float value2)
            {
                return ((number >= value1 && number <= value2) || (number <= value1 && number >= value2));
            }

            //Mirror Wand
            if (modPlayer.mirrorWandEffects)
            {
                float posX = i;
                float posY = j;

                if (TransparentSelection.selectedDirection == 0 || TransparentSelection.selectedDirection == 1)
                {
                    float distanceToMirrorX = MirrorWand.mouseLeftEnd.X - posX;
                    bool hasMirrorAxisPlaced = IsWithinRange(posY, MirrorWand.mouseLeftStart.Y, MirrorWand.mouseLeftEnd.Y);
                    if (hasMirrorAxisPlaced)
                    {
                        if (distanceToMirrorX < 0) //Right to the mirror axis
                        {
                            distanceToMirrorX = Math.Abs(distanceToMirrorX);
                            int newPosX = (int)(MirrorWand.mouseLeftEnd.X - distanceToMirrorX);

                            if (IsWithinRange(newPosX, MirrorWand.start.X, MirrorWand.end.X))
                            {
                                if (wallType == -1)
                                    WorldGen.PlaceTile(newPosX, (int)posY, item.createTile, false, false, -1, item.placeStyle);
                                else
                                    WorldGen.PlaceWall(newPosX, (int)posY, wallType);

                                if (Main.netMode == NetmodeID.MultiplayerClient)
                                    NetMessage.SendTileSquare(-1, newPosX, (int)posY, 1);
                            }
                        }
                        else //Left to the mirror axis
                        {
                            distanceToMirrorX = Math.Abs(distanceToMirrorX);
                            int newPosX = (int)(MirrorWand.mouseLeftEnd.X + distanceToMirrorX);

                            if (IsWithinRange(newPosX, MirrorWand.start.X, MirrorWand.end.X))
                            {
                                if (wallType == -1)
                                    WorldGen.PlaceTile(newPosX, (int)posY, item.createTile, false, false, -1, item.placeStyle);
                                else
                                    WorldGen.PlaceWall(newPosX, (int)posY, wallType);

                                if (Main.netMode == NetmodeID.MultiplayerClient)
                                    NetMessage.SendTileSquare(-1, newPosX, (int)posY, 1);
                            }
                        }
                    }
                }
                else if (TransparentSelection.selectedDirection == 2 || TransparentSelection.selectedDirection == 3)
                {
                    float distanceToMirrorY = MirrorWand.mouseLeftEnd.Y - posY;
                    bool hasMirrorAxisPlaced = IsWithinRange(posX, MirrorWand.mouseLeftStart.X, MirrorWand.mouseLeftEnd.X);
                    if (hasMirrorAxisPlaced)
                    {
                        if (distanceToMirrorY < 0) //Bottom to the mirror axis
                        {
                            distanceToMirrorY = Math.Abs(distanceToMirrorY);
                            int newPosY = (int)(MirrorWand.mouseLeftEnd.Y - distanceToMirrorY);

                            if (IsWithinRange(newPosY, MirrorWand.start.Y, MirrorWand.end.Y))
                            {
                                if (wallType == -1)
                                    WorldGen.PlaceTile((int)posX, newPosY, item.createTile, false, false, -1, item.placeStyle);
                                else
                                    WorldGen.PlaceWall((int)posX, newPosY, wallType);

                                if (Main.netMode == NetmodeID.MultiplayerClient)
                                    NetMessage.SendTileSquare(-1, (int)posX, newPosY, 1);
                            }
                        }
                        else //Top to the mirror axis
                        {
                            distanceToMirrorY = Math.Abs(distanceToMirrorY);
                            int newPosY = (int)(MirrorWand.mouseLeftEnd.Y + distanceToMirrorY);

                            if (IsWithinRange(newPosY, MirrorWand.start.Y, MirrorWand.end.Y))
                            {
                                if (wallType == -1)
                                    WorldGen.PlaceTile((int)posX, newPosY, item.createTile, false, false, -1, item.placeStyle);
                                else
                                    WorldGen.PlaceWall((int)posX, newPosY, wallType);

                                if (Main.netMode == NetmodeID.MultiplayerClient)
                                    NetMessage.SendTileSquare(-1, (int)posX, newPosY, 1);
                            }
                        }
                    }
                }
            }
        }
    }
}