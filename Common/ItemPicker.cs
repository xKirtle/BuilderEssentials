using System.Collections.Generic;
using BuilderEssentials.Common.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuilderEssentials.Common;

public static class ItemPicker
{
    private static Dictionary<int, List<int>> TileToItems => BuilderEssentials.TileToItems;
    private static Dictionary<int, List<int>> WallToItems => BuilderEssentials.WallToItems;

    public static int PickItem(Tile tile)
        => PickItem(new MinimalTile(tile.TileType, tile.WallType, tile.HasTile, TileObjectData.GetTileStyle(tile)));

    public static int PickItem(MinimalTile tile) {
        List<int> itemIDs = new();

        if (tile.TileType >= 0 && tile.HasTile) {
            itemIDs = TileToItems.ContainsKey(tile.TileType) ? TileToItems[tile.TileType]
                : FindAndCacheModTiles(TypeOfItem.Tile, tile.TileType);
        }
        else if (tile.WallType >= 0 && tile.WallType >= 0) {
            itemIDs = WallToItems.ContainsKey(tile.WallType) ? WallToItems[tile.WallType]
                : FindAndCacheModTiles(TypeOfItem.Wall, tile.WallType);
        }

        if (itemIDs.Count > 1)
            return itemIDs[tile.PlaceStyle];
        else if (itemIDs.Count == 1)
            return itemIDs[0];

        return ItemID.None;
    }

    private static List<int> FindAndCacheModTiles(TypeOfItem typeOfItem, int type) {
        List<int> matchingItems = new();
        Item item = new();

        if (typeOfItem == TypeOfItem.Tile) {
            for (int i = 0; i < ItemLoader.ItemCount; i++) {
                item.SetDefaults(i);
                if (item.createTile == type)
                    matchingItems.Add(i);
            }
            
            TileToItems.Add(type, matchingItems);
        }
        else if (typeOfItem == TypeOfItem.Wall) {
            for (int i = 0; i < ItemLoader.ItemCount; i++) {
                item.SetDefaults(i);
                if (item.createWall == type)
                    matchingItems.Add(i);
            }
            
            WallToItems.Add(type, matchingItems);
        }
        
        return matchingItems;
    }
}