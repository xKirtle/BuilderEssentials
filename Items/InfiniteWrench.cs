
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuilderEssentials.Items
{
    class InfiniteWrench : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows infinite range and fast placement");
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

        protected (int index, Item accessory) FindDifferentEquippedExclusiveAccessory()
        {
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
            for (int i = 3; i < 3 + maxAccessoryIndex; i++)
            {
                Item otherAccessory = Main.LocalPlayer.armor[i];
                if (!otherAccessory.IsAir &&
                    !item.IsTheSameAs(otherAccessory) &&
                    otherAccessory.modItem is InfiniteWrench)
                {
                    return (i, otherAccessory);
                }
            }
            return (-1, null);
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot < 10)
            {
                int index = FindDifferentEquippedExclusiveAccessory().index;
                if (index != -1)
                    return slot == index;
            }
            return base.CanEquipAccessory(player, slot);
        }

        public override bool CanRightClick()
        {
            int maxAccessoryIndex = 5 + Main.LocalPlayer.extraAccessorySlots;
            for (int i = 13; i < 13 + maxAccessoryIndex; i++)
            {
                if (Main.LocalPlayer.armor[i].type == item.type)
                    return false;
            }

            if (FindDifferentEquippedExclusiveAccessory().accessory != null)
                return true;

            return base.CanRightClick();
        }

        public override void RightClick(Player player)
        {
            var (index, accessory) = FindDifferentEquippedExclusiveAccessory();
            if (accessory != null)
            {
                Main.LocalPlayer.QuickSpawnClonedItem(accessory);
                Main.LocalPlayer.armor[index] = item.Clone();
            }
        }

        private int oldPosX;
        private int oldPosY;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(mod.BuffType("InfinitePlacementBuff"), 90);
            player.blockRange += 55;
            player.wallSpeed += 10;
            player.tileSpeed += 50;
            Player.tileRangeX = 65;
            Player.tileRangeY = 55;

            if (Main.mouseMiddle)
            {
                //CODE BELOW NEEDS REFACTORING!!
                //HUGE FUNCTIONAL MESS

                int posX = Player.tileTargetX;
                int posY = Player.tileTargetY;
                int brokenTile = -1;
                Item lastItem;

                //Not ready for multiplayer yet
                //This should be all local??
                if (oldPosX != posX || oldPosY != posY && Main.netMode != NetmodeID.Server)
                {
                    //Main.NewText("PosX: " + posX + " / PosY: " + posY);

                    List<Item> blockTempList = new List<Item>();
                    //Maybe check when Main.item[i] == empty item and stop the iteration?
                    for (int i = 0; i < Main.maxItems; i++)
                    {
                        blockTempList.Add(Main.item[i]);
                    }

                    brokenTile = Main.tile[posX, posY].type;
                    Main.LocalPlayer.PickTile(posX, posY, 999);

                    List<Item> blockTempList2 = new List<Item>();
                    for (int i = 0; i < Main.maxItems; i++)
                    {
                        blockTempList2.Add(Main.item[i]);
                        Main.item[i].active = false;
                    }

                    List<Item> blockDroppedItem = blockTempList2.Except(blockTempList).ToList();
                    //No need to send network events since the tile will remain the same after all code runs?
                    //Sends network event that the tile was broken
                    //if (Main.netMode == NetmodeID.Server)
                    //NetMessage.SendData(MessageID.TileChange, -1, -1, null, 14, posX, posY);


                    //IF DROPPEDITEM.COUNT == 0, WorldGen.KillWall and try to grab newest dropped item again

                    //If PickTile is successful, there should be 1 droppedItem. If there's none, it means the tile is a wall
                    if (blockDroppedItem.Count == 1)
                    {
                        lastItem = blockDroppedItem.FirstOrDefault();
                        bool step1AbleToFinish = false;
                        for (int i = 0; i < 50; i++)
                        {
                            if (Main.LocalPlayer.inventory[i].IsTheSameAs(lastItem))
                            {
                                Item tempItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem]; //Item selected saved on a temp variable
                                Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = lastItem; //selected item is now the dropped item
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
                                    Item tempItem = new Item();
                                    tempItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
                                    Main.LocalPlayer.inventory[i] = tempItem;
                                    Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = lastItem;

                                    break;
                                }
                            }
                        }
                        if (brokenTile != -1)
                            WorldGen.PlaceTile(posX, posY, brokenTile);

                        //No need?
                        //Neds to send a network even that a new tile was placed
                        //if (Main.netMode == NetmodeID.Server) 
                        //NetMessage.SendData(MessageID.TileChange, -1, -1, null, 14, posX, posY);

                        oldPosX = posX;
                        oldPosY = posY;
                    }
                    else if (blockDroppedItem.Count == 0)
                    {
                        List<Item> wallTempList = new List<Item>();
                        for (int i = 0; i < Main.maxItems; i++)
                        {
                            wallTempList.Add(Main.item[i]);
                        }

                        brokenTile = Main.tile[posX, posY].type;
                        WorldGen.KillWall(posX, posY);

                        List<Item> wallTempList2 = new List<Item>();
                        for (int i = 0; i < Main.maxItems; i++)
                        {
                            wallTempList2.Add(Main.item[i]);
                            Main.item[i].active = false;
                        }

                        List<Item> wallDroppedItem = wallTempList2.Except(wallTempList).ToList();

                        lastItem = wallDroppedItem.FirstOrDefault();
                        bool step1AbleToFinish = false;
                        for (int i = 0; i < 50; i++)
                        {
                            if (Main.LocalPlayer.inventory[i].IsTheSameAs(lastItem))
                            {
                                Item tempItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem]; //Item selected saved on a temp variable
                                Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = lastItem; //selected item is now the dropped item
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
                                    Item tempItem = new Item();
                                    tempItem = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
                                    Main.LocalPlayer.inventory[i] = tempItem;
                                    Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem] = lastItem;

                                    break;
                                }
                            }
                        }
                        WorldGen.PlaceWall(posX, posY, lastItem.createWall);

                        //No need?
                        //Neds to send a network even that a new tile was placed
                        //if (Main.netMode == NetmodeID.Server) 
                        //NetMessage.SendData(MessageID.TileChange, -1, -1, null, 14, posX, posY);

                        oldPosX = posX;
                        oldPosY = posY;
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            //Not really worried about balancing at this point
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(mod.GetItem("InfinityUpgrade"), 1);
            modRecipe.AddIngredient(mod.GetItem("PlacementWrench"), 1);
            modRecipe.AddTile(TileID.TinkerersWorkbench);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }

    public class InfinitePlacementTile : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, Item item)
        {
            item.consumable = Main.LocalPlayer.GetModPlayer<BuilderPlayer>().InfinitePlacement == true ? false : true;
        }
    }

    public class InfinitePlacementWall : GlobalWall
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            item.consumable = Main.LocalPlayer.GetModPlayer<BuilderPlayer>().InfinitePlacement == true ? false : true;
        }
    }
}
