using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Common;

public static class ItemPicker
{
    public static int PickItem(Tile tile, bool organizeInventory = true, bool only1X1 = false) {
        TileObjectData data = TileObjectData.GetTileData(tile);
        if (data != null && only1X1) return -1;

        Player player = Main.LocalPlayer;
        Item item = new Item();
        int itemID = -1;
        bool foundItem = false;

        if (tile.TileType >= 0 && tile.HasTile) {
            for (int i = 0; i < ItemLoader.ItemCount; i++) {
                item.SetDefaults(i);

                if (item.createTile == tile.TileType) {
                    foundItem = true;
                    itemID = i;
                    break;
                }
            }
        }
        else if (tile.TileType >= 0 && tile.WallType >= 0) {
            for (int i = 0; i < ItemLoader.ItemCount; i++) {
                item.SetDefaults(i);

                if (item.createWall == tile.WallType) {
                    foundItem = true;
                    itemID = i;
                    break;
                }
            }
        }

        if (foundItem) {
            //If it is a furniture and has a different frame, item will be changed to the correct frame item
            int furnitureTileType = FindFurniture(tile, ref item);

            if (furnitureTileType != -1)
                itemID = furnitureTileType;


            if (organizeInventory) {
                bool isItemInInventory = false;

                for (int i = 0; i < 50; i++) {
                    if (player.inventory[i].type == item.type) {
                        //Finds item in inventory and switch with selected item
                        Item selectedItem = player.HeldItem;
                        player.inventory[player.selectedItem] = player.inventory[i];
                        player.inventory[i] = selectedItem;
                        isItemInInventory = true;
                        break;
                    }
                }

                if (!isItemInInventory) {
                    for (int i = 0; i < 50; i++) {
                        if (player.inventory[i].IsAir) {
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

    private static int FindFurniture(Tile tile, ref Item item) {
        int tilePlaceStyle = TileObjectData.GetTileStyle(tile);
        int originalItemType = item.type;

        for (int i = 0; i < ItemLoader.ItemCount; i++) {
            item.SetDefaults(i);

            if (item.createTile == tile.TileType && item.placeStyle == tilePlaceStyle)
                return i;
        }

        //if it reaches here, didn't find any matches
        item.SetDefaults(originalItemType);
        return -1;
    }
}