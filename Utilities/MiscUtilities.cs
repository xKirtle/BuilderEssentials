using BuilderEssentials.Items.Accessories;
using BuilderEssentials.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;

namespace BuilderEssentials.Utilities
{
    public static partial class Tools
    {
        //causing issues when debugging since BuilderPlayer isn't initialized when the Tools ctor is called
        static readonly BuilderPlayer modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();
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

        public static bool IsWithinRange(float number, float value1, float value2) =>
            (number >= value1 && number <= value2) || (number <= value1 && number >= value2);

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

        public static bool ReduceItemStack(int itemType)
        {
            foreach (Item item in Main.LocalPlayer.inventory)
            {
                if (item.type == itemType && item.stack >= 1)
                {
                    --item.stack;
                    return true;
                }
            }
            return false;
        }

        public static bool ValidTilePlacement(int posX, int posY)
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
            //Has Wall behind?
            if (Main.tile[posX, posY].wall != 0)
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

        public static void AutoReplaceStack(Item item, bool reducedStack = true)
        {
            Player player = Main.LocalPlayer;
            if (BuilderEssentials.autoReplaceStack)
            {
                if (item.stack == 1)
                {
                    //Search for more of the same item in the inventory
                    int newItemIndex = -1;
                    int index = -1;
                    foreach (Item invItem in player.inventory)
                    {
                        if (index++ < 50 && player.inventory[index].type == item.type)
                        {
                            if (index != player.selectedItem)
                            {
                                newItemIndex = index;
                                break;
                            }
                        }
                        else if (index > 50)
                            break;
                    }

                    if (newItemIndex != -1)
                    {
                        //Clone found item, place the clone in the heldItem and turn to air the original item
                        Item newItem = player.inventory[newItemIndex].Clone();
                        if (reducedStack) newItem.stack += 1;
                        player.inventory[player.selectedItem] = newItem;
                        player.inventory[newItemIndex].TurnToAir();
                    }
                }
            }
        }

        public static bool InfinitePlacement => (modPlayer.creativeWheelSelectedIndex.Contains(
            CreativeWheelItem.InfinitePlacement.ToInt()) && modPlayer.isCreativeWrenchEquiped) ||
            modPlayer.creativeWheelSelectedIndex.Contains(CreativeWheelItem.InfinityUpgrade.ToInt());

        public static bool PlacementAnywhere => (modPlayer.creativeWheelSelectedIndex.Contains(
            CreativeWheelItem.PlacementAnywhere.ToInt()) && modPlayer.isCreativeWrenchEquiped);

        public enum CreativeWheelItem
        {
            ItemPicker,
            InfinitePlacement,
            AutoHammer,
            PlacementAnywhere,
            InfinitePickupRange,

            //Non important order (independent items)
            InfinityUpgrade
        }

        public static int ToInt(this CreativeWheelItem cwItem) => (int)cwItem;

        public static Item TileToItem(Tile tile)
        {
            Item item = new Item();
            item.SetDefaults(PickItem(tile, false));
            return item;
        }

        public enum ItemTypes
        {
            Air,
            Tile,
            Wall
        }

        public static int ToInt(this ItemTypes itemTypes) => (int)itemTypes;

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
    }
}