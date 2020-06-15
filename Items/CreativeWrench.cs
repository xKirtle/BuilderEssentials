using BuilderEssentials.UI;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class CreativeWrench : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows Block Picking");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.vanity = false;
            item.width = 24;
            item.height = 24;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
        }

        private int oldPosX;
        private int oldPosY;
        int mouseRightTimer = 0;
        BuilderPlayer modPlayer;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            modPlayer = Main.LocalPlayer.GetModPlayer<BuilderPlayer>();

            if (modPlayer.InfinitePlacementSelected)
                player.AddBuff(mod.BuffType("InfinitePlacementBuff"), 10);

            player.blockRange += 55;
            player.wallSpeed += 10;
            player.tileSpeed += 50;
            Player.tileRangeX = 65;
            Player.tileRangeY = 55;

            if (Main.mouseRight)
            {
                if (++mouseRightTimer == 2)
                    BasePanel.creativeWheelUIOpen = !BasePanel.creativeWheelUIOpen;
            }

            if (Main.mouseRightRelease)
                mouseRightTimer = 0;

            if (Main.mouseMiddle && modPlayer.colorPickerSelected)
            {
                //CODE BELOW NEEDS REFACTORING!!
                //HUGE FUNCTIONAL MESS

                int posX = Player.tileTargetX;
                int posY = Player.tileTargetY;
                int brokenTileType = -1;
                Item lastDroppedItem;

                //Not ready for multiplayer yet
                //This should be all local??
                if (oldPosX != posX || oldPosY != posY && Main.netMode != NetmodeID.Server)
                {
                    List<Item> blockTempList = new List<Item>();
                    //Maybe check when Main.item[i] == empty item and stop the iteration?
                    for (int i = 0; i < Main.maxItems; i++)
                    {
                        if (!Main.item[i].IsAir)
                        {
                            blockTempList.Add(Main.item[i]);
                            if (Main.item[i + 1].IsAir)
                                break;
                        }
                    }

                    brokenTileType = Main.tile[posX, posY].type;
                    Main.LocalPlayer.PickTile(posX, posY, 999);

                    List<Item> blockTempList2 = new List<Item>();
                    for (int i = 0; i < Main.maxItems; i++)
                    {
                        if (!Main.item[i].IsAir)
                        {
                            blockTempList2.Add(Main.item[i]);
                            if (Main.item[i + 1].IsAir)
                            {
                                Main.item[i].active = false;
                                break;
                            }
                        }
                    }

                    List<Item> blockDroppedItem = blockTempList2.Except(blockTempList).ToList();

                    //If PickTile is successful, there should be 1 droppedItem. If there's none, it means the tile is a wall
                    if (blockDroppedItem.Count == 1)
                    {
                        lastDroppedItem = blockDroppedItem.FirstOrDefault();
                        bool step1AbleToFinish = false;
                        for (int i = 0; i < 50; i++)
                        {
                            if (Main.LocalPlayer.inventory[i].IsTheSameAs(lastDroppedItem))
                            {
                                Item tempItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem]; //Item selected saved on a temp variable
                                Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = lastDroppedItem; //selected item is now the dropped item
                                Main.LocalPlayer.inventory[i] = tempItem; //Space where the dropped item was now contains the previous selected item
                                step1AbleToFinish = true;
                                break;
                            }
                        }

                        if (!step1AbleToFinish) //Item does not exist in the inventory
                        {
                            bool availableAirSlots = false;
                            for (int i = 0; i < 50; i++)
                            {
                                if (Main.LocalPlayer.inventory[i].IsAir)
                                {
                                    Item tempItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
                                    Main.LocalPlayer.inventory[i] = tempItem;
                                    Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = lastDroppedItem;
                                    availableAirSlots = true;
                                    break;
                                }
                            }

                            if (!availableAirSlots)
                                Main.NewText("Inventory is full!");
                        }
                        if (brokenTileType != -1)
                            WorldGen.PlaceTile(posX, posY, brokenTileType);

                        oldPosX = posX;
                        oldPosY = posY;
                    }
                    else if (blockDroppedItem.Count == 0)
                    {
                        List<Item> wallTempList = new List<Item>();
                        for (int i = 0; i < Main.maxItems; i++)
                        {
                            if (!Main.item[i].IsAir)
                            {
                                wallTempList.Add(Main.item[i]);
                                if (Main.item[i + 1].IsAir)
                                    break;
                            }
                        }

                        brokenTileType = Main.tile[posX, posY].type;
                        WorldGen.KillWall(posX, posY);

                        List<Item> wallTempList2 = new List<Item>();
                        for (int i = 0; i < Main.maxItems; i++)
                        {
                            if (!Main.item[i].IsAir)
                            {
                                wallTempList2.Add(Main.item[i]);
                                if (Main.item[i + 1].IsAir)
                                {
                                    Main.item[i].active = false;
                                    break;
                                }
                            }
                        }

                        List<Item> wallDroppedItem = wallTempList2.Except(wallTempList).ToList();

                        if (wallDroppedItem.Count == 1)
                        {
                            lastDroppedItem = wallDroppedItem.FirstOrDefault();
                            bool step1AbleToFinish = false;
                            for (int i = 0; i < 50; i++)
                            {
                                if (Main.LocalPlayer.inventory[i].IsTheSameAs(lastDroppedItem))
                                {
                                    Item tempItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem]; //Item selected saved on a temp variable
                                    Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = lastDroppedItem; //selected item is now the dropped item
                                    Main.LocalPlayer.inventory[i] = tempItem; //Space where the dropped item was contains now the previous selected item
                                    step1AbleToFinish = true;
                                    break;
                                }
                            }

                            if (!step1AbleToFinish) //Item does not exist in the inventory
                            {
                                for (int i = 0; i < 50; i++)
                                {
                                    if (Main.LocalPlayer.inventory[i].IsAir)
                                    {
                                        Item tempItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
                                        Main.LocalPlayer.inventory[i] = tempItem;
                                        Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = lastDroppedItem;
                                        break;
                                    }
                                }
                            }
                            WorldGen.PlaceWall(posX, posY, lastDroppedItem.createWall);

                            oldPosX = posX;
                            oldPosY = posY;
                        }
                        //else, tile is Air
                    }
                }
            }
        }
    }

    public class InfinitePlacementTile : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, Item item)
        {
            item.consumable = Main.LocalPlayer.GetModPlayer<BuilderPlayer>().InfinitePlacementSelected == true ? false : true;
        }
    }

    public class InfinitePlacementWall : GlobalWall
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            item.consumable = Main.LocalPlayer.GetModPlayer<BuilderPlayer>().InfinitePlacementSelected == true ? false : true;
        }
    }
}
