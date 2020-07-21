using BuilderEssentials.Items;
using BuilderEssentials.Items.Accessories;
using BuilderEssentials.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Utilities
{
    public static partial class Tools
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

        public static bool IsWithinRange(float number, float value1, float value2)
        {
            return ((number >= value1 && number <= value2) || (number <= value1 && number >= value2));
        }

        //2x2 tiles seem to be placed wrong in an odd numbered mirror, might need to do an offet "hotfix"
        public static void MirrorWandPlacement(int i, int j, Item item, int wallType)
        {
            BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            //Mirror Wand
            if (modPlayer.mirrorWandEffects)//War Table seems to crash if attempted to use here?
            {
                float posX = i;
                float posY = j;

                if (TransparentSelection.selectedDirection == 0 || TransparentSelection.selectedDirection == 1)
                {
                    float distanceToMirrorX = MirrorWand.mouseLeftEnd.X - posX;
                    bool hasMirrorAxisPlaced = IsWithinRange(posY, MirrorWand.mouseLeftStart.Y, MirrorWand.mouseLeftEnd.Y);
                    if (hasMirrorAxisPlaced)
                    {
                        int newPosX;
                        bool inRange = false;
                        if (distanceToMirrorX < 0) //Right to the mirror axis
                        {
                            distanceToMirrorX = Math.Abs(distanceToMirrorX);
                            newPosX = (int)(MirrorWand.mouseLeftEnd.X - distanceToMirrorX);

                            if (IsWithinRange(newPosX, MirrorWand.start.X, MirrorWand.end.X))
                                inRange = true;
                        }
                        else //Left to the mirror axis
                        {
                            distanceToMirrorX = Math.Abs(distanceToMirrorX);
                            newPosX = (int)(MirrorWand.mouseLeftEnd.X + distanceToMirrorX);

                            if (IsWithinRange(newPosX, MirrorWand.start.X, MirrorWand.end.X))
                                inRange = true;
                        }

                        if (inRange)
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
                else if (TransparentSelection.selectedDirection == 2 || TransparentSelection.selectedDirection == 3)
                {
                    float distanceToMirrorY = MirrorWand.mouseLeftEnd.Y - posY;
                    bool hasMirrorAxisPlaced = IsWithinRange(posX, MirrorWand.mouseLeftStart.X, MirrorWand.mouseLeftEnd.X);
                    if (hasMirrorAxisPlaced)
                    {
                        int newPosY;
                        bool inRange = false;
                        if (distanceToMirrorY < 0) //Bottom to the mirror axis
                        {
                            distanceToMirrorY = Math.Abs(distanceToMirrorY);
                            newPosY = (int)(MirrorWand.mouseLeftEnd.Y - distanceToMirrorY);

                            if (IsWithinRange(newPosY, MirrorWand.start.Y, MirrorWand.end.Y))
                                inRange = true;
                        }
                        else //Top to the mirror axis
                        {
                            distanceToMirrorY = Math.Abs(distanceToMirrorY);
                            newPosY = (int)(MirrorWand.mouseLeftEnd.Y + distanceToMirrorY);

                            if (IsWithinRange(newPosY, MirrorWand.start.Y, MirrorWand.end.Y))
                                inRange = true;
                        }

                        if (inRange)
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

        //public static void MirrorWandBreaking(int i, int j, int type, Item item)
        //{
        //    BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

        //    if (modPlayer.mirrorWandEffects)
        //    {
        //        float posX = i;
        //        float posY = j;

        //        if (TransparentSelection.selectedDirection == 0 || TransparentSelection.selectedDirection == 1)
        //        {
        //            float distanceToMirrorX = MirrorWand.mouseLeftEnd.X - posX;
        //            bool hasMirrorAxisPlaced = IsWithinRange(posY, MirrorWand.mouseLeftStart.Y, MirrorWand.mouseLeftEnd.Y);

        //            if (hasMirrorAxisPlaced)
        //            {
        //                int newPosX;
        //                bool inRange = false;
        //                if (distanceToMirrorX < 0) //Right to the mirror axis
        //                {
        //                    distanceToMirrorX = Math.Abs(distanceToMirrorX);
        //                    newPosX = (int)(MirrorWand.mouseLeftEnd.X - distanceToMirrorX);

        //                    if (IsWithinRange(newPosX, MirrorWand.start.X, MirrorWand.end.X))
        //                        inRange = true;
        //                }
        //                else //Left to the mirror axis
        //                {
        //                    distanceToMirrorX = Math.Abs(distanceToMirrorX);
        //                    newPosX = (int)(MirrorWand.mouseLeftEnd.X + distanceToMirrorX);

        //                    if (IsWithinRange(newPosX, MirrorWand.start.X, MirrorWand.end.X))
        //                        inRange = true;
        //                }

        //                if (inRange)
        //                {
        //                    Tile tile = Main.tile[newPosX, j];

        //                    if (tile.type >= 0 && item.pick >= 35)
        //                        WorldGen.KillTile(newPosX, j);
        //                    else if (tile.wall > 0 && item.pick >= 35)
        //                        WorldGen.KillWall(newPosX, j);

        //                    if (Main.netMode == NetmodeID.MultiplayerClient)
        //                        NetMessage.SendTileSquare(-1, newPosX, (int)posY, 1);
        //                }
        //            }
        //        }
        //        else if (TransparentSelection.selectedDirection == 2 || TransparentSelection.selectedDirection == 3)
        //        {
        //            float distanceToMirrorY = MirrorWand.mouseLeftEnd.Y - posY;
        //            bool hasMirrorAxisPlaced = IsWithinRange(posX, MirrorWand.mouseLeftStart.X, MirrorWand.mouseLeftEnd.X);
        //            if (hasMirrorAxisPlaced)
        //            {
        //                int newPosY;
        //                bool inRange = false;
        //                if (distanceToMirrorY < 0) //Bottom to the mirror axis
        //                {
        //                    distanceToMirrorY = Math.Abs(distanceToMirrorY);
        //                    newPosY = (int)(MirrorWand.mouseLeftEnd.Y - distanceToMirrorY);

        //                    if (IsWithinRange(newPosY, MirrorWand.start.Y, MirrorWand.end.Y))
        //                        inRange = true;
        //                }
        //                else //Top to the mirror axis
        //                {
        //                    distanceToMirrorY = Math.Abs(distanceToMirrorY);
        //                    newPosY = (int)(MirrorWand.mouseLeftEnd.Y + distanceToMirrorY);

        //                    if (IsWithinRange(newPosY, MirrorWand.start.Y, MirrorWand.end.Y))
        //                        inRange = true;
        //                }

        //                if (inRange)
        //                {
        //                    Tile tile = Main.tile[i, newPosY];

        //                    if (tile.type >= 0 && item.pick >= 35)
        //                        WorldGen.KillTile(i, newPosY);
        //                    else if (tile.wall > 0 && item.pick >= 35)
        //                        WorldGen.KillWall(i, newPosY);

        //                    if (Main.netMode == NetmodeID.MultiplayerClient)
        //                        NetMessage.SendTileSquare(-1, (int)posX, newPosY, 1);
        //                }
        //            }
        //        }
        //    }
        //}

        public static bool IsTownNpc(NPC npc)
        {
            return (npc.type == NPCID.Guide || npc.type == NPCID.Merchant || npc.type == NPCID.Nurse ||
                    npc.type == NPCID.Demolitionist || npc.type == NPCID.DyeTrader || npc.type == NPCID.Angler ||
                    npc.type == NPCID.Dryad || npc.type == NPCID.Painter || npc.type == NPCID.ArmsDealer ||
                    npc.type == NPCID.DD2Bartender || npc.type == NPCID.Stylist || npc.type == NPCID.GoblinTinkerer ||
                    npc.type == NPCID.WitchDoctor || npc.type == NPCID.Clothier || npc.type == NPCID.Mechanic ||
                    npc.type == NPCID.PartyGirl || npc.type == NPCID.Wizard || npc.type == NPCID.TaxCollector ||
                    npc.type == NPCID.Truffle || npc.type == NPCID.Pirate || npc.type == NPCID.Steampunker ||
                    npc.type == NPCID.Cyborg || npc.type == NPCID.SantaClaus || npc.type == NPCID.TravellingMerchant ||
                    npc.type == NPCID.OldMan || npc.type == NPCID.SkeletonMerchant);
        }

        public static int FindNextEmptyInventorySlot()
        {
            Player player = Main.LocalPlayer;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].IsAir)
                    return i;
            }
            return -1;
        }

        public static bool IsCreativeWrenchEquipped()
        {
            Player player = Main.LocalPlayer;
            int maxAccessoryIndex = 5 + player.extraAccessorySlots;
            for (int i = 3; i < 3 + maxAccessoryIndex; i++)
            {
                if (player.armor[i].type == ModContent.ItemType<CreativeWrench>())
                    return true;
            }
            return false;
        }

        public static bool ReduceItemStack(int itemType)
        {
            foreach (Item item in Main.LocalPlayer.inventory)
            {
                if (item.type == itemType)
                {
                    item.stack--;
                    return true;
                }
            }
            return false;
        }

        public static bool HasTileAround(int posX, int posY)
        {
            //Top
            if (Main.tile[posX, posY - 1].active())
                return true;
            //Right
            if (Main.tile[posX + 1, posY].active())
                return true;
            //Bottom
            if (Main.tile[posX, posY + 1].active())
                return true;
            //Left
            if (Main.tile[posX - 1, posY].active())
                return true;

            return false;
        }

        public static bool ToolHasRange(int range)
        {
            Player player = Main.LocalPlayer;
            Vector2 pointedCoord = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
            bool inRange = Vector2.Distance(player.position, pointedCoord) < range * 16;

            return inRange;
        }

        public static bool UIPanelLogic(UIPanel UIPanel, ref bool UIOpen, ref bool UIVisible)
        {
            BasePanel.hoverText?.Remove();
            if (UIPanel != null && Main.playerInventory)
            {
                UIPanel.Remove();
                UIOpen = false;
                UIVisible = false;

                return false;
            }

            if (UIOpen && !UIVisible)
            {
                UIVisible = true;
                return true;
            }
            else if (!UIOpen && UIVisible)
            {
                UIPanel.Remove();
                UIVisible = false;
            }
            return false;
        }

        public static UIText CreateUIText(string text, int left, int top)
        {
            UIText hoverText = new UIText(text, 1, false);
            hoverText.VAlign = 0f;
            hoverText.HAlign = 0f;
            hoverText.Left.Set(left, 0);
            hoverText.Top.Set(top, 0);

            return hoverText;
        }

        public static void FixOldSaveData(ref List<Item> list)
        {
            if (list.Count < list.Capacity)
            {
                for (int i = list.Count; i < list.Capacity; i++)
                    list.Add(new Item());
            }
        }


        //--------------Unused stuff--------------
        public static Item TileToItem(Tile tile)
        {
            Item item = new Item();
            item.SetDefaults(PickItem(tile, false));
            return item;
        }

        public static ItemTypes WhatIsThisItem(int itemType)
        {
            Item item = new Item();
            item.SetDefaults(itemType);

            if (item.createTile != -1 && item.createWall == -1)
                return ItemTypes.Tile;
            else if (item.createTile == -1 && item.createWall != -1)
                return ItemTypes.Wall;
            else
                return ItemTypes.Air;
        }

        public enum ItemTypes
        {
            Air,
            Tile,
            Wall
        }
    }

    //class MirrorWandGlobalTile : GlobalTile
    //{
    //    int oldPosX = 0;
    //    int oldPosY = 0;
    //    public override bool Drop(int i, int j, int type)
    //    {
    //        if (i != oldPosX || j != oldPosY)
    //        {
    //            Tools.MirrorWandBreaking(i, j, type, Main.LocalPlayer.HeldItem);
    //            oldPosX = i;
    //            oldPosY = j;
    //        }

    //        return base.Drop(i, j, type);
    //    }
    //}
}