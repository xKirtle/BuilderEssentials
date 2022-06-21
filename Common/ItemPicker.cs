using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ObjectData;

namespace BuilderEssentials.Common;

public static class ItemPicker
{
    public static int PickItem(Tile tile) {
        List<int> itemIDs = new();
        int placeStyle = TileObjectData.GetTileStyle(tile);

        if (tile.TileType >= 0 && tile.HasTile)
            itemIDs = BuilderEssentials.TileToItems[tile.TileType];
        else if (tile.TileType >= 0 && tile.WallType >= 0)
            itemIDs = BuilderEssentials.WallToItems[tile.WallType];

        if (itemIDs.Count > 1)
            return itemIDs[placeStyle];
        else if (itemIDs.Count == 1)
            return itemIDs[0];

        return ItemID.None;
    }
}