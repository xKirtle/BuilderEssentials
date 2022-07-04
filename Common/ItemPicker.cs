using System.Collections.Generic;
using BuilderEssentials.Common.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ObjectData;

namespace BuilderEssentials.Common;

public static class ItemPicker
{
    public static int PickItem(Tile tile)
        => PickItem(new MinimalTile(tile.TileType, tile.WallType, tile.HasTile, TileObjectData.GetTileStyle(tile)));

    public static int PickItem(MinimalTile tile) {
        List<int> itemIDs = new();
        
        if (tile.TileType >= 0 && tile.HasTile)
            itemIDs = BuilderEssentials.TileToItems[tile.TileType];
        else if (tile.TileType >= 0 && tile.WallType >= 0)
            itemIDs = BuilderEssentials.WallToItems[tile.WallType];

        if (itemIDs.Count > 1)
            return itemIDs[tile.PlaceStyle];
        else if (itemIDs.Count == 1)
            return itemIDs[0];

        return ItemID.None;
    }
}