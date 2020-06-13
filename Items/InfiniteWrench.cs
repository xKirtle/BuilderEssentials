
using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
                int posX = Player.tileTargetX;
                int posY = Player.tileTargetY;
                int brokenTile = -1;

                //Not ready for multiplayer yet
                if (oldPosX != posX || oldPosY != posY && Main.netMode != NetmodeID.Server)
                {
                    //Main.NewText("PosX: " + posX + " / PosY: " + posY);

                    //SAVE A LIST OF ALL ITEM DROPS BEFORE AND AFTER AND COMPARE WHICH ITEM IS THE NEW?
                    //Removes all item drops
                    for (int i = 0; i < Main.maxItems; i++)
                    {
                        Main.item[i].active = false;
                    }

                    brokenTile = Main.tile[posX, posY].type;
                    Main.LocalPlayer.PickTile(posX, posY, 999);

                    //Sends network event that the tile was broken
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.TileChange, -1, -1, null, 14, posX, posY);

                    Item lastItem = new Item();
                    bool foundNewLastItem = false;
                    for (int i = 0; i < Main.maxItems; i++)
                    {
                        if (!Main.item[i].IsAir)
                        {
                            lastItem = Main.item[i];
                            Main.item[i].active = false;
                            foundNewLastItem = true;
                            break;
                        }
                    }

                    if (foundNewLastItem)
                    {

                        //IF ITEM DROPPED EXISTS IN INVENTORY, SWITCH IT WITH THE SELECTEDITEM
                        //ELSE ITEM DOES NOT EXIST IN INVENTORY, FIND FIRST AIR SPACE IN INVENTORY, PUT THE SELECTEDITEM THERE AND
                        //FILL THE SELECTEDITEM INDEX WITH THE ITEM DROPPED

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
                        {
                            WorldGen.PlaceTile(posX, posY, brokenTile);

                            //Neds to send a network even that a new tile was placed
                            if (Main.netMode == NetmodeID.Server) 
                            {
                                //NetMessage.SendData(MessageID.TileChange, -1, -1, null, 14, posX, posY);
                            }
                        }

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

    public partial class InfinitePlacementTile : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, Item item)
        {
            if (Main.LocalPlayer.GetModPlayer<BuilderPlayer>().InfinitePlacement)
            {
                item.stack = item.maxStack + 1;
            }
        }
    }

    public partial class InfinitePlacementWall : GlobalWall
    {
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            if (Main.LocalPlayer.GetModPlayer<BuilderPlayer>().InfinitePlacement)
            {
                item.stack = item.maxStack + 1;
            }
        }
    }
}
