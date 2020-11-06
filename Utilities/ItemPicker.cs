using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Utilities
{
    internal static partial class HelperMethods
    {
        //Thanks Oli. B for the concept
        public static int PickItem(Tile tile, bool organizeInventory = true)
        {
            Player player = Main.LocalPlayer;
            Item item = new Item();
            int itemID = -1;
            bool foundItem = false;

            if (tile.type >= 0 && tile.active())
            {
                for (int i = 0; i < ItemLoader.ItemCount; i++)
                {
                    item.SetDefaults(i);
                    if (item.createTile == tile.type)
                    {
                        foundItem = true;
                        itemID = i;
                        break;
                    }
                }
            }
            else if (tile.type >= 0 && tile.wall >= 0)
            {
                for (int i = 0; i < ItemLoader.ItemCount; i++)
                {
                    item.SetDefaults(i);
                    if (item.createWall == tile.wall)
                    {
                        foundItem = true;
                        itemID = i;
                        break;
                    }
                }
            }

            //organize inventory
            if (foundItem)
            {
                //Furniture Check
                //If it is a furniture and has a different frame, item will be changed to the correct frame item
                int furnitureTileType = FindFurniture(tile, ref item);
                if (furnitureTileType != -1)
                    itemID = furnitureTileType;
                    

                if (organizeInventory)
                {
                    bool isItemInInventory = false;
                    for (int i = 0; i < 50; i++)
                    {
                        if (player.inventory[i].IsTheSameAs(item))
                        {
                            //Finds item in inventory and switch with selected item
                            Item selectedItem = player.HeldItem;
                            player.inventory[player.selectedItem] = player.inventory[i];
                            player.inventory[i] = selectedItem;
                            isItemInInventory = true;
                            break;
                        }
                    }

                    if (!isItemInInventory)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            if (player.inventory[i].IsAir)
                            {
                                //Find first air space in inventory and switches selected item to there
                                Item selectedItem = player.HeldItem;
                                player.inventory[i] = selectedItem;

                                player.inventory[player.selectedItem] = item;
                                break;
                            }
                        }
                    }
                }
            }

            return itemID;
        }
        
        internal static int FindFurniture(Tile tile, ref Item item)
        {
            int tilePlaceStyle = TileObjectData.GetTileStyle(tile);
            int originalItemType = item.type;
            for (int i = 0; i < ItemLoader.ItemCount; i++)
            {
                item.SetDefaults(i);
                if (item.createTile == tile.type && item.placeStyle == tilePlaceStyle)
                    return i;
            }

            //if it reaches here, didn't find any matches
            item.SetDefaults(originalItemType);
            return -1;
        }
    }
}